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
    public class EnumMoveBodyDataPackageAttribute : MoveBodyDataPackageAttribute
    {

        public EnumMoveBodyDataPackageAttribute(int moveLocation, int position) :
            base(moveLocation, position)
        {

        }

        /// <summary>
        /// 进行移位转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public override void ConvertObjectToByteArray<T>(T obj)
        {
            MoveValue = Convert.ToInt32(obj) << MoveLocation;
        }

        /// <summary>
        /// 进行移位转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <param name="bytes"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override T ConvertByteArrayToObject<T, TK>(byte[] bytes, TK obj)
        {
            var sourceObj = base.ConvertByteArrayToObject<T, TK>(bytes, obj);
            var enumValue = Convert.ToInt32(sourceObj);
            var propertyInfo = obj as PropertyInfo;
            if (propertyInfo != null && Enum.IsDefined(propertyInfo.PropertyType, enumValue))
                return (T)Enum.Parse(propertyInfo.PropertyType, enumValue.ToString(CultureInfo.InvariantCulture));
            return default(T);
        }
        
    }
}
