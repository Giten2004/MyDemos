using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using com.latencybusters.lbm;

namespace ReqRep.Request.SimpleRequest
{
    class SimpleRequestNOImmediate
    {
        public void Init()
        {
            LBMContext lbmContext = new LBMContext();
            var topicString = "bxu";

            var lbmTopic = lbmContext.allocTopic(topicString, null);
            var lbmSource = lbmContext.createSource(lbmTopic, onSourceEvent, null, null);

            for (int i = 0; i < 10; i++)
            {
                var requestMsg = Encoding.UTF8.GetBytes(string.Format("request {0}", i));
                LBMRequest lbmRequest = new LBMRequest(requestMsg, requestMsg.Length);
                lbmRequest.addResponseCallback(onResponse);

                Console.Out.WriteLine("Sending SimpleRequestNOImmediate {0}", i);
                lbmSource.send(lbmRequest, 0);

                Thread.Sleep(100);
            }

            Console.ReadLine();
        }

        private void onSourceEvent(object arg, LBMSourceEvent sourceEvent)
        {
            string clientname;

            switch (sourceEvent.type())
            {
                case LBM.SRC_EVENT_CONNECT:
                    clientname = sourceEvent.dataString();
                    Console.Out.WriteLine("Receiver connect " + clientname);
                    break;
                case LBM.SRC_EVENT_DISCONNECT:
                    clientname = sourceEvent.dataString();
                    Console.Out.WriteLine("Receiver disconnect " + clientname);
                    break;
                default:
                    break;
            }
            Console.Out.Flush();
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
