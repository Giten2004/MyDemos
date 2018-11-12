using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] data = new byte[1024];//定义一个数组用来做数据的缓冲区
            string input;
            string stringData;

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);

            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            string welcome = "Hello,are you there?";
            data = Encoding.ASCII.GetBytes(welcome);
            server.SendTo(data, data.Length, SocketFlags.None, ipep);//将数据发送到指定的终结点

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);

            EndPoint remote = sender;
            data = new byte[1024];
            int recv = server.ReceiveFrom(data, ref remote);//接受来自服务器的数据

            Console.WriteLine("Message received from{0}:", remote.ToString());
            Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));

            while (true)//读取数据
            {
                input = Console.ReadLine();//从键盘读取数据
                if (input == "text")//结束标记
                {
                    break;
                }

                server.SendTo(Encoding.ASCII.GetBytes(input), remote);//将数据发送到指定的终结点Remote

                data = new byte[1024];
                recv = server.ReceiveFrom(data, ref remote);//从Remote接受数据

                stringData = Encoding.ASCII.GetString(data, 0, recv);
                Console.WriteLine(stringData);
            }
            Console.WriteLine("Stopping client");
            server.Close();
        }
    }
}
