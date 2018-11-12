using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkSerializeDemo.MessageDemo
{
    public abstract class BaseMessage
    {
        protected const string Br = "\r\n                           ";

        /// <summary>
        /// check message size
        /// </summary>
        /// <param name="headersize"></param>
        /// <param name="messagelength"></param>
        /// <param name="datasize"></param>
        /// <param name="datacount"></param>
        /// <returns></returns>
        protected static bool CheckTheMessageSize(short headersize, int messagelength, int datasize, int datacount)
        {
            return messagelength == ((headersize + 10) + (datasize * datacount));
        }

        /// <summary>
        /// 获取日志记录
        /// </summary>
        /// <returns></returns>
        public abstract string ToLogString();

        protected static string RemoveLastBr(string s)
        {
            var index = s.LastIndexOf(Br, StringComparison.Ordinal);
            if (index != -1)
            {
                s = s.Substring(0, index);
            }
            return s;
        }
    }
}
