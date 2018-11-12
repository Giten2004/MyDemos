using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkSerialize
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AlphaStringDataFieldAttribute : BaseDataFieldAttribute
    {
        public override uint DataFieldIndex { get; }
        public override uint DataFieldSize { get; }
        public override Type DataFieldType { get; }

        public override byte[] ConvertToBytes<T>(T dataFieldValue)
        {
            var dataFieldString = dataFieldValue as string;

            string str;
            if (string.IsNullOrEmpty(dataFieldString))
                str = "\0";
            else
            {
                str = dataFieldString;
            }

            var strarray = Encoding.ASCII.GetBytes(str);
            if (strarray.Length > DataFieldSize)
                throw new ArgumentOutOfRangeException(string.Format("DataFieldSize is out of {0}", DataFieldSize));

            var packageBytes = new byte[DataFieldSize];

            for (var i = 0; i < strarray.Length; i++)
            {
                if (strarray.Length > i)
                {
                    packageBytes[i] = strarray[i];
                }
            }

            return packageBytes;
        }

        public override object ConvertToObject(byte[] bytes)
        {
            if (bytes.Length != DataFieldSize)
                throw new ArgumentException(string.Format("Bytes.length is not equals {0}", DataFieldSize));

            string str = Encoding.ASCII.GetString(bytes);
            if (!string.IsNullOrEmpty(str))
            {
                var indexOf = str.IndexOf("\0", StringComparison.Ordinal);
                if (indexOf != -1)
                {
                    str = str.Substring(0, indexOf);
                }
            }

            return str;
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


        public AlphaStringDataFieldAttribute(uint dataFieldIndex, uint dataFieldSize, Type dataFiledType)
        {
            DataFieldIndex = dataFieldIndex;
            DataFieldSize = dataFieldSize;
            DataFieldType = dataFiledType;

            IsComplexObject = false;
            IsCollection = false;
        }
    }
}
