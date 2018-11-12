
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkSerializeDemo.Body
{
    /// <summary>
    /// 接口的作用解释
    /// </summary>
    public interface IMoveBodyDataPackageAttribute
    {
        /// <summary>
        /// 执行实体类转数组的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">转换的数据</param>
        void ConvertObjectToByteArray<T>(T obj);

        /// <summary>
        /// 执行数组转实体类的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <param name="bytes"></param>
        /// <param name="obj">属性</param>
        /// <returns></returns>
        T ConvertByteArrayToObject<T, TK>(byte[] bytes, TK obj);
    }
}
