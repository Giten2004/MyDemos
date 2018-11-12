using System;

namespace NetworkSerializeDemo.Header
{
    /// <summary>
    /// 头数据包
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class HeaderDataPackageAttribute : Attribute
    {

        #region Fields

        /// <summary>
        /// 数据包
        /// </summary>
        private byte[] _dataPackage;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerName">数据包标识符</param>
        /// <param name="version">版本</param>
        /// <param name="counter">计数</param>
        /// <param name="headerLength">数据包头长度</param>
        public HeaderDataPackageAttribute(string headerName, short version = (short) 0, short counter = (short) 0, int headerLength = 10)
        {
            HeaderName = headerName;
            Version = version;
            Counter = counter;
            HeaderLength = headerLength;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 数据包头信息转换Byte数组
        /// </summary>
        /// <param name="dataAllSize">数据包总长度</param>
        /// <param name="dataBytes">数据包数据信息</param>
        public void Operation(int dataAllSize, byte[] dataBytes)
        {
            _dataPackage = new byte[dataAllSize];
            var size = BitConverter.GetBytes(dataAllSize);
            var version = BitConverter.GetBytes(Version);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(size);
            }
            _dataPackage[0] = size[0];
            _dataPackage[1] = size[1];
            _dataPackage[2] = size[2];
            _dataPackage[3] = size[3];
            var chars = HeaderName.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                _dataPackage[4 + i] = Convert.ToByte(chars[i]);
            }
            _dataPackage[8] = version[0];
            _dataPackage[9] = Convert.ToByte(0);
            dataBytes.CopyTo(_dataPackage, HeaderLength);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 数据包标识符
        /// </summary>
        public string HeaderName
        {
            private set;
            get;
        }

        /// <summary>
        /// 版本
        /// </summary>
        public short Version
        {
            private set;
            get;
        }

        /// <summary>
        /// 计数
        /// </summary>
        public short Counter
        {
            private set;
            get;
        }

        /// <summary>
        /// 数据包
        /// </summary>
        public byte[] DataPackage
        {
            get { return _dataPackage; }
        }

        /// <summary>
        /// 数据包头长度
        /// </summary>
        public int HeaderLength
        {
            private set;
            get;
        }

        #endregion

    }
}
