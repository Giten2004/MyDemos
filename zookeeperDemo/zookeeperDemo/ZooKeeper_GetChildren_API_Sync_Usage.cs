using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZooKeeperNet;

namespace zookeeperDemo
{
    class ZooKeeper_GetChildren_API_Sync_Usage : IWatcher
    {
        static ManualResetEvent manualWaitHandler = new ManualResetEvent(false);//false 即非终止，未触发。  
        static ZooKeeper zk = null;

        static void Main(string[] args)
        {
            string path = "/zk-book";
            zk = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0,0,5), new ZooKeeper_GetChildren_API_Sync_Usage());
            manualWaitHandler.WaitOne();

            zk.Create(path, "".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            zk.Create(path + "/c1", "".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Ephemeral);

            //ZooKeeper服务端在向客户端发送“NodeChildrenChanged”事件通知的时候，仅仅只会发出一个通知，而不会把节点的变化情况发送给客户端，需要客户端自己重新获取。
            //由于Watcher通知是一次性的，即一旦出发一次通知后，该Watcher就失效了，因此客户端需要反复注册Watcher（.net client是否是这样，我暂时未确认）
            var childrenList = zk.GetChildren(path, true);
            Console.WriteLine(childrenList); 

            zk.Create(path + "/c2", "".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Ephemeral);

            Thread.Sleep(5000);

            zk.Create(path + "/c3", "".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Ephemeral);

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
                else if (@event.Type == EventType.NodeChildrenChanged)
                    Console.WriteLine("ReGetChildren: {0}", zk.GetChildren(@event.Path, true));
            }
        }
        #endregion
    }
}