/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * DateTime		:  2008-10-22 12:34:52
 * Description	:  Program 的摘要说明
 *
 * ***********************************************/


using System;
using System.Collections.Generic;
using System.Text;
using Kingthy.Test.Socks5.Server.Core;

namespace Kingthy.Test.Socks5.Server
{
    /// <summary>
    /// http://www.cnblogs.com/kingthy/archive/2008/10/22/1317132.html
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            TCPSocks5Server server = new TCPSocks5Server(8832, "test","test");
            server.LogWatcher = Console.Out;
            server.Start();

            Console.WriteLine("如果要停止服务,请按回车键!!");
            Console.ReadLine();
            server.Stop();
        }
    }
}
