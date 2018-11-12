using System;

namespace NetworkSerializeDemo.Body
{

    /// <summary>
    /// 针对需要移位的数据
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MoveBodyDataPackageAttribute : Attribute, IMoveBodyDataPackageAttribute
    {

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moveLocation">移位</param>
        /// <param name="position">Position</param>
        public MoveBodyDataPackageAttribute(int moveLocation, int position = 0)
        {
            MoveLocation = moveLocation;
            Position = position;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 进行移位转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public virtual void ConvertObjectToByteArray<T>(T obj)
        {
            if (obj is bool)
            {
                MoveValue = (Convert.ToBoolean(obj)
                                 ? 1
                                 : 0) << MoveLocation;
            }
            else if (obj is int)
            {
                MoveValue = Convert.ToInt32(obj) << MoveLocation;
            }
            else if (obj is UInt16)
            {
                MoveValue = Convert.ToUInt16(obj) << MoveLocation;
            }
            else if (obj is short)
            {
                MoveValue = Convert.ToInt16(obj) << MoveLocation;
            }
        }

        /// <summary>
        /// 进行移位转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <param name="bytes"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual T ConvertByteArrayToObject<T, TK>(byte[] bytes, TK obj)
        {
            return (T)(object)((bytes[0] & Position) >> MoveLocation);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 移位
        /// </summary>
        public int MoveLocation
        {
            private set;
            get;
        }

        /// <summary>
        /// 与运算符位置
        /// </summary>
        public int Position
        {
            private set;
            get;
        }

        /// <summary>
        /// 移位过后的值
        /// </summary>
        public int MoveValue
        {
            protected set;
            get;
        }

        #endregion

    }
}
