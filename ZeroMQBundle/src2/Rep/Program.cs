﻿//----------------------------------------------------------------------------------
// Rep Socket Sample
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

using System;
using System.Text;
using System.Threading;
using CommandLine;
using ZeroMQ;

namespace Rep
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var options = new Options();
            var parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));

            if (!parser.ParseArguments(args, options))
                Environment.Exit(1);

            using (var context = ZContext.Create())
            {
                using (var socket = new ZSocket(context, ZSocketType.REP))
                {
                    foreach (var bindEndPoint in options.BindEndPoints)
                        socket.Bind(bindEndPoint);

                    while (true)
                    {
                        Thread.Sleep(options.Delay);

                        var rcvdFrame = socket.ReceiveFrame();
                        var rcvdMsg = rcvdFrame.ReadString(Encoding.UTF8);
                        Console.WriteLine("Received: " + rcvdMsg);

                        var replyMsg = options.ReplyMessage.Replace("#msg#", rcvdMsg);
                        var replyFrame = new ZFrame(replyMsg);
                        Console.WriteLine("Sending : " + replyMsg + Environment.NewLine);
                        socket.Send(replyFrame);
                    }
                }
            }
        }
    }
}