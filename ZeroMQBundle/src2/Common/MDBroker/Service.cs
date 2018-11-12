using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace Common.MDBroker
{
    public class Service : IDisposable
    {
        // Broker Instance
        public Broker Broker { get; protected set; }

        // Service Name
        public string Name { get; protected set; }

        // List of client requests
        public List<ZMessage> Requests { get; protected set; }

        // List of waiting workers
        public List<Worker> Waiting { get; protected set; }

        // How many workers we are
        public int Workers;
        //ToDo check workers var

        internal Service(Broker broker, string name)
        {
            Broker = broker;
            Name = name;
            Requests = new List<ZMessage>();
            Waiting = new List<Worker>();
        }

        ~Service()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var r in Requests)
                {
                    // probably obsolete?
                    using (r)
                    {
                    }
                }
            }
        }

        //  .split service dispatch method
        //  This method sends requests to waiting workers:
        public void Dispatch(ZMessage msg)
        {
            if (msg != null) // queue msg if any
                Requests.Add(msg);

            Broker.Purge();
            while (Waiting.Count > 0
                   && Requests.Count > 0)
            {
                Worker worker = Waiting[0];
                Waiting.RemoveAt(0);
                Broker.Waiting.Remove(worker);
                ZMessage reqMsg = Requests[0];
                Requests.RemoveAt(0);
                using (reqMsg)
                    worker.Send(MdpCommon.MdpwCmd.REQUEST.ToHexString(), null, reqMsg);
            }
        }
    }
}
