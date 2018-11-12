
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
	class lbmimsg
	{
		[DllImport("Kernel32.dll")]
		public static extern int SetEnvironmentVariable( string name , string value ) ;
		
		private static int msgs = 10000000;
		private static string purpose = "Purpose: Send immediate messages on a single topic or send topic-less messages.";
		private static string usage =
			  "Usage: lbmimsg [options] topic\n"
			+ "Available options:\n"
			+ "  -c filename = Use LBM configuration file filename.\n"
			+ "                Multiple config files are allowed.\n"
			+ "                Example:  '-c file1.cfg -c file2.cfg'\n"
			+ "  -d delay = delay sending for delay seconds after source creation\n"
			+ "  -h = help\n"
			+ "  -l len = send messages of len bytes\n"
			+ "  -L linger = linger for linger seconds before closing context\n"
			+ "  -M msgs = send msgs number of messages\n"
			+ "  -o = send topic-less immediate messages\n"
			+ "  -P msec = pause after each send msec milliseconds\n"
			+ "  -R [UM]DATA/RETR = Set transport type to LBT-R[UM], set data rate limit to\n"
			+ "                     DATA bits per second, and set retransmit rate limit to\n"
			+ "                     RETR bits per second.  For both limits, the optional\n"
			+ "                     k, m, and g suffixes may be used.  For example,\n"
			+ "                     '-R 1m/500k' is the same as '-R 1000000/500000'\n"
			+ "  -T target = target for unicast immediate messages\n"
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

			int send_rate = 0;							//	Used for lbmtrm | lbtru transports
			int retrans_rate = 0;						//
			char protocol = '\0';						//
			int linger = 5;
			int delay = 1;
			string target = null;
			string topic = null;
			int msglen = 25;
			long bytes_sent = 0;
			int pause = 0;
			int i;
			int n = args.Length;
			bool error = false;
			bool done = false;
			bool topic_less = false;

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
								System.Console.Error.WriteLine("lbmimsg error: " + Ex.Message);
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
						case "-o":
							topic_less = true;
							break;
						case "-P":
							if (++i >= n)
							{
								error = true;
								break;
							}
							pause = Convert.ToInt32(args[i]);
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
						case "-T":
							if (++i >= n)
							{
								error = true;
								break;
							}
							target = args[i];
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
					System.Console.Error.WriteLine("lbmimsg: error\n" + e.Message + "\n");
					print_help_exit(1);
				}
			}
			
			if (error || (i >= n && !topic_less))
			{
				/* An error occurred processing the command line - print help and exit */
				print_help_exit(1);
			}
			
			if (n > i && topic_less) {
				/* User chose topic-less and yet specified a topic - print help and exit */
				System.Console.Error.WriteLine("lbmimsg: error--selected topic-less option and still specified topic");
				print_help_exit(1);
			}
			
			if (i < n)
			{
				topic = args[i];
			}
			
			byte [] message = new byte[msglen];

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
			
			if (delay > 0) 
			{
				System.Console.Out.WriteLine("Will start sending in {0} second{1}...\n", 
				                             delay, ((delay > 1) ? "s" : ""));
				
				System.Threading.Thread.Sleep(delay * 1000);
			}
			
			System.Console.Out.WriteLine("Sending {0}{1} immediate messages of size {2} bytes to target <{3}> topic <{4}>\n",
			                             msgs, (topic_less == true ? " topic-less" : ""), msglen, 
										 (target == null ? "" : target), (topic == null ? "" : topic));
			System.Console.Out.Flush();							 
			long start_time = System.DateTime.Now.Ticks;
			for (int count = 0; count < msgs; count++)
			{
				try
				{
					if (target == null)
					{
						ctx.send(topic, message, msglen, 0);
					}
					else
					{
						ctx.send(target, topic, message, msglen, 0);
					}
				}
				catch (LBMException ex)
				{
					if (target != null && ex.errorNumber() == LBM.EOP) {
						System.Console.Error.WriteLine("LBM send() error: no connection to target while sending unicast immediate message");
					}
					else {
						System.Console.Error.WriteLine("LBM send() error: " + ex.Message);
					}
				}
				bytes_sent += msglen;
				if (pause > 0)
				{
					System.Threading.Thread.Sleep(pause);
				}

			}
			long end_time = System.DateTime.Now.Ticks;
			double secs = (end_time - start_time) / 10000000.0;
			
			System.Console.Out.WriteLine("Sent {0} messages of size {1} bytes in {2} seconds.\n",
			                             msgs, msglen, secs);
	
			print_bw(secs, msgs, bytes_sent);
			System.Console.Out.Flush();
			if (linger > 0)
			{
				System.Console.Out.WriteLine("Lingering for {0} second{1}...\n", 
				                             linger, ((linger > 1) ? "s" : ""));
				System.Threading.Thread.Sleep(linger * 1000);
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
		}
	}
}
