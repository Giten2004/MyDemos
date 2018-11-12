using System;

namespace NetworkSerializeDemo.Body
{

    /// <summary>
    /// 数据包主体
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class BodyDataPackageAttribute : Attribute, IBodyDataPackageAttribute
    {

        #region Fields

        /// <summary>
        /// 数据包
        /// </summary>
        private byte[] _dataPackage;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSize">字节长度</param>
        /// <param name="sortLocation">排序位置</param>
        protected BodyDataPackageAttribute(int dataSize, int sortLocation)
        {
            CalcSize(dataSize);
            SortLocation = sortLocation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sizePropertyName">字节长度属性名字</param>
        /// <param name="sortLocation">排序位置</param>
        protected BodyDataPackageAttribute(string sizePropertyName, int sortLocation)
        {
            SizePropertyName = sizePropertyName;
            SortLocation = sortLocation;
            IsCalcDataSize = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSize">字节长度</param>
        private void CalcSize(int dataSize)
        {
            DataSize = dataSize;
            _dataPackage = new byte[DataSize];
        }

        /// <summary>
        /// 根据属性名字动态计算长度
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public void CalcDataSize<T>(T obj)
        {
            var sizeProperty = obj.GetType().GetProperty(SizePropertyName);
            if (sizeProperty == null)
                throw new ArgumentException("'SizePropertyName' can not find.");
            CalcSize(Convert.ToInt32(sizeProperty.GetValue(obj, null)));
            IsCalcDataSize = false;
        }

        /// <summary>
        /// 执行实体类转数组的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">转换的数据</param>
        public virtual void ConvertObjectToByteArray<T>(T obj)
        {

        }

        /// <summary>
        /// 执行数组转实体类的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <param name="bytes"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual T ConvertByteArrayToObject<T, TK>(byte[] bytes, TK obj)
        {
            return default(T);
        }

        /// <summary>
        /// 执行数组转实体类的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TP"></typeparam>
        /// <param name="bytes"></param>
        /// <param name="obj"></param>
        /// <param name="parentObj"></param>
        /// <returns></returns>
        public T ConvertByteArrayToObject<T, TK, TP>(byte[] bytes, TK obj, TP parentObj)
        {
            return default(T);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 字节长度属性名字
        /// </summary>
        private string SizePropertyName
        {
            set;
            get;
        }

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
            private set;
            get;
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
