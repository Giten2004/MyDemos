using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using com.latencybusters.lbm;

namespace MinSrc
{
    class MiniServer
    {
        public void Init()
        {
            using (LBMContext myContext = new LBMContext())
            {
                LBMTopic myTopic = new LBMTopic(myContext, "Greeting", new LBMSourceAttributes());
                using (LBMSource mySource = new LBMSource(myContext, myTopic))
                {
                    var index = 1;
                    while (true)
                    {
                        var msg = string.Format("Hello {0} !", index);
                        byte[] myMessage = Encoding.UTF8.GetBytes(msg);

                        Console.WriteLine(msg);
                        mySource.send(myMessage, myMessage.Length, LBM.MSG_FLUSH | LBM.SRC_BLOCK);

                        index++;

                        Thread.Sleep(1000);
                    }
                }
            }
        }
    }
}
