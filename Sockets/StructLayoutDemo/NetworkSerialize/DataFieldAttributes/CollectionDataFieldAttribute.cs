using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetworkSerialize
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CollectionDataFieldAttribute : BaseDataFieldAttribute
    {
        public override uint DataFieldIndex { get; }
        public override uint DataFieldSize { get; }
        public override Type DataFieldType { get; }

        public override byte[] ConvertToBytes<T>(T dataFieldValue)
        {
            var itemBytesList = new List<byte[]>();
            var allBytesSize = 0;
            if (dataFieldValue is ICollection)
            {
                var collection = dataFieldValue as ICollection;
                foreach (var item in collection)
                {
                    var itemBytes = new byte[] { };
                    if (item is string)
                    {
                        itemBytes = Encoding.ASCII.GetBytes(item.ToString());
                    }
                    else
                    {
                        itemBytes = DataPackageConvert.ToDataPackage(item);
                    }
                    allBytesSize += itemBytes.Length;
                    itemBytesList.Add(itemBytes);
                }
            }

            var allBytes = new byte[allBytesSize];
            var bytesIndex = 0;
            foreach (var itemBytes in itemBytesList)
            {
                Array.Copy(itemBytes, 0, allBytes, bytesIndex, itemBytes.Length);
               
                bytesIndex += itemBytes.Length;
            }

            return allBytes;
        }

        public override object ConvertToObject(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public override object ConvertToObject(byte[] bytes, object parentObj)
        {
            var collectionSizePropertyInfo = parentObj.GetType().GetProperty(CollectionSizePropertyName);
            var collectionPropertyInfo = parentObj.GetType().GetProperty(CollectionPropertyName);

            var collectionSizeString = collectionSizePropertyInfo.GetValue(parentObj, null).ToString();
            var collectionPropertyValue = collectionPropertyInfo.GetValue(parentObj, null);

            var collectionSize = int.Parse(collectionSizeString);
            if (collectionSize * SingleDataSize != bytes.Length)
                throw new ArgumentNullException();

            for (int i = 0; i < collectionSize; i++)
            {
                var elementBytes = new byte[SingleDataSize];
                for (int j = 0; j < SingleDataSize; j++)
                {
                    elementBytes[i] = bytes[i * SingleDataSize + j];
                }

                var elementInstance = DataPackageConvert.ToObject(elementBytes, CollectionElementType);

                DataFieldType.InvokeMember("Add", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, collectionPropertyValue, new[] { elementInstance });
            }

            return collectionPropertyValue;
        }

        public override bool IsCollection { get; }
        public override uint SingleDataSize { get; }
        public override string CollectionSizePropertyName { get; }
        public override string CollectionPropertyName { get; }
        public override Type CollectionElementType { get; }
        public override bool IsComplexObject { get; }


        public CollectionDataFieldAttribute(uint dataFieldIndex, uint singleDataSize, string collectionSizePropertyName, string collectionPropertyName, Type dataFieldType, Type collectionElemenType)
        {
            DataFieldIndex = dataFieldIndex;
            SingleDataSize = singleDataSize;
            DataFieldType = dataFieldType;
            CollectionSizePropertyName = collectionSizePropertyName;

            CollectionPropertyName = collectionPropertyName;
            CollectionElementType = collectionElemenType;

            IsCollection = true;
            IsComplexObject = false;
        }
    }
}
