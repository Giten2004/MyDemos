using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetworkSerialize
{
    public static class DataPackageConvert
    {
        public static byte[] ToDataPackage<T>(T obj)
        {
            var type = obj.GetType();
            var dataPackageAttributeArray = type.GetCustomAttributes(typeof(DataPackageAttribute), false);
            if (dataPackageAttributeArray.Length <= 0)
            {
                throw new ArgumentException("DataPackageAttribute is apply to the target object Type");
            }

            var dataFieldList = new List<Tuple<IDataField, byte[]>>();

            var allPropertyInfo = obj.GetType().GetProperties();
            var allDataBytesSize = 0;
            foreach (PropertyInfo propertyInfo in allPropertyInfo)
            {
                var dataFieldAttribute = propertyInfo.GetCustomAttributes(typeof(IDataField), false).FirstOrDefault();
                if (dataFieldAttribute == null)
                    continue;

                var dataFieldValue = propertyInfo.GetValue(obj, null);
                var dataFieldAttributeInstance = (IDataField)dataFieldAttribute;

                var dataFieldBytes = dataFieldAttributeInstance.ConvertToBytes(dataFieldValue);
                allDataBytesSize += dataFieldBytes.Length;
                dataFieldList.Add(new Tuple<IDataField, byte[]>(dataFieldAttributeInstance, dataFieldBytes));
            }

            var dataPackageBytes = new byte[allDataBytesSize];
            var orderedDataFieldList = dataFieldList.OrderBy(x => x.Item1.DataFieldIndex);
            var index = 0;
            long dataFieldBytesIndex = 0;
            foreach (var tuple in orderedDataFieldList)
            {
                //Check the continuity
                if (tuple.Item1.DataFieldIndex != index)
                    throw new ArgumentException();

                Array.Copy(tuple.Item2, 0, dataPackageBytes, dataFieldBytesIndex, tuple.Item2.Length);

                dataFieldBytesIndex += tuple.Item2.Length;

                index++;
            }

            return dataPackageBytes;
        }

        public static object ToObject(byte[] bytes, Type targetType)
        {
            var newInstance = Activator.CreateInstance(targetType);

            var dataPackageAttribute = targetType.GetCustomAttributes(typeof(DataPackageAttribute), false).FirstOrDefault();
            if (dataPackageAttribute == null)
                throw new ArgumentException();

            var dataFieldList = new List<Tuple<PropertyInfo, IDataField>>();
            var propertyInfos = targetType.GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var dataFieldAttribute = propertyInfo.GetCustomAttributes(typeof(IDataField), false).FirstOrDefault();
                if (dataFieldAttribute == null)
                    continue;

                dataFieldList.Add(new Tuple<PropertyInfo, IDataField>(propertyInfo, (IDataField)dataFieldAttribute));
            }

            var orderedDataFieldList = dataFieldList.OrderBy(x => x.Item2.DataFieldIndex);
            var dataFieldIndex = 0;
            long dataFieldBytesIndex = 0;
            foreach (var tuple in orderedDataFieldList)
            {
                if (dataFieldIndex != tuple.Item2.DataFieldIndex)
                    throw new ArgumentException();

                //[[
                var baseDataField = (BaseDataFieldAttribute)tuple.Item2;
                if (baseDataField.IsCollection)
                {
                    var collectionDataField = (ICollectionDataField)tuple.Item2;
                    var collectionSizePropertyInfo = targetType.GetProperty(collectionDataField.CollectionSizePropertyName);
                    var collectionPropertyInfo = targetType.GetProperty(collectionDataField.CollectionPropertyName);

                    var collectionSizeString = collectionSizePropertyInfo.GetValue(newInstance, null).ToString();
                    var collectionPropertyValue = collectionPropertyInfo.GetValue(newInstance, null);
                    var collectionSize = uint.Parse(collectionSizeString);
                    var collectionBytesLength = collectionSize * collectionDataField.SingleDataSize;

                    byte[] dataFieldBytes = GetBytes(bytes, dataFieldBytesIndex, collectionBytesLength);

                    var dataFieldValue = collectionDataField.ConvertToObject(dataFieldBytes, newInstance);

                    tuple.Item1.SetValue(newInstance, dataFieldValue, null);

                    dataFieldBytesIndex += collectionBytesLength;

                }
                else if (baseDataField.IsComplexObject)
                {
                    byte[] dataFieldBytes = GetBytes(bytes, dataFieldBytesIndex, tuple.Item2.DataFieldSize);

                    var dataFieldValue = tuple.Item2.ConvertToObject(dataFieldBytes);
                    tuple.Item1.SetValue(newInstance, dataFieldValue, null);

                    dataFieldBytesIndex += tuple.Item2.DataFieldSize;
                }
                else
                {
                    byte[] dataFieldBytes = GetBytes(bytes, dataFieldBytesIndex, tuple.Item2.DataFieldSize);

                    var dataFieldValue = tuple.Item2.ConvertToObject(dataFieldBytes);

                    tuple.Item1.SetValue(newInstance, dataFieldValue, null);

                    dataFieldBytesIndex += tuple.Item2.DataFieldSize;
                }
                //]]

                dataFieldIndex++;
            }

            return newInstance;
        }

        private static byte[] GetBytes(byte[] bytes, long startIndex, uint dataFieldSize)
        {
            var dataFieldBytes = new byte[dataFieldSize];
            Array.Copy(bytes, startIndex, dataFieldBytes, 0, dataFieldSize);

            return dataFieldBytes;
        }
    }
}
