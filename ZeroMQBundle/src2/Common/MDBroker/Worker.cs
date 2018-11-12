using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace Common.MDBroker
{
    public class Worker
    {
        //  .split worker class structure
        //  The worker class defines a single worker, idle or active:

        // Broker Instance
        public Broker Broker { get; protected set; }

        // Identity of worker as string
        public string IdString { get; protected set; }

        // Identity frame for routing
        public ZFrame Identity { get; protected set; }

        //Ownling service, if known
        public Service Service { get; set; }

        //When worker expires, if no heartbeat; 
        public DateTime Expiry { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idString"></param>
        /// <param name="broker"></param>
        /// <param name="identity">will be dublicated inside the constructor</param>
        public Worker(string idString, Broker broker, ZFrame identity)
        {
            Broker = broker;
            IdString = idString;
            Identity = identity.Duplicate();
        }
        ~Worker()
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
                using (Identity)
                { }
            }
        }

        public void Delete(bool disconnect)
        {
            if (disconnect)
                Send(MdpCommon.MdpwCmd.DISCONNECT.ToHexString(), null, null);

            if (Service != null)
            {
                Service.Waiting.Remove(this);
                Service.Workers--;
            }

            Broker.Waiting.Remove(this);
            if (Broker.Workers.ContainsKey(IdString))
                Broker.Workers.Remove(IdString);
        }

        //  .split worker send method
        //  This method formats and sends a command to a worker. The caller may
        //  also provide a command option, and a message payload:
        public void Send(string command, string option, ZMessage msg)
        {
            msg = msg != null
                    ? msg.Duplicate()
                    : new ZMessage();

            // Stack protocol envelope to start of message
            if (!string.IsNullOrEmpty(option))
                msg.Prepend(new ZFrame(option));
            msg.Prepend(new ZFrame(command));
            msg.Prepend(new ZFrame(MdpCommon.MDPW_WORKER));

            // Stack routing envelope to start of message
            msg.Wrap(Identity.Duplicate());

            if (Broker.Verbose)
                msg.DumpZmsg("I: sending '{0:X}|{0}' to worker", command.ToMdCmd());

            Broker.Socket.Send(msg);
        }

        // This worker is now waiting for work 
        public void Waiting()
        {
            // queue to broker and service waiting lists
            if (Broker == null)
                throw new InvalidOperationException();
            Broker.Waiting.Add(this);
            Service.Waiting.Add(this);
            Expiry = DateTime.UtcNow + MdpCommon.HEARTBEAT_EXPIRY;
            Service.Dispatch(null);
        }
    }
}
