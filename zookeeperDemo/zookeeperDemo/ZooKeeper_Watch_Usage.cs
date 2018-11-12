using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Org.Apache.Zookeeper.Data;
using ZooKeeperNet;

namespace zookeeperDemo
{
    class ZooKeeper_Watch_Usage : IWatcher
    {
        static ManualResetEvent manualWaitHandler = new ManualResetEvent(false);//false 即非终止，未触发。  

        static log4net.ILog _log = null;
        private static ZooKeeper zookeeper = null;

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("zookeeperDemo.log4net"));

            zookeeper = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0, 0, 10), new ZooKeeper_Watch_Usage());
            manualWaitHandler.WaitOne();

            //ASYNC_VerifyZookeeper();

            string path = "/zk-book";
            var testStat = zookeeper.Exists(path, false);
            if (testStat == null)
                zookeeper.Create(path, "123".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Ephemeral);


            var pathData = zookeeper.GetData(path, true, null);
            Console.WriteLine("The path's data(watched):{0}", System.Text.UTF8Encoding.UTF8.GetString(pathData));

            Console.WriteLine("begin to changed the data of path:{0}, which is watched.", path);
            zookeeper.SetData(path, "fuck you".GetBytes(), -1);
            Console.WriteLine("change data of path:{0} is finished, which is watched.", path);

            var newPath = "/ACID";
            zookeeper.Create(newPath, "AAAA".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Ephemeral);
            var acidData = zookeeper.GetData(newPath, false, null);
            Console.WriteLine("The new created node's data(Unwatched):{0}", System.Text.UTF8Encoding.UTF8.GetString(acidData));

            Console.WriteLine("begin to changed the data of path:{0}, which is UNwatched.", newPath);
            zookeeper.SetData(newPath, "laoHUYou".GetBytes(), -1);
            Console.WriteLine("change data of path:{0} is finished, which is Unwatched.", newPath);

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

                if (@event.Type == EventType.NodeDataChanged)
                {
                    Console.WriteLine("Notifiy message, The data of path:{0} is changed.", @event.Path);
                }
            }

        }
        #endregion
    }
}
