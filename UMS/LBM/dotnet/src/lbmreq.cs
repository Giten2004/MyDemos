
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

namespace LBMApplication
{
	class lbmreq
	{
		[DllImport("Kernel32.dll")]
		public static extern int SetEnvironmentVariable( string name , string value ) ;
		private static int MIN_ALLOC_MSGLEN = 25;
		private static int requests = 10000000;
		private static bool send_immediate = false;
		private static int stats_sec = 0;
		private static int verbose = 0;
		private static bool eventq = false;
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
			+ "  -P sec = pause sec seconds after sending request (for responses to arrive)\n"
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
			if (System.Environment.GetEnvironmentVariable("LBM_LICENSE_FILENAME") == null
				&& System.Environment.GetEnvironmentVariable("LBM_LICENSE_INFO") == null)
			{
				SetEnvironmentVariable("LBM_LICENSE_FILENAME", "lbm_license.txt");
			}
			LBM lbm = new LBM();
			lbm.setLogger(new LBMLogging(logger));
			
			string target = null;
			int send_rate = 0;							//	Used for lbmtrm | lbtru transports
			int retrans_rate = 0;						//
			char protocol = '\0';						//
			int linger = 5;
			int msglen = MIN_ALLOC_MSGLEN;
			int pause_sec = 5;
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
								System.Console.Error.WriteLine("lbmreq error: " + Ex.Message);
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
							pause_sec = Convert.ToInt32(args[i]);
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
					System.Console.Error.WriteLine("lbmreq: error\n" + e.Message + "\n");
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
				topic_str  = args[i];
			}

			byte [] message = null;
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
			LBMSourceAttributes sattr = new LBMSourceAttributes();
			LBMContextAttributes cattr = new LBMContextAttributes();

			/* Check if protocol needs to be set to lbtrm | lbtru */
			if (protocol == 'M')
			{
				try
				{
					sattr.setValue("transport", "LBTRM");
					cattr.setValue("transport_lbtrm_data_rate_limit", send_rate.ToString());
					cattr.setValue("transport_lbtrm_retransmit_rate_limit", retrans_rate.ToString());
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
					sattr.setValue("transport", "LBTRU");
					cattr.setValue("transport_lbtru_data_rate_limit", send_rate.ToString());
					cattr.setValue("transport_lbtru_retransmit_rate_limit", retrans_rate.ToString());
				}
				catch (LBMException ex)
				{
					System.Console.Error.WriteLine("Error setting LBTRU rate: " + ex.Message);
					System.Environment.Exit(1);
				}						
			}

			LBMContext ctx = new LBMContext(cattr);
            LBMreqEventQueue evq = null;
            if (eventq)
            {
                evq = new LBMreqEventQueue();
                System.Console.Error.WriteLine("Event queue in use.");
            }
            else
            {
                System.Console.Error.WriteLine("No event queue\n");
            }
            LBMTopic topic;
			LBMSource src = null;
			LBMreqCB srccb = new LBMreqCB(verbose,evq);
			if (!send_immediate)
			{
				topic =  ctx.allocTopic(topic_str, sattr);
				src = ctx.createSource(topic, new LBMSourceEventCallback(srccb.onSourceEvent), null, evq);
				if (delay > 0)
				{
					System.Console.Out.WriteLine("Delaying requests for {0} second{1}...\n", 
												 delay, ((delay > 1) ? "s" : ""));
					System.Threading.Thread.Sleep(delay * 1000);
				}
			}
			ASCIIEncoding enc = new ASCIIEncoding();
			if (requests > 0)
				System.Console.Out.WriteLine("Will send {0} request{1}\n", requests, (requests == 1 ? "" : "s"));
			System.Console.Out.Flush();	
			for (int count = 0; count < requests; count++)
			{
				LBMTimer qTimer;
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("Request data {0}", count);
				enc.GetBytes(sb.ToString(), 0, sb.ToString().Length, message, 0);
				LBMRequest req = new LBMRequest(message, msglen);
				req.addResponseCallback(new LBMResponseCallback(srccb.onResponse));
				System.Console.Out.WriteLine("Sending request " + count);
				if (send_immediate)
				{
					if (target == null)
					{
						if (eventq)
							ctx.send(topic_str, req, evq, 0);
						else
							ctx.send(topic_str, req, 0);
					}
					else
					{
						if (eventq)
							ctx.send(target, topic_str, req, evq, 0);
						else
							ctx.send(target, topic_str, req, 0);
					}
				}
				else
				{
					if (eventq)
						src.send(req, evq, 0);
					else
						src.send(req, 0);
				}

				if (verbose > 0)
					System.Console.Out.Write("Sent request " + count + ". ");

                if ( !eventq )
                {
                    if ( verbose > 0 )
                        System.Console.Out.WriteLine("Pausing " + pause_sec + " seconds.");
                    System.Threading.Thread.Sleep(pause_sec * 1000);
                }
                else 
                {
                    if (verbose > 0)
                        System.Console.Out.WriteLine("Creating timer for " + pause_sec + " seconds and initiating event pump.");
                    qTimer = new EQTimer(ctx, pause_sec * 1000, evq);
                    evq.run(LBM.EVENT_QUEUE_BLOCK);
                }
				
				System.Console.Out.WriteLine("Done waiting for responses, " + 
					"{0} response{1} ({2} total bytes) received. Deleting request.\n",
					srccb.response_count, (srccb.response_count == 1 ? "" : "s"), srccb.response_byte_count);
				srccb.response_count = 0;
				srccb.response_byte_count = 0;
				req.close();
				System.Console.Out.Flush();
			}
			if (linger > 0)
			{
				System.Console.Out.WriteLine("\nLingering for {0} second{1}...\n", 
				                             linger, ((linger > 1) ? "s" : ""));
				System.Threading.Thread.Sleep(linger * 1000);
			}
			System.Console.Out.WriteLine("Quitting...");
			if (src != null)
				src.close();
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

		private static void print_bw(double sec, int msgs, long bytes)
		{
			char[] scale = {'\0', 'K', 'M', 'G'};
			double mps = 0.0, bps = 0.0;
			double kscale = 1000.0;
			int msg_scale_index = 0, bit_scale_index = 0;

			if (sec == 0) return; /* avoid division by zero */
			mps = msgs/sec;
			bps = bytes*8/sec;
			
			while (mps >= kscale) {
				mps /= kscale;
				msg_scale_index++;
			}
	
			while (bps >= kscale) {
				bps /= kscale;
				bit_scale_index++;
			}
			
			System.Console.Out.WriteLine(sec
					   + " secs. "
					   + mps.ToString("0.000")
					   + " " + scale[msg_scale_index] + "msgs/sec. "
					   + bps.ToString("0.000")
					   + " " + scale[bit_scale_index] + "bps");
			System.Console.Out.Flush();
		}
	}

	class LBMreqCB
	{
		public int response_count = 0;
		public int response_byte_count = 0;
		int _verbose;
		LBMreqEventQueue _evq;

		public LBMreqCB(int verbose, LBMreqEventQueue eq )
		{
			_verbose = verbose;
			_evq = eq;
		}

		public int onResponse(object cbArg, LBMRequest req, LBMMessage msg)
		{
			switch (msg.type())
			{
			case LBM.MSG_RESPONSE:
				response_count++;
				response_byte_count += msg.data().Length;
				if (_verbose > 0)
				{
					System.Console.Out.WriteLine("Response ["
							+ msg.source()
							+ "]["
							+ msg.sequenceNumber()
							+ "], "
							+ msg.data().Length
							+ " bytes");
					if (_verbose > 1)
						dump(msg);
				}
				break;
			default:
				System.Console.Out.WriteLine("Unknown message type "
						+ msg.type()
						+ "["
						+ msg.source()
						+"]");
				break;
			}
			msg.dispose();
			System.Console.Out.Flush();
			return 0;
		}

		public void onSourceEvent(object arg, LBMSourceEvent sourceEvent)
		{
			string clientname;

			switch (sourceEvent.type())
			{
				case LBM.SRC_EVENT_CONNECT:
					clientname = sourceEvent.dataString();
					System.Console.Out.WriteLine("Receiver connect " + clientname);
					break;
				case LBM.SRC_EVENT_DISCONNECT:
					clientname = sourceEvent.dataString();
					System.Console.Out.WriteLine("Receiver disconnect " + clientname);
					break;
				default:
					break;
			}
			System.Console.Out.Flush();
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
	}

	class LBMreqEventQueue : LBMEventQueue
	{
		public LBMreqEventQueue()
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
}

class EQTimer : LBMTimer
{
	LBMEventQueue _evq;

	public EQTimer(LBMContext ctx, long tmo, LBMEventQueue evq)
		: base(ctx, tmo, evq)
	{
		_evq = evq;
		this.addTimerCallback(new LBMTimerCallback(onExpiration));
	}
	
	private void onExpiration(object arg)
	{
		_evq.stop();
	}
}
