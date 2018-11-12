using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace Common.PP
{
    public class Worker : IDisposable
    {
        public const int PPP_HEARTBEAT_LIVENESS = 3; // 3-5 is reasonable
        public static readonly TimeSpan PPP_HEARTBEAT_INTERVAL = TimeSpan.FromMilliseconds(500);
        public static readonly TimeSpan PPP_TICK = TimeSpan.FromMilliseconds(250);

        public const string PPP_READY = "READY";
        public const string PPP_HEARTBEAT = "HEARTBEAT";

        public const int PPP_INTERVAL_INIT = 1000;
        public const int PPP_INTERVAL_MAX = 32000;

        public ZFrame Identity;

        public DateTime Expiry;

        public string IdentityString
        {
            get
            {
                Identity.Position = 0;
                return Identity.ReadString();
            }
            set
            {
                if (Identity != null)
                {
                    Identity.Dispose();
                }
                Identity = new ZFrame(value);
            }
        }

        // Construct new worker
        public Worker(ZFrame identity)
        {
            Identity = identity;

            this.Expiry = DateTime.UtcNow + TimeSpan.FromMilliseconds(
                PPP_HEARTBEAT_INTERVAL.Milliseconds * PPP_HEARTBEAT_LIVENESS
            );
        }

        // Destroy specified worker object, including identity frame.
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Identity != null)
                {
                    Identity.Dispose();
                    Identity = null;
                }
            }
        }
    }
}
