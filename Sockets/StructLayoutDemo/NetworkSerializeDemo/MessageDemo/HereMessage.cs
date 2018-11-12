using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkSerializeDemo.Header;

namespace NetworkSerializeDemo.MessageDemo
{
    [HeaderDataPackage("HERE")]
    public sealed class HereMessage : BaseMessage
    {
        /// <summary>
        /// 获取日志记录
        /// </summary>
        /// <returns></returns>
        public override string ToLogString()
        {
            return string.Empty;
        }
    }
}
