
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
using com.latencybusters.lbm;

namespace LBMApplication
{
    class lbmresp
    {
        [DllImport("Kernel32.dll")]
        public static extern int SetEnvironmentVariable(string name, string value);

        private static bool eventq = false;
        private static int verbose = 0;
        private static bool end_on_eos = false;
        private static string purpose = "Purpose: Respond to request messages on a single topic.";
        private static string usage =
              "Usage: lbmresp [options] topic\n"
            + "Available options:\n"
            + "  -c filename = Use LBM configuration file filename.\n"
            + "                Multiple config files are allowed.\n"
            + "                Example:  '-c file1.cfg -c file2.cfg'\n"
            + "  -E = end after end-of-stream\n"
            + "  -h = help\n"
            + "  -l len = use len bytes for the length of each response\n"
            + "  -r responses = send responses messages for each request\n"
            + "  -q = implement with EventQueues\n"
            + "  -v = be verbose about each message\n"
            + "  -v -v = be even more verbose about each message\n"
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

            int response_len = 25;
            int responses = 1;
            int i;
            int n = args.Length;
            bool error = false;
            bool done = false;

            LBMObjectRecycler objRec = new LBMObjectRecycler();
            //Lower the defaults for messages since we expect a lower rate that request will be arriving
            objRec.setLocalMsgPoolSize(10);
            objRec.setSharedMsgPoolSize(20);

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
                                System.Console.Error.WriteLine("lbmresp error: " + Ex.Message);
                                error = true;
                            }
                            break;
                        case "-E":
                            end_on_eos = true;
                            break;
                        case "-h":
                            print_help_exit(0);
                            break;
                        case "-l":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            response_len = Convert.ToInt32(args[i]);
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
                            responses = Convert.ToInt32(args[i]);
                            if (responses <= 0)
                            {
                                /*Negative # of responses not allowed*/
                                print_help_exit(1);
                            }
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
                    System.Console.Error.WriteLine("lbmresp: error\n" + e.Message + "\n");
                    print_help_exit(1);
                }
            }

            if (error || i >= n)
            {
                /* An error occurred processing the command line - print help and exit */
                print_help_exit(1);
            }

            byte[] response_buffer = new byte[response_len];
            LBMContextAttributes lbmContextAttributes = new LBMContextAttributes();
            lbmContextAttributes.setObjectRecycler(objRec, null);

            LBMContext lbmContext = new LBMContext(lbmContextAttributes);
            LBMRespEventQueue evq = null;
            //LBMTopic topic =  ctx.lookupTopic(args[i]);
            LBMReceiverAttributes lbmReceiverAttributes = new LBMReceiverAttributes();
            lbmReceiverAttributes.setObjectRecycler(objRec, null);

            LBMTopic lbmTopic = new LBMTopic(lbmContext, args[i], lbmReceiverAttributes);
            LBMApplication.LBMRespReceiver lbmRespReceiver;

            if (eventq)
            {
                System.Console.Error.WriteLine("Event queue in use");
                evq = new LBMRespEventQueue();
                lbmRespReceiver = new LBMApplication.LBMRespReceiver(lbmContext, lbmTopic, evq, verbose, end_on_eos);
                lbmContext.enableImmediateMessageReceiver(evq);
            }
            else
            {
                System.Console.Error.WriteLine("No event queue");
                lbmRespReceiver = new LBMApplication.LBMRespReceiver(lbmContext, lbmTopic, verbose, end_on_eos);
                lbmContext.enableImmediateMessageReceiver();
            }

            lbmContext.addImmediateMessageReceiver(new LBMImmediateMessageCallback(lbmRespReceiver.onReceiveImmediate));

            try
            {
                ASCIIEncoding enc = new ASCIIEncoding();
                while (true)
                {
                    if (!eventq)
                    {
                        System.Threading.Thread.Sleep(0);
                    }
                    else
                    {
                        evq.run(0);
                    }

                    if (lbmRespReceiver.lbmReqMessage != null)
                    {
                        System.Console.Out.WriteLine("Sending response. {0} response {1} of {2} bytes{3} ({4} total bytes).\n",
                                responses, (responses == 1 ? "" : "s"), response_len,
                                (responses == 1 ? "" : " each"), responses * response_len);
                        System.Console.Out.Flush();

                        for (i = 0; i < responses; i++)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendFormat("response {0}", i);
                            enc.GetBytes(sb.ToString(), 0, sb.ToString().Length, response_buffer, 0);

                            lbmRespReceiver.lbmReqMessage.respond(response_buffer, response_len, 0);
                        }

                        lbmRespReceiver.lbmReqMessage.dispose();
                        objRec.doneWithMessage(lbmRespReceiver.lbmReqMessage);
                        lbmRespReceiver.lbmReqMessage = null;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: {0}", e.Message);
            }
        }

        private static void print_help_exit(int exit_value)
        {
            System.Console.Error.WriteLine(LBM.version());
            Console.WriteLine();

            System.Console.Error.WriteLine(purpose);
            Console.WriteLine();

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
    }
}
