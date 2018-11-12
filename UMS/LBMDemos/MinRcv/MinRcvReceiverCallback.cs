using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using com.latencybusters.lbm;

namespace MinRcv
{
    class MinRcvReceiverCallback
    {
        private int _delay = 0;

        public MinRcvReceiverCallback(int delay)
        {
            _delay = delay;
        }

        public int onReceive(Object cbArgs, LBMMessage lbmMsg)
        {
            /* There are several different events that can cause the
             *  receiver callbackto be called.  Decode the event that
             * caused this.  */
            switch (lbmMsg.type())
            {
                case LBM.MSG_DATA:

                    /* NOTE:  Normally it would be a bad idea to do
                     * something as slow as a print statement in the
                     * callback function itself.  In this example, we'll
                     * probably only receive one message, so it doesn't
                     * matter.
                     */
                    System.Console.WriteLine("Received " + lbmMsg.length() + " bytes on topic " + lbmMsg.topicName() + ": '" + Encoding.UTF8.GetString(lbmMsg.data()) + "'");

                    Thread.Sleep(_delay);
                    break;

                case LBM.MSG_BOS:
                    System.Console.WriteLine("[" + lbmMsg.topicName() + "][" + lbmMsg.source() + "], Beginning of Transport Session");
                    break;

                case LBM.MSG_EOS:
                    System.Console.WriteLine("[" + lbmMsg.topicName() + "][" + lbmMsg.source() + "], End of Transport Session");
                    break;

                default:
                    System.Console.WriteLine("unexpected event: " + lbmMsg.type());
                    System.Environment.Exit(1);
                    break;
            }
            lbmMsg.dispose();
            /*
             * Return 0 if there were no errors. Returning a non-zero value will
             * cause LBM to log a generic error message.
             */
            return 0;
        }
    }  /* MinRcvReceiverCallback */
}
