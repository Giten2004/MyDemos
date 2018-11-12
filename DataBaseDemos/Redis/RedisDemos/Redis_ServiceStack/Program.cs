using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redis_ServiceStack
{
    /**
     * Free Quotas
Whilst ServiceStack v4 is a commercially-supported product, we also allow free usage for small projects and evaluation purposes. 
The NuGet packages above include the quota's below which can be unlocked with a license key:

10 Operations in ServiceStack (i.e. Request DTOs)
10 Database Tables in OrmLite
10 DynamoDB Tables in PocoDynamo
20 Different Types in JSON, JSV and CSV Serializers *
20 Different Types in Redis Client Typed APIs
6000 requests per hour with the Redis Client
     * 
     **/
    class Program
    {
        const string testKey = "localtion";

        static void Main(string[] args)
        {
            RedisClient redisCli = new RedisClient("192.168.20.128", 6379);
            redisCli.Set<string>(testKey, "https://www.google.com/ncr");

            var index = 0;
            //{"The free-quota limit on '6000 Redis requests per hour' has been reached. 
            //Please see https://servicestack.net to upgrade to a commercial license or visit https://github.com/ServiceStackV3/ServiceStackV3 to revert back to the free ServiceStack v3."}
            while (index < 6117)
            {
                string location = redisCli.Get<string>(testKey);
                Console.WriteLine("{0} {1}", index, location);

                index++;
            }

            Console.ReadLine();
        }
    }
}
