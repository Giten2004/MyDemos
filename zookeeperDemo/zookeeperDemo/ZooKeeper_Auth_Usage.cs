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
    class ZooKeeper_Auth_Usage : IWatcher
    {
        static ManualResetEvent manualWaitHandler = new ManualResetEvent(false);//false 即非终止，未触发。  

        static log4net.ILog _log = null;
        private static ZooKeeper zookeeper = null;

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("zookeeperDemo.log4net"));

            zookeeper = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0, 0, 10), new ZooKeeper_Auth_Usage());
            zookeeper.AddAuthInfo("digest", "taokeeper:true".GetBytes());

            manualWaitHandler.WaitOne();

            //ASYNC_VerifyZookeeper();

            string path = "/zk-book";
            var testStat = zookeeper.Exists(path, false);
            if (testStat == null)
                zookeeper.Create(path, "123".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Ephemeral);


            var pathData = zookeeper.GetData(path, true, null);
            Console.WriteLine("The path's data(watched):{0}", System.Text.UTF8Encoding.UTF8.GetString(pathData));

            //
            var errorzk = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0, 0, 10), new ZooKeeper_Auth_Usage());
            errorzk.AddAuthInfo("digest", "taokeeper:error".GetBytes());

            manualWaitHandler.WaitOne();

            var tempdata= errorzk.GetData(path, true, null);
            Console.WriteLine("The path's data(watched):{0}", System.Text.UTF8Encoding.UTF8.GetString(tempdata));

            Console.ReadLine();
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