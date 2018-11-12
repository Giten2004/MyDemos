using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using ZeroMQ;
using Common;

namespace InterBrokerRouting
{
    class Program
    {
        /// <summary>
        /// We need two queues, one for requests from local clients and one for requests from cloud
        /// clients.One option would be to pull messages off the local and cloud frontends and
        /// pump these onto their respective queues.But this is kind of pointless, because ØMQ
        /// sockets are queues already. So let’s use the ØMQ socket buffers as queues.
        /// 
        /// We use broker identities to route messages between brokers. 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var options = new Options();
            var parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));

            if (!parser.ParseArguments(args, options))
                Environment.Exit(1);

            using (var context = new ZContext())
            using (ZSocket cloudFrontend = new ZSocket(context, ZSocketType.ROUTER),
                           cloudBackend = new ZSocket(context, ZSocketType.ROUTER))
            using (ZSocket localFrontend = new ZSocket(context, ZSocketType.ROUTER),
                           localBackend = new ZSocket(context, ZSocketType.ROUTER))
            using (ZSocket stateFrontend = new ZSocket(context, ZSocketType.SUB),
                           stateBackend = new ZSocket(context, ZSocketType.PUB))
            using (ZSocket monitorSocket = new ZSocket(context, ZSocketType.PULL))
            {
                var self = options.LocalFrontEndPoint;
                // Prepare local frontend and backend
                Console.WriteLine("localFrontend binding {0}", options.LocalFrontEndPoint);
                localFrontend.Bind(options.LocalFrontEndPoint);

                Console.WriteLine("localBackend binding {0}", options.LocalBackendPoint);
                localBackend.Bind(options.LocalBackendPoint);

                // Bind cloud frontend to endpoint. used to communicate with other broker in cluster
                cloudFrontend.IdentityString = self;
                Console.WriteLine("cloudFrontend binding {0}", options.CloudFrontendPoint);
                cloudFrontend.Bind(options.CloudFrontendPoint);

                // Connect cloud backend to all peers
                cloudBackend.IdentityString = self;
                foreach (var cloudBackendPoint in options.CloudBackendPoints)
                {
                    Console.WriteLine("I: connecting to cloud frontend at {0}", cloudBackendPoint);
                    cloudBackend.Connect(cloudBackendPoint);
                }

                //  Bind state backend to endpoint
                Console.WriteLine("stateBackend binding {0}", options.StateBackendPoint);
                stateBackend.Bind(options.StateBackendPoint);

                //  Connect state frontend to all peers           
                foreach (var stateFrontendPoint in options.StateFrontendPoints)
                {
                    Console.WriteLine("I: connecting to state backend at {0}", stateFrontendPoint);
                    stateFrontend.Connect(stateFrontendPoint);
                }

                //  Prepare monitor socket
                Console.WriteLine("monitorSocket binding {0}", options.MonitorPoint);
                monitorSocket.Bind(options.MonitorPoint);

                // Get user to tell us when we can start...
                Console.WriteLine("Press ENTER when all brokers are started...");
                Console.ReadLine();

                //  Queue of available workers
                int local_worker_capacity = 0;
                int cloud_worker_capacity = 0;

                var localworkerList = new List<string>();
                //  .split main loop
                //  The main loop has two parts. First, we poll workers and our two service
                //  sockets (statefe and monitor), in any case. If we have no ready workers,
                //  then there's no point in looking at incoming requests. These can remain 
                //  on their internal 0MQ queues:

                ZError error;
                ZMessage incoming;
                TimeSpan? waitTimeSpan;

                var poll = ZPollItem.CreateReceiver();

                while (true)
                {
                    //  Track if capacity changes during this iteration
                    int previous = local_worker_capacity;

                    // If we have no workers, wait indefinitely
                    waitTimeSpan = localworkerList.Count > 0 ? (TimeSpan?)TimeSpan.FromMilliseconds(1000) : null;
                    Console.WriteLine("workers count:{0}, waitTime:{1}", localworkerList.Count, waitTimeSpan);

                    ZMessage msgFromWorker, msgFromCloudBacked;

                    // Poll localBackend
                    if (localBackend.PollIn(poll, out msgFromWorker, out error, waitTimeSpan))
                    {
                        msgFromWorker.DumpZmsg("--------------------------");
                        // Handle reply from local worker
                        string identity = msgFromWorker[0].ReadString();
                        localworkerList.Add(identity);
                        // If it's READY, don't route the message any further
                        string hello = msgFromWorker[2].ReadString();
                        if (hello == "READY")
                        {
                            using (msgFromWorker)
                            {
                                local_worker_capacity++;
                                Console.WriteLine("Local worker is READY, don't route the ready message");
                            }
                            msgFromWorker = null;
                        }
                    }
                    else if (error == ZError.EAGAIN && cloudBackend.PollIn(poll, out msgFromCloudBacked, out error, waitTimeSpan))
                    {
                        //Handle reply from peer broker

                        // We don't use peer broker identity for anything
                        using (msgFromCloudBacked)
                        {
                            string identity = msgFromCloudBacked[0].ReadString();

                            // string ok = incoming[2].ReadString();
                        }
                        msgFromCloudBacked = null;
                    }
                    else
                    {
                        if (error == ZError.ETERM)
                            return; // Interrupted

                        if (error != ZError.EAGAIN)
                            throw new ZException(error);
                    }

                    //  Route worker reply to cloud if it's addressed to a broker
                    if (msgFromWorker != null)
                    {
                        Console.WriteLine("Route reply to cloud if it's addressed to a broker");
                        // Route reply to cloud if it's addressed to a broker
                        string identity = msgFromWorker[0].ReadString();
                        Console.WriteLine("identity:{0}", identity);

                        foreach (var cloudBackendPoint in options.CloudBackendPoints)
                        {
                            if (identity == cloudBackendPoint)
                            {
                                using (msgFromWorker)
                                    cloudFrontend.Send(msgFromWorker);

                                msgFromWorker = null;
                                break;
                            }
                        }
                    }

                    //  Route reply to client if we still need to
                    if (msgFromWorker != null)
                    {
                        Console.WriteLine("Send the worker msg to client");
                        localFrontend.Send(msgFromWorker);
                    }

                    //  .split handle state messages
                    //  If we have input messages on our statefe or monitor sockets, we
                    //  can process these immediately:
                    //using (var stateMsg = stateFrontend.ReceiveMessage())
                    //{
                    //    var peer = stateMsg[0].ReadString(Encoding.UTF8);
                    //    var status = stateMsg[1].ReadInt32();
                    //    cloud_worker_capacity = status;
                    //}

                    //using (var monitorMsg = monitorSocket.ReceiveMessage())
                    //{
                    //    Console.WriteLine(monitorMsg[0].ReadString(Encoding.UTF8));
                    //}

                    //  .split route client requests
                    //  Now route as many clients requests as we can handle. If we have
                    //  local capacity, we poll both localfe and cloudfe. If we have cloud
                    //  capacity only, we poll just localfe. We route any request locally
                    //  if we can, else we route to the cloud.

                    ZMessage localFrontMsg = null;
                    ZMessage cloudFrontMsg = null;
                    while (local_worker_capacity + cloud_worker_capacity > 0)
                    {
                        if (localFrontend.PollIn(poll, out localFrontMsg, out error, new TimeSpan(10 * 1000)))
                        {
                            if (local_worker_capacity > 0)
                            {
                                //zframe_t* frame = (zframe_t*)zlist_pop(workers);
                                //zmsg_wrap(msg, frame);
                                //zmsg_send(&msg, localbe);

                                var worker = localworkerList[0];
                                localworkerList.RemoveAt(0);

                                //send to client msg to worker, there is no identify
                                localBackend.Send(new ZFrame(worker));

                                local_worker_capacity--;
                            }
                        }

                        if (local_worker_capacity <= 0)
                        {
                            if (cloudFrontend.PollIn(poll, out cloudFrontMsg, out error, new TimeSpan(10 * 1000)))
                            {
                                //  Route to random broker peer
                                var tempRamdom = new Random(options.CloudBackendPoints.Count - 1);
                                var cbIndex = tempRamdom.Next();
                                cloudBackend.Send(new ZFrame(options.CloudBackendPoints[cbIndex]));
                                //int peer = randof(argc - 2) + 2;
                                //zmsg_pushmem(msg, argv[peer], strlen(argv[peer]));
                                //zmsg_send(&msg, cloudbe);
                            }
                        }

                        if (localFrontMsg == null && cloudFrontMsg == null)
                            //  No work, go back to primary
                            break;
                    }

                    //  .split broadcast capacity
                    //  We broadcast capacity messages to other peers; to reduce chatter,
                    //  we do this only if our capacity changed.

                    if (local_worker_capacity != previous)
                    {
                        //  We stick our own identity onto the envelope
                        var statemsg = new ZMessage();

                        statemsg.Add(new ZFrame(self));
                        statemsg.Add(new ZFrame(local_worker_capacity));

                        //  Broadcast new capacity
                        stateBackend.Send(statemsg);
                    }
                }
                //end of while true
            }
        }
    }
}
