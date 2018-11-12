using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;
using Common;

namespace ROUTER
{
    class Program
    {
        // asynchronous server

        static void Main(string[] args)
        {
            var options = new Options();
            var parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));

            if (!parser.ParseArguments(args, options))
                Environment.Exit(1);

            using (var context = ZContext.Create())
            {
                using (var socket = new ZSocket(context, ZSocketType.ROUTER))
                {
                    foreach (var bindEndPoint in options.BindEndPoints)
                        socket.Bind(bindEndPoint);

                    var opllItem = ZPollItem.CreateReceiver();

                    while (true)
                    {
                        Thread.Sleep(options.Delay);

                        ZMessage message;
                        ZError error;

                        //
                        if (socket.PollIn(opllItem, out message, out error, new TimeSpan(10 * 1000)))
                        {
                            message.DumpZmsg("--------------------------");

                            var rcvdMsg = message[2].ReadString(Encoding.UTF8);
                            Console.WriteLine("Received: " + rcvdMsg);

                            //be carefull the message format
                            var outMsg = new ZMessage();
                            outMsg.Add(message[0]);
                            outMsg.Add(new ZFrame());

                            var replyMsg = options.ReplyMessage.Replace("#msg#", rcvdMsg);
                            var replyFrame = new ZFrame(replyMsg);
                            outMsg.Add(replyFrame);

                            Console.WriteLine("Sending : " + replyMsg + Environment.NewLine);

                            socket.Send(outMsg);
                        }


                        //2) sync
                        //using (ZMessage identity = socket.ReceiveMessage())
                        //{
                        //    identity.DumpZmsg("--------------------------");

                        //    var rcvdMsg = identity[2].ReadString(Encoding.UTF8);
                        //    Console.WriteLine("Received: " + rcvdMsg);

                        //    //be carefull the message format
                        //    var outMsg = new ZMessage();
                        //    outMsg.Add(identity[0]);
                        //    outMsg.Add(new ZFrame());

                        //    var replyMsg = options.ReplyMessage.Replace("#msg#", rcvdMsg);
                        //    var replyFrame = new ZFrame(replyMsg);
                        //    outMsg.Add(replyFrame);

                        //    Console.WriteLine("Sending : " + replyMsg + Environment.NewLine);

                        //    socket.Send(outMsg);
                        //}
                    }
                }
            }
        }
    }
}
