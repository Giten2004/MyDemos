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

                using (var ctx = ZmqContext.Create())
                {
                    using (var socket = ctx.CreateSocket(SocketType.SUB))
                    {
                        if (options.SubscriptionPrefixes.Count() == 0)
                        {
                            socket.SubscribeAll();
                        }
                        else
                        {
                            foreach (var subscriptionPrefix in options.SubscriptionPrefixes)
                                socket.Subscribe(Encoding.UTF8.GetBytes(subscriptionPrefix));
                        }
                        //match the message if it beginwith the prifex.

                        foreach (var endPoint in options.ConnectEndPoints)
                            socket.Connect(endPoint);

                        while (true)
                        {
                            Thread.Sleep(options.Delay);
                            var msg = socket.Receive(Encoding.UTF8);
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