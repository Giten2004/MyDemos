
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
		public static extern int SetEnvironmentVariable( string name , string value ) ;
		
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
							if (responses <= 0){
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
            LBMContextAttributes cattr = new LBMContextAttributes();
            cattr.setObjectRecycler(objRec, null);
			LBMContext ctx = new LBMContext(cattr);
			LBMRespEventQueue evq = null;
			//LBMTopic topic =  ctx.lookupTopic(args[i]);
            LBMReceiverAttributes rattr = new LBMReceiverAttributes();
            rattr.setObjectRecycler(objRec, null);
            LBMTopic topic = new LBMTopic(ctx, args[i], rattr);
			LBMRespReceiver rcv;
			if (eventq)
			{
				System.Console.Error.WriteLine("Event queue in use");
				evq = new LBMRespEventQueue();
				rcv = new LBMRespReceiver(ctx, topic, evq, verbose, end_on_eos);
				ctx.enableImmediateMessageReceiver(evq);
			}
			else
			{
				System.Console.Error.WriteLine("No event queue");
				rcv = new LBMRespReceiver(ctx, topic, verbose, end_on_eos);
				ctx.enableImmediateMessageReceiver();
			}
			ctx.addImmediateMessageReceiver(new LBMImmediateMessageCallback(rcv.onReceiveImmediate));
			try {
				ASCIIEncoding enc = new ASCIIEncoding();
				while (true)
				{
					if (!eventq)
					{
						System.Threading.Thread.Sleep(100);
					}
					else
					{
						evq.run(100);
					}
					if (rcv.request != null)
					{
						System.Console.Out.WriteLine("Sending response. " + 
						        "{0} response{1} of {2} bytes{3} ({4} total bytes).\n",
								responses, (responses == 1 ? "" : "s"), response_len, 
								(responses == 1 ? "" : " each"), responses * response_len);
						System.Console.Out.Flush();
						for (i = 0; i < responses; i++)
						{
							StringBuilder sb = new StringBuilder();
							sb.AppendFormat("response {0}", i);
							enc.GetBytes(sb.ToString(), 0, sb.ToString().Length, response_buffer, 0);
							rcv.request.respond(response_buffer, response_len, 0);
						}
						rcv.request.dispose();
                        objRec.doneWithMessage(rcv.request);
						rcv.request = null;
					}
				}
			}
			catch(Exception e)
			{
				Console.WriteLine("An error occurred: {0}", e.Message);
			}
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
	}

	class LBMRespEventQueue : LBMEventQueue
	{
		public LBMRespEventQueue()
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

	class LBMRespReceiver
	{
		public long imsg_count = 0;
		public long request_count = 0;
		public LBMMessage request = null;

		int _verbose = 0;
		bool _end_on_eos = false;
		LBMEventQueue _evq = null;
		private LBMReceiver _rcv = null;
		//LBMRespTimer timer;

		public LBMRespReceiver(LBMContext ctx, LBMTopic topic, LBMEventQueue evq, int verbose, bool end_on_eos)
		{
			_verbose = verbose;
			_evq = evq;
			_end_on_eos = end_on_eos;
			_rcv = new LBMReceiver(ctx, topic, new LBMReceiverCallback(onReceive), null, evq);
		}

		public LBMRespReceiver(LBMContext ctx, LBMTopic topic, int verbose, bool end_on_eos)
		{
			_verbose = verbose;
			_end_on_eos = end_on_eos;
			_rcv = new LBMReceiver(ctx, topic, new LBMReceiverCallback(onReceive), null);
		}
 
		public int onReceiveImmediate(object cbArg, LBMMessage msg)
		{
			imsg_count++;
			return onReceive(cbArg, msg);
		}

		protected int onReceive(object cbArg, LBMMessage msg)
		{
            bool promoted = false;
			switch (msg.type())
			{
				case LBM.MSG_DATA:
					if (_verbose > 0)
					{
						System.Console.Out.Write("["
							+ msg.topicName()
							+ "]["
							+ msg.source()
							+ "]["
							+  msg.sequenceNumber()
							+ "], ");
						System.Console.Out.WriteLine(msg.data().Length
																+ " bytes");
						if (_verbose > 1)
							dump(msg);
					}
					break;
				case LBM.MSG_BOS:
					System.Console.Out.WriteLine("[" + msg.topicName() + "][" + msg.source() + "], Beginning of Transport Session");
					break;
				case LBM.MSG_EOS:
					System.Console.Out.WriteLine("[" + msg.topicName() + "][" + msg.source() + "], End of Transport Session");
					if (_end_on_eos)
					{
						end();
					}
					break;
				case LBM.MSG_UNRECOVERABLE_LOSS:
					if (_verbose > 0)
					{
						System.Console.Out.Write("[" + msg.topicName() + "][" + msg.source() + "][" + msg.sequenceNumber() + "],");
						System.Console.Out.WriteLine(" LOST");
					}
					break;
				case LBM.MSG_UNRECOVERABLE_LOSS_BURST:
					if (_verbose > 0)
					{
						System.Console.Out.Write("[" + msg.topicName() + "][" + msg.source() + "][" + msg.sequenceNumber() + "],");
						System.Console.Out.WriteLine(" LOST BURST");
					}
					break;
				case LBM.MSG_REQUEST:
					request_count++;
					bool skipped = request != null;
					if (_verbose > 0)
					{
						System.Console.Out.Write("Request ["
							+ msg.topicName()
							+ "]["
							+ msg.source()
							+ "]["
							+  msg.sequenceNumber()
							+ "], ");
						System.Console.Out.WriteLine(msg.data().Length
										+ " bytes"
										+ (skipped ? " (ignored)" : ""));
						if (_verbose > 1)
							dump(msg);
					}
					if (!skipped)
					{
						request = msg;
                        promoted = true;
					}
					break;
				default:
					System.Console.Out.WriteLine("Unknown lbm_msg_t type " + msg.type() + " [" + msg.topicName() + "][" + msg.source() + "]");
					break;
			}
            if (!promoted)
            {
                msg.dispose();
            }
			System.Console.Out.Flush();
			return 0;
		}

		private void dump(LBMMessage msg)
		{
			int i, j;
			byte [] data = msg.data();
			int size = msg.data().Length;
			StringBuilder sb;
			int b;
			ASCIIEncoding encoding = new ASCIIEncoding();

			sb = new StringBuilder();
			for (i=0; i < (size >> 4); i++)
			{
				for (j=0; j < 16; j++)
				{
					b = ((int)data[(i<<4)+j]) & 0xff;
					sb.Append(b.ToString("X2"));
					sb.Append(" ");
				}
				sb.Append("\t");
				sb.Append(encoding.GetString(data, i<<4, 16));
				System.Console.Out.WriteLine(sb.ToString());
			}
			j = size % 16;
			if (j > 0)
			{
				sb = new StringBuilder();
				for (i=0; i < j; i++)
				{
					b = ((int)data[size-j+i]) & 0xff;
					sb.Append(b.ToString("X2"));
					sb.Append(" ");
				}
				for (i = j; i < 16; i++)
				{
					sb.Append("   ");
				}
				sb.Append("\t");
				sb.Append(encoding.GetString(data, size-j, j));
				System.Console.Out.WriteLine(sb.ToString());
			}
			System.Console.Out.Flush();
		}


		private void end()
		{
			System.Environment.Exit(0);
		}

	}

}
