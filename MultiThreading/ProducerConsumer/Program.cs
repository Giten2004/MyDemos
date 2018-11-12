using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    class BlockingQueue<T>
    {
        private object _lockObj = new object();
        private Queue<T> _queue;

        public int QueueSize { get; private set; }

        public BlockingQueue(int queueSize)
        {
            QueueSize = queueSize;
            _queue = new Queue<T>(QueueSize);
        }

        public bool EnQueue(T item)
        {
            lock (_lockObj)
            {
                while (_queue.Count() >= QueueSize)
                {
                    Monitor.Wait(_lockObj);
                }

                _queue.Enqueue(item);

                Console.WriteLine("---> 0000" + item.ToString());

                Monitor.PulseAll(_lockObj);
            }

            return true;
        }

        public bool DeQueue(out T item)
        {
            lock (_lockObj)
            {
                while (_queue.Count() == 0)
                {
                    if (!Monitor.Wait(_lockObj, 3000))
                    {
                        item = default(T);
                        return false;
                    }
                }

                item = _queue.Dequeue();

                Monitor.PulseAll(_lockObj);
            }

            return true;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //DemoOne();
            DemoTwo();

            Console.ReadLine();
        }

        private static void DemoOne()
        {
            BlockingQueue<string> bQueue = new BlockingQueue<string>(3);

            Random ran = new Random();

            //producer
            new Thread(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(ran.Next(1000));

                    bQueue.EnQueue(i.ToString());
                }
                Console.WriteLine("producer quit!");
            }).Start();

            //producer
            new Thread(() =>
            {
                for (int i = 5; i < 10; i++)
                {
                    Thread.Sleep(ran.Next(1000));
                    bQueue.EnQueue(i.ToString());
                }
                Console.WriteLine("producer quit!");
            }).Start();

            //consumer
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(ran.Next(1000));
                    string item;
                    if (!bQueue.DeQueue(out item))
                    {
                        break;
                    }
                }
                Console.WriteLine("consumer quit!");
            }).Start();

            Console.Read();
        }

        private static void DemoTwo()
        {
            BlockingQueue<string> bQueue = new BlockingQueue<string>(10);

            Random ran = new Random();

            //producer one
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(ran.Next(1000));

                    bQueue.EnQueue(string.Format("ProducerOne:{0}", ran.Next(int.MaxValue)));
                }
            }).Start();

            //producer two
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(ran.Next(1000));

                    bQueue.EnQueue(string.Format("ProducerTwo:{0}", ran.Next(int.MaxValue)));
                }
            }).Start();

            //producer three
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(ran.Next(1000));

                    bQueue.EnQueue(string.Format("ProducerThree:{0}", ran.Next(int.MaxValue)));
                }
            }).Start();

            //consumer one
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(ran.Next(1000));
                    string item;
                    if (!bQueue.DeQueue(out item))
                    {
                        break;
                    }
                    Console.WriteLine("Consumer One -" + item + " <---");
                }
                Console.WriteLine("consumer one quit!");
            }).Start();

            //consumer two
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(ran.Next(1000));
                    string item;
                    if (!bQueue.DeQueue(out item))
                    {
                        break;
                    }
                    Console.WriteLine("Consumer Two -" + item + " <---");
                }
                Console.WriteLine("consumer two quit!");
            }).Start();

            Console.Read();
        }
    }
}
