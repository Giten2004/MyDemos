
/*
  Copyright (c) 2005-2013 Informatica Corporation  Permission is granted to licensees to use
  or alter this software for any purpose, including commercial applications,
  according to the terms laid out in the Software License Agreement.

  This source code example is provided by Informatica for educational
  and evaluation purposes only.

  THE SOFTWARE IS PROVIDED "AS IS" AND INFORMATICA DISCLAIMS ALL WARRANTIES 
  EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION, ANY IMPLIED WARRANTIES OF 
  NON-INFRINGEMENT, MERCHANTABILITY OR FITNESS FOR A PARTICULAR 
  PURPOSE.  INFORMATICA DOES NOT WARRANT THAT USE OF THE SOFTWARE WILL BE 
  UNINTERRUPTED OR ERROR-FREE.  INFORMATICA SHALL NOT, UNDER ANY CIRCUMSTANCES, BE 
  LIABLE TO LICENSEE FOR LOST PROFITS, CONSEQUENTIAL, INCIDENTAL, SPECIAL OR 
  INDIRECT DAMAGES ARISING OUT OF OR RELATED TO THIS AGREEMENT OR THE 
  TRANSACTIONS CONTEMPLATED HEREUNDER, EVEN IF INFORMATICA HAS BEEN APPRISED OF 
  THE LIKELIHOOD OF SUCH DAMAGES.
*/
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using com.latencybusters.lbm;

namespace LBMApplication
{
    class lbmhfxrcv
    {
        [DllImport("Kernel32.dll")]
        public static extern int SetEnvironmentVariable(string name, string value);

        private const int DEFAULT_MAX_NUM_SRCS = 10000;
        private static int nstats = 10;
        private static int reap_msgs = 0;
        private static int stat_secs = 0;
        private static bool eventq = false;
        public static bool verbose = false;
        private static bool end_on_eos = false;
        private static bool summary = false;
        private static string purpose = "Purpose: Receive messages via a HFX.";
        public static bool verifiable = false;
        private static int deletions_complete = 0;
        private static LBMRcvEventQueue evq = null;
        private static string usage =
            "Usage: lbmhfxrcv [options] topic\n"
            + "Available options:\n"
            + "  -c filename = Use LBM configuration file filename.\n"
            + "                Multiple config files are allowed.\n"
            + "                Example:  '-c file1.cfg -c file2.cfg'\n"
            + "  -d qdelay = monitor event queue delay above qdelay usecs\n"
            + "  -E = exit after source ends\n"
            + "  -h = help\n"
            + "  -I CIDR = create a receiver on the interface specified by CIDR\n"
            + "            Multiple interfaces are allowed.\n"
            + "            Example:  '-I 10.29.1.0/24 -I 10.29.2.0/24\n"
            + "  -q = use an LBM event queue\n"
            + "  -S = exit after source ends, print throughput summary\n"
            + "  -s num_secs = print statistics every num_secs along with bandwidth\n"
            + "  -r msgs = delete receiver after msgs messages\n"
            + "  -v = be verbose about each message\n"
            + "  -V = verify message contents\n"
            + "  -z qsize = monitor event queue size above qsize in length\n"
            + "\nMonitoring options:\n"
            + "  --monitor-ctx NUM = monitor context every NUM seconds\n"
            + "  --monitor-rcv NUM = monitor receiver every NUM seconds\n"
            + "  --monitor-transport TRANS = use monitor transport module TRANS\n"
            + "                              TRANS may be `lbm', `udp', or `lbmsnmp', default is `lbm'\n"
            + "  --monitor-transport-opts OPTS = use OPTS as transport module options\n"
            + "  --monitor-format FMT = use monitor format module FMT\n"
            + "                         FMT may be `csv'\n"
            + "  --monitor-format-opts OPTS = use OPTS as format module options\n"
            + "  --monitor-appid ID = use ID as application ID string\n"
            ;

        static void Main(string[] args)
        {
            if (System.Environment.GetEnvironmentVariable("LBM_LICENSE_FILENAME") == null
                && System.Environment.GetEnvironmentVariable("LBM_LICENSE_INFO") == null)
            {
                SetEnvironmentVariable("LBM_LICENSE_FILENAME", "lbm_license.txt");
            }
            LBM lbm = new LBM();
            lbm.setLogger(new LBMLogging(logger));

            LBMObjectRecycler objRec = new LBMObjectRecycler();

            string qdelay = null;
            string qsize = null;
            int i;
            int n = args.Length;

            bool monitor_context = false;
            int monitor_context_ivl = 0;
            bool monitor_receiver = false;
            int monitor_receiver_ivl = 0;
            string application_id = null;
            int mon_format = LBMMonitor.FORMAT_CSV;
            int mon_transport = LBMMonitor.TRANSPORT_LBM;
            string mon_format_options = null;
            string mon_transport_options = null;
            bool error = false;
            bool done = false;
            List<String> interfaces = new List<String>();
            const string OPTION_MONITOR_CTX = "--monitor-ctx";
            const string OPTION_MONITOR_RCV = "--monitor-rcv";
            const string OPTION_MONITOR_TRANSPORT = "--monitor-transport";
            const string OPTION_MONITOR_TRANSPORT_OPTS = "--monitor-transport-opts";
            const string OPTION_MONITOR_FORMAT = "--monitor-format";
            const string OPTION_MONITOR_FORMAT_OPTS = "--monitor-format-opts";
            const string OPTION_MONITOR_APPID = "--monitor-appid";
            for (i = 0; i < n; i++)
            {
                try
                {
                    switch (args[i])
                    {
                        case OPTION_MONITOR_APPID:
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            application_id = args[i];
                            break;

                        case OPTION_MONITOR_CTX:
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            monitor_context = true;
                            monitor_context_ivl = Convert.ToInt32(args[i]);
                            break;

                        case OPTION_MONITOR_RCV:
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            monitor_receiver = true;
                            monitor_receiver_ivl = Convert.ToInt32(args[i]);
                            break;

                        case OPTION_MONITOR_FORMAT:
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            if (args[i].ToLower().CompareTo("csv") == 0)
                                mon_format = LBMMonitor.FORMAT_CSV;
                            else
                            {
                                error = true;
                                break;
                            }
                            break;

                        case OPTION_MONITOR_TRANSPORT:
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            if (args[i].ToLower().CompareTo("lbm") == 0)
                                mon_transport = LBMMonitor.TRANSPORT_LBM;
                            else if (args[i].ToLower().CompareTo("udp") == 0)
                                mon_transport = LBMMonitor.TRANSPORT_UDP;
                            else if (args[i].ToLower().CompareTo("lbmsnmp") == 0)
                                mon_transport = LBMMonitor.TRANSPORT_LBMSNMP;
                            else
                            {
                                error = true;
                                break;
                            }
                            break;

                        case OPTION_MONITOR_TRANSPORT_OPTS:
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            mon_transport_options += args[i];
                            break;
                        case OPTION_MONITOR_FORMAT_OPTS:
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            mon_format_options += args[i];
                            break;
                        case "-c":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            try
                            {
                                LBM.setConfiguration(args[i]);
                            }
                            catch (LBMException Ex)
                            {
                                System.Console.Error.WriteLine("lbmhfxrcv error: " + Ex.Message);
                                error = true;
                            }
                            break;
                        case "-d":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            qdelay = args[i];
                            eventq = true;
                            break;
                        case "-E":
                            end_on_eos = true;
                            break;
                        case "-h":
                            print_help_exit(0);
                            break;
                        case "-I":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            try
                            {
                                interfaces.Add(args[i]);
                            }
                            catch (Exception e)
                            {
                                System.Console.Error.WriteLine("lbmhfxrcv error: " + e.Message);
                                error = true;
                            }
                            break;
                        case "-q":
                            eventq = true;
                            break;
                        case "-r":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            reap_msgs = Convert.ToInt32(args[i]);
                            break;
                        case "-S":
                            end_on_eos = true;
                            summary = true;
                            break;
                        case "-s":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            stat_secs = Convert.ToInt32(args[i]);
                            break;
                        case "-v":
                            verbose = true;
                            break;
                        case "-V":
                            verifiable = true;
                            break;
                        case "-z":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            qsize = args[i];
                            eventq = true;
                            break;
                        default:
                            if (args[i].StartsWith("-"))
                            {
                                error = true;
                            }
                            else
                            {
                                done = true;
                            }
                            break;
                    }
                    if (error || done)
                        break;
                }
                catch (Exception e)
                {
                    /* type conversion exception */
                    System.Console.Error.WriteLine("lbmhfxrcv: error\n" + e.Message + "\n");
                    print_help_exit(1);
                }
            }
            if (error || i >= n)
            {
                /* An error occurred processing the command line - print help and exit */
                print_help_exit(1);
            }
            LBMRcvReceiver rcv = new LBMRcvReceiver(verbose, end_on_eos, summary);


            LBMContextAttributes ctx_attr = new LBMContextAttributes();
            ctx_attr.setObjectRecycler(objRec, null);
            if (eventq)
            {
                System.Console.Error.WriteLine("Using an event queue");
                LBMEventQueueAttributes evqattr = null;
                if (qsize != null || qdelay != null)
                {
                    evqattr = new LBMEventQueueAttributes();
                    if (qdelay != null)
                        evqattr.setValue("queue_delay_warning", qdelay);
                    if (qsize != null)
                        evqattr.setValue("queue_size_warning", qsize);
                }
                evq = new LBMRcvEventQueue(evqattr);
                ctx_attr.setImmediateMessageCallback(new LBMImmediateMessageCallback(rcv.onReceive), evq);
            }
            else
            {
                System.Console.Error.WriteLine("No event queue");
                ctx_attr.setImmediateMessageCallback(new LBMImmediateMessageCallback(rcv.onReceive));
            }

            LBMHFXAttributes hfxattr = new LBMHFXAttributes();
            hfxattr.setObjectRecycler(objRec, null);

            LBMHFX hfx = new LBMHFX(hfxattr, args[i], rcv.onReceive, evq);

            List<LBMContext> ctxs = new List<LBMContext>();
            List<LBMHFXReceiver> hfxrcvs = new List<LBMHFXReceiver>();
            if (interfaces.Count > 0)
            {
                foreach (String iface in interfaces)
                {
                    LBMContext ctx;

                    ctx_attr.setValue("resolver_multicast_interface", iface);

                    ctxs.Add(new LBMContext(ctx_attr));
                    ctx = ctxs[ctxs.Count - 1];
                    if (ctx.getAttributeValue("request_tcp_bind_request_port") != "0")
                    {
                        string request_tcp_iface = ctx.getAttributeValue("request_tcp_interface");
                        // Check if a different interface was specified for request_tcp_interface
                        if (!request_tcp_iface.Equals("0.0.0.0"))
                        {
                            System.Console.Out.WriteLine("Immediate messaging target: TCP:"
                                + request_tcp_iface + ":"
                                + ctx.getAttributeValue("request_tcp_port"));
                        }
                        else
                        {
                            System.Console.Out.WriteLine("Immediate messaging target: TCP:"
                                + ctx.getAttributeValue("resolver_multicast_interface") + ":"
                                + ctx.getAttributeValue("request_tcp_port"));
                        }
                    }
                    else
                    {
                        System.Console.Out.WriteLine("Request port binding disabled, no immediate messaging target");
                    }

                    LBMReceiverAttributes rattr = new LBMReceiverAttributes();

                    rattr.setValue("transport_lbtru_interface", iface);
                    rattr.setValue("transport_tcp_interface", iface);
                    rattr.setObjectRecycler(objRec, null);

                    hfxrcvs.Add(hfx.createReceiver(ctx, rattr, iface));

                }
            }
            else
            {
                LBMContext ctx;
                ctxs.Add(new LBMContext(ctx_attr));
                ctx = ctxs[ctxs.Count - 1];
                if (ctx.getAttributeValue("request_tcp_bind_request_port") != "0")
                {
                    string request_tcp_iface = ctx.getAttributeValue("request_tcp_interface");
                    // Check if a different interface was specified for request_tcp_interface
                    if (!request_tcp_iface.Equals("0.0.0.0"))
                    {
                        System.Console.Out.WriteLine("Immediate messaging target: TCP:"
                            + request_tcp_iface + ":"
                            + ctx.getAttributeValue("request_tcp_port"));
                    }
                    else
                    {
                        System.Console.Out.WriteLine("Immediate messaging target: TCP:"
                            + ctx.getAttributeValue("resolver_multicast_interface") + ":"
                            + ctx.getAttributeValue("request_tcp_port"));
                    }
                }
                else
                {
                    System.Console.Out.WriteLine("Request port binding disabled, no immediate messaging target");
                }

                LBMReceiverAttributes rattr = new LBMReceiverAttributes();
                rattr.setObjectRecycler(objRec, null);

                hfxrcvs.Add(hfx.createReceiver(ctx, rattr, null));

            }
            LBMMonitorSource lbmmonsrc = null;
            if (monitor_context || monitor_receiver)
            {
                lbmmonsrc = new LBMMonitorSource(mon_format, mon_format_options, mon_transport, mon_transport_options);

                if (monitor_context)
                {
                    foreach (LBMContext ctx in ctxs)
                        lbmmonsrc.start(ctx, application_id, monitor_context_ivl);
                }
                else
                {
                    foreach (LBMHFXReceiver lbmhfxrcv in hfxrcvs)
                        lbmmonsrc.start(lbmhfxrcv, application_id, monitor_receiver_ivl);
                }
            }
			System.Console.Out.Flush();
            long start_time;
            long end_time;
            long last_lost = 0, lost_tmp = 0, lost = 0;
            bool have_stats;
            LBMReceiverStatistics stats = null;
            long stat_time = System.DateTime.Now.AddSeconds(stat_secs).Ticks;
            for (; ; )
            {
                start_time = System.DateTime.Now.Ticks;
                if (eventq)
                {
                    evq.run(1000);
                }
                else
                {
                    System.Threading.Thread.Sleep(1000);
                }
                end_time = System.DateTime.Now.Ticks;

                lost = 0;
                foreach (LBMHFXReceiver lbmhfxrcv in hfxrcvs)
                {
                    have_stats = false;
                    while (!have_stats)
                    {
                        try
                        {
                            stats = lbmhfxrcv.getStatistics(nstats);
                            have_stats = true;
                        }
                        catch (LBMException ex)
                        {
                            /* Double the number of stats passed to the API to be retrieved */
                            /* Do so until we retrieve stats successfully or hit the max limit */
                            nstats *= 2;
                            if (nstats > DEFAULT_MAX_NUM_SRCS)
                            {
                                System.Console.Error.WriteLine("Error getting receiver statistics: " + ex.Message);
                                System.Environment.Exit(1);
                            }
                            /* have_stats is still false */
                        }
                    }

                    /* If we get here, we have the stats */
                    for (i = 0; i < stats.size(); i++)
                    {
                        lost += stats.lost(i);
                    }

                    if (stat_secs > 0 && stat_time <= end_time)
                    {
                        stat_time = System.DateTime.Now.AddSeconds(stat_secs).Ticks;
                        print_stats(stats, evq);
                    }
                    objRec.doneWithReceiverStatistics(stats);
                }

                /* Account for loss in previous iteration */
                lost_tmp = lost;
                if (last_lost <= lost)
                {
                    lost -= last_lost;
                }
                else
                {
                    lost = 0;
                }
                last_lost = lost_tmp;

                print_bw((end_time - start_time) / 10000,
                    rcv.msg_count,
                    rcv.byte_count,
                    rcv.unrec_count,
                    lost,
                    rcv.burst_loss,
                    rcv.rx_msgs,
                    rcv.otr_msgs);

                rcv.msg_count = 0;
                rcv.byte_count = 0;
                rcv.unrec_count = 0;
                rcv.burst_loss = 0;
                rcv.rx_msgs = 0;
                rcv.otr_msgs = 0;

                if (reap_msgs != 0 && rcv.total_msg_count >= reap_msgs)
                {
                    break;
                }
            }

            System.Console.Error.WriteLine("Quitting.... received "
                + rcv.total_msg_count
                + " messages");

            objRec.close();

            LBMOperationCompleteCallback cb = new LBMOperationCompleteCallback(deletionComplete);

            System.Console.Error.WriteLine("Deleting Receivers.");
            foreach (LBMHFXReceiver lbmhfxrcv in hfxrcvs)
                lbmhfxrcv.close(cb, "Receiver");

            System.Console.Error.WriteLine("Waiting for deletions to complete");
            while (deletions_complete < hfxrcvs.Count)
            {
                if (eventq)
                    evq.run(1000);
                else
                    System.Threading.Thread.Sleep(1000);
            }
            System.Console.Error.WriteLine("Deleting Contexts");
            foreach (LBMContext ctx in ctxs)
                ctx.close();

            deletions_complete = 0;
            System.Console.Error.WriteLine("Deleting HFX.");
            hfx.close(cb, "HFX");
            System.Console.Error.WriteLine("Waiting for HFX deletion to complete");
            while (deletions_complete < 1)
            {
                if (eventq)
                    evq.run(1000);
                else
                    System.Threading.Thread.Sleep(1000);
            }

            if (eventq)
                evq.close();
        }

        private static void deletionComplete(object cbArg)
        {
            deletions_complete++;
            System.Console.Error.WriteLine("{0} {1} deleted.\n", cbArg.ToString(), deletions_complete);
            if (eventq)
                evq.stop();
        }

        private static void print_help_exit(int exit_value)
        {
            System.Console.Error.WriteLine(LBM.version());
            System.Console.Error.WriteLine(purpose);
            System.Console.Error.WriteLine(usage);
            System.Environment.Exit(exit_value);
        }

        private static void logger(int loglevel, string message)
        {
            string level;
            switch (loglevel)
            {
                case LBM.LOG_ALERT: level = "Alert"; break;
                case LBM.LOG_CRIT: level = "Critical"; break;
                case LBM.LOG_DEBUG: level = "Debug"; break;
                case LBM.LOG_EMERG: level = "Emergency"; break;
                case LBM.LOG_ERR: level = "Error"; break;
                case LBM.LOG_INFO: level = "Info"; break;
                case LBM.LOG_NOTICE: level = "Note"; break;
                case LBM.LOG_WARNING: level = "Warning"; break;
                default: level = "Unknown"; break;
            }
            System.Console.Out.WriteLine(System.DateTime.Now.ToString() + " [" + level + "]: " + message);
			System.Console.Out.Flush();
        }

        private static int sourceNotification(string topic, string source, object cbArg)
        {
            System.Console.Error.WriteLine("new topic ["
                                            + topic
                                            + "], source ["
                                            + source
                                            + "]");
            return 0;

        }

        private static void print_stats(LBMReceiverStatistics stats, LBMEventQueue evq)
        {
            if (evq != null)
            {
                if (Convert.ToInt32(evq.getAttributeValue("queue_size_warning")) > 0)
                {
                    System.Console.Out.WriteLine("Event queue size: " + evq.size());
                }
            }
            for (int i = 0; i < stats.size(); i++)
            {
                switch (stats.type(i))
                {
                    case LBM.TRANSPORT_STAT_TCP:
                        System.Console.Out.WriteLine("TCP, source " + stats.source(i)
                                                    + ", received " + stats.lbmMessagesReceived(i)
                                                    + "/" + stats.bytesReceived(i)
                                                    + ", no topics " + stats.noTopicMessagesReceived(i)
                                                    + ", requests " + stats.lbmRequestsReceived(i));
                        break;
                    case LBM.TRANSPORT_STAT_LBTRU:
                    case LBM.TRANSPORT_STAT_LBTRM:
                        if (stats.type() == LBM.TRANSPORT_STAT_LBTRU)
                            System.Console.Out.Write("LBT-RU");
                        else
                            System.Console.Out.Write("LBT-RM");
                        System.Console.Out.WriteLine(", source " + stats.source(i)
                                                    + ", received " + stats.messagesReceived(i)
                                                    + "/" + stats.bytesReceived(i)
                                                    + ", naks " + stats.nakPacketsSent(i)
                                                    + "/" + stats.naksSent(i)
                                                    + ", lost " + stats.lost(i)
                                                    + ", ncfs " + stats.ncfsIgnored(i)
                                                    + "/" + stats.ncfsShed(i)
                                                    + "/" + stats.ncfsRetransmissionDelay(i)
                                                    + "/" + stats.ncfsUnknown(i)
                                                    + ", recovery " + stats.minimumRecoveryTime(i)
                                                    + "/" + stats.meanRecoveryTime(i)
                                                    + "/" + stats.maximumRecoveryTime(i)
                                                    + ", nak tx " + stats.minimumNakTransmissions(i)
                                                    + "/" + stats.minimumNakTransmissions(i)
                                                    + "/" + stats.maximumNakTransmissions(i)
                                                    + ", dup " + stats.duplicateMessages(i)
                                                    + ", unrecovered " + stats.unrecoveredMessagesWindowAdvance(i)
                                                    + "/" + stats.unrecoveredMessagesNakGenerationTimeout(i)
                                                    + ", LBM msgs " + stats.lbmMessagesReceived(i)
                                                    + ", no topics " + stats.noTopicMessagesReceived(i)
                                                    + ", requests " + stats.lbmRequestsReceived(i));
                        break;
                    case LBM.TRANSPORT_STAT_LBTIPC:
                        System.Console.Out.WriteLine("LBT-IPC, source " + stats.source(i)
                                                    + ", received " + stats.messagesReceived(i)
                                                    + "/" + stats.bytesReceived(i)
                                                    + ", LBM msgs " + stats.lbmMessagesReceived(i)
                                                    + ", no topics " + stats.noTopicMessagesReceived(i)
                                                    + ", requests " + stats.lbmRequestsReceived(i));
                        break;
                    case LBM.TRANSPORT_STAT_LBTSMX:
                        System.Console.Out.WriteLine("LBT-SMX, source " + stats.source(i)
                                                    + ", received " + stats.messagesReceived(i)
                                                    + "/" + stats.bytesReceived(i)
                                                    + ", LBM msgs " + stats.lbmMessagesReceived(i)
                                                    + ", no topics " + stats.noTopicMessagesReceived(i)
                                                    + ", requests " + stats.lbmRequestsReceived(i));
                        break;
                    case LBM.TRANSPORT_STAT_LBTRDMA:
                        System.Console.Out.WriteLine("LBT-RDMA, source " + stats.source(i)
                                                    + ", received " + stats.messagesReceived(i)
                                                    + "/" + stats.bytesReceived(i)
                                                    + ", LBM msgs " + stats.lbmMessagesReceived(i)
                                                    + ", no topics " + stats.noTopicMessagesReceived(i)
                                                    + ", requests " + stats.lbmRequestsReceived(i));
                        break;
                }
            }
			System.Console.Out.Flush();
        }

        private static void print_bw(long msec, long msgs, long bytes, long unrec, long lost, long burst_loss, long rx_msgs, long otr_msgs)
        {
            char[] scale = { '\0', 'K', 'M', 'G' };
            double mps = 0.0, bps = 0.0, sec = 0.0;
            double kscale = 1000.0;
            int msg_scale_index = 0, bit_scale_index = 0;

            sec = msec / 1000.0;
            if (sec == 0) return; /* avoid division by zero */
            mps = ((double)msgs) / sec;
            bps = ((double)bytes * 8) / sec;

            while (mps >= kscale)
            {
                mps /= kscale;
                msg_scale_index++;
            }

            while (bps >= kscale)
            {
                bps /= kscale;
                bit_scale_index++;
            }

            if ((rx_msgs > 0) || (otr_msgs > 0))
            {
                System.Console.Out.Write(sec
                + " secs. "
                + mps.ToString("0.000")
                + " " + scale[msg_scale_index]
                + "msgs/sec. "
                + bps.ToString("0.000")
                + " " + scale[bit_scale_index]
                + "bps"
                + " [RX: " + rx_msgs + "][OTR: " + otr_msgs + "]");
            }
            else
            {
                System.Console.Out.Write(sec
                + " secs. "
                + mps.ToString("0.000")
                + " " + scale[msg_scale_index]
                + "msgs/sec. "
                + bps.ToString("0.000")
                + " " + scale[bit_scale_index]
                + "bps");
            }

            if (lost != 0 || unrec != 0 || burst_loss != 0)
            {
                System.Console.Out.Write(" [" + lost + " pkts lost, "
                                              + unrec + " msgs unrecovered, "
                                              + burst_loss + " bursts]");
            }
            System.Console.Out.WriteLine("");
			System.Console.Out.Flush();
        }
    }

    class LBMRcvEventQueue : LBMEventQueue
    {
        public LBMRcvEventQueue()
            : this(null)
        {
        }

        public LBMRcvEventQueue(LBMEventQueueAttributes evqattr)
            : base(evqattr)
        {
            this.addMonitor(new LBMEventQueueCallback(monitor));
        }

        protected void monitor(object cbarg, int evtype, int evq_size, long evq_delay)
        {
            System.Console.Error.WriteLine("Event Queue Monitor: Type: " + evtype +
                ", Size: " + evq_size +
                ", Delay: " + evq_delay + " usecs.");
        }
    }

    class LBMRcvReceiver
    {
        public long imsg_count = 0;
        public long msg_count = 0;
        public long total_msg_count = 0;
        public long subtotal_msg_count = 0;
        public long byte_count = 0;
        public long unrec_count = 0;
        public long total_unrec_count = 0;
        public long burst_loss = 0;
        public long rx_msgs = 0;
        public long otr_msgs = 0;

        public long data_start_time = 0;
        public long data_end_time = 0;

        public int stotal_msg_count = 0;
        public long total_byte_count = 0;

        private bool _verbose = false;
        private bool _end_on_eos = false;
        private bool _summary = false;

        public LBMRcvReceiver(bool verbose, bool end_on_eos, bool summary)
        {
            _verbose = verbose;
            _end_on_eos = end_on_eos;
            _summary = summary;
            LBMReceiverCallback cb = new LBMReceiverCallback(onReceive);
        }

        // This immediate-mode receiver is *only* used for topicless
        // immediate-mode sends.  Immediate sends that use a topic
        // are received with normal receiver objects.
        public int onReceiveImmediate(object cbArg, LBMMessage msg)
        {
            imsg_count++;
            return onReceive(cbArg, msg);
        }

        public int onReceive(object cbArg, LBMMessage msg)
        {
            switch (msg.type())
            {
                case LBM.MSG_DATA:
                    if (stotal_msg_count == 0)
                        data_start_time = Environment.TickCount;
                    else
                        data_end_time = Environment.TickCount;

                    msg_count++;
                    total_msg_count++;
                    stotal_msg_count++;
                    subtotal_msg_count++;
                    byte_count += msg.length();
                    total_byte_count += msg.length();

                    if ((msg.flags() & LBM.MSG_FLAG_RETRANSMIT) != 0)
                    {
                        rx_msgs++;
                    }
                    if ((msg.flags() & LBM.MSG_FLAG_OTR) != 0)
                    {
                        otr_msgs++;
                    }

                    if (_verbose)
                    {
                        Console.Write("[@{0}.{1:000000}]", msg.timestampSeconds(), msg.timestampMicroseconds());
                        if (msg.channelInfo() != null)
                        {

                            switch (msg.channelInfo().channelFlags())
                            {
                                case LBM.MSG_FLAG_NUMBERED_CHANNEL:

                                    System.Console.Out.Write("["
                                        + msg.topicName()
                                        + ":"
                                        + msg.channelInfo().channelNumber()
                                        + "]["
                                        + msg.source()
                                        + "]["
                                        + msg.sequenceNumber()
                                        + "], ");
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            System.Console.Out.Write("["
                            + msg.topicName()
                            + "]["
                            + msg.source()
                            + "]["
                            + msg.sequenceNumber()
                            + "], ");
                        }
                        if ((msg.flags() & LBM.MSG_FLAG_RETRANSMIT) != 0)
                        {
                            System.Console.Out.Write("-RX- ");
                        }
                        if ((msg.flags() & LBM.MSG_FLAG_OTR) != 0)
                        {
                            System.Console.Out.Write("-OTR- ");
                        }

                        System.Console.Out.WriteLine(msg.data().Length + " bytes");

                        if (lbmhfxrcv.verifiable)
                        {
                            int rc = VerifiableMessage.verifyMessage(msg.data(), msg.data().Length, lbmhfxrcv.verbose);
                            if (rc == 0)
                            {
                                System.Console.WriteLine("Message sqn " + msg.sequenceNumber() + " does not verify!");
                            }
                            else if (rc == -1)
                            {
                                System.Console.Error.WriteLine("Message sqn " + msg.sequenceNumber() + " is not a verifiable message.");
                                System.Console.Error.WriteLine("Use -V option on source and restart receiver.");
                            }
                            else
                            {
                                if (lbmhfxrcv.verbose)
                                {
                                    System.Console.WriteLine("Message sqn " + msg.sequenceNumber() + " verfies");
                                }
                            }
                        }
                    }
                    break;
                case LBM.MSG_BOS:
                    System.Console.Out.WriteLine("[" + msg.topicName() + "][" + msg.source() + "], Beginning of Transport Session");
                    break;
                case LBM.MSG_EOS:
                    //data_end_time = System.DateTime.Now.Ticks;
                    System.Console.Out.WriteLine("[" + msg.topicName() + "][" + msg.source() + "], End of Transport Session");
                    if (_end_on_eos)
                    {
                        if (_summary)
                            print_summary();
                        end();
                    }
                    subtotal_msg_count = 0;
                    break;
                case LBM.MSG_UNRECOVERABLE_LOSS:
                    unrec_count++;
                    total_unrec_count++;
                    if (_verbose)
                    {
                        System.Console.Out.Write("[" + msg.topicName() + "][" + msg.source() + "][" + msg.sequenceNumber() + "],");
                        System.Console.Out.WriteLine(" LOST");
                    }
                    break;
                case LBM.MSG_UNRECOVERABLE_LOSS_BURST:
                    burst_loss++;
                    if (_verbose)
                    {
                        System.Console.Out.Write("[" + msg.topicName() + "][" + msg.source() + "][" + msg.sequenceNumber() + "],");
                        System.Console.Out.WriteLine(" LOST BURST");
                    }
                    break;
                case LBM.MSG_REQUEST:
                    if (stotal_msg_count == 0)
                        data_start_time = Environment.TickCount;
                    else
                        data_end_time = Environment.TickCount;
                    msg_count++;
                    stotal_msg_count++;
                    subtotal_msg_count++;
                    byte_count += msg.data().Length;
                    total_byte_count += msg.data().Length;
                    if (_verbose)
                    {
                        System.Console.Out.Write("Request ["
                            + msg.topicName()
                            + "]["
                            + msg.source()
                            + "]["
                            + msg.sequenceNumber()
                            + "], ");
                        System.Console.Out.WriteLine(msg.data().Length
                                                                + " bytes");
                    }
                    break;
                default:
                    System.Console.Out.WriteLine("Unknown lbm_msg_t type " + msg.type() + " [" + msg.topicName() + "][" + msg.source() + "]");
                    break;
            }
            msg.dispose();
			System.Console.Out.Flush();
            return 0;
        }

        private void print_summary()
        {
            double total_time_sec, mps, bps;

            total_time_sec = 0.0;
            mps = 0.0;
            bps = 0.0;

            long bits_received = total_byte_count * 8;
            long total_time = Math.Abs(data_end_time - data_start_time);

            total_time_sec = total_time / 1000.0;

            if (total_time_sec > 0)
            {
                mps = stotal_msg_count / total_time;
                bps = bits_received / total_time / 1000.0;
            }

            System.Console.Out.WriteLine("\nTotal time         : "
                       + total_time_sec.ToString("0.000")
                       + "  sec");
            System.Console.Out.WriteLine("Messages received  : "
                       + stotal_msg_count);
            System.Console.Out.WriteLine("Bytes received     : "
                       + total_byte_count);
            System.Console.Out.WriteLine("Avg. throughput    : "
                       + mps.ToString("0.000")
                       + " Kmsgs/sec, "
                       + bps.ToString("0.000")
                       + " Mbps\n\n");

        }

        private void end()
        {
            System.Console.Out.WriteLine("Quitting.... received "
                + total_msg_count
                + " messages");
            System.Environment.Exit(0);
        }
    }

}
