using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Shadowsocks.Controller
{
    public interface IService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstPacket"></param>
        /// <param name="length"></param>
        /// <param name="socket">client socket</param>
        /// <param name="state"></param>
        /// <returns></returns>
        bool Handle(byte[] firstPacket, int length, Socket socket, object state);
    }
}
