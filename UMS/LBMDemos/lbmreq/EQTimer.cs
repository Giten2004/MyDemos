using com.latencybusters.lbm;

namespace lbmreq
{
    class EQTimer : LBMTimer
    {
        LBMEventQueue _evq;

        public EQTimer(LBMContext lbmctx, long milliseconds, LBMEventQueue lbmevq)
            : base(lbmctx, milliseconds, lbmevq)
        {
            _evq = lbmevq;
            addTimerCallback(onExpiration);
        }

        private void onExpiration(object arg)
        {
            _evq.stop();
        }
    }
}
