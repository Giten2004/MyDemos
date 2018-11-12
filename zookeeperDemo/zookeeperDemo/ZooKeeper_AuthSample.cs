using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooKeeperNet;

namespace zookeeperDemo
{
    class ZooKeeper_AuthSample
    {
        const string PATH = "/zk-book-auth_test";

        static void Main(string[] args)
        {
            var zookeeper = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0, 0, 5), null);
            //添加权限信息后创建的节点就 受到了权限控制
            zookeeper.AddAuthInfo("digest", "foo:true".GetBytes());
            zookeeper.Create(PATH, "init".GetBytes(), Ids.CREATOR_ALL_ACL, CreateMode.Ephemeral);

            try
            {
                var zooKeeper2 = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0, 0, 5), null);
                //这儿会抛出异常，无权限访问
                zooKeeper2.GetData(PATH, false, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine();
            }


            try
            {
                var zooKeeper3 = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0, 0, 5), null);
                zooKeeper3.AddAuthInfo("digest", "foo:false".GetBytes());
                zooKeeper3.GetData(PATH, false, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine();
            }

            try
            {
                var zooKeeper4 = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0, 0, 5), null);
                zooKeeper4.AddAuthInfo("digest5555555", "foo:true".GetBytes());
                zooKeeper4.GetData(PATH, false, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            zookeeper.GetData(PATH, null, null);
            zookeeper.Dispose();

            Console.ReadLine();
        }
    }
}
