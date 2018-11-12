using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZooKeeperNet;

namespace zookeeperDemo
{
    class ZooKeeper_Constructor_Usage_With_SID_PASSWD : IWatcher
    {
        static ManualResetEvent manualWaitHandler = new ManualResetEvent(false);//false 即非终止，未触发。  

        static void Main(string[] args)
        {
            ZooKeeper zookeeper = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0,0,5), new ZooKeeper_Constructor_Usage_With_SID_PASSWD());

            manualWaitHandler.WaitOne();

            long sessionId = zookeeper.SessionId;
            byte[] passwd = zookeeper.SesionPassword;

            manualWaitHandler.Reset();
            //Use illegal sessionId and session password
            //使用错误的sessionid与paswd来创建Zookeeper 客户端实例，客户端将会收到服务端的Expirted事件通知。
            var zk1 = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0,0,5), new ZooKeeper_Constructor_Usage_With_SID_PASSWD(), 1, "test".GetBytes());
            manualWaitHandler.WaitOne();

            //todo:use the breakpotint them you can see the expired event. why?
            manualWaitHandler.Reset();
            //Use correct sessionId and sessionPassword
            var zk2 = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0,0,5), new ZooKeeper_Constructor_Usage_With_SID_PASSWD(), sessionId, passwd);


            Console.ReadLine();
        }

        #region Implement methods of interface IWatcher
        public void Process(WatchedEvent @event)
        {
            Console.WriteLine("Received watched event: {0}, ThreadId:{1}", @event, Thread.CurrentThread.ManagedThreadId);

            if (@event.State == KeeperState.SyncConnected)
            {
                Console.WriteLine("to set signal");
                manualWaitHandler.Set();
            }
        }
        #endregion
    }
}
