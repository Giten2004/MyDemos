using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MonitorDemo
{
    class GamePlayer
    {
        public string PlayerName { get; set; }
        public string EnemyName { get; set; }
    }

    class Program
    {
        private static object _monitorObj = new object();
        private static int _bloodAttack = 0;

        static void Main(string[] args)
        {
            //DemoOne();
            DemoTwo();

            Console.Read();
        }

        private static void DemoOne()
        {
            GamePlayer spiderMan = new GamePlayer { PlayerName = "Spider Man", EnemyName = "Super Man" };
            Thread spiderManThread = new Thread(new ParameterizedThreadStart(GameAttack));

            GamePlayer superMan = new GamePlayer { PlayerName = "Super Man", EnemyName = "Spider Man" };
            Thread superManThread = new Thread(new ParameterizedThreadStart(GameAttack));

            spiderManThread.Start(spiderMan);
            superManThread.Start(superMan);

            spiderManThread.Join();
            superManThread.Join();

            Console.WriteLine("Game Over");
            Console.ReadLine();
        }

        private static void GameAttack(object param)
        {
            GamePlayer gamePlayer = (GamePlayer)param;

            try
            {
                Monitor.Enter(_monitorObj);

                int blood = 100;
                Random ran = new Random();

                while (blood > 0 && _bloodAttack >= 0)
                {
                    blood -= _bloodAttack;
                    if (blood > 0)
                    {
                        _bloodAttack = ran.Next(100);
                        Console.WriteLine("{0}'s blood is {1}, attack {2} {3}", gamePlayer.PlayerName, blood, gamePlayer.EnemyName, _bloodAttack);
                    }
                    else
                    {
                        Console.WriteLine("{0} is dead!!!", gamePlayer.PlayerName);
                        _bloodAttack = -1;
                    }

                    Thread.Sleep(1000);

                    Monitor.Pulse(_monitorObj);
                    Monitor.Wait(_monitorObj);
                }
            }
            finally
            {
                Monitor.PulseAll(_monitorObj);
                Monitor.Exit(_monitorObj);
            }
        }


        private static void DemoTwo()
        {
            Thread firstThread = new Thread(new ThreadStart(TryEnterTest));
            firstThread.Name = "firstThread";
            Thread secondThread = new Thread(new ThreadStart(TryEnterTest));
            secondThread.Name = "secondThread";

            firstThread.Start();
            secondThread.Start();

            Console.ReadLine();
        }

        private static void TryEnterTest()
        {
            if (!Monitor.TryEnter(_monitorObj, 5000))
            {
                Console.WriteLine("Thread {0} wait 5 seconds, didn't get the lock", Thread.CurrentThread.Name);
                Console.WriteLine("Thread {0} completed!", Thread.CurrentThread.Name);
                return;
            }

            try
            {
                Monitor.Enter(_monitorObj);
                Console.WriteLine("Thread {0} get the lock and will run 10 seconds", Thread.CurrentThread.Name);
                Thread.Sleep(10000);
                Console.WriteLine("Thread {0} completed!", Thread.CurrentThread.Name);
            }
            finally
            {
                Monitor.Exit(_monitorObj);
            }
        }
    }
}
