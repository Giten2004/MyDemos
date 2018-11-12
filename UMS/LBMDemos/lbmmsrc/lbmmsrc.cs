
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
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using com.latencybusters.lbm;

namespace LBMApplication
{
    class lbmmsrc
    {
        [DllImport("Kernel32.dll")]
        public static extern int SetEnvironmentVariable(string name, string value);

        private const int default_num_sources = 100;
        private const int default_num_threads = 1;
        private const int max_num_sources = 100000;
        private const int max_num_threads = 16;
        private const int max_msg_sz = 3000000;
        private const int default_max_messages = 10000000;
        private const string default_topic_root = "29west.example.multi";
        private const int default_initial_topic_number = 0;

        private static int msgs = 10000000;
        private static string purpose = "Purpose: Send messages on multiple topics.";
        private static string usage =
              "Usage: lbmmsrc [options]\n"
            + "Available options:\n"
            + "  -c filename = Use LBM configuration file filename.\n"
            + "                Multiple config files are allowed.\n"
            + "                Example:  '-c file1.cfg -c file2.cfg'\n"
            + "  -d delay = delay sending for delay seconds after source creation\n"
            + "  -h = help\n"
            + "  -i num = initial topic number\n"
            + "  -l len = send messages of len bytes\n"
            + "  -L linger = linger for linger seconds before closing context\n"
            + "  -M msgs = send maximum of msgs number of messages\n"
            + "  -r root = use topic names with root of \"root\"\n"
            + "  -s = print source statistics before exiting\n"
            + "  -P msec = pause msec milliseconds after each send\n"
            + "  -R [UM]DATA/RETR = Set transport type to LBT-R[UM], set data rate limit to\n"
            + "                     DATA bits per second, and set retransmit rate limit to\n"
            + "                     RETR bits per second.  For both limits, the optional\n"
            + "                     k, m, and g suffixes may be used.  For example,\n"
            + "                     '-R 1m/500k' is the same as '-R 1000000/500000'\n"
            + "  -S srcs = use srcs sources\n"
            + "  -T thrds = use thrds threads\n"
            + "\nMonitoring options:\n"
            + "  --monitor-ctx NUM = monitor context every NUM seconds\n"
            + "  --monitor-src NUM = monitor each source every NUM seconds\n"
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

            int send_rate = 0;                          //	Used for lbmtrm | lbtru transports
            int retrans_rate = 0;                       //
            char protocol = '\0';                       //
            int linger = 5;
            int delay = 1;
            int msglen = 25;
            int pause = 0;
            bool do_stats = false;
            int initial_topic_number = default_initial_topic_number;
            string topicroot = default_topic_root;
            int num_srcs = default_num_sources;
            int num_thrds = default_num_threads;
            int i;
            int n = args.Length;
            bool monitor_context = false;
            int monitor_context_ivl = 0;
            bool monitor_source = false;
            int monitor_source_ivl = 0;
            string application_id = null;
            int mon_format = LBMMonitor.FORMAT_CSV;
            int mon_transport = LBMMonitor.TRANSPORT_LBM;
            string mon_format_options = null;
            string mon_transport_options = null;
            bool error = false;
            bool done = false;

            const string OPTION_MONITOR_CTX = "--monitor-ctx";
            const string OPTION_MONITOR_SRC = "--monitor-src";
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
                        case OPTION_MONITOR_SRC:
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            monitor_source = true;
                            monitor_source_ivl = Convert.ToInt32(args[i]);
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
                                System.Console.Error.WriteLine("lbmmsrc error: " + Ex.Message);
                                error = true;
                            }
                            break;
                        case "-d":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            delay = Convert.ToInt32(args[i]);
                            System.Console.Out.WriteLine("DELAY " + delay);
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
                        case "-l":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            msglen = Convert.ToInt32(args[i]);
                            break;
                        case "-L":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            linger = Convert.ToInt32(args[i]);
                            break;
                        case "-M":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            msgs = Convert.ToInt32(args[i]);
                            break;
                        case "-P":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            pause = Convert.ToInt32(args[i]);
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
                            ParseRateVars parseRateVars = lbmExampleUtil.parseRate(args[i]);
                            if (parseRateVars.error)
                            {
                                print_help_exit(1);
                            }
                            send_rate = parseRateVars.rate;
                            retrans_rate = parseRateVars.retrans;
                            protocol = parseRateVars.protocol;
                            break;
                        case "-s":
                            do_stats = true;
                            break;
                        case "-S":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            num_srcs = Convert.ToInt32(args[i]);
                            if (num_srcs > max_num_sources)
                            {
                                System.Console.Error.WriteLine("Too many sources specified. Max number of sources is " + max_num_sources);
                                System.Environment.Exit(1);
                            }
                            break;
                        case "-T":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            num_thrds = Convert.ToInt32(args[i]);
                            if (num_thrds > max_num_threads)
                            {
                                System.Console.Error.WriteLine("Too many threads specified. Max number of threads is " + max_num_threads);
                                System.Environment.Exit(1);
                            }
                            break;
                        default:
                            if (args[i].StartsWith("-"))
                            {
                                System.Console.Out.WriteLine("DEFAULT ERROR=TRUE");
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
                    System.Console.Error.WriteLine("lbmmsrc: error\n" + e.Message + "\n");
                    print_help_exit(1);
                }
            }
            if (error)
            {
                /* An error occurred processing the command line - print help and exit */
                print_help_exit(1);
            }


            byte[] message = new byte[msglen];
            if (num_thrds > num_srcs)
            {
                System.Console.Error.WriteLine("Number of threads must be less than or equal to number of sources");
                System.Environment.Exit(1);
            }

            LBMSourceAttributes lbmSourceAttributes = new LBMSourceAttributes();
            lbmSourceAttributes.setObjectRecycler(objRec, null);
            LBMContextAttributes lbmContextAttributes = new LBMContextAttributes();
            lbmContextAttributes.setObjectRecycler(objRec, null);

            /* Check if protocol needs to be set to lbtrm | lbtru */
            if (protocol == 'M')
            {
                try
                {
                    lbmSourceAttributes.setValue("transport", "LBTRM");
                    lbmContextAttributes.setValue("transport_lbtrm_data_rate_limit", send_rate.ToString());
                    lbmContextAttributes.setValue("transport_lbtrm_retransmit_rate_limit", retrans_rate.ToString());
                }
                catch (LBMException ex)
                {
                    System.Console.Error.WriteLine("Error setting LBTRM rate: " + ex.Message);
                    System.Environment.Exit(1);
                }
            }
            if (protocol == 'U')
            {
                try
                {
                    lbmSourceAttributes.setValue("transport", "LBTRU");
                    lbmContextAttributes.setValue("transport_lbtru_data_rate_limit", send_rate.ToString());
                    lbmContextAttributes.setValue("transport_lbtru_retransmit_rate_limit", retrans_rate.ToString());
                }
                catch (LBMException ex)
                {
                    System.Console.Error.WriteLine("Error setting LBTRU rate: " + ex.Message);
                    System.Environment.Exit(1);
                }
            }

            LBMContext ctx = new LBMContext(lbmContextAttributes);
            LBMMonitorSource lbmMonitorSource = null;
            if (monitor_context || monitor_source)
            {
                lbmMonitorSource = new LBMMonitorSource(mon_format, mon_format_options, mon_transport, mon_transport_options);
                if (monitor_context)
                    lbmMonitorSource.start(ctx, application_id, monitor_context_ivl);
            }

            MSrcCB srccb = new MSrcCB();
            LBMSource[] sources = new LBMSource[num_srcs]; ;
            for (i = 0; i < num_srcs; i++)
            {
                int topicnum = initial_topic_number + i;
                string topicname = topicroot + "." + topicnum;

                LBMTopic topic = ctx.allocTopic(topicname, lbmSourceAttributes);
                sources[i] = ctx.createSource(topic, new LBMSourceEventCallback(srccb.onSourceEvent), null, null);

                if (i > 1 && (i % 1000) == 0)
                {
                    System.Console.Out.WriteLine("Created " + i + " sources");
                }
                if (monitor_source)
                {
                    lbmMonitorSource.start(sources[i], application_id + "(" + i + ")", monitor_source_ivl);
                }
            }

            if (delay > 0)
            {
                System.Console.Out.WriteLine("Delaying sending for {0} second{1}...\n", delay, ((delay > 1) ? "s" : ""));
                Thread.Sleep(delay * 1000);
            }

            System.Console.Out.WriteLine("Created " + num_srcs + " sources. Will start sending data now.\n");
            System.Console.Out.WriteLine("Using " + num_thrds + " threads to send " +
                                            msgs + " messages of size " + msglen +
                                            " bytes (" + (msgs / num_thrds) + " messages per thread).");
            System.Console.Out.Flush();


            LBMSrcThread[] srcthreads = new LBMSrcThread[num_thrds];
            for (i = 1; i < num_thrds; i++)
            {
                srcthreads[i] = new LBMSrcThread(i, num_thrds, message, msglen, msgs / num_thrds, sources, num_srcs, pause);
                srcthreads[i].start();
            }
            srcthreads[0] = new LBMSrcThread(0, num_thrds, message, msglen, msgs / num_thrds, sources, num_srcs, pause);
            srcthreads[0].run();

            System.Console.Out.WriteLine("\nDone sending on thread 0. Waiting for any other threads to finish.");

            for (i = 1; i < num_thrds; i++)
            {
                System.Console.Out.WriteLine("Joining thread " + i);
                srcthreads[i].join();
                System.Console.Out.WriteLine("Joined thread " + i);
            }
            System.Console.Out.Flush();

            if (linger > 0)
            {
                System.Console.Out.WriteLine("\nLingering for {0} second{1}...\n",
                                             linger, ((linger > 1) ? "s" : ""));
                System.Threading.Thread.Sleep(linger * 1000);
            }

            if (do_stats)
                print_stats(ctx, num_srcs, sources[0].getAttributeValue("transport"), objRec);
            System.Console.Out.WriteLine("Quitting...");

            objRec.close();
            for (i = 0; i < num_srcs; i++)
            {
                sources[i].close();
            }
            ctx.close();
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

        private static void print_stats(LBMContext ctx, int nsrcs, string transport_type, LBMObjectRecyclerBase recycler)
        {
            int n;
            if (transport_type == "LBT-RM")
            {
                n = (int)(inet_aton(ctx.getAttributeValue("transport_lbtrm_multicast_address_high"))
                    - inet_aton(ctx.getAttributeValue("transport_lbtrm_multicast_address_low"))) + 1;
            }
            else if (transport_type == "LBT-IPC")
            {
                n = Convert.ToInt32(ctx.getAttributeValue("transport_lbtipc_id_high"))
                    - Convert.ToInt32(ctx.getAttributeValue("transport_lbtipc_id_low")) + 1;
            }
            else
                n = Convert.ToInt32(ctx.getAttributeValue("transport_tcp_maximum_ports"));

            if (nsrcs < n)
                n = nsrcs;

            LBMSourceStatistics stats = ctx.getSourceStatistics(n);
            for (int i = 0; i < stats.size(); i++)
            {
                switch (stats.type(i))
                {
                    case LBM.TRANSPORT_STAT_TCP:
                        System.Console.Out.WriteLine("TCP, source " + stats.source(i)
                                                    + " buffered " + stats.bytesBuffered(i)
                                                    + ", clients " + stats.numberOfClients(i));
                        break;
                    case LBM.TRANSPORT_STAT_LBTRU:
                        System.Console.Out.WriteLine("LBT-RU, source " + stats.source(i)
                                                    + " sent " + stats.messagesSent(i)
                                                    + "/" + stats.bytesSent(i)
                                                    + ", naks " + stats.naksReceived(i)
                                                    + "/" + stats.nakPacketsReceived(i)
                                                    + ", ignored " + stats.naksIgnored(i)
                                                    + "/" + stats.naksIgnoredRetransmitDelay(i)
                                                    + ", shed " + stats.naksShed(i)
                                                    + ", rxs " + stats.retransmissionsSent(i)
                                                    + ", clients " + stats.numberOfClients(i));
                        break;
                    case LBM.TRANSPORT_STAT_LBTRM:
                        System.Console.Out.WriteLine("LBT-RM, source " + stats.source(i)
                                                    + " sent " + stats.messagesSent(i)
                                                    + "/" + stats.bytesSent(i)
                                                    + ", txw " + stats.transmissionWindowMessages(i)
                                                    + "/" + stats.transmissionWindowBytes(i)
                                                    + ", naks " + stats.naksReceived(i)
                                                    + "/" + stats.nakPacketsReceived(i)
                                                    + ", ignored " + stats.naksIgnored(i)
                                                    + "/" + stats.naksIgnoredRetransmitDelay(i)
                                                    + ", shed " + stats.naksShed(i)
                                                    + ", rxs " + stats.retransmissionsSent(i)
                                                    + ", rctl " + stats.messagesQueued(i)
                                                    + "/" + stats.retransmissionsQueued(i));
                        break;
                    case LBM.TRANSPORT_STAT_LBTIPC:
                        System.Console.Out.WriteLine("LBT-IPC, source " + stats.source(i)
                                                    + " clients " + stats.numberOfClients(i)
                                                    + ", sent " + stats.messagesSent(i)
                                                    + "/" + stats.bytesSent(i));
                        break;
                    case LBM.TRANSPORT_STAT_LBTSMX:
                        System.Console.Out.WriteLine("LBT-SMX, source " + stats.source(i)
                                                    + " clients " + stats.numberOfClients(i)
                                                    + ", sent " + stats.messagesSent(i)
                                                    + "/" + stats.bytesSent(i));
                        break;
                    case LBM.TRANSPORT_STAT_LBTRDMA:
                        System.Console.Out.WriteLine("LBT-RDMA, source " + stats.source(i)
                                                    + " clients " + stats.numberOfClients(i)
                                                    + ", sent " + stats.messagesSent(i)
                                                    + "/" + stats.bytesSent(i));
                        break;
                }
            }
            recycler.doneWithSourceStatistics(stats);
            System.Console.Out.Flush();
        }

        public static uint inet_aton(string addr)
        {
            int i;
            string[] arrDec;
            uint num = 0;
            if (addr == "")
            {
                return 0;
            }
            else
            {
                arrDec = addr.Split('.');
                for (i = arrDec.Length - 1; i >= 0; i--)
                {
                    num += Convert.ToUInt32(arrDec[i]) << (8 * (3 - i));
                }
                return num;
            }
        }

    }
}
