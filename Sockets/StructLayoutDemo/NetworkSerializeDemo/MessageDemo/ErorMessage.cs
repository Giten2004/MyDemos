using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkSerializeDemo.Body;
using NetworkSerializeDemo.Header;

namespace NetworkSerializeDemo.MessageDemo
{
    [HeaderDataPackage("EROR")]
    public sealed class ErorMessage : BaseMessage
    {
        /// <summary>
        /// </summary>
        [Uint32BodyDataPackage(4, 1)]
        public uint ServerId { get; set; }

        /// <summary>
        /// signature
        /// </summary>
        [StringBodyDataPackage(4, 2)]
        public string Tag { get; set; }

        /// <summary>
        /// error code
        /// </summary>
        [Uint32BodyDataPackage(4, 3)]
        public uint ErrorCode { get; set; }

        /// <summary>
        /// error message
        /// </summary>
        [StringBodyDataPackage(200, 4)]
        public string ErrorMessage { get; set; }

        internal static bool CheckMessageSize(byte[] message)
        {
            return CheckTheMessageSize(212, message.Length, 0, 0);
        }

        /// <summary>
        /// 获取日志记录
        /// </summary>
        /// <returns></returns>
        public override string ToLogString()
        {
            return string.Format("[EROR] ServerId:{0}, Tag:{1}, ErrorCode:{2}, ErrorMessage:{3}", ServerId, Tag, ErrorCode, ErrorMessage);
        }
    }
}
