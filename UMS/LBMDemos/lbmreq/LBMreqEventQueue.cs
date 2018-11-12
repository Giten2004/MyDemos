using System;
using com.latencybusters.lbm;

namespace lbmreq
{
    class LBMreqEventQueue : LBMEventQueue
    {
        public LBMreqEventQueue()
        {
            addMonitor(monitor);
        }

        protected void monitor(object cbarg, int evtype, int evq_size, long evq_delay)
        {
            Console.Error.WriteLine("Event Queue Monitor: Type: " + evtype +
                                                     ", Size: " + evq_size +
                                                     ", Delay: " + evq_delay + " usecs.");
        }
    }
}
