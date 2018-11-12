using System;


namespace NetworkSerializeDemo.Body
{

    /// <summary>
    /// 针对Uint32数据类型
    /// </summary>
    public sealed class Uint32BodyDataPackageAttribute : BodyDataPackageAttribute
    {

        public Uint32BodyDataPackageAttribute(int byteLength, int sortLocation)
            : base(byteLength, sortLocation)
        {
        }

        public override void ConvertObjectToByteArray<T>(T obj)
        {
            var bytes = BitConverter.GetBytes(Convert.ToUInt32(obj));
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            for (var i = 0; i < DataSize; i++)
            {
                DataPackage[i] = bytes[i];
            }
        }

        public override T ConvertByteArrayToObject<T, TK>(byte[] bytes, TK obj)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return (T)(object)BitConverter.ToUInt32(bytes, 0);
        }

    }
}
