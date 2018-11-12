using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZooKeeperNet;

namespace zookeeperDemo
{
    class ZooKeeper_SetData_API_Sync_Usage : IWatcher
    {
        static ManualResetEvent manualWaitHandler = new ManualResetEvent(false);//false 即非终止，未触发。  
        static ZooKeeper zk = null;

        //static List<Org.Apache.Zookeeper.Data.ACL> CreateAllAccessACLList()
        //{
        //    var list = new List<Org.Apache.Zookeeper.Data.ACL>();

        //    var ZKid = new Org.Apache.Zookeeper.Data.ZKId("world", "anyone");
        //    var acl = new Org.Apache.Zookeeper.Data.ACL(ZooKeeperNet.Perms.ALL, ZKid);

        //    list.Add(acl);

        //    return list;
        //}

        static void Main(string[] args)
        {
            string path = "/zk-book";

            zk = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0,0,5), new ZooKeeper_SetData_API_Sync_Usage());
            manualWaitHandler.WaitOne();

            if (zk.Exists(path, false) == null)
                zk.Create(path, "123".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Ephemeral);
            Console.WriteLine(zk.GetData(path, true, null));

            var stat = zk.SetData(path, "456".GetBytes(), -1);
            Console.WriteLine("{0}, {1}, {2}", stat.Czxid, stat.Mzxid, stat.Version);

            var stat2 = zk.SetData(path, "456".GetBytes(), stat.Version);
            Console.WriteLine("{0}, {1}, {2}", stat2.Czxid, stat2.Mzxid, stat2.Version);

            try
            {
                zk.SetData(path, "456".GetBytes(), stat.Version);

            }
            catch (Exception ex)
            {

                Console.WriteLine("Error: " + ex.ToString());
            }

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
            }
        }
        #endregion
    }
}