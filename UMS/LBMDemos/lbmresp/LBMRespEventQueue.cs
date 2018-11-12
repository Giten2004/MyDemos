using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.latencybusters.lbm;

namespace LBMApplication
{
    class LBMRespEventQueue : LBMEventQueue
    {
        public LBMRespEventQueue()
        {
            this.addMonitor(new LBMEventQueueCallback(monitor));
        }

        protected void monitor(object cbarg, int evtype, int evq_size, long evq_delay)
        {
            System.Console.Error.WriteLine("Event Queue Monitor: Type: {0}, Size: {1}, Delay: {2} usecs.", evtype, evq_size, evq_delay);
        }
    }
}
