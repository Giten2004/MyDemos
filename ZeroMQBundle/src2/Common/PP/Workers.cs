using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace Common.PP
{
    public static class Workers
    {
        public static void Ready(this IList<Worker> workers, Worker worker)
        {
            workers.Add(worker);

            Console.WriteLine("worker Count:{0}, new added worker:{1}", workers.Count, worker);
        }

        public static ZFrame Next(this IList<Worker> workers)
        {
            Worker worker = workers[0];
            workers.RemoveAt(0);

            ZFrame identity = worker.Identity;
            worker.Identity = null;
            worker.Dispose();

            return identity;
        }

        public static void Purge(this IList<Worker> workers)
        {
            foreach (Worker worker in workers.ToList())
            {
                if (DateTime.UtcNow < worker.Expiry)
                    continue;   // Worker is alive, we're done here

                workers.Remove(worker);
            }
        }
    }
}
