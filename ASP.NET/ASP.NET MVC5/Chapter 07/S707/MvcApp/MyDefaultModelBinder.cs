using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public class MyDefaultModelBinder : IModelBinder
    {
        #region BindModel
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string prefix = bindingContext.ModelName;
            IValueProvider valueProvider = bindingContext.ValueProvider;
            bool containsPrefix = valueProvider.ContainsPrefix(prefix);

            //如果ValueProvider的数据容器中不包含指定前缀的数据
            //并且启用“去除前缀后的二次绑定”，会将ModelName设置为Null
            if (!containsPrefix)
            {
                if (!bindingContext.FallbackToEmptyPrefix)
                {
                    return null;
                }
                bindingContext.ModelName = null;
            }
            else
            {
                //采用针对简单类型的数据绑定
                ValueProviderResult valueProviderResult = valueProvider.GetValue(prefix);
                if (null != valueProviderResult)
                {
                    return this.BindSimpleModel(controllerContext, bindingContext, valueProviderResult);
                }
            }

            if (bindingContext.ModelMetadata.IsComplexType)
            {
                //采用针对复杂类型的数据绑定
                return this.BindComplexModel(controllerContext, bindingContext);
            }
            return null;
        }
        #endregion

        #region BindSimpleModel
        private object BindSimpleModel(ControllerContext controllerContext, ModelBindingContext bindingContext, ValueProviderResult valueProviderResult)
        {
            SetModelState(bindingContext, valueProviderResult);

            Type modelType = bindingContext.ModelType;
            if (typeof(string) != modelType && this.Match(modelType, typeof(IEnumerable<>)))
            {
                Type arrayType = modelType.IsArray ? modelType : modelType.GetGenericArguments()[0].MakeArrayType();
                object array = valueProviderResult.ConvertTo(arrayType);
                if (bindingContext.ModelType.IsArray)
                {
                    return array;
                }
                object list = this.CreateModel(controllerContext, bindingContext, modelType);
                this.Copy(modelType.GetGenericArguments()[0], list, array);
                return list;
            }
            return valueProviderResult.ConvertTo(modelType);
        }

        private void SetModelState(ModelBindingContext bindingContext, ValueProviderResult valueProviderResult)
        {
            ModelState modelState;
            if (!bindingContext.ModelState.TryGetValue(bindingContext.ModelName, out modelState))
            {
                bindingContext.ModelState.Add(bindingContext.ModelName, modelState = new ModelState());
            }
            modelState.Value = valueProviderResult;
        }
        #endregion

        #region BindComplexModel
        private object BindComplexModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Type modelType = bindingContext.ModelType;

            //绑定字典
            if (this.Match(modelType, typeof(IDictionary<,>)))
            {
                return this.BindDictionary(controllerContext, bindingContext);
            }

            //绑定集合
            if (this.Match(modelType, typeof(IEnumerable<>)))
            {
                return this.BindCollection(controllerContext, bindingContext);
            }

            object model = this.CreateModel(controllerContext, bindingContext, modelType);
            bindingContext.ModelMetadata.Model = model;
            //针对每个描述属性的PropertyDescriptor对象调用BindProperty方法对相应属性赋值
            ICustomTypeDescriptor modelTypeDescriptor = new AssociatedMetadataTypeTypeDescriptionProvider(modelType).GetTypeDescriptor(modelType);
            PropertyDescriptorCollection propertyDescriptors = modelTypeDescriptor.GetProperties();
            foreach (PropertyDescriptor propertyDescriptor in propertyDescriptors)
            {
                this.BindProperty(controllerContext, bindingContext, propertyDescriptor);
            }
            return model;
        }

        private void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            //将属性名附加到现有的前缀上
            string prefix = (bindingContext.ModelName ?? "") + "." + (propertyDescriptor.Name ?? "");
            prefix = prefix.Trim('.');

            //针对属性创建绑定上下文
            ModelMetadata metadata = bindingContext.PropertyMetadata[propertyDescriptor.Name];
            ModelBindingContext context = new ModelBindingContext
            {
                ModelName = prefix,
                ModelMetadata = metadata,
                ModelState = bindingContext.ModelState,
                ValueProvider = bindingContext.ValueProvider
            };
            //针对属性实施Model绑定并对属性赋值
            object propertyValue = ModelBinders.Binders.GetBinder(propertyDescriptor.PropertyType).BindModel(controllerContext, context);
            propertyDescriptor.SetValue(bindingContext.Model, propertyValue);
        }

        private object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (modelType.IsGenericType)
            {
                Type genericTypeDefinition = modelType.GetGenericTypeDefinition();

                if (typeof(IDictionary<,>) == genericTypeDefinition)
                {
                    return Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(modelType.GetGenericArguments()));
                }

                if (new Type[] { typeof(IEnumerable<>), typeof(ICollection<>), typeof(IList<>) }.Any(type => type == genericTypeDefinition))
                {
                    return Activator.CreateInstance(typeof(List<>).MakeGenericType(modelType.GetGenericArguments()));
                }
            }
            return Activator.CreateInstance(modelType);
        }

        private bool Match(Type type, Type typeToMatch)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeToMatch)
            {
                return true;
            }
            foreach (Type interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeToMatch)
                {
                    return true;
                }
            }
            return false;
        }
        
        private object BindCollection(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Type modelType = bindingContext.ModelType;
            Type elementType = modelType.IsArray ? modelType.GetElementType() : modelType.GetGenericArguments()[0];

            //创建一个空集合对象并作为当前绑定上下文的Model
            Type collectionType = modelType.IsArray ? typeof(List<>).MakeGenericType(elementType) : modelType;
            object model = this.CreateModel(controllerContext, bindingContext, collectionType);
            bindingContext.ModelMetadata.Model = model;

            //针对每个索引实施Model绑定，并将绑定生成的元素添加到集合之中
            bool isZeroBased;
            IEnumerable<string> indexes = this.GetIndexes(controllerContext, bindingContext, out isZeroBased);
            List<object> elements = new List<object>();
            foreach (string index in indexes)
            {
                string prefix = string.Format("{0}[{1}]", bindingContext.ModelName, index);
                if (!bindingContext.ValueProvider.ContainsPrefix(prefix))
                {
                    //零基索引要求索引数值必须连续
                    if (isZeroBased)
                    {
                        break;
                    }
                    continue;
                }
                ModelBindingContext context = new ModelBindingContext
                {
                    ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, elementType),
                    ModelName = prefix,
                    ModelState = bindingContext.ModelState,
                    PropertyFilter = bindingContext.PropertyFilter,
                    ValueProvider = bindingContext.ValueProvider
                };
                object element = ModelBinders.Binders.GetBinder(elementType).BindModel(controllerContext, context);
                elements.Add(element);
            }
            this.Copy(elementType, bindingContext.Model, elements);

            //如果绑定的目标类型为数组，创建一个空数组对象并向其拷贝绑定生成的集合元素
            if (modelType.IsArray)
            {
                IList list = model as IList;
                if (null == list)
                {
                    return null;
                }
                Array array = Array.CreateInstance(elementType, list.Count);
                list.CopyTo(array, 0);
                return array;
            }

            //直接返回集合对象
            return model;
        }
        private object BindDictionary(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Type modelType = bindingContext.ModelType;
            Type keyType = modelType.GetGenericArguments()[0];
            Type valueType = modelType.GetGenericArguments()[1];
            object model = this.CreateModel(controllerContext, bindingContext, modelType);

            List<KeyValuePair<object, object>> list = new List<KeyValuePair<object, object>>();
            bool isZeroBased;
            IEnumerable<string> indexes = this.GetIndexes(controllerContext, bindingContext, out isZeroBased);
            foreach (string index in indexes)
            {
                string prefix = string.Format("{0}[{1}]", bindingContext.ModelName, index);
                if (!bindingContext.ValueProvider.ContainsPrefix(prefix))
                {
                    if (isZeroBased)
                    {
                        break;
                    }
                    continue;
                }
                ModelBindingContext contextForKey = new ModelBindingContext
                {
                    ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, keyType),
                    ModelName = prefix + ".key",
                    ModelState = bindingContext.ModelState,
                    PropertyFilter = bindingContext.PropertyFilter,
                    ValueProvider = bindingContext.ValueProvider
                };
                ModelBindingContext contextForValue = new ModelBindingContext
                {
                    ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, valueType),
                    ModelName = prefix + ".value",
                    ModelState = bindingContext.ModelState,
                    PropertyFilter = bindingContext.PropertyFilter,
                    ValueProvider = bindingContext.ValueProvider
                };
                object key = ModelBinders.Binders.GetBinder(keyType).BindModel(controllerContext, contextForKey);
                object value = ModelBinders.Binders.GetBinder(valueType).BindModel(controllerContext, contextForValue);
                list.Add(new KeyValuePair<object, object>(key, value));
            }
            this.Copy(keyType, valueType, model, list);
            return model;
        }

        #region Copy
        private static void CopyCollection<T>(ICollection<T> destination, IEnumerable source)
        {
            foreach (object item in source)
            {
                destination.Add(item is T ? (T)item : default(T));
            }
        }

        private void Copy(Type elementType, object destination, object source)
        {
            MethodInfo copyMethod = typeof(MyDefaultModelBinder).GetMethod("CopyCollection", BindingFlags.Static | BindingFlags.NonPublic);
            copyMethod.MakeGenericMethod(elementType).Invoke(null, new object[] { destination, source });
        }

        private static void CopyDictionary<TKey, TValue>(IDictionary<TKey, TValue> destination, IEnumerable<KeyValuePair<object, object>> source)
        {
            foreach (KeyValuePair<object, object> item in source)
            {
                if (item.Key is TKey)
                {
                    destination.Add((TKey)item.Key, item.Value is TValue ? (TValue)item.Value : default(TValue));
                }
            }
        }

        private void Copy(Type keyType, Type valueType, object destination, List<KeyValuePair<object, object>> source)
        {
            MethodInfo copyMethod = typeof(MyDefaultModelBinder).GetMethod("CopyDictionary", BindingFlags.Static | BindingFlags.NonPublic);
            copyMethod.MakeGenericMethod(keyType, valueType).Invoke(null, new object[] { destination, source });
        }
        #endregion

        #region GetIndexes
        private IEnumerable<string> GetIndexes(ControllerContext controllerContext, ModelBindingContext bindingContext, out bool isZeroBased)
        {
            string key = (bindingContext.ModelName ?? "") + ".index";
            key = key.Trim('.');
            ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(key);
            if (null != valueProviderResult)
            {
                isZeroBased = false;
                return (string[])valueProviderResult.ConvertTo(typeof(string[]));
            }
            isZeroBased = true;
            return GetZeroBasedIndexes();
        }

        private static IEnumerable<string> GetZeroBasedIndexes()
        {
            int index = 0;
            while (true)
            {
                yield return (index++).ToString();
            }
        }
        #endregion
        #endregion
    }
}