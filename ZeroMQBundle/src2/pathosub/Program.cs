﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using ZeroMQ;

namespace pathosub
{
    class Program
    {
        static void Main(string[] args)
        {
            //
            // Pathological subscriber
            // Subscribes to one random topic and prints received messages
            //
            // Author: metadings
            //

            if (args == null || args.Length < 1)
            {
                Console.WriteLine();
                Console.WriteLine("Usage: ./{0} PathoSub [Endpoint]", AppDomain.CurrentDomain.FriendlyName);
                Console.WriteLine();
                Console.WriteLine("    Endpoint  Where PathoSub should connect to.");
                Console.WriteLine("              Default is tcp://127.0.0.1:5556");
                Console.WriteLine();
                args = new string[] { "tcp://127.0.0.1:5556" };
            }
            Console.WriteLine("----------------------------------------------------");
            using (var context = new ZContext())
            using (var subscriber = new ZSocket(context, ZSocketType.SUB))
            {
                subscriber.Connect(args[0]);

                var rnd = new Random();
                var subscription = string.Format("{0:D3}", rnd.Next(1000));
                Console.WriteLine("Subscribing topic:{0}", subscription);
                subscriber.Subscribe(subscription);

                ZMessage msg;
                ZError error;

                while (true)
                {
                    if (null == (msg = subscriber.ReceiveMessage(out error)))
                    {
                        if (error == ZError.ETERM)
                            break;  // Interrupted
                        throw new ZException(error);
                    }

                    using (msg)
                    {
                        msg.DumpZmsg("---------");

                        if (msg[0].ReadString() != subscription)
                        {
                            throw new InvalidOperationException();
                        }
                        Console.WriteLine(msg[1].ReadString());
                    }
                }
            }
        }
    }
}