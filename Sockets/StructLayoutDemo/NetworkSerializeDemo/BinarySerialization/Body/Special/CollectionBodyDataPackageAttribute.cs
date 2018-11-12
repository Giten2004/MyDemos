using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetworkSerializeDemo.Body
{


    /// <summary>
    /// 针对集合数据
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CollectionBodyDataPackageAttribute : Attribute, IBodyDataPackageAttribute
    {

        #region Fields

        /// <summary>
        /// 数据包
        /// </summary>
        private byte[] _dataPackage;

        /// <summary>
        /// 内联属性名字（用于动态计算集合包大小）
        /// </summary>
        private readonly string _innerPropertyName0;

        private readonly string _innerPropertyName1;

        /// <summary>
        /// 单个数据包长度
        /// </summary>
        private readonly int _singleDataSize;

        #endregion

        #region Constructors

        public CollectionBodyDataPackageAttribute(int singleDataSize, int sortLocation, string innerPropertyName0, string innerPropertyName1)
        {
            SortLocation = sortLocation;
            _singleDataSize = singleDataSize;
            _innerPropertyName0 = innerPropertyName0;
            _innerPropertyName1 = innerPropertyName1;
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
            if (obj is ICollection)
            {
                var collection = obj as ICollection;
                var dataAll = new byte[0];
                foreach (var coll in collection)
                {
                    //获取当前实体类转换成的字节数组
                    var data = CommonConvert.GetObjData(coll);
                    //创建新的数组进行合并并赋值给变量保存
                    var bytes = new byte[data.Length + dataAll.Length];
                    dataAll.CopyTo(bytes, 0);
                    data.CopyTo(bytes, dataAll.Length);
                    dataAll = bytes;
                }
                DataSize = dataAll.Length;
                _dataPackage = dataAll;
            }
        }

        public T ConvertByteArrayToObject<T, TK>(byte[] bytes, TK obj)
        {
            return default(T);
        }

        public T ConvertByteArrayToObject<T, TK, TP>(byte[] bytes, TK obj, TP parentObj)
        {
            //获取数据包总大小
            DataSize = parentObj.ExtGetPropertyValueByName<TP, short>(_innerPropertyName0) * _singleDataSize;
            if (!string.IsNullOrEmpty(_innerPropertyName1))
                DataSize += parentObj.ExtGetPropertyValueByName<TP, short>(_innerPropertyName1) * _singleDataSize;
            
            //截取需要的字节数组
            var allBytes = new byte[DataSize];
            Array.Copy(bytes, 0, allBytes, 0, DataSize);

            //获取集合里面的参数类型（默认都只有一种参数类型）
            var type = obj.GetType();
            var argument = type.GetGenericArguments()[0];

            //获取单个数据包的总个数
            var numPackages = DataSize / _singleDataSize;
            for (int i = 0; i < numPackages; i++)
            {
                //创建集合里面的对应参数类型实例
                var instance = argument.ExtCreateInstance<object>();
                if (instance != null)
                {
                    var startLocation = i * _singleDataSize;

                    var bodys = new List<Tuple<PropertyInfo, IBodyDataPackageAttribute, object>>();
                    //读取当前类包含的所有特性集合
                    CommonConvert.ReadChildProperty(instance.GetType().GetProperties(), instance, ref bodys);
                    //排序
                    var sl = bodys.OrderBy(s => s.Item2.SortLocation);
                    //对每个属性赋值
                    foreach (var packageAttribute in sl)
                    {
                        var newBytes = new byte[packageAttribute.Item2.DataSize];
                        Array.Copy(allBytes, startLocation, newBytes, 0, newBytes.Length);
                        startLocation += packageAttribute.Item2.DataSize;
                        packageAttribute.Item1.SetValue(instance, packageAttribute.Item2.ConvertByteArrayToObject<object, PropertyInfo>(newBytes, packageAttribute.Item1), null);
                        //instance.ExtSetPropertyValue(packageAttribute.Item2.ConvertByteArrayToObject<object, PropertyInfo>(newBytes, packageAttribute.Item1), packageAttribute.Item1);
                    }
                    //像集合中添加一条数据
                    type.InvokeMember("Add", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, obj, new[] { instance });
                }
            }
            return (T)(object)obj;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 排序位置
        /// </summary>
        public int SortLocation
        {
            private set;
            get;
        }

        /// <summary>
        /// 是否是集合
        /// </summary>
        public bool IsList
        {
            get { return true; }
        }

        /// <summary>
        /// 是否动态计算包长度
        /// </summary>
        public bool IsCalcDataSize
        {
            get { return false; }
        }

        /// <summary>
        /// 数据包
        /// </summary>
        public byte[] DataPackage
        {
            get { return _dataPackage; }
        }

        /// <summary>
        /// 数据包总长度
        /// </summary>
        public int DataSize
        {
            private set;
            get;
        }

        #endregion

    }
}
