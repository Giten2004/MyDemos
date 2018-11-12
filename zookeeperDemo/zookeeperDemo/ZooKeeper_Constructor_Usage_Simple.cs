using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZooKeeperNet;

namespace zookeeperDemo
{
    class ZooKeeper_Constructor_Usage_Simple : IWatcher
    {
        static ManualResetEvent manualWaitHandler = new ManualResetEvent(false);//false 即非终止，未触发。  

        static void Main(string[] args)
        {
            ZooKeeper zookeeper = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0,0,5), new ZooKeeper_Constructor_Usage_Simple());
            try
            {
                manualWaitHandler.WaitOne();

                Console.WriteLine("ZooKeeper session established.");
            }
            catch (Exception ex)
            {

            }

            Console.ReadLine();
        }

        #region Implement methods of interface IWatcher
        public void Process(WatchedEvent @event)
        {
            Console.WriteLine("Received watched event: {0}", @event);

            if (@event.State == KeeperState.SyncConnected)
            {
                manualWaitHandler.Set();
            }
        }
        #endregion
    }
}
