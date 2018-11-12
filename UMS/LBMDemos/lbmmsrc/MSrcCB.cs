using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.latencybusters.lbm;

namespace LBMApplication
{
    class MSrcCB
    {
        public void onSourceEvent(Object arg, LBMSourceEvent sourceEvent)
        {
            string clientname;

            switch (sourceEvent.type())
            {
                case LBM.SRC_EVENT_CONNECT:
                    clientname = sourceEvent.dataString();
                    System.Console.Out.WriteLine("Receiver connect " + clientname);
                    break;
                case LBM.SRC_EVENT_DISCONNECT:
                    clientname = sourceEvent.dataString();
                    System.Console.Out.WriteLine("Receiver disconnect " + clientname);
                    break;
                default:
                    break;
            }
            System.Console.Out.Flush();
        }
    }
}
