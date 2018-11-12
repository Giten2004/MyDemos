using Common;
using Common.PP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace ParanoidPirateQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new ZContext())
            using (var backend = new ZSocket(context, ZSocketType.ROUTER))
            using (var frontend = new ZSocket(context, ZSocketType.ROUTER))
            {
                backend.Bind("tcp://*:5556");
                frontend.Bind("tcp://*:5555");

                // List of available workers
                var workers = new List<Worker>();

                // Send out heartbeats at regular intervals
                DateTime heartbeat_at = DateTime.UtcNow + Worker.PPP_HEARTBEAT_INTERVAL;

                // Create a Receiver ZPollItem (ZMQ_POLLIN)
                var poll = ZPollItem.CreateReceiver();

                ZError error;
                ZMessage incoming;

                while (true)
                {
                    // Handle worker activity on backend
                    if (backend.PollIn(poll, out incoming, out error, Worker.PPP_TICK))
                    {
                        using (incoming)
                        {
                            incoming.DumpZmsg("--------from worker---------");

                            // Any sign of life from worker means it's ready
                            ZFrame identity = incoming.Unwrap();
                            var worker = new Worker(identity);
                            workers.Ready(worker);

                            // Validate control message, or return reply to client
                            if (incoming.Count == 1)
                            {
                                string message = incoming[0].ReadString();
                                if (message == Worker.PPP_READY)
                                {
                                    Console.WriteLine("I:        worker ready ({0})", worker.IdentityString);
                                }
                                else if (message == Worker.PPP_HEARTBEAT)
                                {
                                    Console.WriteLine("I: receiving heartbeat ({0})", worker.IdentityString);
                                }
                                else
                                {
                                    ProgramerHelper.Console_WriteZMessage("E: invalid message from worker", incoming);
                                }
                            }
                            else
                            {
                                if (ProgramerHelper.Verbose)
                                    ProgramerHelper.Console_WriteZMessage("I: [backend sending to frontend] ({0})", incoming, worker.IdentityString);

                                frontend.Send(incoming);
                            }
                        }
                    }
                    else
                    {
                        if (error == ZError.ETERM)
                            break;  // Interrupted
                        if (error != ZError.EAGAIN)
                            throw new ZException(error);
                    }

                    // Handle client activity on frontend
                    if (workers.Count > 0)
                    {
                        // Poll frontend only if we have available workers
                        if (frontend.PollIn(poll, out incoming, out error, Worker.PPP_TICK))
                        {
                            // Now get next client request, route to next worker
                            using (incoming)
                            {
                                ZFrame workerIdentity = workers.Next();
                                incoming.Prepend(workerIdentity);

                                if (ProgramerHelper.Verbose)
                                    ProgramerHelper.Console_WriteZMessage("I: [frontend sending to backend] ({0})", incoming, workerIdentity.ReadString());

                                backend.Send(incoming);
                            }
                        }
                        else
                        {
                            if (error == ZError.ETERM)
                                break;  // Interrupted
                            if (error != ZError.EAGAIN)
                                throw new ZException(error);
                        }
                    }

                    // We handle heartbeating after any socket activity. First, we send
                    // heartbeats to any idle workers if it's time. Then, we purge any
                    // dead workers:
                    if (DateTime.UtcNow > heartbeat_at)
                    {
                        heartbeat_at = DateTime.UtcNow + Worker.PPP_HEARTBEAT_INTERVAL;

                        foreach (Worker worker in workers)
                        {
                            using (var outgoing = new ZMessage())
                            {
                                outgoing.Add(ZFrame.CopyFrom(worker.Identity));
                                outgoing.Add(new ZFrame(Worker.PPP_HEARTBEAT));

                                Console.WriteLine("I:   sending heartbeat ({0})", worker.IdentityString);
                                backend.Send(outgoing);
                            }
                        }
                    }

                    workers.Purge();
                }

                // When we're done, clean up properly
                foreach (Worker worker in workers)
                {
                    worker.Dispose();
                }
            }
        }
    }
}
