using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Org.Apache.Zookeeper.Data;
using ZooKeeperNet;

namespace MasterWorker
{
    public class Client : IWatcher
    {
        ZooKeeper zk;
        String hostPort;
        bool connected = false;
        bool expired = false;

        public Client(string hostPort)
        {
            this.hostPort = hostPort;
        }

        #region Implement methods of interface IWatcher

        public void Process(WatchedEvent @event)
        {
            Console.WriteLine(@event);
            if (@event.Type == EventType.None)
            {
                switch (@event.State)
                {
                    case KeeperState.SyncConnected:
                        connected = true;
                        break;
                    case KeeperState.Disconnected:
                        connected = false;
                        break;
                    case KeeperState.Expired:
                        expired = true;
                        connected = false;
                        Console.WriteLine("Exiting due to session expiration");
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        public bool isConnected()
        {
            return connected;
        }

        /**
         * Check if the ZooKeeper session is expired.
         * 
         * @return
         */
        public bool isExpired()
        {
            return expired;
        }

        public void startZK()
        {
            zk = new ZooKeeper(hostPort, new TimeSpan(0, 0, 15), this);
            InitMasterWorker();
        }


        public void test()
        {
            var stat = zk.Exists("/tasks", this);
            if (stat != null)
            {
                var data = zk.GetData("/tasks", true, stat);
                zk.GetChildren("/tasks", true);
            }
        }

        public void submitTask(String task, TaskObject taskCtx)
        {
            taskCtx.setTask(task);

            var name = zk.Create("/tasks/task-", task.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.PersistentSequential);
            taskCtx.setTaskName(name);

            watchStatus(name.Replace("/tasks/", "/status/"), taskCtx);
        }

        protected Dictionary<String, Object> ctxMap = new Dictionary<String, Object>();

        void watchStatus(String path, Object ctx)
        {
            ctxMap.Add(path, ctx);

            var state = zk.Exists(path, null);

            if (state != null)
            {
                Stat getStat = new Stat();
                var data = zk.GetData(path, false, getStat);
            }
        }

        void InitMasterWorker()
        {
            InitNode("/workers");
            InitNode("/assign");
            InitNode("/tasks");
            InitNode("/status");
        }

        void InitNode(string nodeName)
        {
            var stat = zk.Exists(nodeName, false);
            if (stat == null)
                zk.Create(nodeName, new byte[] { }, Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
        }


        public static void Main(string[] args)
        {
            //host: 192.168.183.134:2181,192.168.183.135:2181,192.168.183.136:2181
            Client c = new Client(args[0]);
            c.startZK();

            while (!c.isConnected())
            {
                Thread.Sleep(100);
            }

            TaskObject task1 = new TaskObject();
            TaskObject task2 = new TaskObject();

            c.test();
            c.submitTask("Sample task", task1);
            //c.submitTask("Another sample task", task2);

            //task1.waitUntilDone();
            //task2.waitUntilDone();

            Console.ReadLine();
        }
    }

    public class TaskObject
    {
        private String task;
        private String taskName;
        private bool done = false;
        private bool succesful = false;

        static ManualResetEvent manualWaitHandler = new ManualResetEvent(false);//false 即非终止，未触发。  

        public String getTask()
        {
            return task;
        }

        public void setTask(String task)
        {
            this.task = task;
        }

        public void setTaskName(String name)
        {
            this.taskName = name;
        }

        public String getTaskName()
        {
            return taskName;
        }

        public void setStatus(bool status)
        {
            succesful = status;
            done = true;
            manualWaitHandler.Set();
        }

        public void waitUntilDone()
        {
            try
            {
                manualWaitHandler.WaitOne();
            }
            catch (Exception e)
            {
                Console.WriteLine("InterruptedException while waiting for task to get done");
            }
        }

        public bool isDone()
        {
            return done;
        }

        public bool isSuccesful()
        {
            return succesful;
        }

    }
}
