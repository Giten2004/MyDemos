﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
            return valueProviderResult.ConvertTo(bindingContext.ModelType);
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
            //针对目标类型创建一个空Model对象
            Type modelType = bindingContext.ModelType;
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
            return Activator.CreateInstance(modelType);
        }
        #endregion 
    }
}