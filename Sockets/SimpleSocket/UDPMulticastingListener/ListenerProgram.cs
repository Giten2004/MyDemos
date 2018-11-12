using System;
using System.Collections.Generic;

using System.Net;
using System.Net.Sockets;
using System.Text;
using UDPMulticasting;

namespace UDPMulticastingListener
{
    class ListenerProgram
    {
        static void Main(string[] args)
        {
            //UdpClientListener();

            //this does not on local subnet work
            //RawSocketListener("234.123.12.1", "2222");

            //RawSocketListener(AddressDefine.MulticastIP, AddressDefine.MulticastPort);

            //works on local subnet work
            RawSocketListener("224.0.0.251", "2222");
        }

        public static void UdpClientListener()
        {
            UdpClient client = new UdpClient();

            client.ExclusiveAddressUse = false;
            IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 2222);

            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            client.ExclusiveAddressUse = false;

            client.Client.Bind(localEp);

            IPAddress multicastaddress = IPAddress.Parse("239.0.0.222");
            client.JoinMulticastGroup(multicastaddress);

            Console.WriteLine("Listening this will never quit so you will need to ctrl-c it");

            while (true)
            {
                Byte[] data = client.Receive(ref localEp);
                string strData = Encoding.Unicode.GetString(data);
                Console.WriteLine(strData);
            }
        }

        public static void RawSocketListener(string mcastGroup, string port)
        {
            Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, int.Parse(port));
            udpSocket.Bind(ipep);

            //add socket to the multicast group
            IPAddress multicastIPAddr = IPAddress.Parse(mcastGroup);
            udpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(multicastIPAddr, IPAddress.Any));

            Console.WriteLine("Listening this will never quit so you will need to ctrl-c it");

            while (true)
            {
                byte[] data = new byte[1024];

                var receivedLength = udpSocket.Receive(data);

                string strData = Encoding.Unicode.GetString(data, 0, receivedLength);
                Console.WriteLine(strData);
            }
            //s.Close();
        }
    }
}
