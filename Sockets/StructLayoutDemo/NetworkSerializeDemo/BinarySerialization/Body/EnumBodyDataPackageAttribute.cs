
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;


namespace NetworkSerializeDemo.Body
{
    /// <summary>
    /// 针对枚举类型
    /// </summary>
    public class EnumBodyDataPackageAttribute : BodyDataPackageAttribute
    {
        public EnumBodyDataPackageAttribute(int dataSize, int sortLocation) :
            base(dataSize, sortLocation)
        {

        }

        public EnumBodyDataPackageAttribute(string sizePropertyName, int sortLocation) :
            base(sizePropertyName, sortLocation)
        {

        }

        public override void ConvertObjectToByteArray<T>(T obj)
        {
            var bytes = BitConverter.GetBytes(Convert.ToInt32(obj));
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
                int startIndex = 0;
                for (var i = bytes.Length - 1; i >= 0; i--)
                {
                    DataPackage[startIndex] = bytes[i];
                    startIndex++;
                    if (startIndex >= DataSize)
                        break;
                }
            }
            else
            {
                for (var i = 0; i < DataSize; i++)
                {
                    DataPackage[i] = bytes[i];
                }
            }
        }

        public override T ConvertByteArrayToObject<T, TK>(byte[] bytes, TK obj)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            var enumValue = bytes.ConvertInt32();
            var propertyInfo = obj as PropertyInfo;
            if (propertyInfo != null && Enum.IsDefined(propertyInfo.PropertyType, enumValue))
                return (T)Enum.Parse(propertyInfo.PropertyType, enumValue.ToString(CultureInfo.InvariantCulture));
            return default(T);
        }

    }
}
