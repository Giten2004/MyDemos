using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using com.latencybusters.lbm;

namespace ReqRep.Request.SimpleRequest
{
    class SimpleRequestImmediate
    {
        public void Init()
        {
            LBMContext ldbmContext = new LBMContext();
            var topicString = "bxu";

            for (int i = 0; i < 10; i++)
            {
                var requestMsg = Encoding.UTF8.GetBytes(string.Format("request {0}", i));
                LBMRequest lbmRequest = new LBMRequest(requestMsg, requestMsg.Length);
                lbmRequest.addResponseCallback(onResponse);

                Console.Out.WriteLine("Sending SimpleRequestImmediate {0}", i);
                ldbmContext.send(topicString, lbmRequest, 0);

                Thread.Sleep(1000);
            }
        }

        private int onResponse(object cbArg, LBMRequest req, LBMMessage msg)
        {
            switch (msg.type())
            {
                case LBM.MSG_RESPONSE:
                    Console.Out.WriteLine("LBM.MSG_RESPONSE [" + msg.source() + "][" + msg.sequenceNumber() + "], " + msg.data().Length + " bytes");

                    break;
                default:
                    Console.Out.WriteLine("Unknown message type " + msg.type() + "[" + msg.source() + "]");
                    break;
            }
            msg.dispose();
            Console.Out.Flush();
            return 0;
        }
    }
}
