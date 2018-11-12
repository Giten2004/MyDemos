using System;
using NetworkSerializeDemo.Header;

namespace NetworkSerializeDemo
{

    /// <summary>
    /// 数据包转换
    /// </summary>
    public static class DataPackageConvert
    {

        /// <summary>
        /// 把实体类转换成数据包
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ToDataPackage<T>(this T obj)
        {
            var dataPackage = new byte[] { };

            var type = obj.GetType();
            var headers = type.GetCustomAttributes(typeof(HeaderDataPackageAttribute), false);
            if (headers.Length > 0)
            {
                var header = headers[0];
                if (header is HeaderDataPackageAttribute)
                {
                    var data = CommonConvert.GetObjData(obj);

                    #region 数据包头+数据包数据=总数据包

                    //拼装数据包(头长度+数据长度)并转换成数组
                    var headerDataPackage = header as HeaderDataPackageAttribute;
                    //数据包总长度（递增）
                    var dataAllSize = headerDataPackage.HeaderLength + data.Length;
                    //执行字节数组转换和拼装
                    headerDataPackage.Operation(dataAllSize, data);

                    #endregion

                    //赋值
                    dataPackage = headerDataPackage.DataPackage;
                }
            }
            return dataPackage;
        }

        /// <summary>
        /// 把数据包转换成实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T ToObjectPackage<T>(this byte[] bytes) where T : class
        {
            var type = typeof(T);
            var objValue = type.ExtCreateInstance<object>();

            var headers = type.GetCustomAttributes(typeof(HeaderDataPackageAttribute), false);
            if (headers.Length > 0)
            {
                var header = headers[0] as HeaderDataPackageAttribute;
                if (header != null)
                {
                    var byteDatas = new byte[bytes.Length - header.HeaderLength];
                    Array.Copy(bytes, header.HeaderLength, byteDatas, 0, byteDatas.Length);
                    CommonConvert.SetObjData(objValue, byteDatas);
                }
            }
            return (T)objValue;
        }

        /// <summary>
        /// 把数据包转换成实体类 （return object）
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToObjectPackage(this byte[] bytes, Type type)
        {
            var objValue = type.ExtCreateInstance<object>();
            var headers = type.GetCustomAttributes(typeof(HeaderDataPackageAttribute), false);
            if (headers.Length > 0)
            {
                var header = headers[0] as HeaderDataPackageAttribute;
                if (header != null)
                {
                    var byteDatas = new byte[bytes.Length - header.HeaderLength];
                    Array.Copy(bytes, header.HeaderLength, byteDatas, 0, byteDatas.Length);
                    CommonConvert.SetObjData(objValue, byteDatas);
                }
            }
            return objValue;
        }
    }
}
