//----------------------------------------------------------------------------------
// Req Socket Sample
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

namespace Req
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var options = new Options();
            var parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));

            if (!parser.ParseArguments(args, options))
                Environment.Exit(1);

            using (var context = ZmqContext.Create())
            {
                using (var socket = context.CreateSocket(SocketType.REQ))
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

                        socket.Send(reqMsg, Encoding.UTF8);
                        var replyMsg = socket.Receive(Encoding.UTF8);

                        Console.WriteLine("Received: " + replyMsg + Environment.NewLine);
                    }
                }
            }
        }
    }
}