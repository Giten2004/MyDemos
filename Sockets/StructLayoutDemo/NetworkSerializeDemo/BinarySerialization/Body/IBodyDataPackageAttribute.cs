
namespace NetworkSerializeDemo.Body
{
    /// <summary>
    /// 定义数据包主体
    /// </summary>
    public interface IBodyDataPackageAttribute
    {

        /// <summary>
        /// 排序位置
        /// </summary>
        int SortLocation { get; }

        /// <summary>
        /// 数据包
        /// </summary>
        byte[] DataPackage { get; }

        /// <summary>
        /// 数据包长度
        /// </summary>
        int DataSize { get; }

        /// <summary>
        /// 是否是集合
        /// </summary>
        bool IsList { get; }

        /// <summary>
        /// 是否动态计算包长度
        /// </summary>
        bool IsCalcDataSize { get; }

        /// <summary>
        /// 根据属性名字动态计算长度
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        void CalcDataSize<T>(T obj);

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

        /// <summary>
        /// 执行数组转实体类的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TP"></typeparam>
        /// <param name="bytes"></param>
        /// <param name="obj">属性</param>
        /// <param name="parentObj">属性所在的类</param>
        /// <returns></returns>
        T ConvertByteArrayToObject<T, TK, TP>(byte[] bytes, TK obj, TP parentObj);

    }
}
