using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using ZeroMQ;

namespace DemonstrateIdentities
{
    class Program
    {
        static void Main(string[] args)
        {
            //
            // Demonstrate request-reply identities
            //
            // Author: metadings
            //

            using (var context = new ZContext())
            using (var sink = new ZSocket(context, ZSocketType.ROUTER))
            {
                sink.Bind("inproc://example");

                // First allow 0MQ to set the identity
                using (var anonymous = new ZSocket(context, ZSocketType.REQ))
                {
                    anonymous.Connect("inproc://example");
                    anonymous.Send(new ZFrame("ROUTER uses REQ's generated 5 byte identity"));

                    ZMessage msg = sink.ReceiveMessage();
                    msg.DumpZmsg("-------------anonymous-------------");

                    using (msg)
                    using (ZMessage newMsg = new ZMessage())
                    {
                        //send three frame, but the client received is the only data frame, it's the 3rd frame.
                        newMsg.Add(msg[0]);
                        newMsg.Add(new ZFrame());
                        newMsg.Add(new ZFrame("router to REQ"));

                        sink.Send(newMsg);
                    }

                    using (ZMessage routerMsg = anonymous.ReceiveMessage())
                    {
                        routerMsg.DumpZmsg("-------------anonymous-------------");
                    }
                }

                // Then set the identity ourselves
                using (var identified = new ZSocket(context, ZSocketType.REQ))
                {
                    identified.IdentityString = "PEER2";
                    identified.Connect("inproc://example");
                    identified.Send(new ZFrame("ROUTER uses REQ's socket identity"));

                    ZMessage msg = sink.ReceiveMessage();
                    msg.DumpZmsg("------------identified--------------");

                    using (msg)
                    using (ZMessage newMsg = new ZMessage())
                    {
                        //send three frame, but the client received is the only data frame, it's the 3rd frame.
                        newMsg.Add(msg[0]);
                        //here, this is very important. If we do not add the empty frame, client will receive nothing.
                        newMsg.Add(new ZFrame());
                        newMsg.Add(new ZFrame("router to REQ"));

                        sink.Send(newMsg);
                    }

                    using (ZMessage routerMsg = identified.ReceiveMessage())
                    {
                        routerMsg.DumpZmsg("-------------identified-------------");
                    }
                }

                using (var DEALERIdentify = new ZSocket(context, ZSocketType.DEALER))
                {
                    DEALERIdentify.Connect("inproc://example");
                    DEALERIdentify.Send(new ZFrame("ROUTER uses Dealer's generated 5 byte identity, But there is no empty frame as separeator"));

                    ZMessage msg = sink.ReceiveMessage();
                    msg.DumpZmsg("------------DEALERIdentify--------------");

                    using (msg)
                    using (ZMessage newMsg = new ZMessage())
                    {
                        //send three frame, but the client received is the only data frame, it's the 3rd frame.
                        newMsg.Add(msg[0]);
                        //here, this is very important. That there is no separeator for Dealer!!!!!!!!!!!!!!, If we add the empty frame, it will be treat as data frame
                        //newMsg.Add(new ZFrame());
                        newMsg.Add(new ZFrame("router to REQ"));

                        sink.Send(newMsg);
                    }

                    using (ZMessage routerMsg = DEALERIdentify.ReceiveMessage())
                    {
                        routerMsg.DumpZmsg("-------------DEALERIdentify-------------");
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}