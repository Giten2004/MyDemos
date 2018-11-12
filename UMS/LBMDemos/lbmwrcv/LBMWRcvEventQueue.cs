using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.latencybusters.lbm;

namespace LBMApplication
{
    class LBMWRcvEventQueue : LBMEventQueue
    {
        public LBMWRcvEventQueue()
        {
            this.addMonitor(new LBMEventQueueCallback(monitor));
        }

        protected void monitor(object cbArg, int evtype, int evq_size, long evq_delay)
        {
            System.Console.Error.WriteLine("Event Queue Monitor: Type: " + evtype +
                ", Size: " + evq_size +
                ", Delay: " + evq_delay + " usecs.");
        }
    }
}
