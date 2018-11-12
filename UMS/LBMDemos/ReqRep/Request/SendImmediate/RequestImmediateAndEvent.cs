using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.latencybusters.lbm;
using ReqRep.Request;

namespace ReqRep
{
    public class RequestImmediateAndEvent
    {
        public void Init()
        {
            LBMContext lbmContext = new LBMContext();
            LBMEventQueue lbmEventQueue = new LBMEventQueue();

            var topicString = "bxu";

            LBMreqCB lbMreqCallBack = new LBMreqCB(0);

            for (int i = 0; i < 10; i++)
            {
                var requestMsg = Encoding.UTF8.GetBytes(string.Format("request {0}", i));
                LBMRequest lbmRequest = new LBMRequest(requestMsg, requestMsg.Length);
                lbmRequest.addResponseCallback(lbMreqCallBack.onResponse);

                Console.Out.WriteLine("Sending RequestImmediateAndEvent {0}", i);

                lbmContext.send(topicString, lbmRequest, lbmEventQueue, 0);

                var qTimer = new EQTimer(lbmContext, 5000, lbmEventQueue);
                lbmEventQueue.run(LBM.EVENT_QUEUE_BLOCK);

                Console.Out.WriteLine("Done waiting for responses, {0} response{1} ({2} total bytes) received. Deleting request.\n",
                    lbMreqCallBack.response_count, (lbMreqCallBack.response_count == 1 ? "" : "s"), lbMreqCallBack.response_byte_count);

                lbMreqCallBack.response_count = 0;
                lbMreqCallBack.response_byte_count = 0;

                lbmRequest.close();

            }

            Console.ReadLine();
        }
    }
}
