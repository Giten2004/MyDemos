//----------------------------------------------------------------------------------
// Sub Socket Sample
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Text;
using System.Threading;
using CommandLine;
using ZeroMQ;

namespace Sub
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var options = new Options();
                var parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));
                if (!parser.ParseArguments(args, options))
                    Environment.Exit(1);

                using (var context = ZContext.Create())
                {
                    using (var socket = new ZSocket(context, ZSocketType.SUB))
                    {
                        if (options.SubscriptionPrefixes.Count() == 0)
                        {
                            Console.WriteLine("socket.SubscribeAll()");
                            socket.SubscribeAll();
                        }
                        else
                        {
                            foreach (var subscriptionPrefix in options.SubscriptionPrefixes)
                                socket.Subscribe(Encoding.UTF8.GetBytes(subscriptionPrefix));
                        }
                        //match the message if it beginwith the prifex.

                        foreach (var endPoint in options.ConnectEndPoints)
                        {
                            Console.WriteLine("connect to {0}", endPoint);
                            socket.Connect(endPoint);
                        }

                        while (true)
                        {
                            Thread.Sleep(options.Delay);

                            Console.WriteLine("ReceiveFrame");
                            var msgFrame = socket.ReceiveFrame();
                            var msg = msgFrame.ReadString(Encoding.UTF8);
                            Console.WriteLine("Received: " + msg);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }
    }
}