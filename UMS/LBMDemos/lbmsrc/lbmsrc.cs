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
using System.Runtime.InteropServices;
using com.latencybusters.lbm;
using com.latencybusters.lbm.sdm;
using System.IO;

namespace LBMApplication
{
    class lbmsrc
    {
        const string OPTION_MONITOR_CTX = "--monitor-ctx";
        const string OPTION_MONITOR_SRC = "--monitor-src";
        const string OPTION_MONITOR_TRANSPORT = "--monitor-transport";
        const string OPTION_MONITOR_TRANSPORT_OPTS = "--monitor-transport-opts";
        const string OPTION_MONITOR_FORMAT = "--monitor-format";
        const string OPTION_MONITOR_FORMAT_OPTS = "--monitor-format-opts";
        const string OPTION_MONITOR_APPID = "--monitor-appid";

        [DllImport("Kernel32.dll")]
        public static extern int SetEnvironmentVariable(string name, string value);

        private static long msgs = 10000000;
        private static int stats_sec = 0;
        private static string purpose = "Purpose: Send messages on a single topic.";
        private static bool sdm = false;
        private static bool verifiable = false;
        private static string usage =
              "Usage: lbmsrc [options] topic\n"
            + "Available options:\n"
            + "  -c filename = Use LBM configuration file filename.\n"
            + "                Multiple config files are allowed.\n"
            + "                Example:  '-c file1.cfg -c file2.cfg'\n"
            + "  -d delay = delay sending for delay seconds after source creation\n"
            + "  -D = Use SDM Messages\n"
            + "  -f = use hot-failover\n"
            + "  -i seq = hot-failover: begin sequencing with this number\n"
            + "  -h = help\n"
            + "  -l len = send messages of len bytes\n"
            + "  -L linger = linger for linger seconds before closing context\n"
            + "  -M msgs = send msgs number of messages\n"
            + "  -N chn = send messages on channel chn\n"
            + "  -n = used non-blocking I/O\n"
            + "  -P msec = pause after each send msec milliseconds\n"
            + "  -R [UM]DATA/RETR = Set transport type to LBT-R[UM], set data rate limit to\n"
            + "                     DATA bits per second, and set retransmit rate limit to\n"
            + "                     RETR bits per second.  For both limits, the optional\n"
            + "                     k, m, and g suffixes may be used.  For example,\n"
            + "                     '-R 1m/500k' is the same as '-R 1000000/500000'\n"
            + "  -s sec = print stats every sec seconds\n"
            + "  -t filename = use filename contents as a recording of message sequence numbers (HF only!)\n"
            + "  -V = construct verifiable messages\n"
            + "  -x bits = Use 32 or 64 bits for hot-failover sequence numbers\n"
            + "\nMonitoring options:\n"
            + "  --monitor-ctx NUM = monitor context every NUM seconds\n"
            + "  --monitor-src NUM = monitor source every NUM seconds\n"
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
            int msglen = 25;
            int linger = 5;
            int delay = 1;
            ulong bytes_sent = 0;
            int pause = 0;
            int i;
            bool block = true;
            int hfbits = 32;
            int argsLength = args.Length;
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
            bool use_hf = false;
            ulong seq_counter = 0;
            long channel = -1;
            string tape_file = null;
            StreamReader tape_scanner = null;
            int tape_msgs_sent = 0;

            for (i = 0; i < argsLength; i++)
            {
                try
                {
                    switch (args[i])
                    {
                        case OPTION_MONITOR_APPID:
                            if (++i >= argsLength)
                            {
                                error = true;
                                break;
                            }
                            application_id = args[i];
                            break;
                        case OPTION_MONITOR_CTX:
                            if (++i >= argsLength)
                            {
                                error = true;
                                break;
                            }
                            monitor_context = true;
                            monitor_context_ivl = Convert.ToInt32(args[i]);
                            break;
                        case OPTION_MONITOR_SRC:
                            if (++i >= argsLength)
                            {
                                error = true;
                                break;
                            }
                            monitor_source = true;
                            monitor_source_ivl = Convert.ToInt32(args[i]);
                            break;
                        case OPTION_MONITOR_FORMAT:
                            if (++i >= argsLength)
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
                            if (++i >= argsLength)
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
                            if (++i >= argsLength)
                            {
                                error = true;
                                break;
                            }
                            mon_transport_options += args[i];
                            break;
                        case OPTION_MONITOR_FORMAT_OPTS:
                            if (++i >= argsLength)
                            {
                                error = true;
                                break;
                            }
                            mon_format_options += args[i];
                            break;
                        case "-c":
                            if (++i >= argsLength)
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
                                System.Console.Error.WriteLine("lbmsrc error: " + Ex.Message);
                                error = true;
                            }
                            break;
                        case "-d":
                            if (++i >= argsLength)
                            {
                                error = true;
                                break;
                            }
                            delay = Convert.ToInt32(args[i]);
                            break;
                        case "-D":
                            if (verifiable)
                            {
                                System.Console.Error.WriteLine("Unable to use SDM because verifiable messages are on. Turn off verifiable messages (-V).");
                                System.Environment.Exit(1);
                            }
                            sdm = true;
                            break;
                        case "-f":
                            use_hf = true;
                            break;
                        case "-i":
                            if (++i >= argsLength)
                            {
                                error = true;
                                break;
                            }
                            seq_counter = ulong.Parse(args[i]);
                            break;
                        case "-h":
                            print_help_exit(0);
                            break;

                        case "-l":
                            if (++i >= argsLength)
                            {
                                error = true;
                                break;
                            }
                            msglen = Convert.ToInt32(args[i]);
                            break;
                        case "-n":
                            block = false;
                            break;
                        case "-L":
                            if (++i >= argsLength)
                            {
                                error = true;
                                break;
                            }
                            linger = Convert.ToInt32(args[i]);
                            break;
                        case "-M":
                            if (++i >= argsLength)
                            {
                                error = true;
                                break;
                            }
                            msgs = long.Parse(args[i]);
                            break;
                        case "-N":
                            if (++i >= argsLength)
                            {
                                error = true;
                                break;
                            }
                            channel = Convert.ToInt64(args[i]);
                            break;
                        case "-P":
                            if (++i >= argsLength)
                            {
                                error = true;
                                break;
                            }
                            pause = Convert.ToInt32(args[i]);
                            break;
                        case "-R":
                            if (++i >= argsLength)
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
                            if (++i >= argsLength)
                            {
                                error = true;
                                break;
                            }
                            stats_sec = Convert.ToInt32(args[i]);
                            break;
                        case "-t":
                            if (++i >= argsLength)
                            {
                                error = true;
                                break;
                            }
                            tape_file = args[i];
                            if (!File.Exists(tape_file))
                            {
                                System.Console.Error.WriteLine("{0} does not exist. Verify the file specified by (-t) exists.", tape_file);
                                print_help_exit(1);
                            }
                            tape_scanner = new StreamReader(tape_file);
                            break;
                        case "-V":
                            if (sdm)
                            {
                                System.Console.Error.WriteLine("Unable to use verifiable messages because sdm is on. Turn off sdm (-D).");
                                System.Environment.Exit(1);
                            }
                            verifiable = true;
                            break;
                        case "-x":
                            if (++i >= argsLength)
                            {
                                error = true;
                                break;
                            }
                            hfbits = Convert.ToInt32(args[i]);

                            if (hfbits != 32 && hfbits != 64)
                            {
                                Console.WriteLine("-x " + hfbits + " invalid, HF sequence numbers must be 32 or 64 bit");
                                print_help_exit(1);
                            }
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
                    System.Console.Error.WriteLine("lbmsrc: error\n" + e.Message + "\n");
                    print_help_exit(1);
                }
            }

            if (error || i >= argsLength)
            {
                /* An error occurred processing the command line - print help and exit */
                print_help_exit(1);
            }
            if (tape_scanner != null && !use_hf)
            {
                print_help_exit(1);
            }

            byte[] message = new byte[msglen];

            if (verifiable)
            {
                int min_msglen = VerifiableMessage.MINIMUM_VERIFIABLE_MSG_LEN;
                if (msglen < min_msglen)
                {
                    System.Console.WriteLine("Specified message length " + msglen + " is too small for verifiable message.");
                    System.Console.WriteLine("Setting message length to minimum (" + min_msglen + ").");
                    msglen = min_msglen;
                }
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
            LBMTopic topic = ctx.allocTopic(args[i], lbmSourceAttributes);
            LBMSource src;
            SrcCB srccb = new SrcCB();

            if (use_hf)
                src = ctx.createHotFailoverSource(topic, new LBMSourceEventCallback(srccb.onSourceEvent), null, null);
            else
                src = ctx.createSource(topic, new LBMSourceEventCallback(srccb.onSourceEvent), null, null);

            LBMSourceChannelInfo channel_info = null;
            LBMSourceSendExInfo exInfo = null;
            if (channel != -1)
            {
                if (use_hf)
                {
                    Console.WriteLine("Error creating channel: cannot send on channels with hot failover.");
                    Environment.Exit(1);
                }
                channel_info = src.createChannel(channel);
                exInfo = new LBMSourceSendExInfo(LBM.SRC_SEND_EX_FLAG_CHANNEL, null, channel_info);
            }

            SrcStatsTimer stats;
            if (stats_sec > 0)
            {
                stats = new SrcStatsTimer(ctx, src, stats_sec * 1000, null, objRec);
            }

            LBMMonitorSource lbmmonsrc = null;
            if (monitor_context || monitor_source)
            {
                lbmmonsrc = new LBMMonitorSource(mon_format, mon_format_options, mon_transport, mon_transport_options);
                if (monitor_context)
                    lbmmonsrc.start(ctx, application_id, monitor_context_ivl);
                else
                    lbmmonsrc.start(src, application_id, monitor_source_ivl);
            }

            if (delay > 0)
            {
                System.Console.Out.WriteLine("Will start sending in {0} second{1}...\n",
                                             delay, ((delay > 1) ? "s" : ""));

                System.Threading.Thread.Sleep(delay * 1000);
            }

            System.Console.Out.WriteLine("Sending {0} messages of size {1} bytes to topic [{2}]\n",
                                         msgs, msglen, args[i]);
            System.Console.Out.Flush();
            long start_time = System.DateTime.Now.Ticks;
            long msgcount = 0;

            if (verifiable)
            {
                message = VerifiableMessage.constructVerifiableMessage(msglen);
            }
            else if (sdm) // If using SDM messages, create the message now
            {
                CreateSDMessage();
            }

            // hfexinfo holds hot failover sequence number at bit size
            LBMSourceSendExInfo hfexinfo = null;
            if (use_hf)
            {
                int hfflags = (hfbits == 64) ? LBM.SRC_SEND_EX_FLAG_HF_64 : LBM.SRC_SEND_EX_FLAG_HF_32;
                hfexinfo = new LBMSourceSendExInfo(hfflags, null);
            }
            for (msgcount = 0; msgcount < msgs;)
            {
                int tape_sqn = 0;
                bool tape_sqn_opt = false;
                try
                {
                    // If using SDM messages, Update the sequence number
                    if (sdm)
                    {
                        byte[] m = UpdateSDMessage(seq_counter);
                        if (m != null)
                        {
                            message = m;
                            msglen = message.Length;
                        }
                    }

                    bool sendReset = false;
                    srccb.blocked = true;

                    if (use_hf)
                    {
                        if (tape_scanner != null)
                        {
                            string tape_str, tape_line = null;
                            tape_line = tape_scanner.ReadLine();

                            if (tape_line == null)
                                break;

                            // Make sure the hf optional send ex flag is off
                            hfexinfo.setFlags(hfexinfo.flags() & ~LBM.SRC_SEND_EX_FLAG_HF_OPTIONAL);
                            if (tape_line.EndsWith("o"))
                            {
                                tape_str = tape_line.Substring(0, tape_line.Length - 1);
                                hfexinfo.setFlags(hfexinfo.flags() | LBM.SRC_SEND_EX_FLAG_HF_OPTIONAL);
                            }
                            else if (tape_line.EndsWith("r"))
                            {
                                tape_str = tape_line.Substring(0, tape_line.Length - 1);
                                sendReset = true;
                            }
                            else
                            {
                                tape_str = tape_line;
                            }

                            if (hfbits == 64)
                            {
                                hfexinfo.setHfSequenceNumber64(ulong.Parse(tape_str));
                            }
                            else
                            {
                                hfexinfo.setHfSequenceNumber32(uint.Parse(tape_str));
                            }
                        }
                        else if (hfbits == 64)
                        {
                            hfexinfo.setHfSequenceNumber64(seq_counter);
                        }
                        else
                        {
                            hfexinfo.setHfSequenceNumber32((uint)seq_counter);
                        }

                        if (sendReset)
                        {
                            ((LBMHotFailoverSource)src).sendReceiverReset(LBM.MSG_FLUSH, hfexinfo);
                        }
                        else
                        {
                            ((LBMHotFailoverSource)src).send(message, msglen, 0, block ? 0 : LBM.SRC_NONBLOCK, hfexinfo);
                        }
                        if (tape_scanner != null)
                        {
                            tape_msgs_sent++;
                        }
                    }
                    else if (exInfo != null)
                        src.send(message, msglen, block ? 0 : LBM.SRC_NONBLOCK, exInfo);
                    else
                        src.send(message, msglen, block ? 0 : LBM.SRC_NONBLOCK);

                    srccb.blocked = false;
                    if (tape_scanner == null)
                    {
                        seq_counter++;
                        msgcount++;
                    }
                }
                catch (LBMEWouldBlockException)
                {
                    while (srccb.blocked)
                        System.Threading.Thread.Sleep(100);
                    continue;
                }
                bytes_sent += (ulong)msglen;
                if (pause > 0)
                {
                    System.Threading.Thread.Sleep(pause);
                }
            }
            long end_time = System.DateTime.Now.Ticks;
            double secs = (end_time - start_time) / 10000000.0;

            System.Console.Out.WriteLine("Sent {0} messages of size {1} bytes in {2} seconds.\n",
                          (tape_scanner == null) ? msgs : tape_msgs_sent, msglen, secs);

            print_bw(secs, (tape_scanner == null) ? msgs : tape_msgs_sent, bytes_sent);
            System.Console.Out.Flush();

            if (linger > 0)
            {
                System.Console.Out.WriteLine("Lingering for {0} second{1}...\n",
                                             linger, ((linger > 1) ? "s" : ""));
                System.Threading.Thread.Sleep(linger * 1000);
            }

            stats = new SrcStatsTimer(ctx, src, 0, null, objRec);
            if (channel_info != null)
            {
                src.deleteChannel(channel_info);

            }
            objRec.close();
            src.close();

            if (tape_scanner != null)
            {
                tape_scanner.Close();
                tape_scanner.Dispose();
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

        private static LBMSDMessage SDMsg;
        private static void CreateSDMessage()
        {
            try
            {
                // Create an SDM message and add several fields to the message
                SDMsg = new LBMSDMessage();

                // The sequence number is a specific fields which is updated as the message is sent
                LBMSDMFieldUint64 seqfield = new LBMSDMFieldUint64("Sequence Number", 0UL);
                SDMsg.add(seqfield);

                // Some other field types demonstrating SDM Fields.
                LBMSDMFieldUint16 int16bitfield = new LBMSDMFieldUint16("16 Bit Unsigned Int", 16 << 8);
                SDMsg.add(int16bitfield);

                LBMSDMFieldString stringfield = new LBMSDMFieldString("Application Name", "lbmsrc");
                SDMsg.add(stringfield);
            }
            catch (LBMSDMException sdme)
            {
                System.Console.Out.WriteLine("Failed to create the SDM message:" + sdme);
                System.Console.Out.Flush();
            }
        }

        private static byte[] UpdateSDMessage(ulong seq_num)
        {
            if (SDMsg == null)
            {
                // Should not be possible since CreateSDMessage is called before the message loop
                System.Console.Out.WriteLine("No SDM message available");
                return null;
            }
            LBMSDMField f = SDMsg.locate("Sequence Number");
            if (f == null)
            {
                System.Console.Out.WriteLine("Could not find field 'Sequence Number'");
                System.Console.Out.Flush();
                return null;
            }

            ((LBMSDMFieldUint64)f).set(seq_num);

            try
            {
                return SDMsg.data();
            }
            catch (LBMSDMException sdme)
            {
                System.Console.Out.WriteLine("Error occurred updating SDM message with sequence number " + seq_num + ":" + sdme);
                System.Console.Out.Flush();
                return null;
            }
        }

        private static void print_bw(double sec, long msgs, ulong bytes)
        {
            char[] scale = { '\0', 'K', 'M', 'G' };
            double mps = 0.0, bps = 0.0;
            double kscale = 1000.0;
            int msg_scale_index = 0, bit_scale_index = 0;

            if (sec == 0) return; /* avoid division by zero */
            mps = msgs / sec;
            bps = bytes * 8 / sec;

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

            System.Console.Out.WriteLine(sec
                       + " secs. "
                       + mps.ToString("0.000")
                       + " " + scale[msg_scale_index] + "msgs/sec. "
                       + bps.ToString("0.000")
                       + " " + scale[bit_scale_index] + "bps");
        }

    }
}