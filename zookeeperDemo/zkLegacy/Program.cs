using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Org.Apache.Zookeeper.Data;
using ZooKeeperNet;

namespace zkLegacy
{
    class Program : IWatcher
    {
        static ManualResetEvent manualWaitHandler = new ManualResetEvent(false);//false 即非终止，未触发。  

        static log4net.ILog _log = null;
        private static ZooKeeper zookeeper = null;

        static void Main(string[] args)
        {
            zookeeper = new ZooKeeper("192.168.183.134:2181,192.168.183.135:2181,192.168.183.136:2181", new TimeSpan(0, 0, 10), new Program());
            manualWaitHandler.WaitOne();
            //It's impossible to create path which the parent node does't exist
            //zookeeper.Create(@"/typhon", "123".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            //zookeeper.Create(@"/typhon/services", "123".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);

            ASYNC_VerifyZookeeper();

            string path = "/zk-book";
            var testStat = zookeeper.Exists(path, false);
            if (testStat == null)
                zookeeper.Create(path, "123".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Ephemeral);
            Console.WriteLine(Encoding.UTF8.GetString(zookeeper.GetData(path, true, null)));

            Console.ReadLine();
        }

        private static bool checkMaster(ZooKeeper zk)
        {
            while (true)
            {
                try
                {
                    Stat stat = new Stat();
                    byte[] data = zk.GetData("/master", false, stat);
                    var isLeader = System.Text.Encoding.UTF8.GetString(data).Equals("230");
                    return true;
                }
                catch (KeeperException.NoNodeException)
                {

                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        private static void ASYNC_VerifyZookeeper()
        {
            Stat servicesRoot = zookeeper.Exists("/typhon/services", false);
            Stat connectionsRoot = zookeeper.Exists("/typhon/connections", false);
            Stat configurationRoot = zookeeper.Exists("/typhon/configuration", false);

            if (servicesRoot == null || connectionsRoot == null || configurationRoot == null)
            {
                throw new Exception("Expected znode(s) \"/typhon/services\", \"/typhon/configuration\" , and/or \"/typhon/connections\"" +
                                    " were not found! Is this the correct Zookeeper server?");
            }
        }

        #region Implement methods of interface IWatcher
        public void Process(WatchedEvent @event)
        {
            Console.WriteLine("Received watched event: {0}", @event);

            if (@event.State == KeeperState.SyncConnected)
            {
                if (@event.Type == EventType.None && @event.Path == null)
                    manualWaitHandler.Set();
            }
        }
        #endregion
    }
}
