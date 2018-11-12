using System;


namespace NetworkSerializeDemo.Body
{

    /// <summary>
    /// 针对Uint16数据类型
    /// </summary>
    public sealed class Uint16BodyDataPackageAttribute : BodyDataPackageAttribute
    {

        public Uint16BodyDataPackageAttribute(int dataSize, int sortLocation)
            : base(dataSize, sortLocation)
        {
        }

        public override void ConvertObjectToByteArray<T>(T obj)
        {
            var bytes = BitConverter.GetBytes(Convert.ToUInt16(obj));
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
            return (T)(object)BitConverter.ToUInt16(bytes, 0);
        }

    }
}
