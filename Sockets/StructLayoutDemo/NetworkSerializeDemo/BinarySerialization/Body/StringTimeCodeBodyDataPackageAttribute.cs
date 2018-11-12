using System;
using System.Text;

namespace NetworkSerializeDemo.Body
{
    /// <summary>
    /// 针对StringTimeCode时间类型
    /// </summary>
    public sealed class StringTimeCodeBodyDataPackageAttribute : BodyDataPackageAttribute
    {

        /// <summary>
        /// 分隔符集合
        /// </summary>
        private readonly char[] _separator;

        public StringTimeCodeBodyDataPackageAttribute(int byteLength, int sortLocation)
            : this(byteLength, sortLocation, new[] { '-', ' ', ':' })
        {

        }

        public StringTimeCodeBodyDataPackageAttribute(int byteLength, int sortLocation, char[] separator)
            : base(byteLength, sortLocation)
        {
            _separator = separator;
        }

        public override void ConvertObjectToByteArray<T>(T obj)
        {
            if (obj == null)
            {
                for (int i = 0; i < DataSize; i++)
                {
                    DataPackage[i] = 0;
                }
            }
            else
            {
                var nums = Convert.ToString(obj).Split(_separator);
                for (var i = 0; i < DataSize; i++)
                {
                    if (i == 0)
                    {
                        var year = Convert.ToString(Convert.ToInt32(nums[i]), 16).PadLeft(4, '0');
                        var yearpart1 = year.Substring(0, 2);
                        var yearpart2 = year.Substring(2, 2);

                        var yearpart1Hex = Byte.Parse(yearpart1, System.Globalization.NumberStyles.AllowHexSpecifier);
                        var yearpart2Hex = Byte.Parse(yearpart2, System.Globalization.NumberStyles.AllowHexSpecifier);

                        DataPackage[i] = yearpart1Hex;
                        DataPackage[i + 1] = yearpart2Hex;
                        i++;
                        continue;
                    }
                    if (nums.Length > i - 1)
                    {
                        DataPackage[i] = Convert.ToByte(nums[i - 1]);
                    }
                }
            }
        }

        public override T ConvertByteArrayToObject<T, TK>(byte[] bytes, TK obj)
        {
            var str = new StringBuilder();
            for (var i = 0; i < DataSize; i++)
            {
                if (i == 0)
                {
                    var year = Convert.ToString(bytes[0], 16) + Convert.ToString(bytes[1], 16);
                    str.Append(Convert.ToInt32(year, 16));
                    str.Append(_separator[0]);
                    i++;
                    continue;
                }

                str.Append(bytes[i].ToString("D2"));
                switch (i)
                {
                    case 2:
                        str.Append(_separator[0]);
                        break;
                    case 3:
                        str.Append(_separator[1]);
                        break;
                    case 6:
                    case 5:
                    case 4:
                        str.Append(_separator[2]);
                        break;
                }
            }
            return (T)(object)str.ToString();
        }

    }
}
