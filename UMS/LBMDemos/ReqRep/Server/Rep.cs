using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.latencybusters.lbm;

namespace ReqRep
{
    class Rep
    {
        public void Init()
        {
            LBMObjectRecycler objRec = new LBMObjectRecycler();
            //Lower the defaults for messages since we expect a lower rate that request will be arriving
            objRec.setLocalMsgPoolSize(10);
            objRec.setSharedMsgPoolSize(20);

            LBMContextAttributes lbmContextAttributes = new LBMContextAttributes();
            lbmContextAttributes.setObjectRecycler(objRec, null);
          
            LBMReceiverAttributes lbmReceiverAttributes = new LBMReceiverAttributes();
            lbmReceiverAttributes.setObjectRecycler(objRec, null);

            LBMContext lbmContext = new LBMContext(lbmContextAttributes);
            LBMTopic lbmTopic = new LBMTopic(lbmContext, "test.topic", lbmReceiverAttributes);
           
            var lbmEventQueue = new LBMRespEventQueue();
           
            var lbmReveiver = new LBMRespReceiver(lbmContext, lbmTopic, lbmEventQueue, 1, false);
         
            lbmContext.enableImmediateMessageReceiver(lbmEventQueue);
            lbmContext.addImmediateMessageReceiver(lbmReveiver.onReceiveImmediate);

            lbmEventQueue.run(0);

            Console.ReadLine();
        }
    }
}
