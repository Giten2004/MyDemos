using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace DEALER
{
    class Program
    {
        //asynchronous client
        //When we use a DEALER to talk to a REP socket, we must accurately emulate the envelope
        //that the REQ socket would have sent, or the REP socket will discard the message as invalid.
        static void Main(string[] args)
        {
            var options = new Options();
            var parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));

            if (!parser.ParseArguments(args, options))
                Environment.Exit(1);

            using (var context = ZContext.Create())
            {
                using (var socket = new ZSocket(context, ZSocketType.DEALER))
                {
                    foreach (var connectEndpoint in options.ConnectEndPoints)
                        socket.Connect(connectEndpoint);

                    long msgCptr = 0;
                    var msgIndex = 0;

                    while (true)
                    {
                        if (msgCptr == long.MaxValue)
                            msgCptr = 0;

                        msgCptr++;
                        if (options.MaxMessage >= 0)
                            if (msgCptr > options.MaxMessage)
                                break;

                        if (msgIndex == options.AlterMessages.Count())
                            msgIndex = 0;

                        var reqMsg = options.AlterMessages[msgIndex++].Replace("#nb#", msgCptr.ToString("d2"));

                        Thread.Sleep(options.Delay);

                        Console.WriteLine("Sending : " + reqMsg);

                        /*
                         * When we use a DEALER to talk to a REP socket, we must accurately emulate the envelope
                        that the REQ socket would have sent, or the REP socket will discard the message as
                        invalid. So, to send a message, we:

                        1. Send an empty message frame with the MORE flag set.
                        2. Send the message body.

                        And when we receive a message, we:

                        1. Receive the first frame and, if it’s not empty, discard the whole message.
                        2. Receive the next frame and pass that to the application.
                         */
                        var message = new ZMessage();
                        message.Add(new ZFrame());
                        message.Add(new ZFrame(reqMsg, Encoding.UTF8));

                        socket.Send(message);
                     
                        var replyFrame = socket.ReceiveFrame();
                        if (string.IsNullOrEmpty(replyFrame.ReadString(Encoding.UTF8)))
                        {
                            replyFrame = socket.ReceiveFrame();
                        }

                        Console.WriteLine("Received: " + replyFrame.ReadString() + Environment.NewLine);
                    }
                }
            }
        }
    }
}
