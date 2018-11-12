//----------------------------------------------------------------------------------
// MultipartMessaging
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

namespace MultipartMessages
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using ZeroMQ;

    class MultipartMessaging
    {
        private BackgroundWorker subThread;

        public void Start()
        {
            subThread = new BackgroundWorker();
            subThread.DoWork += new DoWorkEventHandler(subThread_DoWork);
            subThread.RunWorkerAsync();
            
            using (var ctx = ZContext.Create())
            {
                using(var socket = new ZSocket(ctx, ZSocketType.PUB))
                {
                    socket.Bind("tcp://127.0.0.1:5000");
                    while (true)
                    {
                        Thread.Sleep(1000);
                        // Create a ZmqMessage containing 3 frames
                        ZMessage zmqMessage = new ZMessage();
                        zmqMessage.Append(new ZFrame(Encoding.UTF8.GetBytes("My Frame 01")));
                        zmqMessage.Append(new ZFrame(Encoding.UTF8.GetBytes("My Frame 02")));
                        zmqMessage.Append(new ZFrame(Encoding.UTF8.GetBytes("My Frame 03")));

                        Console.WriteLine("PUB; publishing: ");
                        foreach (var msg in zmqMessage)                        
                            Console.WriteLine("\t" + msg.ReadString(Encoding.UTF8));                        
                        socket.SendMessage(zmqMessage);
                    }
                }
            }
        }

        void subThread_DoWork(object sender, DoWorkEventArgs e)
        {
            using (var ctx = ZContext.Create())
            {
                using(var socket = new ZSocket(ctx, ZSocketType.SUB))
                {
                    socket.Connect("tcp://127.0.0.1:5000");

                    //can set filter
                    //socket.Subscribe("prefixdemo");
                    socket.SubscribeAll();


                    while (true)
                    {
                        var zmqMessage = socket.ReceiveMessage();
                        var frameContents = zmqMessage
                                            .Select(f => f.ReadString(Encoding.UTF8)).ToList();
                        Console.WriteLine("SUB; Received: ");
                        foreach (var frameContent in frameContents)
                        {
                            Console.WriteLine("\t" + frameContent);
                        }
                    }
                }
            }
        }       
    }
}
