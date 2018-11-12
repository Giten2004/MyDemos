using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace PackingAndUnPacking
{
    /// <summary>
    /// http://www.cnblogs.com/jiangj/archive/2010/08/18/1802357.html
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //socket一些初始化
                int port = 7000;
                string host = "127.0.0.1";
                IPAddress ip = IPAddress.Parse(host);
                IPEndPoint ipe = new IPEndPoint(ip, port);//把ip和端口转化为IPEndPoint实例
                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个Socket
                clientSocket.Connect(ipe);

                //初始化协议包
                PACKETHEADER packet = new PACKETHEADER();
                packet.HEAD = new byte[2];
                packet.HEAD = System.Text.Encoding.Default.GetBytes("[[");

                packet.LENGTH = new byte[10];
                packet.LENGTH = System.Text.Encoding.Default.GetBytes("         0");
                packet.ISZIP = new byte[1];
                packet.ISZIP = System.Text.Encoding.Default.GetBytes("0");
                packet.PACKTYPE = new byte[1];
                packet.PACKTYPE = System.Text.Encoding.Default.GetBytes("0");
                packet.SERVICE = new byte[4];
                packet.SERVICE = System.Text.Encoding.Default.GetBytes("1007");
                packet.PARAMENT = new byte[100];
                string strtemp = "MAC=11-11-11-11-11-11&PHONE=1234567&PASSWORD=111111&USERTYPE=1&";
                int nlen = 100 - strtemp.Length;
                char[] szblank = new char[nlen];
                for (int i = 0; i < nlen; i++)
                {
                    strtemp += " ";
                }
                //MessageBox.Show(szblank.ToString());
                //strtemp = strtemp + str2;
                int nLen = strtemp.Length;
                packet.PARAMENT = System.Text.Encoding.Default.GetBytes(strtemp);

                packet.TAIL = new byte[2];
                packet.TAIL = System.Text.Encoding.Default.GetBytes("]]");

                //将协议包转换成byte数组

                int size = Marshal.SizeOf(packet);

                byte[] packetparams = new byte[size];
                //分配结构体大小的内存空间
                IntPtr structPtr = Marshal.AllocHGlobal(size);
                //将结构体拷到分配好的内存空间
                Marshal.StructureToPtr(packet, structPtr, true);
                //从内存空间拷到byte数组
                Marshal.Copy(structPtr, packetparams, 0, size);
                //释放内存空间
                Marshal.FreeHGlobal(structPtr);
                //返回byte数组


                int nRetSize = clientSocket.Send(packetparams, packetparams.Length, 0);//发送测试信息
                string recvStr = "";
                byte[] recvBytes = new byte[1024];
                int bytes;
                bytes = clientSocket.Receive(recvBytes, recvBytes.Length, 0);//从服务器端接受返回信息

                PACKETHEADER packet2 = new PACKETHEADER();
                Object objstruct = BytesToStruct(recvBytes, packet.GetType());
                packet2 = (PACKETHEADER)objstruct;
                string strmonth = Encoding.Default.GetString(packet2.PARAMENT, 0, 100);

                recvStr = Encoding.Default.GetString(recvBytes, 0, bytes);

                Console.WriteLine(recvStr);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /**//// <summary>
            /// byte数组转结构体
            /// </summary>
            /// <param name="bytes">byte数组</param>
            /// <param name="type">结构体类型</param>
            /// <returns>转换后的结构体</returns>
        public static object BytesToStruct(byte[] bytes, Type type)
        {
            try
            {
                //得到结构体的大小
                int size = Marshal.SizeOf(type);
                //byte数组长度小于结构体的大小
                if (size > bytes.Length)
                {
                    //返回空
                    return null;
                }
                //分配结构体大小的内存空间
                IntPtr structPtr = Marshal.AllocHGlobal(size);
                //将byte数组拷到分配好的内存空间
                Marshal.Copy(bytes, 0, structPtr, size);
                //将内存空间转换为目标结构体
                object obj = Marshal.PtrToStructure(structPtr, type);
                //释放内存空间
                Marshal.FreeHGlobal(structPtr);
                //返回结构体
                return obj;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
