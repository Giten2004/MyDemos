using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZooKeeperNet;

namespace zookeeperDemo
{
    class ZooKeeper_Create_API_ASync_Usage : IWatcher
    {
        static ManualResetEvent manualWaitHandler = new ManualResetEvent(false);//false 即非终止，未触发。  

        static void Main(string[] args)
        {
            ZooKeeper zookeeper = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0,0,5), new ZooKeeper_Constructor_Usage_Simple());
            try
            {
                manualWaitHandler.WaitOne(); 

                //临时节点，API的返回值就是传入的path
                string path1 = zookeeper.Create("/zk-test-ephemeral-", "".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Ephemeral);
                Console.WriteLine("Success create znode: {0}", path1);

                //临时顺序节点，API的返回值会在传入的path后边加上一个数字
                string path2 = zookeeper.Create("/zk-test-ephemeral-", "".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.EphemeralSequential);
                Console.WriteLine("Success create znode: {0}", path2);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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
