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

                using (var context = ZContext.Create())
                {
                    using (ZSocket receiver = new ZSocket(context, ZSocketType.PULL),
                                     sender = new ZSocket(context, ZSocketType.PUSH))
                    {
                        receiver.Connect(options.PullEndPoint);
                        sender.Connect(options.PushEndPoint);

                        while (true)
                        {
                            var rcvdFrame = receiver.ReceiveFrame();
                            var rcvdMsg = rcvdFrame.ReadString(Encoding.UTF8);
                            Console.WriteLine("Pulled : " + rcvdMsg);

                            var sndMsg = options.RcvdMessageTag.Replace("#msg#", rcvdMsg);
                            Thread.Sleep(options.Delay);

                            Console.WriteLine("Pushing: " + sndMsg);
                            var sndFrame = new ZFrame(sndMsg, Encoding.UTF8);
                            sender.Send(sndFrame);
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