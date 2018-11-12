
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
using com.latencybusters.lbm;

namespace LBMApplication
{
    class lbmmrcv
    {
        [DllImport("Kernel32.dll")]
        public static extern int SetEnvironmentVariable(string name, string value);

        private const int DEFAULT_MAX_NUM_SRCS = 10000;
        private static int nstats = 10;
        private const int default_max_messages = 10000000;
        private const int max_num_rcvs = 300000;
        private const int default_num_rcvs = 100;
        private const int max_num_ctxs = 5;
        private const int default_num_ctxs = 1;
        private const string default_topic_root = "29west.example.multi";
        private const int default_initial_topic_number = 0;

        private static int msgs = 200;
        private static bool verbose = false;
        private static bool print_stats_flag = false;
        private static string purpose = "Purpose: Receive messages on multiple topics.";
        private static string usage =
            "Usage: lbmmrcv [options]\n"
            + "Available options:\n"
            + "  -B # = Set receive socket buffer size to # (in MB)\n"
            + "  -c filename = Use LBM configuration file filename.\n"
            + "                Multiple config files are allowed.\n"
            + "                Example:  '-c file1.cfg -c file2.cfg'\n"
            + "  -C ctxs = use ctxs number of context objects\n"
            + "  -h = help\n"
            + "  -i num = initial topic number num\n"
            + "  -r root = use topic names with root of \"root\"\n"
            + "  -R rcvs = create rcvs receivers\n"
            + "  -s = print statistics along with bandwidth\n"
            + "  -v = be verbose about each message\n"
            + "\nMonitoring options:\n"
            + "  --monitor-ctx NUM = monitor context every NUM seconds\n"
            + "  --monitor-rcv NUM = monitor each receiver every NUM seconds\n"
            + "  --monitor-transport TRANS = use monitor transport module TRANS\n"
            + "                              TRANS may be `lbm', `udp', or `lbmsnmp', default is `lbm'\n"
            + "  --monitor-transport-opts OPTS = use OPTS as transport module options\n"
            + "  --monitor-format FMT = use monitor format module FMT\n"
            + "                         FMT may be `csv'\n"
            + "  --monitor-format-opts OPTS = use OPTS as format module options\n"
            + "  --monitor-appid ID = use ID as application ID string\n"
            ;
        public static int imsg_count = 0;
        public static int imsg_byte_count = 0;

        static void Main(string[] args)
        {
            if (System.Environment.GetEnvironmentVariable("LBM_LICENSE_FILENAME") == null
                && System.Environment.GetEnvironmentVariable("LBM_LICENSE_INFO") == null)
            {
                SetEnvironmentVariable("LBM_LICENSE_FILENAME", "lbm_license.txt");
            }

            LBM lbm = new LBM();
            lbm.setLogger(new LBMLogging(logger));

            int num_ctxs = default_num_ctxs;
            int num_rcvs = default_num_rcvs;
            int initial_topic_number = default_initial_topic_number;
            string topicroot = default_topic_root;
            int i;
            int n = args.Length;
            bool monitor_context = false;
            int monitor_context_ivl = 0;
            bool monitor_receiver = false;
            int monitor_receiver_ivl = 0;
            string application_id = null;
            int mon_format = LBMMonitor.FORMAT_CSV;
            int mon_transport = LBMMonitor.TRANSPORT_LBM;
            long bufsize = 8;
            string mon_format_options = null;
            string mon_transport_options = null;
            bool error = false;

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
                        case "-B":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            bufsize = Convert.ToInt32(args[i]);
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
                                System.Console.Error.WriteLine("lbmmrcv: " + Ex.Message);
                                error = true;
                            }
                            break;
                        case "-C":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            num_ctxs = Convert.ToInt32(args[i]);
                            break;
                        case "-h":
                            print_help_exit(0);
                            break;
                        case "-i":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            initial_topic_number = Convert.ToInt32(args[i]);
                            break;
                        case "-M":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            msgs = Convert.ToInt32(args[i]);
                            break;
                        case "-r":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            topicroot = args[i];
                            break;
                        case "-R":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            num_rcvs = Convert.ToInt32(args[i]);
                            if (num_rcvs > max_num_rcvs)
                            {
                                System.Console.Error.WriteLine("Too many receivers specified.  Max number of receivers is " + max_num_rcvs);
                                System.Environment.Exit(1);
                            }
                            break;
                        case "-s":
                            print_stats_flag = true;
                            break;
                        case "-v":
                            verbose = true;
                            break;

                        default:
                            error = true;
                            break;
                    }
                    if (error)
                        break;
                }
                catch (Exception e)
                {
                    /* type conversion exception */
                    System.Console.Error.WriteLine("lbmmrcv: error\n" + e.Message + "\n");
                    print_help_exit(1);
                }
            }
            if (error)
            {
                /* An error occurred processing the command line - print help and exit */
                print_help_exit(1);
            }
            System.Console.Out.WriteLine("Using " + num_ctxs + " context(s)");


            LBMContextAttributes ctx_attr = new LBMContextAttributes();
            if (bufsize > 0)
            {
                bufsize *= 1024 * 1024;
                ctx_attr.setValue("transport_tcp_receiver_socket_buffer", "" + bufsize);
                ctx_attr.setValue("transport_lbtrm_receiver_socket_buffer", "" + bufsize);
                ctx_attr.setValue("transport_lbtru_receiver_socket_buffer", "" + bufsize);
            }

            LBMContext[] ctxs = new LBMContext[num_ctxs];
            for (i = 0; i < num_ctxs; i++)
            {
                ctxs[i] = new LBMContext(ctx_attr);
            }

            LBMMonitorSource lbmmonsrc = null;
            if (monitor_context || monitor_receiver)
            {
                lbmmonsrc = new LBMMonitorSource(mon_format, mon_format_options, mon_transport, mon_transport_options);
                if (monitor_context)
                {
                    for (i = 0; i < num_ctxs; i++)
                    {
                        lbmmonsrc.start(ctxs[i], application_id, monitor_context_ivl);
                    }
                }
            }

            LBMMRcvReceiver[] rcvs = new LBMMRcvReceiver[num_rcvs];
            System.Console.Out.WriteLine("Creating " + num_rcvs + " receivers");
            int ctxidx = 0;

            for (i = 0; i < num_rcvs; i++)
            {
                int topicnum = initial_topic_number + i;
                string topicname = topicroot + "." + topicnum;

                LBMTopic topic = ctxs[ctxidx].lookupTopic(topicname);
                rcvs[i] = new LBMMRcvReceiver(ctxs[ctxidx], topic, verbose);
                if (i > 1 && (i % 1000) == 0)
                {
                    System.Console.Out.WriteLine("Created " + i + " receivers");
                }

                if (++ctxidx >= num_ctxs)
                    ctxidx = 0;
                if (monitor_receiver)
                {
                    lbmmonsrc.start(rcvs[i].rcv, application_id + "(" + i + ")", monitor_receiver_ivl);
                }
            }
            //
            // Setup just one immediate message receiver to receive topicless 
            // messages.  We'll just pick the first context to set it up on.
            //
            ctxs[0].enableImmediateMessageReceiver();
            ctxs[0].addImmediateMessageReceiver(new LBMImmediateMessageCallback(onReceiveImmediate));

            System.Console.Out.WriteLine("Created " + num_rcvs + " receivers. Will start calculating aggregate throughput.");
            System.Console.Out.Flush();

            long start_time;
            long end_time;
            long total_msg_count = 0;
            long last_lost = 0, lost_tmp = 0, lost;

            while (true)
            {
                start_time = System.DateTime.Now.Ticks;
                System.Threading.Thread.Sleep(1000);
                end_time = System.DateTime.Now.Ticks;

                long msg_count = 0;
                long byte_count = 0;
                long unrec_count = 0;
                long burst_loss = 0;
                long rx_msgs = 0;
                long otr_msgs = 0;

                for (i = 0; i < num_rcvs; i++)
                {
                    msg_count += rcvs[i].msg_count;
                    total_msg_count += rcvs[i].msg_count;
                    byte_count += rcvs[i].byte_count;
                    unrec_count += rcvs[i].unrec_count;
                    burst_loss += rcvs[i].burst_loss;
                    rx_msgs += rcvs[i].rx_msgs;
                    otr_msgs += rcvs[i].otr_msgs;

                    rcvs[i].msg_count = 0;
                    rcvs[i].byte_count = 0;
                    rcvs[i].unrec_count = 0;
                    rcvs[i].burst_loss = 0;
                    rcvs[i].rx_msgs = 0;
                    rcvs[i].otr_msgs = 0;
                }

                /* Calculate aggregate transport level loss */
                /* Pass 0 for the print flag indicating interested in retrieving loss stats */
                lost = get_loss_or_print_stats(ctxs, false);
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
                    msg_count + imsg_count,
                    byte_count + imsg_byte_count,
                    unrec_count,
                    lost,
                    burst_loss,
                    rx_msgs,
                    otr_msgs);
                imsg_count = 0;
                imsg_byte_count = 0;

                if (print_stats_flag)
                {
                    /* Display transport level statistics */
                    /* Pass print_stats_flag for the print flag indicating interested in displaying stats */
                    get_loss_or_print_stats(ctxs, print_stats_flag);
                }
            }
        }

        /*
	    * function to retrieve transport level loss or display transport level stats
	    * @ctxs -- contexts to retrieve transport stats for
	    * @print_flag -- if 1, display stats, retrieve loss otherwise
	    */
        private static long get_loss_or_print_stats(LBMContext[] ctxs, bool print_flag)
        {
            long lost = 0;
            LBMReceiverStatistics stats = null;
            bool have_stats;

            for (int ctx = 0; ctx < ctxs.Length; ctx++)
            {
                have_stats = false;
                while (!have_stats)
                {
                    try
                    {
                        stats = ctxs[ctx].getReceiverStatistics(nstats);
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
                if (print_flag)
                {
                    /* Display transport level stats */
                    print_stats(stats);
                }
                else
                {
                    /* Accumlate transport level loss */
                    for (int stat = 0; stat < stats.size(); stat++)
                    {
                        switch (stats.type(stat))
                        {
                            case LBM.TRANSPORT_STAT_LBTRU:
                            case LBM.TRANSPORT_STAT_LBTRM:
                                lost += stats.lost(stat);
                                break;
                        }
                    }
                }
            }
            return lost;
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

        private static void print_stats(LBMReceiverStatistics stats)
        {
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
                        if (stats.type(i) == LBM.TRANSPORT_STAT_LBTRU)
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
                System.Console.Out.Write(" ["
                    + lost
                    + " pkts lost, "
                    + unrec
                    + " msgs unrecovered, "
                    + burst_loss
                    + " bursts]");
            }
            System.Console.Out.WriteLine("");
            System.Console.Out.Flush();
        }

        static int onReceiveImmediate(object cbArg, LBMMessage msg)
        {
            string nontopic_str = "Non-Topic Immediate Message";

            switch (msg.type())
            {
                case LBM.MSG_DATA:
                    imsg_count++;
                    imsg_byte_count += msg.data().Length;
                    if (verbose)
                    {
                        System.Console.Out.Write("["
                            + nontopic_str
                            + "]["
                            + msg.source()
                            + "]["
                            + msg.sequenceNumber()
                            + "], ");
                        System.Console.Out.WriteLine(msg.data().Length
                                                                + " bytes");
                    }
                    break;
                case LBM.MSG_REQUEST:
                    imsg_count++;
                    imsg_byte_count += msg.data().Length;
                    if (verbose)
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
                    System.Console.Out.WriteLine("Unknown lbm_msg_t type " + msg.type() + " [" + nontopic_str + "][" + msg.source() + "]");
                    break;
            }

            System.Console.Out.Flush();
            return 0;
        }
    }
}
