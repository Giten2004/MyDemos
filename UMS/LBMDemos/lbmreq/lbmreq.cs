
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
using System.Text;
using System.Threading;
using com.latencybusters.lbm;
using lbmreq;

namespace LBMApplication
{
    class lbmreq
    {
        [DllImport("Kernel32.dll")]
        public static extern int SetEnvironmentVariable(string name, string value);
        private static int MIN_ALLOC_MSGLEN = 25;
        private static int requests = 10000000;
        private static bool send_immediate;
        private static int stats_sec;
        private static int verbose;
        private static bool eventq;
        private static string purpose = "Purpose: Send request messages from a single source with setttable interval between messages.";
        private static string usage =
              "Usage: lbmreq [options] topic\n"
            + "Available options:\n"
            + "  -c filename = Use LBM configuration file filename.\n"
            + "                Multiple config files are allowed.\n"
            + "                Example:  '-c file1.cfg -c file2.cfg'\n"
            + "  -d sec = delay sending initial request sec seconds after\n"
            + "            source creation\n"
            + "  -h = help\n"
            + "  -i = send immediate requests\n"
            + "  -q = implement with EventQueues\n"

            + "  -l len = send messages of len bytes\n"
            + "  -L linger = linger for linger seconds before closing context\n"
            + "  -P lbmevq = pause lbmevq after sending request (for responses to arrive)\n"
            + "  -r [UM]DATA/RETR = Set transport type to LBT-R[UM], set data rate limit to\n"
            + "                     DATA bits per second, and set retransmit rate limit to\n"
            + "                     RETR bits per second.  For both limits, the optional\n"
            + "                     k, m, and g suffixes may be used.  For example,\n"
            + "                     '-r 1m/500k' is the same as '-r 1000000/500000'\n"
            + "  -R requests = send request number of requests\n"
            + "  -T target = target for unicast immediate requests\n"
            + "  -v = be verbose \n"
            + "  -v -v = be even more verbose\n"
            ;

        static void Main(string[] args)
        {
            if (Environment.GetEnvironmentVariable("LBM_LICENSE_FILENAME") == null
                && Environment.GetEnvironmentVariable("LBM_LICENSE_INFO") == null)
            {
                SetEnvironmentVariable("LBM_LICENSE_FILENAME", "lbm_license.txt");
            }

            LBM lbm = new LBM();
            lbm.setLogger(logger);

            string target = null;
            int send_rate = 0;                          //	Used for lbmtrm | lbtru transports
            int retrans_rate = 0;                       //
            char protocol = '\0';                       //
            int linger = 5;
            int msglen = MIN_ALLOC_MSGLEN;
            int pause_milliseconds = 5;
            int delay = 1;
            int i;
            int n = args.Length;
            bool error = false;
            bool done = false;

            for (i = 0; i < n; i++)
            {
                try
                {
                    switch (args[i])
                    {
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
                                Console.Error.WriteLine("lbmreq error: " + Ex.Message);
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
                            break;
                        case "-h":
                            print_help_exit(0);
                            break;
                        case "-i":
                            send_immediate = true;
                            break;
                        case "-q":
                            eventq = true;
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
                        case "-P":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            pause_milliseconds = Convert.ToInt32(args[i]);
                            break;
                        case "-R":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            requests = Convert.ToInt32(args[i]);
                            break;
                        case "-r":
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
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            stats_sec = Convert.ToInt32(args[i]);
                            break;
                        case "-T":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            target = args[i];
                            break;
                        case "-v":
                            verbose++;
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
                    Console.Error.WriteLine("lbmreq: error\n" + e.Message + "\n");
                    print_help_exit(1);
                }
            }
            if (error || i >= n)
            {
                /* An error occurred processing the command line - print help and exit */
                print_help_exit(1);
            }
            string topic_str = null;
            if (i >= n)
            {
                if (!send_immediate)
                {
                    print_help_exit(1);
                }
            }
            else
            {
                topic_str = args[i];
            }

            byte[] message = null;
            /* if message buffer is too small, then the enc.GetBytes will cause issues. 
			Therefore, allocate with a MIN_ALLOC_MSGLEN */
            if (msglen < MIN_ALLOC_MSGLEN)
            {
                message = new byte[MIN_ALLOC_MSGLEN];
            }
            else
            {
                message = new byte[msglen];
            }

            LBMSourceAttributes lbmSourceAttributes = new LBMSourceAttributes();
            LBMContextAttributes lbmContextAttributes = new LBMContextAttributes();

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
                    Console.Error.WriteLine("Error setting LBTRM rate: " + ex.Message);
                    Environment.Exit(1);
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
                    Console.Error.WriteLine("Error setting LBTRU rate: " + ex.Message);
                    Environment.Exit(1);
                }
            }

            LBMContext ctx = new LBMContext(lbmContextAttributes);
            LBMreqEventQueue lbmReqEventQueue = null;
            if (eventq)
            {
                lbmReqEventQueue = new LBMreqEventQueue();
                Console.Error.WriteLine("Event queue in use.");
            }
            else
            {
                Console.Error.WriteLine("No event queue\n");
            }

            LBMTopic topic;
            LBMSource src = null;
            LBMreqCB lbMreqCallBack = new LBMreqCB(verbose);

            if (!send_immediate)
            {
                topic = ctx.allocTopic(topic_str, lbmSourceAttributes);
                src = ctx.createSource(topic, lbMreqCallBack.onSourceEvent, null, lbmReqEventQueue);
                if (delay > 0)
                {
                    Console.Out.WriteLine("Delaying requests for {0} second{1}...\n",
                                                 delay, ((delay > 1) ? "s" : ""));
                    Thread.Sleep(delay * 1000);
                }
            }
            ASCIIEncoding enc = new ASCIIEncoding();
            if (requests > 0)
                Console.Out.WriteLine("Will send {0} request{1}\n", requests, (requests == 1 ? "" : "s"));

            Console.Out.Flush();

            for (int count = 0; count < requests; count++)
            {
                LBMTimer qTimer;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Request data {0}", count);
                enc.GetBytes(sb.ToString(), 0, sb.ToString().Length, message, 0);

                LBMRequest req = new LBMRequest(message, msglen);
                req.addResponseCallback(lbMreqCallBack.onResponse);

                Console.Out.WriteLine("Sending request " + count);

                if (send_immediate)
                {
                    if (target == null)
                    {
                        if (eventq)
                            ctx.send(topic_str, req, lbmReqEventQueue, 0);
                        else
                            ctx.send(topic_str, req, 0);
                    }
                    else
                    {
                        if (eventq)
                            ctx.send(target, topic_str, req, lbmReqEventQueue, 0);
                        else
                            ctx.send(target, topic_str, req, 0);
                    }
                }
                else
                {
                    if (eventq)
                        src.send(req, lbmReqEventQueue, 0);
                    else
                        src.send(req, 0);
                }

                if (verbose > 0)
                    Console.Out.Write("Sent request " + count + ". ");

                if (!eventq)
                {
                    if (verbose > 0)
                        Console.Out.WriteLine("Pausing " + pause_milliseconds + " seconds.");

                    Thread.Sleep(pause_milliseconds);
                }
                else
                {
                    if (verbose > 0)
                        Console.Out.WriteLine("Creating timer for " + pause_milliseconds + " seconds and initiating event pump.");

                    qTimer = new EQTimer(ctx, pause_milliseconds, lbmReqEventQueue);
                    lbmReqEventQueue.run(LBM.EVENT_QUEUE_BLOCK);
                }

                Console.Out.WriteLine("Done waiting for responses, {0} response{1} ({2} total bytes) received. Deleting request.\n",
                    lbMreqCallBack.response_count, (lbMreqCallBack.response_count == 1 ? "" : "s"), lbMreqCallBack.response_byte_count);

                lbMreqCallBack.response_count = 0;
                lbMreqCallBack.response_byte_count = 0;

                req.close();

                Console.Out.Flush();
            }//end of for

            if (linger > 0)
            {
                Console.Out.WriteLine("\nLingering for {0} second{1}...\n", linger, ((linger > 1) ? "s" : ""));
                Thread.Sleep(linger * 1000);
            }

            Console.Out.WriteLine("Quitting...");
            if (src != null)
                src.close();
            ctx.close();
        }

        private static void print_help_exit(int exit_value)
        {
            Console.Error.WriteLine(LBM.version());
            Console.WriteLine();
            Console.Error.WriteLine(purpose);
            Console.WriteLine();
            Console.Error.WriteLine(usage);

            Environment.Exit(exit_value);
        }

        private static void logger(int loglevel, string message)
        {
            string level;
            switch (loglevel)
            {
                case LBM.LOG_ALERT:
                    level = "Alert";
                    break;
                case LBM.LOG_CRIT:
                    level = "Critical";
                    break;
                case LBM.LOG_DEBUG:
                    level = "Debug";
                    break;
                case LBM.LOG_EMERG:
                    level = "Emergency";
                    break;
                case LBM.LOG_ERR:
                    level = "Error";
                    break;
                case LBM.LOG_INFO:
                    level = "Info";
                    break;
                case LBM.LOG_NOTICE:
                    level = "Note";
                    break;
                case LBM.LOG_WARNING:
                    level = "Warning";
                    break;
                default:
                    level = "Unknown";
                    break;
            }
            Console.Out.WriteLine(DateTime.Now + " [" + level + "]: " + message);
            Console.Out.Flush();
        }

        private static void print_bw(double sec, int msgs, long bytes)
        {
            char[] scale = { '\0', 'K', 'M', 'G' };
            double mps = 0.0, bps = 0.0;
            double kscale = 1000.0;
            int msg_scale_index = 0, bit_scale_index = 0;

            if (sec == 0)
                return; /* avoid division by zero */

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

            Console.Out.WriteLine(sec
                       + " secs. "
                       + mps.ToString("0.000")
                       + " " + scale[msg_scale_index] + "msgs/sec. "
                       + bps.ToString("0.000")
                       + " " + scale[bit_scale_index] + "bps");
            Console.Out.Flush();
        }
    }
}