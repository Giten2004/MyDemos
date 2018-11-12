using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumConvertDemo
{
    public static class EnumUtil<T> where T : struct
    {
        public static T Parse(int enumValue)
        {
            var isDefined = IsDefined(enumValue);
            if (!isDefined)
                throw new ArgumentException(string.Format("{0} is not a defined value for enum type {1}", enumValue, typeof(T).FullName));

            return (T)Enum.ToObject(typeof(T), enumValue);
        }

        private static bool IsDefined(object enumValue)
        {
            return Enum.IsDefined(typeof(T), enumValue);
        }

    }
}
