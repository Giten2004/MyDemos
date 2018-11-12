//----------------------------------------------------------------------------------
// Push Socket Sample
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

namespace Push
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
                    using (var socket = new ZSocket(context, ZSocketType.PUSH))
                    {
                        foreach (var endPoint in options.BindEndPoints)
                            socket.Bind(endPoint);

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

                            if (msgIndex == options.AltMessages.Count())
                                msgIndex = 0;

                            var msg = options.AltMessages[msgIndex++].Replace("#nb#", msgCptr.ToString("d2"));
                            Thread.Sleep(options.Delay);

                            Console.WriteLine("Pushing: " + msg);

                            var msgFrame = new ZFrame(msg, Encoding.UTF8);
                            socket.Send(msgFrame);
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