using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReqRep.Request.RequestWithEvent;
using ReqRep.Request.SimpleRequest;

namespace ReqRep
{
    class Program
    {
        static void Main(string[] args)
        {
            //1)send Immediate
            //var req = new SimpleRequestImmediate();
            //var req = new RequestImmediateAndEvent();

            //2)send NoImmediate()
            //var req = new SimpleRequestNOImmediate();
            var req = new RequestNOImmediateAndEvent();
            

            var rep = new Rep();

            var requestThread = new System.Threading.Thread(req.Init);
            //var responseThread = new System.Threading.Thread(rep.Init);

            //responseThread.Start();
            //Thread.Sleep(10000);
            requestThread.Start();

            Console.ReadLine();
        }
    }
}
