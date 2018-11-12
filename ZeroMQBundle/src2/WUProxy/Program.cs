using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using CommandLine;
using WUProxy;
using ZeroMQ;
using ZeroMQ.lib;

namespace Examples
{
    static partial class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            var parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));

            if (!parser.ParseArguments(args, options))
            {
                Console.ReadLine();
                throw new ArgumentException();
            }
            //
            // Weather proxy device
            //
            // Author: metadings
            //

            using (var context = new ZContext())
            using (var frontend = new ZSocket(context, ZSocketType.XSUB))
            using (var backend = new ZSocket(context, ZSocketType.XPUB))
            {
                // Frontend is where the weather server sits
                foreach (var frontendBindEndPoint in options.FrontendBindEndPoints)
                {
                    var ipPortProtocal = frontendBindEndPoint.Split(':');
                    var ip = ipPortProtocal[0];
                    var port = ipPortProtocal[1];
                    var protocal = ipPortProtocal[2];

                    switch (protocal.ToLower())
                    {
                        case "pgm":
                            var pgmAddress = string.Format("pgm://;239.192.1.1:{0}", port);
                            Console.WriteLine("I: Connecting to FrontEndPoint {0}...", pgmAddress);
                            frontend.Connect(pgmAddress);
                            break;
                        case "epgm":
                            var epgmAddress = string.Format("epgm://;239.192.1.1:{0}", port);
                            Console.WriteLine("I: Connecting to FrontEndPoint {0}...", epgmAddress);
                            frontend.Connect(epgmAddress);
                            break;
                        default:
                            {
                                string frontHost = string.Format("{0}://{1}:{2}", protocal, ip, port);
                                Console.WriteLine("I: Connecting to FrontEndPoint: {0}", frontHost);
                                frontend.Connect(frontHost);
                            }
                            break;
                    }
                }


                // Backend is our public endpoint for subscribers
                //Proxy 还可以装换协议，把前边的unicast转换为 multicast
                foreach (var backendBindEndPoint in options.BackendBindEndPoints)
                {
                    var ipPortProtocal = backendBindEndPoint.Split(':');
                    var ip = ipPortProtocal[0];
                    var port = ipPortProtocal[1];
                    var protocal = ipPortProtocal[2];

                    var tcpAddress = string.Format("tcp://{0}:{1}", ip, port);
                    var pgmAddress = string.Format("pgm://{0};239.192.1.1:{1}", ip, port);

                    var epgmAddress = string.Format("epgm://{0};239.192.1.1:{1}", ip, port);


                    switch (protocal.ToLower())
                    {
                        case "pgm":
                            Console.WriteLine("I: Binding on {0}", epgmAddress);
                            backend.Bind(epgmAddress);
                            break;
                        case "epgm":
                            Console.WriteLine("I: Binding on {0}", epgmAddress);
                            backend.Bind(epgmAddress);
                            break;
                        case "tcp":
                            {
                                Console.WriteLine("I: Binding on {0}", tcpAddress);
                                backend.Bind(tcpAddress);
                            }
                            break;
                        default:
                            {
                                Console.WriteLine("I: Binding on {0}", epgmAddress);
                                backend.Bind(epgmAddress);

                                Console.WriteLine("I: Binding on {0}", tcpAddress);
                                backend.Bind(tcpAddress);
                                break;
                            }
                    }
                }

                using (var newFrame = ZFrame.Create(1))
                {
                    newFrame.Write(new byte[] { 0x1 }, 0, 1);

                    backend.Send(newFrame);
                }

                //here, it's more like a bridge
                // Run the proxy until the user interrupts us
                ZContext.Proxy(frontend, backend);
            }
        }

        static IEnumerable<IPAddress> WUProxy_GetPublicIPs()
        {
            var list = new List<IPAddress>();
            NetworkInterface[] ifaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface iface in ifaces)
            {
                if (iface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    continue;
                if (iface.OperationalStatus != OperationalStatus.Up)
                    continue;

                var props = iface.GetIPProperties();
                var addresses = props.UnicastAddresses;
                foreach (UnicastIPAddressInformation address in addresses)
                {
                    if (address.Address.AddressFamily == AddressFamily.InterNetwork)
                        list.Add(address.Address);
                    // if (address.Address.AddressFamily == AddressFamily.InterNetworkV6)
                    //	list.Add(address.Address);
                }
            }
            return list;
        }
    }
}