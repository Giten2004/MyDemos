using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZooKeeperNet;

namespace zookeeperDemo
{
    class ZooKeeper_GetData_API_Sync_Usage : IWatcher
    {
        static ManualResetEvent manualWaitHandler = new ManualResetEvent(false);//false 即非终止，未触发。  
        static ZooKeeper zk = null;
        static Org.Apache.Zookeeper.Data.Stat stat = new Org.Apache.Zookeeper.Data.Stat();

        static void Main(string[] args)
        {
            string path = "/zk-book";
            zk = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0,0,5), new ZooKeeper_GetData_API_Sync_Usage());
            manualWaitHandler.WaitOne();

            zk.Create(path, "123".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Ephemeral);
            Console.WriteLine(zk.GetData(path, true, stat));

            Console.WriteLine("{0}, {1}, {2}", stat.Czxid, stat.Mzxid, stat.Version);

            zk.SetData(path, "123".GetBytes(), -1);

           

            Console.ReadLine();
        }

        #region Implement methods of interface IWatcher
        public void Process(WatchedEvent @event)
        {
            Console.WriteLine("Received watched event: {0}", @event);

            if (@event.State == KeeperState.SyncConnected)
            {
                if (@event.Type == EventType.None && @event.Path == null)
                    manualWaitHandler.Set();
                else if (@event.Type == EventType.NodeDataChanged) 
                {
                    Console.WriteLine(zk.GetData(@event.Path, true, stat));

                    Console.WriteLine("{0}, {1}, {2}", stat.Czxid, stat.Mzxid, stat.Version);
                }

            }
        }
        #endregion
    }
}