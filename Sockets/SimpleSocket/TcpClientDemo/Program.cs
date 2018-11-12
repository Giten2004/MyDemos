using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpClientDemo
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            byte[] data = new byte[1024];
            Socket newclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.Write("please input the server ip:");
            string ipadd = Console.ReadLine();

            Console.WriteLine();
            Console.Write("please input the server port:");
            int port = Convert.ToInt32(Console.ReadLine());
            IPEndPoint ie = new IPEndPoint(IPAddress.Parse(ipadd), port);//服务器的IP和端口

            try
            {
                //因为客户端只是用来向特定的服务器发送信息，所以不需要绑定本机的IP和端口。不需要监听。
                newclient.Connect(ie);
            }
            catch (SocketException e)
            {
                Console.WriteLine("unable to connect to server");
                Console.WriteLine(e.ToString());
                return;
            }

            int recv = newclient.Receive(data);
            string stringdata = Encoding.ASCII.GetString(data, 0, recv);
            Console.WriteLine("Received server message:{0}", stringdata);

            while (true)
            {
                string input = Console.ReadLine();
                if (input == "exit")
                    break;

                newclient.Send(Encoding.ASCII.GetBytes(input));
                data = new byte[1024];
                recv = newclient.Receive(data);

                stringdata = Encoding.ASCII.GetString(data, 0, recv);
                Console.WriteLine("Received server message:{0}", stringdata);
            }

            Console.WriteLine("disconnect from sercer");

            newclient.Shutdown(SocketShutdown.Both);
            newclient.Close();

        }
    }
}
