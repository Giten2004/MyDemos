using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.latencybusters.lbm;

namespace LBMApplication
{
    class SrcStatsTimer : LBMTimer
    {
        private LBMSource _src;
        private bool _done = false;
        private long _milliseconds;
        private LBMObjectRecyclerBase _recycler;

        public SrcStatsTimer(LBMContext lbmctx, LBMSource src, long milliseconds, LBMEventQueue lbmevq, LBMObjectRecyclerBase recycler) 
            : base(lbmctx, milliseconds, lbmevq)
        {
            _recycler = recycler;
            _src = src;
            _milliseconds = milliseconds;

            if (milliseconds == 0)
                print_stats();
            else
                this.addTimerCallback(new LBMTimerCallback(onExpiration));
        }

        public void done()
        {
            _done = true;
        }

        private void onExpiration(object arg)
        {
            print_stats();

            if (!_done)
            {
                this.reschedule(_milliseconds);
            }
        }

        private void print_stats()
        {
            LBMSourceStatistics stats = _src.getStatistics();

            switch (stats.type())
            {
                case LBM.TRANSPORT_STAT_TCP:
                    System.Console.Out.WriteLine("TCP, buffered " + stats.bytesBuffered() + ", clients " + stats.numberOfClients());
                    break;
                case LBM.TRANSPORT_STAT_LBTRU:
                    System.Console.Out.WriteLine("LBT-RU, sent " + stats.messagesSent()
                                                   + "/" + stats.bytesSent()
                                                   + ", naks " + stats.naksReceived()
                                                   + "/" + stats.nakPacketsReceived()
                                                   + ", ignored " + stats.naksIgnored()
                                                   + "/" + stats.naksIgnoredRetransmitDelay()
                                                + ", shed " + stats.naksShed()
                                                   + ", rxs " + stats.retransmissionsSent()
                                                   + ", clients " + stats.numberOfClients());
                    break;
                case LBM.TRANSPORT_STAT_LBTRM:
                    System.Console.Out.WriteLine("LBT-RM, sent " + stats.messagesSent()
                                                   + "/" + stats.bytesSent()
                                                   + ", txw " + stats.transmissionWindowMessages()
                                                   + "/" + stats.transmissionWindowBytes()
                                                   + ", naks " + stats.naksReceived()
                                                   + "/" + stats.nakPacketsReceived()
                                                   + ", ignored " + stats.naksIgnored()
                                                   + "/" + stats.naksIgnoredRetransmitDelay()
                                                   + ", shed " + stats.naksShed()
                                                   + ", rxs " + stats.retransmissionsSent()
                                                   + ", rctl " + stats.messagesQueued()
                                                   + "/" + stats.retransmissionsQueued());
                    break;
                case LBM.TRANSPORT_STAT_LBTIPC:
                    System.Console.Out.WriteLine("LBT-IPC, source " + stats.source()
                                                + " clients " + stats.numberOfClients()
                                                + ", sent " + stats.messagesSent()
                                                + "/" + stats.bytesSent());
                    break;
                case LBM.TRANSPORT_STAT_LBTRDMA:
                    System.Console.Out.WriteLine("LBT-RDMA, source " + stats.source()
                                                + " clients " + stats.numberOfClients()
                                                + ", sent " + stats.messagesSent()
                                                + "/" + stats.bytesSent());
                    break;
            }
            _recycler.doneWithSourceStatistics(stats);

            System.Console.Out.Flush();
        }
    }
}