using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkSerialize
{
    public abstract class BaseDataFieldAttribute : Attribute, IDataField, ICollectionDataField, IObjectDataField
    {
        public abstract uint DataFieldIndex { get; }
        public abstract uint DataFieldSize { get; }
        public abstract Type DataFieldType { get; }
        public abstract byte[] ConvertToBytes<T>(T dataFieldValue);

        public abstract object ConvertToObject(byte[] bytes);
        public abstract object ConvertToObject(byte[] bytes, object parentObj);

        #region Implement Properties and methods of interface ICollectionDataField
        public abstract bool IsCollection { get; }
        public abstract uint SingleDataSize { get; }

        public abstract string CollectionSizePropertyName { get; }
        public abstract string CollectionPropertyName { get; }
        public abstract Type CollectionElementType { get; }

        #endregion

        #region Implement Property of interface IObjectDataField
        public abstract bool IsComplexObject { get; }
        #endregion
    }
}
