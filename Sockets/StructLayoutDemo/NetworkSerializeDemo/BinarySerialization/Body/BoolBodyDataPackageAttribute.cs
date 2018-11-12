using System;

namespace NetworkSerializeDemo.Body
{

    /// <summary>
    /// 针对Bool数据类型
    /// </summary>
    public sealed class BoolBodyDataPackageAttribute : BodyDataPackageAttribute
    {

        public BoolBodyDataPackageAttribute(int dataSize, int sortLocation)
            : base(dataSize, sortLocation)
        {
        }

        public override void ConvertObjectToByteArray<T>(T obj)
        {
            var b = Convert.ToByte(obj);
            DataPackage[0] = b;
        }

        public override T ConvertByteArrayToObject<T, TK>(byte[] bytes, TK obj)
        {
            return (T)(object)Convert.ToBoolean(bytes[0]);
        }

    }
}
