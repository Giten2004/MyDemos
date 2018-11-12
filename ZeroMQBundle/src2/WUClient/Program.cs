using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ZeroMQ;

namespace Examples
{
    static partial class Program
    {
        public static void Main(string[] args)
        {
            //
            // Weather update client
            // Connects SUB socket to tcp://127.0.0.1:5556
            // Collects weather updates and finds avg temp in zipcode
            //
            // Author: metadings
            //

            if (args.Length < 4)
                throw new ArgumentException();

            // Socket to talk to server
            using (var context = new ZContext())
            using (var subscriber = new ZSocket(context, ZSocketType.SUB))
            {
                var address = args[1];
                var port = args[2];
                var connectType = args[3];
                switch (connectType.ToLower())
                {
                    case "pgm":
                        var pgmAddress = string.Format("pgm://;239.192.1.1:{0}", port);
                        Console.WriteLine("I: Connecting to {0}...", pgmAddress);
                        subscriber.Connect(pgmAddress);
                        break;
                    case "epgm":
                        var epgmAddress = string.Format("epgm://;239.192.1.1:{0}", port);
                        Console.WriteLine("I: Connecting to {0}...", epgmAddress);
                        subscriber.Connect(epgmAddress);
                        break;
                    default:
                        {
                            string connect_to = string.Format("tcp://{0}:{1}", address, port);

                            Console.WriteLine("I: Connecting to {0}...", connect_to);
                            subscriber.Connect(connect_to);
                        }
                        break;
                }
                // Subscribe to zipcode
                string zipCode = args[0];
                Console.WriteLine("I: Subscribing to zip code {0}...", zipCode);

                subscriber.Subscribe(zipCode);

                // Process 10 updates
                int i = 0;
                decimal total_temperature = 0;
                for (; i < int.MaxValue; ++i)
                {
                    //using (var replyFrame = subscriber.ReceiveFrame())
                    //{
                    //    string reply = replyFrame.ReadString();

                    //    Console.WriteLine(reply);
                    //    total_temperature += Convert.ToInt64(reply.Split(' ')[1]);
                    //}

                    using (var recMsg = subscriber.ReceiveMessage())
                    {
                        var zipcode = recMsg[0].ReadString(Encoding.UTF8);
                        var temperature = recMsg[1].ReadInt32();

                        Console.WriteLine(string.Format("{0} {1}", zipcode, temperature));

                        total_temperature += temperature;
                    }
                }
                Console.WriteLine("Average temperature for zipcode '{0}' was {1}°", zipCode, (total_temperature / i));
            }

            Console.ReadLine();
        }
    }
}