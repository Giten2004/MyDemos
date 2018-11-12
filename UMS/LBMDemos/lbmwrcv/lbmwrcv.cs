
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
    class lbmwrcv
    {
        [DllImport("Kernel32.dll")]
        public static extern int SetEnvironmentVariable(string name, string value);

        private const int DEFAULT_MAX_NUM_SRCS = 10000;
        private static int nstats = 10;
        private static int reap_msgs = 0;
        private static bool eventq = false;
        private static bool verbose = false;
        private static bool end_on_eos = false;
        private static bool print_stats_flag = false;
        private static string purpose = "Purpose: Receive messages using a wildcard receiver.";
        private static string usage =
            "Usage: [options] topic\n"
            + "Available options\n"
            + "  -c filename = Use LBM configuration file filename.\n"
            + "                Multiple config files are allowed.\n"
            + "                Example:  '-c file1.cfg -c file2.cfg'\n"
            + "  -E = exit after source ends\n"
            + "  -D = Deregister receiver after 100 messages\n"
            + "  -h = help\n"
            + "  -q = use an LBM event queue\n"
            + "  -r msgs = delete receiver after msgs messages\n"
            + "  -s = print statistics along with bandwidth\n"
            + "  -N NUM = subscribe to channel NUM\n"
            + "  -v = be verbose about each message\n"
            + "\nMonitoring options:\n"
            + "  --monitor-ctx NUM = monitor context every NUM seconds\n"
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

            int i;
            int n = args.Length;
            bool error = false;
            bool done = false;

            List<uint> channels = new List<uint>();
            bool monitor_context = false;
            int monitor_context_ivl = 0;
            long dereg = 0;
            string application_id = null;
            int mon_format = LBMMonitor.FORMAT_CSV;
            int mon_transport = LBMMonitor.TRANSPORT_LBM;
            string mon_format_options = null;
            string mon_transport_options = null;
            const string OPTION_MONITOR_CTX = "--monitor-ctx";
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
                                System.Console.Error.WriteLine("lbmwrcv error: " + Ex.Message);
                                error = true;
                            }
                            break;
                        case "-E":
                            end_on_eos = true;
                            break;
                        case "-D":
                            dereg = 1;
                            break;
                        case "-h":
                            print_help_exit(0);
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
                        case "-s":
                            print_stats_flag = true;
                            break;
                        case "-N":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            channels.Add(Convert.ToUInt32(args[i]));
                            break;
                        case "-v":
                            verbose = true;
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
                    System.Console.Error.WriteLine("lbmwrcv: error\n" + e.Message + "\n");
                    print_help_exit(1);
                }
            }
            if (error || i >= n)
            {
                /* An error occurred processing the command line - print help and exit */
                print_help_exit(1);
            }
            LBMWRcvSourceNotify srcNotify = new LBMWRcvSourceNotify();

            LBMContextAttributes lbmContextAttributes = new LBMContextAttributes();
            lbmContextAttributes.enableSourceNotification();
            LBMContext ctx = new LBMContext(lbmContextAttributes);
            ctx.addSourceNotifyCallback(new LBMSourceNotification(srcNotify.sourceNotification));

            LBMWildcardReceiverAttributes lbmWildcardReceiverAttributes = new LBMWildcardReceiverAttributes();
            string pattern = args[i];
            string pattern_type = lbmWildcardReceiverAttributes.getValue("pattern_type");

            LBMWRcvTopicFilter topicFilter;
            if (pattern == "*"
                && (pattern_type.ToUpper() == "PCRE"
                || pattern_type.ToLower() == "regex"))
            {
                topicFilter = new LBMWRcvTopicFilter();
                pattern_type = "appcb";
                lbmWildcardReceiverAttributes.setValue("pattern_type", pattern_type);
                lbmWildcardReceiverAttributes.setPatternCallback(new LBMWildcardPatternCallback(topicFilter.comparePattern), null);
            }

            System.Console.Error.WriteLine("Creating wildcard receiver for pattern [" + pattern + "] - using pattern type: " + pattern_type);
            LBMWRcvReceiver wrcv = new LBMWRcvReceiver(verbose, end_on_eos);

            LBMWildcardReceiver lbmWildcardReceiver;
            LBMWRcvEventQueue evq = null;

            if (eventq)
            {
                System.Console.Error.WriteLine("Event queue in use");
                evq = new LBMWRcvEventQueue();
                lbmWildcardReceiver = new LBMWildcardReceiver(ctx, pattern, null, lbmWildcardReceiverAttributes, wrcv.onReceive, null, evq);
                ctx.enableImmediateMessageReceiver(evq);
            }
            else
            {
                System.Console.Error.WriteLine("No event queue");
                lbmWildcardReceiver = new LBMWildcardReceiver(ctx, pattern, null, lbmWildcardReceiverAttributes, wrcv.onReceive, null);
                ctx.enableImmediateMessageReceiver();
            }
            ctx.addImmediateMessageReceiver(new LBMImmediateMessageCallback(wrcv.onReceiveImmediate), null);

            wrcv.setWrcvr(lbmWildcardReceiver);
            wrcv.setDereg(dereg);

            if (channels.Count > 0)
            {
                System.Console.Error.Write("Subscribing to channels: ");
                foreach (uint channel in channels)
                {
                    try
                    {
                        lbmWildcardReceiver.subscribeChannel(channel, wrcv.onReceive, null);
                        System.Console.Error.Write("{0} ", channel);
                    }
                    catch (Exception e)
                    {
                        System.Console.Error.WriteLine();
                        System.Console.Error.WriteLine(e.Message);
                    }
                }
                System.Console.Error.WriteLine();
            }

            LBMMonitorSource lbmmonsrc = null;
            if (monitor_context)
            {
                lbmmonsrc = new LBMMonitorSource(mon_format, mon_format_options, mon_transport, mon_transport_options);
                lbmmonsrc.start(ctx, application_id, monitor_context_ivl);
            }
            System.Console.Out.Flush();
            long start_time;
            long end_time;
            long last_lost = 0, lost_tmp = 0, lost = 0;
            bool have_stats;
            LBMReceiverStatistics stats = null;
            while (true)
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

                have_stats = false;
                while (!have_stats)
                {
                    try
                    {
                        stats = ctx.getReceiverStatistics(nstats);
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

                lost = 0;
                for (int stat = 0; stat < stats.size(); stat++)
                {
                    lost += stats.lost(stat);
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
                    wrcv.msg_count,
                    wrcv.byte_count,
                    wrcv.unrec_count,
                    lost,
                    wrcv.burst_loss,
                    wrcv.rx_msgs,
                    wrcv.otr_msgs);

                wrcv.msg_count = 0;
                wrcv.byte_count = 0;
                wrcv.unrec_count = 0;
                wrcv.burst_loss = 0;
                wrcv.rx_msgs = 0;
                wrcv.otr_msgs = 0;

                if (print_stats_flag)
                {
                    print_stats(stats, evq);
                }

                if (reap_msgs != 0 && wrcv.total_msg_count >= reap_msgs)
                {
                    break;
                }
            }

            System.Console.Error.WriteLine("Quitting.... received " + wrcv.total_msg_count + " messages");

            if (channels.Count > 0)
            {
                /* Unsubscribe from channels */
                foreach (uint channel in channels)
                {
                    lbmWildcardReceiver.unsubscribeChannel(channel);
                }
            }

            lbmWildcardReceiver.close();
            ctx.close();

            if (eventq)
                evq.close();
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

    }
}
