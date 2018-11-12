using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkSerialize
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ObjectDataFieldAttribute: BaseDataFieldAttribute
    {
        public override uint DataFieldIndex { get; }
        public override uint DataFieldSize { get; }
        public override Type DataFieldType { get; }

        public override byte[] ConvertToBytes<T>(T dataFieldValue)
        {
            return DataPackageConvert.ToDataPackage(dataFieldValue);
        }

        public override object ConvertToObject(byte[] bytes)
        {
            return DataPackageConvert.ToObject(bytes, DataFieldType);
         
        }

        public override object ConvertToObject(byte[] bytes, object parentObj)
        {
            throw new NotImplementedException();
        }

        public override bool IsCollection { get; }
        public override uint SingleDataSize { get; }
        public override string CollectionSizePropertyName { get; }
        public override string CollectionPropertyName { get; }
        public override Type CollectionElementType { get; }
        public override bool IsComplexObject { get; }

        public ObjectDataFieldAttribute(uint dataFieldIndex, uint dataFieldSize, Type dataFieldType)
        {
            DataFieldIndex = dataFieldIndex;
            DataFieldSize = dataFieldSize;
            DataFieldType = dataFieldType;

            IsCollection = false;
            IsComplexObject = true;
        }
    }
}
