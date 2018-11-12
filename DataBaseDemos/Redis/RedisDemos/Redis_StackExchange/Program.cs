using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redis_StackExchange
{
    class Program
    {
        private static ConnectionMultiplexer GetManager(string connectionString)
        {
            return ConnectionMultiplexer.Connect(connectionString);
        }

        static void Main(string[] args)
        {
            var redisCli = GetManager("192.168.20.128:6379");

            var db = redisCli.GetDatabase();
            RedisKey testKey = "MBOA";
            db.StringSet(testKey, "https://www.google.com/ncr");
            string location = db.StringGet(testKey);

            Console.WriteLine(location);

            Console.ReadLine();
        }
    }
}
