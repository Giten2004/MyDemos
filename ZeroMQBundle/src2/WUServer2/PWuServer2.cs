using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace WUServer2
{
    static partial class Program
    {
        public static void Main(string[] args)
        {
            //
            // Weather update server
            // Binds PUB socket to tcp://*:5556
            // Publishes random weather updates
            //
            // Author: metadings
            //

            // Prepare our context and publisher
            using (var context = new ZContext())
            using (var publisher = new ZSocket(context, ZSocketType.PUB))
            {
                var address = args[0];
                var port = args[1];
                var protocal = args[2];

                var pgmAddress = string.Format("pgm://;239.192.1.1:{0}", port);
                var epgmAddress = string.Format("epgm://;239.192.1.1:{0}", port);
                string unicastAddress = string.Format("tcp://{0}:{1}", address, port);

                switch (protocal.ToLower())
                {
                    case "pgm":
                        Console.WriteLine("I: Publisher.Bind'ing on {0}...", pgmAddress);
                        publisher.Bind(pgmAddress);
                        break;
                    case "epgm":
                        Console.WriteLine("I: Publisher.Bind'ing on {0}...", epgmAddress);
                        publisher.Bind(epgmAddress);
                        break;
                    case "tcp":
                        {
                            Console.WriteLine("I: Publisher.Bind'ing on {0}", unicastAddress);
                            publisher.Bind(unicastAddress);
                        }
                        break;
                    default:
                        {
                            Console.WriteLine("I: Publisher.Bind'ing on {0}...", pgmAddress);
                            publisher.Bind(pgmAddress);
                            Console.WriteLine("I: Publisher.Bind'ing on {0}", unicastAddress);
                            publisher.Bind(unicastAddress);
                        }
                        break;
                }

                // Initialize random number generator
                var rnd = new Random();

                while (true)
                {
                    // Get values that will fool the boss
                    int zipcode = rnd.Next(610040, 610050);
                    int temperature = rnd.Next(-55, +45);

                    // Send message to all subscribers
                    // Method 1:
                    //var update = string.Format("{0:D5} {1}", zipcode, temperature);
                    //using (var updateFrame = new ZFrame(update))
                    //{
                    //    publisher.Send(updateFrame);
                    //}

                    // Method 2:
                    using (var zMsg = new ZMessage())
                    {
                        zMsg.Add(new ZFrame(string.Format("{0:D5}", zipcode)));
                        zMsg.Add(new ZFrame(temperature));

                        publisher.Send(zMsg);
                    }

                    Thread.Sleep(500);
                }
            }
        }
    }
}
