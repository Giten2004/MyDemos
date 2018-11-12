
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
	class lbmpong
	{
		[DllImport("Kernel32.dll")]
		public static extern int SetEnvironmentVariable( string name , string value ) ;
		
		private static int msecpause = 0;
		private static int msgs = 200;
		private static int msglen = 10;
		private static bool eventq = false;
		private static bool ping = false;
		private static int run_secs = 300;
		private static bool verbose = false;
		private static bool end_on_eos = false;
		private static bool rtt_collect = false;
		private static bool use_mim = false;
        private static bool use_smx = false;
		private static int rtt_ignore = 0;
		private static string purpose = "Purpose: Message round trip processor.";
		private static string usage =
			  "Usage: lbmpong [options] id\n"
			+ "Available options:\n"
			+ "  -c filename = Use LBM configuration file filename.\n"
			+ "                Multiple config files are allowed.\n"
			+ "                Example:  '-c file1.cfg -c file2.cfg'\n"
			+ "  -C = collect RTT data\n"
			+ "  -E = exit after source ends\n"
			+ "  -h = help\n"
			+ "  -i msgs = send and ignore msgs messages to warm up\n"
			+ "  -I = Use MIM\n"
			+ "  -l len = use len length messages\n"
			+ "  -M msgs = stop after receiving msgs messages\n"
			+ "  -P msec = pause after each send msec milliseconds\n"
			+ "  -q = use an LBM event queue\n"
			+ "  -r [UM]DATA/RETR = Set transport type to LBT-R[UM], set data rate limit to\n"
			+ "                     DATA bits per second, and set retransmit rate limit to\n"
			+ "                     RETR bits per second.  For both limits, the optional\n"
			+ "                     k, m, and g suffixes may be used.  For example,\n"
			+ "                     '-r 1m/500k' is the same as '-r 1000000/500000'\n"			
			+ "  -t secs = run for secs seconds\n"
			+ "  -v = be verbose about each message (for RTT only)\n"
			+ "  id = either \"ping\" or \"pong\"\n"
			;

		static void Main(string[] args)
		{
			lbmpong pongapp = new lbmpong(args);
		}

		int send_rate = 0;				//	Used for lbmtrm | lbtru transports
		int retrans_rate = 0;			//
		char protocol = '\0';			//

		private void process_cmdline(string[] args)
		{
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
								System.Console.Error.WriteLine("lbmpong error: " + Ex.Message);
								error = true;
							}
							break;
						case "-C": 
							rtt_collect = true;
							break;
						case "-E":
							end_on_eos = true;
							break;
						case "-h":
							print_help_exit(1);
							break;
						case "-i": 
							if (++i >= n)
							{
								error = true;
								break;
							}
							rtt_ignore = Convert.ToInt32(args[i]);
							break;
						case "-I": 
							use_mim = true;
							break;
						case "-l":
							if (++i >= n)
							{
								error = true;
								break;
							}
							msglen = Convert.ToInt32(args[i]);
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
							msecpause = Convert.ToInt32(args[i]);
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
							ParseRateVars parseRateVars = lbmExampleUtil.parseRate(args[i]);
							if (parseRateVars.error)
							{
								print_help_exit(1);
							}
							send_rate = parseRateVars.rate;
							retrans_rate = parseRateVars.retrans;
							protocol = parseRateVars.protocol;
							break;
						case "-t":
							if (++i >= n)
							{
								error = true;
								break;
							}
							run_secs = Convert.ToInt32(args[i]);
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
					System.Console.Error.WriteLine("lbmpong: error\n" + e.Message + "\n");
					print_help_exit(1);
				}
			}
			if (error || i >= n)
			{
				/* An error occurred processing the command line - print help and exit */
				print_help_exit(1);
			}
			if (args[i] == "ping")
			{
				ping = true;
			}
			else if (args[i] != "pong")
			{
				print_help_exit(1);
			}
		}
		
		public static long pspertick = (1000L * 1000L * 1000L * 1000L)	// picoseconds per tick
										/ System.Diagnostics.Stopwatch.Frequency;
												 	
		private lbmpong(string[] args)
		{		
		
			if (System.Environment.GetEnvironmentVariable("LBM_LICENSE_FILENAME") == null
				&& System.Environment.GetEnvironmentVariable("LBM_LICENSE_INFO") == null)
			{
				SetEnvironmentVariable("LBM_LICENSE_FILENAME", "lbm_license.txt");
			}
			LBM lbm = new LBM();
			lbm.setLogger(new LBMLogging(logger));

			process_cmdline(args);

			if (use_mim && !eventq)
			{
				System.Console.Out.WriteLine("Using mim requires event queue to send from receive callback - forcing use");
				eventq = true;
			}
			if (msecpause > 0 && !eventq) {
				System.Console.Out.WriteLine("Setting pause value requires event queue - forcing use");
				eventq = true;
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
			PongLBMEventQueue evq = null;
			if (eventq)
			{
				System.Console.Error.WriteLine("Event queue in use");
				evq = new PongLBMEventQueue();
			}
			else
			{
				System.Console.Error.WriteLine("No event queue");
			}
			System.Console.Out.Flush();
			LBMSource src = null;
			PongLBMReceiver rcv;
			LBMTopic src_topic = null;
			LBMTopic rcv_topic;
			if (ping)
			{
				System.Console.Error.WriteLine(
					"Sending " + msgs + " " + msglen +
					" byte messages to topic lbmpong/ping pausing "
					+ msecpause + " msec between");
				if(!use_mim)
					src_topic = ctx.allocTopic("lbmpong/ping", sattr);
				rcv_topic = ctx.lookupTopic("lbmpong/pong");
			}
			else
			{
				rcv_topic =  ctx.lookupTopic("lbmpong/ping");
				if(!use_mim)
					src_topic =  ctx.allocTopic("lbmpong/pong", sattr);
			}
			PongSrcCB srccb = new PongSrcCB();
            if (!use_mim)
            {
                src = ctx.createSource(src_topic, new LBMSourceEventCallback(srccb.onSourceEvent), null);
                use_smx = src.getAttributeValue("transport").ToLower().Contains("smx");

                if (use_smx)
                {
                    /* Perform configuration validation */
                    const int smx_header_size = 16;
                    int max_payload_size =
                            Convert.ToInt32(src.getAttributeValue("transport_lbtsmx_datagram_max_size")) + smx_header_size;
                    if (msglen > max_payload_size)
                    {
                        /* The SMX transport doesn't fragment, so payload must be within maximum size limits */
                        System.Console.WriteLine("Error: Message size requested is larger than configured SMX datagram size.");
                        System.Environment.Exit(1);
                    }
                }
            }
			rcv = new PongLBMReceiver(ctx, rcv_topic, evq, src, ping, msecpause, msgs, verbose,
										end_on_eos, rtt_collect,rtt_ignore,use_mim);
			System.Threading.Thread.Sleep(5000);
			if (ping)
			{
				byte [] message = new byte[msglen];
				rcv.start();
				
				format(message, 0, System.Diagnostics.Stopwatch.GetTimestamp() * lbmpong.pspertick / 1000);
				if(use_mim) 
					ctx.send("lbmpong/ping", message, msglen, LBM.MSG_FLUSH);
                else if (use_smx)
                {
                    try
                    {
                        IntPtr writeBuff;
                        if (src.buffAcquire(out writeBuff, (uint)msglen, 0) == 0)
                        {
                            Marshal.Copy(message, 0, writeBuff, msglen);
                            src.buffsComplete();
                        }
                    }
                    catch (LBMException ex)
                    {
                        System.Console.Error.WriteLine("Error (while doing SMX acquire/complete): " + ex.Message);
                        System.Environment.Exit(1);
                    }
				}
                else
					src.send(message, msglen, LBM.MSG_FLUSH);
			}
			if (eventq)
			{
				evq.run(run_secs * 1000);
			}
			else
			{
				System.Threading.Thread.Sleep(run_secs * 1000);
			}
						
			System.Console.Error.WriteLine("Quitting....");
			if(!use_mim)
				src.close();
			rcv.close();
			ctx.close();
			if (eventq)
				evq.close();
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

		/* Format the long into the byte array */
		public static void format(byte[] buf, int offset, long v)
		{			
			if (buf.Length - offset < 8)
		    {
				throw new LBMException(0,"Not enough space in buffer to format (offset " 
											+ offset + " length " + buf.Length + " needed 8");
			}
            GCHandle pinnedBuf = GCHandle.Alloc(buf, GCHandleType.Pinned);
            IntPtr ptr = pinnedBuf.AddrOfPinnedObject();

            Marshal.WriteInt64(ptr, offset, v);
            pinnedBuf.Free();
		}

        /* Format the long into the IntPtr object */
        public static void format(IntPtr ptr, int offset, long v)
        {
            Marshal.WriteInt64(ptr, offset, v);
        }
      
		/* Parse a byte array containing a long */
		public static long parse_s(byte[] buf, int offset)
		{
			if (buf.Length - offset < 8)
				throw new LBMException(0,"Not enough data in buffer to parse (offset " 
										+ offset + " length " + buf.Length + " needed 8");
            
            GCHandle pinnedBuf = GCHandle.Alloc(buf, GCHandleType.Pinned);
            IntPtr ptr = pinnedBuf.AddrOfPinnedObject();

            long v = Marshal.ReadInt64(ptr, offset);
            pinnedBuf.Free();
            return v;
		}

        /* Parse a IntPtr object containing a long */
        public static long parse_s(IntPtr buf, int offset)
        {
            long v = Marshal.ReadInt64(buf, offset);
            return v;
        }
	}

	class PongLBMEventQueue : LBMEventQueue
	{
		public PongLBMEventQueue()
		{
			this.addMonitor(new LBMEventQueueCallback(monitor));
		}
		
		protected void monitor(object cbArg, int evtype, int evq_size, long evq_delay)
		{
			System.Console.Error.WriteLine("Event Queue Monitor: Type: " + evtype +
				", Size: " + evq_size +
				", Delay: " + evq_delay + " usecs.");
		}
	}

	class PongLBMReceiver
	{
        [DllImport("msvcrt.dll", SetLastError = false)]
        static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);

		private LBMContext _ctx = null;
		private int _msg_count = 0;
		private int _msgs = 200;
		private int _msecpause = 0;
		private bool _ping = false;
		private bool _verbose = false;
		private bool _end_on_eos = false;
		private LBMEventQueue _evq = null;
		private LBMSource _src = null;
		private long _start_time = 0;
		private LBMReceiver _rcv = null;
		private int rtt_ignore = 0;
		private bool use_mim = false;
        private bool use_smx = false;
			
		public PongLBMReceiver(LBMContext ctx, LBMTopic topic, LBMEventQueue evq, LBMSource src, 
							   bool ping, int msecpause, int msgs, bool verbose, bool end_on_eos, 
							   bool rtt_collect, int ignore, bool mim)
		{
			_ctx = ctx;
			_rcv = new LBMReceiver(ctx, topic, new LBMReceiverCallback(onReceive), null, evq);
			_msgs = msgs;
			_verbose = verbose;
			_msecpause = msecpause;
			_ping = ping;
			_evq = evq;
			_end_on_eos = end_on_eos;
			_src = src;
			if (rtt_collect)
				rtt_data = new double[msgs];
			rtt_ignore = ignore;
			use_mim = mim;
            use_smx = src.getAttributeValue("transport").ToLower().Contains("smx");
		}

		public void close()
		{
			_rcv.close();
		}

		public void start()
		{
			_start_time = System.DateTime.Now.Ticks;			
		}
	 
		protected int onReceive(object cbArg, LBMMessage msg)
		{
			long t;
			if(_ping)
			{
				t = System.Diagnostics.Stopwatch.GetTimestamp() * lbmpong.pspertick / 1000;
			}
			else
				t = 0;	
			switch (msg.type())
			{
			case LBM.MSG_DATA:
				if (rtt_ignore == 0)
				{
					_msg_count++;
				}

                byte[] message = null;
				IntPtr messagePtr = (IntPtr) 0;
				long s = 0;
                if (use_smx)
                    messagePtr = msg.dataPointerSafe();
                else
                    message = msg.data();

                if (_ping)
				{
					s = (use_smx) ? lbmpong.parse_s(messagePtr, 0) : lbmpong.parse_s(message, 0);
					calc_latency(t, s);
					if (rtt_ignore == 0 && _msg_count == _msgs)
					{
						rtt_avg_usec = ((double) total_nsec / 1000.0) / (double) _msg_count;

						print_rtt_data();
																		
						print_latency(System.Console.Out);

						try {
							print_stats();
						}
						catch (LBMException ex) {
							System.Console.Error.WriteLine("Error printing transport stats in ponglbmreceiver" 
															+ ex.Message);
						}

						end();

						msg.dispose();

						return 0;
					}
					if (_msecpause > 0)
					{
						// This would not normally be a good
						// thing in a callback on the context
						// thread.
						System.Threading.Thread.Sleep(_msecpause);
					}
                    if (!use_smx)
                        lbmpong.format(message, 0,
                                    System.Diagnostics.Stopwatch.GetTimestamp() * lbmpong.pspertick / 1000);
				}
                if (use_mim)
                {
                    _ctx.send(_ping ? "lbmpong/ping" : "lbmpong/pong", message,
                                message.Length, LBM.MSG_FLUSH | LBM.SRC_NONBLOCK);
                }
                else if (use_smx)
                {
                    try
                    {
                        IntPtr writeBuff;
                        if (_src.buffAcquire(out writeBuff, (uint)msg.length(), 0) == 0)
                        {
                            if (_ping)
                                lbmpong.format(writeBuff, 0,
                                        System.Diagnostics.Stopwatch.GetTimestamp() * lbmpong.pspertick / 1000);
                            else
                                memcpy(writeBuff, messagePtr, (int)msg.length());
                            _src.buffsComplete();
                        }
                    }
                    catch (LBMException ex)
                    {
                        System.Console.Error.WriteLine("Error (while doing SMX acquire/complete): " + ex.Message);
                        System.Environment.Exit(1);
                    }
                }
                else
                {
                    _src.send(message, message.Length, LBM.MSG_FLUSH | LBM.SRC_NONBLOCK);
                }

				if(_ping && _verbose)
					System.Console.Out.WriteLine(_msg_count + " curr " + t + " sent " + s 
													+ " latency " + (t - s) + " ns");
				if (rtt_ignore > 0)
					rtt_ignore--;
				break;
			case LBM.MSG_BOS:
				System.Console.Error.WriteLine("[" + msg.topicName() + "][" + msg.source() 
											  + "], Beginning of Transport Session");
				break;
			case LBM.MSG_EOS:
				System.Console.Error.WriteLine("[" + msg.topicName() + "][" + msg.source() 
											  + "], End of Transport Session");
				if (_end_on_eos)
				{
					end();
				}
				break;
			case LBM.MSG_UNRECOVERABLE_LOSS:
				if (_verbose)
					System.Console.Error.WriteLine("[" + msg.topicName() + "][" + msg.source() + "][" 
												  + msg.sequenceNumber() + "], LOST");
				/* Any kind of loss makes this test invalid */	
				System.Console.WriteLine("Unrecoverable loss occurred.  Quitting...");
				System.Environment.Exit(1);
				break;
			case LBM.MSG_UNRECOVERABLE_LOSS_BURST:
				System.Console.Error.WriteLine("[" + msg.topicName() + "][" + msg.source() + "], LOST BURST");
				/* Any kind of loss makes this test invalid */	
				System.Console.WriteLine("Unrecoverable loss occurred.  Quitting...");
				System.Environment.Exit(1);
				break;
			default:
				System.Console.Error.WriteLine("Unknown lbm_msg_t type " + msg.type() 
											 + " [" + msg.topicName() + "][" + msg.source() + "]");
				break;
			}
			msg.dispose();
			System.Console.Out.Flush();
			return 0;
		}

		private double total_nsec;
		private long min_nsec, max_nsec = 0;
		private long min_nsec_idx, max_nsec_idx;
		private int datanum = 0;
		private double[] rtt_data;
		private double rtt_median_msec, rtt_avg_usec, rtt_stddev_msec;
		
		public virtual void  calc_latency(long curr, long sent)
		{
			long nsec_diff = curr - sent;
			if (rtt_ignore == 0)
			{
				total_nsec += nsec_diff;
				if (nsec_diff < min_nsec || min_nsec == 0)
				{
					min_nsec = nsec_diff; 
					min_nsec_idx = datanum;
				}
				if (nsec_diff > max_nsec)
				{
					max_nsec = nsec_diff;
					max_nsec_idx = datanum;
				}
				if (rtt_data != null)
				{
					rtt_data[datanum] = (double) nsec_diff / 1000000000.0;
				}
				datanum++;
			}
		}
		
		internal virtual double calc_med()
		{
			int r;
			bool changed;
			double t;
			long msgs = _msg_count;
			
			/* sort the result set */
			do 
			{
				changed = false;
				
				for (r = 0; r < msgs - 1; r++)
				{
					if (rtt_data[r] > rtt_data[r + 1])
					{
						t = rtt_data[r];
						rtt_data[r] = rtt_data[r + 1];
						rtt_data[r + 1] = t;
						changed = true;
					}
				}
			}
			while (changed);
			
			if ((msgs & 1) == 1)
			{
				/* Odd number of data elements - take middle */
				return rtt_data[(int) (msgs / 2) + 1];
			}
			else
			{
				/* Even number of data element avg the two middle ones */
				return (rtt_data[(int) (msgs / 2)] + rtt_data[((int) msgs / 2) + 1]) / 2;
			}
		}
		
		internal virtual double calc_stddev(double mean)
		{
			int r;
			double sum;
			
			/* Subtract the mean from the data points, square them and sum them */
			sum = 0.0;
			for (r = 0; r < _msg_count; r++)
			{
				rtt_data[r] -= mean;
				rtt_data[r] *= rtt_data[r];
				sum += rtt_data[r];
			}
			
			sum /= (_msg_count - 1);
			
			return System.Math.Sqrt(sum);
		}
		
		internal virtual void  print_rtt_data()
		{
			if (rtt_data != null)
			{
				int r;
				
				for (r = 0; r < _msg_count; r++) {
					Double d = rtt_data[r] * 1000.0;
                    if (d > 0.100)
                        System.Console.Error.WriteLine("RTT " + d.ToString("0.000") + " msec, msg " + r);
                    else
                        System.Console.Error.WriteLine("RTT " + (d * 1000.0).ToString("0.000") + " usec, msg " + r);
				}
				
				/* Calculate median and stddev */
				rtt_median_msec = calc_med() * 1000.0;
				rtt_stddev_msec = calc_stddev(rtt_avg_usec / 1000.0);
				
				print_latency(System.Console.Error);
			}
		}
		
		public void print_latency(System.IO.TextWriter fstr)
		{
			double avg = rtt_avg_usec / 1000.0;
			double min = min_nsec / 1000000.0, max = max_nsec / 1000000.0;
			double latency = rtt_avg_usec / 2.0;
			
			if (rtt_data != null)
			{
				fstr.WriteLine("min/max msg = "    + min_nsec_idx + "/" + max_nsec_idx 
							+ " median/stddev " + rtt_median_msec.ToString("0.0000") + "/" 
							+ rtt_stddev_msec.ToString("0.0000") + " msec");
			}
            if (latency > 1.0)
            {
                fstr.WriteLine("Elapsed time " + (total_nsec / 1000.0) + " usecs "
                              + _msg_count + " messages (RTTs). "
                              + "min/avg/max " + min.ToString("0.0000") + "/"
                              + avg.ToString("0.0000") + "/"
                              + max.ToString("0.0000") + " msec RTT\n"
                              + "        " + latency.ToString("0.0000") + " usec latency");
            }
            else
            {
                fstr.WriteLine("Elapsed time " + total_nsec + " nsecs "
                              + _msg_count + " messages (RTTs). "
                              + "min/avg/max " + (min * 1000.0).ToString("0.0000") + "/"
                              + (avg * 1000.0).ToString("0.0000") + "/"
                              + (max * 1000.0).ToString("0.0000") + " usec RTT\n"
                              + "        " + (latency * 1000.0).ToString("0.0000") + " nsec latency");
            }
			fstr.Flush();
		}

		private void end()
		{
			if (_evq != null)
			{
				_evq.stop();
			}
			else
			{
				System.Environment.Exit(0);
			}
		}

		private long ba2l(byte[] b, int offset)
		{
			long value = 0;
			for (int i = 0; i < 4; i++)
			{
				int shift = (3-i) * 8;
				value += (b[i+offset] & 0x000000ff) << shift;
			}
			return value;
		}
						
		private int print_stats() 
		{
			LBMReceiverStatistics rcv_stats = null;
			LBMSourceStatistics src_stats = null;
			string source_type = "";
			int nstats = 1;
			int stat_index = 0;
						
			// Get receiver stats
			try {
				rcv_stats = _rcv.getStatistics(nstats);
			} catch (LBMException ex) {
				System.Console.Error.WriteLine("Error getting receiver statistics: " + ex.Message);
				return -1;
			}
			
			if (rcv_stats == null) {
				System.Console.Error.WriteLine("Cannot print stats, because receiver stats are null");
				return -1;
			}
			
			// Get source stats
			try {
				src_stats = _src.getStatistics();
			} catch (LBMException ex) {
				System.Console.Error.WriteLine("Error getting source statistics: " + ex.Message);
				return -1;
			}
			
			if (src_stats == null) {
				System.Console.Error.WriteLine("Cannot print stats, because source stats are null");
				return -1;
			}
			
			// Print transport stats
			switch(src_stats.type(stat_index))
			{
				case LBM.TRANSPORT_STAT_TCP:
					break;
				case LBM.TRANSPORT_STAT_LBTRU:
				case LBM.TRANSPORT_STAT_LBTRM:
					if (rcv_stats.lost() != 0 || src_stats.retransmissionsSent() != 0) {
						source_type = (src_stats.type() == LBM.TRANSPORT_STAT_LBTRM) ? "LBT-RM" : "LBT-RU";
						System.Console.Out.WriteLine("The latency for this " + source_type 
													+ " session of lbmpong might be skewed by loss");
						System.Console.Out.WriteLine("Source loss: " + src_stats.retransmissionsSent() 
													+ "    "  + "Receiver loss: " + rcv_stats.lost());
					}
					break;
				case LBM.TRANSPORT_STAT_LBTIPC:
					break;
				case LBM.TRANSPORT_STAT_LBTSMX:
					break;
				case LBM.TRANSPORT_STAT_LBTRDMA:
					break;
			}
	
			src_stats.dispose();
			rcv_stats.dispose();
			System.Console.Out.Flush();			
			return 0;
		}
	}

	class PongSrcCB
	{
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
	}	
}
