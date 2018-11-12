using System;
using System.Collections.Generic;
using System.Reflection;

namespace NetworkSerializeDemo.Body
{
    /// <summary>
    /// 多个字节合并规则
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public sealed class PlusBodyDataPackageAttribute : Attribute, IBodyDataPackageAttribute
    {

        #region Fields

        /// <summary>
        /// 数据包
        /// </summary>
        private readonly byte[] _dataPackage;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSize">数据包大小</param>
        /// <param name="sortLocation">排序位置</param>
        public PlusBodyDataPackageAttribute(int dataSize, int sortLocation)
        {
            DataSize = dataSize;
            SortLocation = sortLocation;
            _dataPackage = new byte[DataSize];
        }

        #endregion

        #region Methods

        /// <summary>
        /// 根据属性名字动态计算长度
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public void CalcDataSize<T>(T obj)
        {
        }

        public void ConvertObjectToByteArray<T>(T obj)
        {
            var lists = new List<int>();
            foreach (var propertyInfo in obj.GetType().GetProperties())
            {
                var attributes = propertyInfo.GetCustomAttributes(typeof(MoveBodyDataPackageAttribute), false);
                if (attributes.Length > 0)
                {
                    var moveBody = attributes[0] as MoveBodyDataPackageAttribute;
                    if (moveBody != null)
                    {
                        moveBody.ConvertObjectToByteArray(propertyInfo.GetValue(obj, null));
                        lists.Add(moveBody.MoveValue);
                    }
                }
            }
            var result = 0;
            if (lists.Count >= 1)
            {
                result = lists[0];
            }
            for (var i = 1; i <= lists.Count - 1; i++)
            {
                result = result.ExtOr(lists[i]);
            }
            var b = BitConverter.GetBytes(result);
            DataPackage[0] = b[0];
        }

        public T ConvertByteArrayToObject<T, TK>(byte[] bytes, TK obj)
        {
            //var type = obj.GetType();
            var instance = (obj as PropertyInfo).PropertyType.ExtCreateInstance<T>();
            if (instance != null)
            {
                var properties = instance.GetType().GetProperties();
                foreach (var propertyInfo in properties)
                {
                    var attributes = propertyInfo.GetCustomAttributes(typeof(IMoveBodyDataPackageAttribute), false);
                    if (attributes.Length > 0)
                    {
                        var moveBody = attributes[0] as IMoveBodyDataPackageAttribute;
                        if (moveBody != null)
                        {
                            var value = moveBody.ConvertByteArrayToObject<object, PropertyInfo>(bytes, propertyInfo);

                            object propertyvalue;
                            //if (value == null && propertyInfo.PropertyType.BaseType.FullName == "System.Enum")
                            //    propertyvalue = propertyInfo.PropertyType.Assembly.CreateInstance(propertyInfo.PropertyType.FullName);
                            //else
                                propertyvalue = Convert.ChangeType(value, propertyInfo.PropertyType);

                            propertyInfo.SetValue(instance, propertyvalue, null);
                            //instance.ExtSetPropertyValue(propertyvalue, propertyInfo);
                        }
                    }
                }
            }
            return instance;
        }

        public T ConvertByteArrayToObject<T, TK, TP>(byte[] bytes, TK obj, TP parentObj)
        {
            return default(T);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 是否是集合
        /// </summary>
        public bool IsList
        {
            get { return false; }
        }

        /// <summary>
        /// 是否动态计算包长度
        /// </summary>
        public bool IsCalcDataSize
        {
            get { return false; }
        }

        /// <summary>
        /// 字节长度
        /// </summary>
        public int DataSize
        {
            private set;
            get;
        }

        /// <summary>
        /// 排序位置
        /// </summary>
        public int SortLocation
        {
            private set;
            get;
        }

        /// <summary>
        /// 数据包
        /// </summary>
        public byte[] DataPackage
        {
            get { return _dataPackage; }
        }

        #endregion

    }

}
