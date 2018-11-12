using System;

namespace NetworkSerializeDemo.Body
{

    /// <summary>
    /// 针对byte数据类型
    /// </summary>
    public sealed class ByteBodyDataPackageAttribute : BodyDataPackageAttribute
    {

        public ByteBodyDataPackageAttribute(int byteLength, int sortLocation)
            : base(byteLength, sortLocation)
        {
        }

        public override void ConvertObjectToByteArray<T>(T obj)
        {
            DataPackage[0] = Convert.ToByte(obj);
        }

        public override T ConvertByteArrayToObject<T, TK>(byte[] bytes, TK obj)
        {
            return (T)(object)bytes[0];
        }

    }
}
