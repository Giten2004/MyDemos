//----------------------------------------------------------------------------------
// Pull Socket Sample
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

using System;
using System.Text;
using System.Threading;
using CommandLine;
using ZeroMQ;

namespace Pull
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
                    using (var socket = new ZSocket(context, ZSocketType.PULL))
                    {
                        foreach (var endPoint in options.BindEndPoints)
                            socket.Bind(endPoint);

                        while (true)
                        {
                            Thread.Sleep(options.Delay);

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