using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.latencybusters.lbm;

namespace ReqRep.Request
{
    class EQTimer : LBMTimer
    {
        LBMEventQueue _lbmEventQueue;

        public EQTimer(LBMContext lbmctx, long milliseconds, LBMEventQueue lbmevq)
            : base(lbmctx, milliseconds, lbmevq)
        {
            _lbmEventQueue = lbmevq;
            addTimerCallback(onExpiration);
        }

        private void onExpiration(object arg)
        {
            _lbmEventQueue.stop();
        }
    }
}
