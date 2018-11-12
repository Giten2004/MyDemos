using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UDPMulticasting;

namespace UDPMulticastingSender
{
    class SenderProgram
    {
        static void Main(string[] args)
        {
            //this does not on local subnet work
            //RawSocketSender("234.123.12.1", "2222", "1", "2");

            //RawSocketSender(AddressDefine.MulticastIP, AddressDefine.MulticastPort, AddressDefine.TTL.ToString(), "2");

            //works on local subnet work
            RawSocketSender("224.0.0.251", "2222", "1", "2");

            //UdpClientSender();

            Console.WriteLine("All Done! Press ENTER to quit.");
            Console.ReadLine();
        }

        public static void UdpClientSender()
        {
            UdpClient udpclient = new UdpClient();

            IPAddress multicastaddress = IPAddress.Parse("239.0.0.222");
            udpclient.JoinMulticastGroup(multicastaddress);

            IPEndPoint remoteep = new IPEndPoint(multicastaddress, 2222);

            Byte[] buffer = null;

            Console.WriteLine("Press ENTER to start sending messages");
            Console.ReadLine();

            for (int i = 0; i <= 8000; i++)
            {
                buffer = Encoding.Unicode.GetBytes(i.ToString());
                udpclient.Send(buffer, buffer.Length, remoteep);
                Console.WriteLine("Sent " + i);
            }
        }

        public static void RawSocketSender(string mcastGroup, string port, string ttl, string rep)
        {
            try
            {
                Console.WriteLine("MCAST Send on Group: {0} Port: {1} TTL: {2}", mcastGroup, port, ttl);

                IPAddress multicastIPAddress = IPAddress.Parse(mcastGroup);

                Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                udpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(multicastIPAddress));
                // <summary>
                // This sets the time to live for the socket - this is very important in defining scope for the multicast data. 
                // Setting a value of 1 will mean the multicast data will not leave the local network, 
                // setting it to anything above this will allow the multicast data to pass through several routers, 
                // with each router decrementing the TTL by 1. 
                // Getting the TTL value right is important for bandwidth considerations.
                // </summary>
                udpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, int.Parse(ttl));

                IPEndPoint ipep = new IPEndPoint(multicastIPAddress, int.Parse(port));

                Console.WriteLine("Connecting...");

                udpSocket.Connect(ipep);

                Byte[] buffer = null;

                Console.WriteLine("Press ENTER to start sending messages");
                Console.ReadLine();

                for (int i = 0; i <= 8000; i++)
                {
                    buffer = Encoding.Unicode.GetBytes(i.ToString());                    
                    udpSocket.Send(buffer, buffer.Length, SocketFlags.None);

                    Console.WriteLine("Sent " + i);
                }

                Console.WriteLine("Closing Connection...");
                udpSocket.Close();
            }
            catch (System.Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }
    }
}
