using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NetworkSerializeDemo.Body
{
    /// <summary>
    /// 针对byte数组数据类型
    /// </summary>
    public class BytesBodyDataPackageAttribute : BodyDataPackageAttribute
    {

        public BytesBodyDataPackageAttribute(int byteLength, int sortLocation)
            : base(byteLength, sortLocation)
        {
        }

        public BytesBodyDataPackageAttribute(string sizePropertyName, int sortLocation)
            : base(sizePropertyName, sortLocation)
        {

        }

        public override void ConvertObjectToByteArray<T>(T obj)
        {
            var bytes = obj.ToBytes();
            for (int i = 0; i < DataSize; i++)
            {
                DataPackage[i] = bytes[i];
            }
        }

        public override T ConvertByteArrayToObject<T, TK>(byte[] bytes, TK obj)
        {
            return (T)(object)bytes;
        }
    }
}
