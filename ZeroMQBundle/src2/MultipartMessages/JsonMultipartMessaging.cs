//----------------------------------------------------------------------------------
// JsonMultipartMessaging
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

using ServiceStack.Text;

namespace MultipartMessages
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using ZeroMQ;

    class JsonMultipartMessaging
    {
        private BackgroundWorker _subThread;

        public void Start()
        {
            _subThread = new BackgroundWorker();
            _subThread.DoWork += new DoWorkEventHandler(subThread_DoWork);
            _subThread.RunWorkerAsync();

            using (var ctx = ZContext.Create())
            {
                using (var socket = new ZSocket(ctx, ZSocketType.PUB))
                {
                    socket.Bind("tcp://127.0.0.1:5000");

                    while (true)
                    {
                        Thread.Sleep(1000);

                        var shoppingBasket = new ShoppingBasket()
                        {
                            StoreName = "Fruits City",
                            ShoppingItems = new List<ShoppingItem>() {
                                new ShoppingItem() { Description="Orange", Weight=0.5f},
                                new ShoppingItem() { Description="Apple", Weight=1.4f},
                                new ShoppingItem() { Description="Banana", Weight=0.75f}
                            }
                        };

                        ZMessage zmqMessage = new ZMessage();
                        var msg1 = "Shopping Basket";
                        var msg2 = JsonSerializer.SerializeToString<ShoppingBasket>(shoppingBasket);
                        zmqMessage.Append(new ZFrame(msg1, Encoding.UTF8));
                        zmqMessage.Append(JsonFrame.Serialize<ShoppingBasket>(shoppingBasket));

                        Console.WriteLine("PUB; publishing: ");
                        Console.WriteLine("\t" + msg1);
                        Console.WriteLine("\t" + msg2);

                        socket.SendMessage(zmqMessage);
                    }
                }
            }
        }

        private void subThread_DoWork(object sender, DoWorkEventArgs e)
        {
            using (var ctx = ZContext.Create())
            {
                using (var socket = new ZSocket(ctx, ZSocketType.SUB))
                {
                    socket.Connect("tcp://127.0.0.1:5000");
                    //socket.Subscribe("Shopping");
                    socket.SubscribeAll();

                    while (true)
                    {
                        var zmqMessage = socket.ReceiveMessage();

                        var msgTitle = zmqMessage[0].ReadString(Encoding.UTF8);

                        ShoppingBasket shoppingBasket = JsonFrame.DeSerialize<ShoppingBasket>(zmqMessage[1]);

                        Console.WriteLine("SUB; Received: ");
                        Console.WriteLine("\t" + msgTitle);

                        var msg2 = JsonSerializer.SerializeToString<ShoppingBasket>(shoppingBasket);
                        Console.WriteLine("\t" + msg2);
                    }
                }
            }
        }
    }
}
