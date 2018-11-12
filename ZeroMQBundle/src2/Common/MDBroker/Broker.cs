using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace Common.MDBroker
{
    public class Broker : IDisposable
    {
        //  Majordomo Protocol broker
        //  A minimal C implementation of the Majordomo Protocol as defined in
        //  http://rfc.zeromq.org/spec:7 and http://rfc.zeromq.org/spec:8.

        //  .split broker class structure
        //  The broker class defines a single broker instance:

        // Our Context
        private ZContext _context;

        //Socket for clients & workers
        public ZSocket Socket;

        //  Print activity to console
        public bool Verbose { get; protected set; }

        //  Broker binds to this endpoint
        public string Endpoint { get; protected set; }

        // Hash of known services
        public Dictionary<string, Service> Services;

        // Hash of known workes
        public Dictionary<string, Worker> Workers;

        // Waiting workers
        public List<Worker> Waiting;

        // When to send HEARTBEAT
        public DateTime HeartbeatAt;

        // waiting
        // heartbeat_at

        public Broker(bool verbose)
        {
            // Constructor
            _context = new ZContext();

            Socket = new ZSocket(_context, ZSocketType.ROUTER);

            Verbose = verbose;

            Services = new Dictionary<string, Service>(); //new HashSet<Service>();
            Workers = new Dictionary<string, Worker>(); //new HashSet<Worker>();
            Waiting = new List<Worker>();

            HeartbeatAt = DateTime.UtcNow + MdpCommon.HEARTBEAT_INTERVAL;
        }

        ~Broker()
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
                // Destructor
                if (Socket != null)
                {
                    Socket.Dispose();
                    Socket = null;
                }

                if (_context != null)
                {
                    // Do Context.Dispose()
                    _context.Dispose();
                    _context = null;
                }
            }
        }

        // added ShutdownContext, to call shutdown in main method.
        public void ShutdownContext()
        {
            if (_context != null)
                _context.Shutdown();
        }

        //  .split broker bind method
        //  This method binds the broker instance to an endpoint. We can call
        //  this multiple times. Note that MDP uses a single Socket for both clients 
        //  and workers:
        public void Bind(string endpoint)
        {
            Socket.Bind(endpoint);
            // if you dont wanna see utc timeshift, remove zzz and use DateTime.UtcNow instead
            "I: MDP broker/0.2.0 is active at {0}".DumpString(endpoint);
        }

        //  .split broker worker_msg method
        //  This method processes one READY, REPLY, HEARTBEAT, or
        //  DISCONNECT message sent to the broker by a worker:
        public void WorkerMsg(ZFrame sender, ZMessage msg)
        {
            if (msg.Count < 1) // At least, command
                throw new InvalidOperationException();

            ZFrame command = msg.Pop();
            //string id_string = sender.ReadString();
            bool isWorkerReady;
            //string id_string;
            using (var sfrm = sender.Duplicate())
            {
                var idString = sfrm.Read().ToHexString();
                isWorkerReady = Workers.ContainsKey(idString);
            }
            Worker worker = RequireWorker(sender);
            using (msg)
            using (command)
            {
                if (command.StrHexEq(MdpCommon.MdpwCmd.READY))
                {
                    if (isWorkerReady)
                        // Not first command in session
                        worker.Delete(true);
                    else if (command.Length >= 4
                          && command.ToString().StartsWith("mmi."))
                        // Reserd servicee name
                        worker.Delete(true);
                    else
                    {
                        // Attach worker to service and mark as idle
                        using (ZFrame serviceFrame = msg.Pop())
                        {
                            worker.Service = RequireService(serviceFrame);
                            worker.Service.Workers++;
                            worker.Waiting();
                        }
                    }
                }
                else if (command.StrHexEq(MdpCommon.MdpwCmd.REPLY))
                {
                    if (isWorkerReady)
                    {
                        //  Remove and save client return envelope and insert the
                        //  protocol header and service name, then rewrap envelope.
                        ZFrame client = msg.Unwrap();
                        msg.Prepend(new ZFrame(worker.Service.Name));
                        msg.Prepend(new ZFrame(MdpCommon.MDPC_CLIENT));
                        msg.Wrap(client);
                        Socket.Send(msg);
                        worker.Waiting();
                    }
                    else
                    {
                        worker.Delete(true);
                    }
                }
                else if (command.StrHexEq(MdpCommon.MdpwCmd.HEARTBEAT))
                {
                    if (isWorkerReady)
                    {
                        worker.Expiry = DateTime.UtcNow + MdpCommon.HEARTBEAT_EXPIRY;
                    }
                    else
                    {
                        worker.Delete(true);
                    }
                }
                else if (command.StrHexEq(MdpCommon.MdpwCmd.DISCONNECT))
                    worker.Delete(false);
                else
                {
                    msg.DumpZmsg("E: invalid input message");
                }
            }
        }

        //  .split broker client_msg method
        //  Process a request coming from a client. We implement MMI requests
        //  directly here (at present, we implement only the mmi.service request):
        public void ClientMsg(ZFrame sender, ZMessage msg)
        {
            // service & body
            if (msg.Count < 2)
                throw new InvalidOperationException();

            using (ZFrame serviceFrame = msg.Pop())
            {
                Service service = RequireService(serviceFrame);

                // Set reply return identity to client sender
                msg.Wrap(sender.Duplicate());

                //if we got a MMI Service request, process that internally
                if (serviceFrame.Length >= 4
                    && serviceFrame.ToString().StartsWith("mmi."))
                {
                    string returnCode;
                    if (serviceFrame.ToString().Equals("mmi.service"))
                    {
                        string name = msg.Last().ToString();
                        returnCode = Services.ContainsKey(name)
                                     && Services[name].Workers > 0
                                        ? "200"
                                        : "404";
                    }
                    else
                        returnCode = "501";

                    var client = msg.Unwrap();

                    msg.Clear();
                    msg.Add(new ZFrame(returnCode));
                    msg.Prepend(serviceFrame);
                    msg.Prepend(new ZFrame(MdpCommon.MDPC_CLIENT));

                    msg.Wrap(client);
                    Socket.Send(msg);
                }
                else
                {
                    // Else dispatch the message to the requested Service
                    service.Dispatch(msg);
                }
            }
        }

        //  .split broker purge method
        //  This method deletes any idle workers that haven't pinged us in a
        //  while. We hold workers from oldest to most recent so we can stop
        //  scanning whenever we find a live worker. This means we'll mainly stop
        //  at the first worker, which is essential when we have large numbers of
        //  workers (we call this method in our critical path):

        public void Purge()
        {
            Worker worker = Waiting.FirstOrDefault();
            while (worker != null)
            {
                if (DateTime.UtcNow < worker.Expiry)
                    break;   // Worker is alive, we're done here
                if (Verbose)
                    "I: deleting expired worker: '{0}'".DumpString(worker.IdString);

                worker.Delete(false);
                worker = Waiting.FirstOrDefault();
            }
        }

        //  .split service methods
        //  Here is the implementation of the methods that work on a service:

        //  Lazy constructor that locates a service by name or creates a new
        //  service if there is no service already with that name.
        public Service RequireService(ZFrame serviceFrame)
        {
            if (serviceFrame == null)
                throw new InvalidOperationException();

            string name = serviceFrame.ToString();

            Service service;
            if (Services.ContainsKey(name))
            {
                service = Services[name];
            }
            else
            {
                service = new Service(this, name);
                Services[name] = service;

                //zhash_freefn(self->workers, id_string, s_worker_destroy);
                if (Verbose)
                    "I: added service: '{0}'".DumpString(name);
            }

            return service;
        }

        //  .split worker methods
        //  Here is the implementation of the methods that work on a worker:

        //  Lazy constructor that locates a worker by identity, or creates a new
        //  worker if there is no worker already with that identity.
        public Worker RequireWorker(ZFrame identity)
        {
            if (identity == null)
                throw new InvalidOperationException();

            string idString;
            using (var tstfrm = identity.Duplicate())
            {
                idString = tstfrm.Read().ToHexString();
            }

            Worker worker = Workers.ContainsKey(idString)
                ? Workers[idString]
                : null;

            if (worker == null)
            {
                worker = new Worker(idString, this, identity);
                Workers[idString] = worker;
                if (Verbose)
                    "I: registering new worker: '{0}'".DumpString(idString);
            }

            return worker;
        }
    }
}
