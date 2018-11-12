using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.latencybusters.lbm;

namespace LBMApplication
{
    class LBMRcvEventQueue : LBMEventQueue
    {
        public LBMRcvEventQueue()
            : this(null)
        {
        }

        public LBMRcvEventQueue(LBMEventQueueAttributes evqattr)
            : base(evqattr)
        {
            this.addMonitor(new LBMEventQueueCallback(monitor));
        }

        protected void monitor(object cbarg, int evtype, int evq_size, long evq_delay)
        {
            System.Console.Error.WriteLine("Event Queue Monitor: Type: " + evtype +
                ", Size: " + evq_size +
                ", Delay: " + evq_delay + " usecs.");
        }
    }
}
