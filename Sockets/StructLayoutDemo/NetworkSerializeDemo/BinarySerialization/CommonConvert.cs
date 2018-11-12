
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using NetworkSerializeDemo.Body;


namespace NetworkSerializeDemo
{


    /// <summary>
    /// 公共转换类
    /// </summary>
    internal static class CommonConvert
    {

        /// <summary>
        /// 根据obj类进行特性解析并返回数据包字节数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static byte[] GetObjData<T>(T obj)
        {
            //数据包数据长度
            var dataSize = 0;
            var bodys = new List<IBodyDataPackageAttribute>();
            ReadChildDataPackage(obj.GetType().GetProperties(), obj, ref bodys, ref dataSize);

            //拼装数组数据包信息
            var data = GetDataBody(dataSize, bodys);
            return data;
        }

        /// <summary>
        /// 解析每个属性上标有的数组转换特性
        /// </summary>
        /// <param name="propertyInfos"></param>
        /// <param name="obj"></param>
        /// <param name="bodys"></param>
        /// <param name="dataSize"></param>
        private static void ReadChildDataPackage<T>(IEnumerable<PropertyInfo> propertyInfos, T obj, ref List<IBodyDataPackageAttribute> bodys, ref int dataSize)
        {
            foreach (var propertyInfo in propertyInfos)
            {
                var bodyDataPackages = propertyInfo.GetCustomAttributes(typeof(IBodyDataPackageAttribute), false);
                var objValue = propertyInfo.GetValue(obj, null);

                if (objValue != null)
                {
                    if (bodyDataPackages.Length == 0 && objValue.GetType().IsClass && !(objValue is string))
                    {
                        ReadChildDataPackage(objValue.GetType().GetProperties(), objValue, ref bodys, ref dataSize);
                    }
                }
                if (bodyDataPackages.Length > 0)
                {
                    var bodyData = bodyDataPackages[0] as IBodyDataPackageAttribute;
                    if (bodyData != null)
                    {
                        if (bodyData.IsCalcDataSize)
                            bodyData.CalcDataSize(obj);
                        bodyData.ConvertObjectToByteArray(objValue);
                        dataSize += bodyData.DataSize;
                        bodys.Add(bodyData);
                    }
                }
            }
        }

        /// <summary>
        /// 获取数据包数据信息
        /// </summary>
        /// <param name="dataSize">数据包长度</param>
        /// <param name="bodys">特性集合</param>
        /// <returns></returns>
        private static byte[] GetDataBody(int dataSize, List<IBodyDataPackageAttribute> bodys)
        {
            //组装数据包数据信息
            var data = new byte[dataSize];
            //数组组合起始位置
            var startLocation = 0;
            if (bodys.Count > 0)
            {
                var sl = bodys.OrderBy(s => s.SortLocation);
                foreach (var body in sl)
                {
                    body.DataPackage.CopyTo(data, startLocation);
                    startLocation += body.DataSize;
                }
            }
            return data;
        }

        /// <summary>
        /// 根据obj
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objValue"></param>
        /// <param name="bytes"></param>
        internal static void SetObjData<T>(T objValue, byte[] bytes)
        {
            var list = new List<Tuple<PropertyInfo, IBodyDataPackageAttribute, object>>();
            //先把所有特性获取出来
            ReadChildProperty(objValue.GetType().GetProperties(), objValue, ref list);
            //为每个属性赋值
            SetPropertysValue(list, bytes);
        }

        /// <summary>
        /// 递归读取所有特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyInfos"></param>
        /// <param name="obj"></param>
        /// <param name="list"></param>
        internal static void ReadChildProperty<T>(IEnumerable<PropertyInfo> propertyInfos, T obj, ref List<Tuple<PropertyInfo, IBodyDataPackageAttribute, object>> list)
        {
            foreach (var propertyInfo in propertyInfos)
            {
                var bodyDatas = propertyInfo.GetCustomAttributes(typeof(IBodyDataPackageAttribute), false);
                if (bodyDatas.Length > 0)
                {
                    var bodyData = bodyDatas[0] as IBodyDataPackageAttribute;
                    if (bodyData != null)
                    {
                        list.Add(Tuple.Create(propertyInfo, bodyData, (object)obj));
                    }
                }
                else if (propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType != typeof(string))
                {
                    var objValue = propertyInfo.PropertyType.ExtCreateInstance<object>();
                    propertyInfo.SetValue(obj, objValue, null);
                    if (objValue != null)
                    {
                        ReadChildProperty(objValue.GetType().GetProperties(), objValue, ref list);
                    }
                }
            }
        }

        /// <summary>
        /// 设置集合属性的值
        /// </summary>
        /// <param name="list"></param>
        /// <param name="bytes"></param>
        private static void SetPropertysValue(IEnumerable<Tuple<PropertyInfo, IBodyDataPackageAttribute, object>> list, byte[] bytes)
        {
            var startLocation = 0;
            //根据位置排序
            var sl = list.OrderBy(s => s.Item2.SortLocation);
            foreach (var keyValuePair in sl)
            {
                byte[] newBytes;
                //当前属性不包含特性集合
                if (!keyValuePair.Item2.IsList)
                {
                    if (keyValuePair.Item2.IsCalcDataSize)
                        keyValuePair.Item2.CalcDataSize(keyValuePair.Item3);
                    newBytes = new byte[keyValuePair.Item2.DataSize];
                    Array.Copy(bytes, startLocation, newBytes, 0, newBytes.Length);
                    startLocation += keyValuePair.Item2.DataSize;
                    keyValuePair.Item1.SetValue(keyValuePair.Item3, keyValuePair.Item2.ConvertByteArrayToObject<object, PropertyInfo>(newBytes, keyValuePair.Item1), null);
                    //keyValuePair.Item3.ExtSetPropertyValue(keyValuePair.Item2.ConvertByteArrayToObject<object, PropertyInfo>(newBytes, keyValuePair.Item1), keyValuePair.Item1);
                }
                //当前属性包含特性集合
                else
                {
                    newBytes = new byte[bytes.Length - startLocation];
                    Array.Copy(bytes, startLocation, newBytes, 0, newBytes.Length);
                    keyValuePair.Item1.SetValue(keyValuePair.Item3, keyValuePair.Item2.ConvertByteArrayToObject<object, object, object>(newBytes, keyValuePair.Item1.GetType().Assembly.CreateInstance(keyValuePair.Item1.PropertyType.FullName), keyValuePair.Item3), null);
                    //keyValuePair.Item3.ExtSetPropertyValue(keyValuePair.Item2.ConvertByteArrayToObject<object, object, object>(newBytes, keyValuePair.Item1.PropertyType.ExtCreateInstance<object>(), keyValuePair.Item3), keyValuePair.Item1);
                    //如果是集合特性，由于数据包大小需要进行动态计算，所以stratLocation需要在计算完后进行叠加。
                    startLocation += keyValuePair.Item2.DataSize;
                }
            }
        }

    }

}
