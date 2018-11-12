//----------------------------------------------------------------------------------
// PullPush worker Socket Sample
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

using System;
using System.Text;
using System.Threading;
using CommandLine;
using ZeroMQ;

namespace PullPushWorker
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
                    using (ZmqSocket receiver = ctx.CreateSocket(SocketType.PULL),
                                     sender = ctx.CreateSocket(SocketType.PUSH))
                    {
                        receiver.Connect(options.PullEndPoint);
                        sender.Connect(options.PushEndPoint);

                        while (true)
                        {
                            var rcvdMsg = receiver.Receive(Encoding.UTF8);
                            Console.WriteLine("Pulled : " + rcvdMsg);

                            var sndMsg = options.RcvdMessageTag.Replace("#msg#", rcvdMsg);
                            Thread.Sleep(options.Delay);

                            Console.WriteLine("Pushing: " + sndMsg);
                            sender.Send(sndMsg, Encoding.UTF8);
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