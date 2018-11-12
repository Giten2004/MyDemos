using System;
using System.Net;
using com.latencybusters.lbm;


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

namespace LBMApplication
{
	class lbmmon
	{
		private static string purpose = "Purpose: Example LBM statistics monitoring application.";
		private static string usage =
			"Usage: lbmmon [options]\n"
			+ "Available options:\n"
			+ "  -h = help\n"
			+ "  -t, --transport TRANS      use transport module TRANS\n"
			+ "                             TRANS may be `lbm', `udp', or `lbmsnmp', default is `lbm'\n"
			+ "      --transport-opts OPTS  use OPTS as transport module options\n"
			+ "  -f, --format FMT           use format module FMT\n"
			+ "                             FMT may be `csv'\n"
			+ "      --format-opts OPTS     use OPTS as format module options\n"
			+ "\n"
			+ "Transport and format options are passed as name=value pairs, separated by a semicolon.\n"
			+ "\n"
			+ "LBM transport options:\n"
			+ "  config=FILE            use LBM configuration file FILE\n"
			+ "  topic=TOPIC            receive statistics on topic TOPIC\n"
			+ "                         default is /29west/statistics\n"
			+ "  wctopic=PATTERN        receive statistics on wildcard topic PATTERN\n"
			+ "\n"
			+ "UDP transport options:\n"
			+ "  port=NUM               receive on UDP port NUM\n"
			+ "  interface=IP           receive multicast on interface IP\n"
			+ "  mcgroup=GRP            receive on multicast group GRP\n"
			+ "\n"
			+ "LBMSNMP transport options:\n"
			+ "  config=FILE            use LBM configuration file FILE\n"
			+ "  topic=TOPIC            receive statistics on topic TOPIC\n"
			+ "                         default is /29west/statistics\n"
			+ "  wctopic=PATTERN        receive statistics on wildcard topic PATTERN\n"
			+ "\n"
			+ "CSV format options:\n"
			+ "  separator=CHAR         separate CSV fields with character CHAR\n"
			+ "                         defaults to `,'\n"
			;
	
		public static void  Main(string[] args)
		{
			LBM lbm = new LBM();
			lbm.setLogger(new LBMLogging(logger));
			LBMObjectRecyclerBase objRec = new LBMObjectRecycler();
		
			int transport = LBMMonitor.TRANSPORT_LBM;
			int format = LBMMonitor.FORMAT_CSV;
			string transport_options = "";
			string format_options = "";
			const string OPTION_MONITOR_TRANSPORT = "--transport";
			const string OPTION_MONITOR_TRANSPORT_OPTS = "--transport-opts";
			const string OPTION_MONITOR_FORMAT = "--format";
			const string OPTION_MONITOR_FORMAT_OPTS = "--format-opts";
			int n = args.Length;
			int i;
			bool error = false;
			for (i = 0; i < n ; i++)
			{
				switch (args[i])
				{
				case "-h":
					print_help_exit(0);
					break;

				case "-f": 
				case OPTION_MONITOR_FORMAT: 
					if (++i >= n)
					{
						error = true;
						break;
					}
					if (args[i].ToLower().CompareTo("csv") == 0)
						format = LBMMonitor.FORMAT_CSV;
					else
					{
						error = true;
						break;
					}
					break;
			
				case "-t": 
				case OPTION_MONITOR_TRANSPORT: 
					if (++i >= n)
					{
						error = true;
						break;
					}
					if (args[i].ToLower().CompareTo("lbm") == 0)
						transport = LBMMonitor.TRANSPORT_LBM;
					else if (args[i].ToLower().CompareTo("udp") == 0)
						transport = LBMMonitor.TRANSPORT_UDP;
					else if (args[i].ToLower().CompareTo("lbmsnmp") == 0)
						transport = LBMMonitor.TRANSPORT_LBMSNMP;
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
					transport_options += args[i];
					break;
			
				case OPTION_MONITOR_FORMAT_OPTS: 
					if (++i >= n)
					{
						error = true;
						break;
					}
					format_options += args[i];
					break;
			
				default: 
					error = true;
					break;
				
				}
				if (error)
					break;
			}
			
			if (error)
			{
				/* An error occurred processing the command line - print help and exit */
				print_help_exit(1);
			}
			
			LBMMonitorReceiver lbmmonrcv = new LBMMonitorReceiver(format, format_options, transport, transport_options, objRec, null);
			//If not using object recycling
			//LBMMonitorReceiver lbmmonrcv = new LBMMonitorReceiver(format, format_options, transport, transport_options);
			LBMMonCallbacks lbmmoncbs = new LBMMonCallbacks(lbmmonrcv, objRec);
			LBMMonitorSourceStatisticsCallback scb;
			scb = new LBMMonitorSourceStatisticsCallback(lbmmoncbs.onReceive);
			LBMMonitorReceiverStatisticsCallback rcb;
			rcb = new LBMMonitorReceiverStatisticsCallback(lbmmoncbs.onReceive);
			LBMMonitorEventQueueStatisticsCallback evqcb;
			evqcb = new LBMMonitorEventQueueStatisticsCallback(lbmmoncbs.onReceive);
			LBMMonitorContextStatisticsCallback ctxcb;
			ctxcb = new LBMMonitorContextStatisticsCallback(lbmmoncbs.onReceive);
			LBMMonitorImmediateMessageReceiverStatisticsCallback imrcvcb;
			imrcvcb = new LBMMonitorImmediateMessageReceiverStatisticsCallback(lbmmoncbs.onReceive);
			LBMMonitorImmediateMessageSourceStatisticsCallback imsrccb;
			imsrccb = new LBMMonitorImmediateMessageSourceStatisticsCallback(lbmmoncbs.onReceive);

			lbmmonrcv.addStatisticsCallback(scb, rcb, evqcb, ctxcb);
			lbmmonrcv.addStatisticsCallback(imsrccb);
			lbmmonrcv.addStatisticsCallback(imrcvcb);
			for (; ; )
			{
				System.Threading.Thread.Sleep(2000);
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
}

class LBMMonCallbacks
{
	private LBMMonitorReceiver _lbmmonrcv;
	private LBMObjectRecyclerBase _recycler;
	
	internal LBMMonCallbacks(LBMMonitorReceiver lbmmonrcv, LBMObjectRecyclerBase recycler)
	{
		_lbmmonrcv = lbmmonrcv;
		_recycler = recycler;
	}
	
	public virtual void  onReceive(LBMSourceStatistics stats)
	{
		System.Console.Error.Write("\nSource statistics received");
		System.Console.Error.Write(" from " + stats.getApplicationSourceId());
		System.Console.Error.Write(" at " + stats.getSender().ToString());
		System.Console.Error.WriteLine(", sent " + stats.getTimestamp().ToLocalTime().ToString());
		System.Console.Error.WriteLine("Source: " + stats.source());
		System.Console.Error.WriteLine("Transport: " + stats.typeName());
		switch (stats.type())
		{
			case LBM.TRANSPORT_STAT_TCP: 
				System.Console.Error.WriteLine("\tClients       : " + stats.numberOfClients());
				System.Console.Error.WriteLine("\tBytes buffered: " + stats.bytesBuffered());
				break;
			
			case LBM.TRANSPORT_STAT_LBTRM: 
				System.Console.Error.WriteLine("\tLBT-RM datagrams sent                                 : " + stats.messagesSent());
				System.Console.Error.WriteLine("\tLBT-RM datagram bytes sent                            : " + stats.bytesSent());
				System.Console.Error.WriteLine("\tLBT-RM datagrams in transmission window               : " + stats.transmissionWindowMessages());
				System.Console.Error.WriteLine("\tLBT-RM datagram bytes in transmission window          : " + stats.transmissionWindowBytes());
				System.Console.Error.WriteLine("\tLBT-RM NAK packets received                           : " + stats.nakPacketsReceived());
				System.Console.Error.WriteLine("\tLBT-RM NAKs received                                  : " + stats.naksReceived());
				System.Console.Error.WriteLine("\tLBT-RM NAKs ignored                                   : " + stats.naksIgnored());
				System.Console.Error.WriteLine("\tLBT-RM NAKs shed                                      : " + stats.naksShed());
				System.Console.Error.WriteLine("\tLBT-RM NAKs ignored (retransmit delay)                : " + stats.naksIgnoredRetransmitDelay());
				System.Console.Error.WriteLine("\tLBT-RM retransmission datagrams sent                  : " + stats.retransmissionsSent());
				System.Console.Error.WriteLine("\tLBT-RM retransmission datagram bytes sent             : " + stats.retransmissionBytesSent());
				System.Console.Error.WriteLine("\tLBT-RM datagrams queued by rate control               : " + stats.messagesQueued());
				System.Console.Error.WriteLine("\tLBT-RM retransmission datagrams queued by rate control: " + stats.retransmissionsQueued());
				break;
			
			case LBM.TRANSPORT_STAT_LBTRU: 
				System.Console.Error.WriteLine("\tLBT-RU datagrams sent                    : " + stats.messagesSent());
				System.Console.Error.WriteLine("\tLBT-RU datagram bytes sent               : " + stats.bytesSent());
				System.Console.Error.WriteLine("\tLBT-RU NAK packets received              : " + stats.nakPacketsReceived());
				System.Console.Error.WriteLine("\tLBT-RU NAKs received                     : " + stats.naksReceived());
				System.Console.Error.WriteLine("\tLBT-RU NAKs ignored                      : " + stats.naksIgnored());
				System.Console.Error.WriteLine("\tLBT-RU NAKs shed                         : " + stats.naksShed());
				System.Console.Error.WriteLine("\tLBT-RU NAKs ignored (retransmit delay)   : " + stats.naksIgnoredRetransmitDelay());
				System.Console.Error.WriteLine("\tLBT-RU retransmission datagrams sent     : " + stats.retransmissionsSent());
				System.Console.Error.WriteLine("\tLBT-RU retransmission datagram bytes sent: " + stats.retransmissionBytesSent());
				System.Console.Error.WriteLine("\tClients                                  : " + stats.numberOfClients());
				break;

			case LBM.TRANSPORT_STAT_LBTIPC:
				System.Console.Error.WriteLine("\tClients                    :" + stats.numberOfClients());
				System.Console.Error.WriteLine("\tLBT-IPC datagrams sent     :" + stats.messagesSent());
				System.Console.Error.WriteLine("\tLBT-IPC datagram bytes sent:" + stats.bytesSent());
				break;

			case LBM.TRANSPORT_STAT_LBTSMX:
				System.Console.Error.WriteLine("\tClients                    :" + stats.numberOfClients());
				System.Console.Error.WriteLine("\tLBT-SMX datagrams sent     :" + stats.messagesSent());
				System.Console.Error.WriteLine("\tLBT-SMX datagram bytes sent:" + stats.bytesSent());
				break;

			case LBM.TRANSPORT_STAT_LBTRDMA:
				System.Console.Error.WriteLine("\tClients                    :" + stats.numberOfClients());
				System.Console.Error.WriteLine("\tLBT-RDMA datagrams sent     :" + stats.messagesSent());
				System.Console.Error.WriteLine("\tLBT-RDMA datagram bytes sent:" + stats.bytesSent());
				break;

			default:
				System.Console.Error.WriteLine("Error: Unknown transport type received:" + stats.type());
				break;
		}
		_recycler.doneWithSourceStatistics(stats);
	}
	
	public virtual void  onReceive(LBMReceiverStatistics stats)
	{
		System.Console.Error.Write("\nReceiver statistics received");
		System.Console.Error.Write(" from " + stats.getApplicationSourceId());
		System.Console.Error.Write(" at " + stats.getSender().ToString());
		System.Console.Error.WriteLine(", sent " + stats.getTimestamp().ToLocalTime().ToString());
		System.Console.Error.WriteLine("Source: " + stats.source());
		System.Console.Error.WriteLine("Transport: " + stats.typeName());
		switch (stats.type())
		{
			case LBM.TRANSPORT_STAT_TCP: 
				System.Console.Error.WriteLine("\tLBT-TCP bytes received                                    : " + stats.bytesReceived());
				System.Console.Error.WriteLine("\tLBM messages received                                     : " + stats.lbmMessagesReceived());
				System.Console.Error.WriteLine("\tLBM messages received with uninteresting topic            : " + stats.noTopicMessagesReceived());
				System.Console.Error.WriteLine("\tLBM requests received                                     : " + stats.lbmRequestsReceived());
				break;
			
			case LBM.TRANSPORT_STAT_LBTRM: 
				System.Console.Error.WriteLine("\tLBT-RM datagrams received                                 : " + stats.messagesReceived());
				System.Console.Error.WriteLine("\tLBT-RM datagram bytes received                            : " + stats.bytesReceived());
				System.Console.Error.WriteLine("\tLBT-RM NAK packets sent                                   : " + stats.nakPacketsSent());
				System.Console.Error.WriteLine("\tLBT-RM NAKs sent                                          : " + stats.naksSent());
				System.Console.Error.WriteLine("\tLost LBT-RM datagrams detected                            : " + stats.lost());
				System.Console.Error.WriteLine("\tNCFs received (ignored)                                   : " + stats.ncfsIgnored());
				System.Console.Error.WriteLine("\tNCFs received (shed)                                      : " + stats.ncfsShed());
				System.Console.Error.WriteLine("\tNCFs received (retransmit delay)                          : " + stats.ncfsRetransmissionDelay());
				System.Console.Error.WriteLine("\tNCFs received (unknown)                                   : " + stats.ncfsUnknown());
				System.Console.Error.WriteLine("\tLoss recovery minimum time                                : " + stats.minimumRecoveryTime() + "ms");
				System.Console.Error.WriteLine("\tLoss recovery mean time                                   : " + stats.meanRecoveryTime() + "ms");
				System.Console.Error.WriteLine("\tLoss recovery maximum time                                : " + stats.maximumRecoveryTime() + "ms");
				System.Console.Error.WriteLine("\tMinimum transmissions per individual NAK                  : " + stats.minimumNakTransmissions());
				System.Console.Error.WriteLine("\tMean transmissions per individual NAK                     : " + stats.meanNakTransmissions());
				System.Console.Error.WriteLine("\tMaximum transmissions per individual NAK                  : " + stats.maximumNakTransmissions());
				System.Console.Error.WriteLine("\tDuplicate LBT-RM datagrams received                       : " + stats.duplicateMessages());
				System.Console.Error.WriteLine("\tLBT-RM datagrams unrecoverable (window advance)           : " + stats.unrecoveredMessagesWindowAdvance());
				System.Console.Error.WriteLine("\tLBT-RM datagrams unrecoverable (NAK generation expiration): " + stats.unrecoveredMessagesNakGenerationTimeout());
				System.Console.Error.WriteLine("\tLBT-RM LBM messages received                              : " + stats.lbmMessagesReceived());
				System.Console.Error.WriteLine("\tLBT-RM LBM messages received with uninteresting topic     : " + stats.noTopicMessagesReceived());
				System.Console.Error.WriteLine("\tLBT-RM LBM requests received                              : " + stats.lbmRequestsReceived());
				System.Console.Error.WriteLine("\tLBT-RM datagrams dropped (size)                           : " + stats.datagramsDroppedIncorrectSize());
				System.Console.Error.WriteLine("\tLBT-RM datagrams dropped (type)                           : " + stats.datagramsDroppedType());
				System.Console.Error.WriteLine("\tLBT-RM datagrams dropped (version)                        : " + stats.datagramsDroppedVersion());
				System.Console.Error.WriteLine("\tLBT-RM datagrams dropped (header)                         : " + stats.datagramsDroppedHeader());
				System.Console.Error.WriteLine("\tLBT-RM datagrams dropped (other)                          : " + stats.datagramsDroppedOther());
				System.Console.Error.WriteLine("\tLBT-RM datagrams received out of order                    : " + stats.outOfOrder());
				break;
			
			case LBM.TRANSPORT_STAT_LBTRU: 
				System.Console.Error.WriteLine("\tLBT-RU datagrams received                                 : " + stats.messagesReceived());
				System.Console.Error.WriteLine("\tLBT-RU datagram bytes received                            : " + stats.bytesReceived());
				System.Console.Error.WriteLine("\tLBT-RU NAK packets sent                                   : " + stats.nakPacketsSent());
				System.Console.Error.WriteLine("\tLBT-RU NAKs sent                                          : " + stats.naksSent());
				System.Console.Error.WriteLine("\tLost LBT-RU datagrams detected                            : " + stats.lost());
				System.Console.Error.WriteLine("\tNCFs received (ignored)                                   : " + stats.ncfsIgnored());
				System.Console.Error.WriteLine("\tNCFs received (shed)                                      : " + stats.ncfsShed());
				System.Console.Error.WriteLine("\tNCFs received (retransmit delay)                          : " + stats.ncfsRetransmissionDelay());
				System.Console.Error.WriteLine("\tNCFs received (unknown)                                   : " + stats.ncfsUnknown());
				System.Console.Error.WriteLine("\tLoss recovery minimum time                                : " + stats.minimumRecoveryTime() + "ms");
				System.Console.Error.WriteLine("\tLoss recovery mean time                                   : " + stats.meanRecoveryTime() + "ms");
				System.Console.Error.WriteLine("\tLoss recovery maximum time                                : " + stats.maximumRecoveryTime() + "ms");
				System.Console.Error.WriteLine("\tMinimum transmissions per individual NAK                  : " + stats.minimumNakTransmissions());
				System.Console.Error.WriteLine("\tMean transmissions per individual NAK                     : " + stats.meanNakTransmissions());
				System.Console.Error.WriteLine("\tMaximum transmissions per individual NAK                  : " + stats.maximumNakTransmissions());
				System.Console.Error.WriteLine("\tDuplicate LBT-RU datagrams received                       : " + stats.duplicateMessages());
				System.Console.Error.WriteLine("\tLBT-RU datagrams unrecoverable (window advance)           : " + stats.unrecoveredMessagesWindowAdvance());
				System.Console.Error.WriteLine("\tLBT-RU datagrams unrecoverable (NAK generation expiration): " + stats.unrecoveredMessagesNakGenerationTimeout());
				System.Console.Error.WriteLine("\tLBT-RU LBM messages received                              : " + stats.lbmMessagesReceived());
				System.Console.Error.WriteLine("\tLBT-RU LBM messages received with uninteresting topic     : " + stats.noTopicMessagesReceived());
				System.Console.Error.WriteLine("\tLBT-RU LBM requests received                              : " + stats.lbmRequestsReceived());
				System.Console.Error.WriteLine("\tLBT-RU datagrams dropped (size)                           : " + stats.datagramsDroppedIncorrectSize());
				System.Console.Error.WriteLine("\tLBT-RU datagrams dropped (type)                           : " + stats.datagramsDroppedType());
				System.Console.Error.WriteLine("\tLBT-RU datagrams dropped (version)                        : " + stats.datagramsDroppedVersion());
				System.Console.Error.WriteLine("\tLBT-RU datagrams dropped (header)                         : " + stats.datagramsDroppedHeader());
				System.Console.Error.WriteLine("\tLBT-RU datagrams dropped (SID)                            : " + stats.datagramsDroppedSID());
				System.Console.Error.WriteLine("\tLBT-RU datagrams dropped (other)                          : " + stats.datagramsDroppedOther());
				break;

			case LBM.TRANSPORT_STAT_LBTIPC:
				System.Console.Error.WriteLine("\tLBT-IPC datagrams received                                :" + stats.messagesReceived());
				System.Console.Error.WriteLine("\tLBT-IPC datagram bytes received                           :" + stats.bytesReceived());
				System.Console.Error.WriteLine("\tLBT-IPC LBM messages received                             :" + stats.lbmMessagesReceived());
				System.Console.Error.WriteLine("\tLBT-IPC LBM messages received with uninteresting topic    :" + stats.noTopicMessagesReceived());
				System.Console.Error.WriteLine("\tLBT-IPC LBM requests received                             :" + stats.lbmRequestsReceived());
				break;

			case LBM.TRANSPORT_STAT_LBTSMX:
				System.Console.Error.WriteLine("\tLBT-SMX datagrams received                                :" + stats.messagesReceived());
				System.Console.Error.WriteLine("\tLBT-SMX datagram bytes received                           :" + stats.bytesReceived());
				System.Console.Error.WriteLine("\tLBT-SMX LBM messages received                             :" + stats.lbmMessagesReceived());
				System.Console.Error.WriteLine("\tLBT-SMX LBM messages received with uninteresting topic    :" + stats.noTopicMessagesReceived());
				System.Console.Error.WriteLine("\tLBT-SMX LBM requests received                             :" + stats.lbmRequestsReceived());
				break;

			case LBM.TRANSPORT_STAT_LBTRDMA:
				System.Console.Error.WriteLine("\tLBT-RDMA datagrams received                                :" + stats.messagesReceived());
				System.Console.Error.WriteLine("\tLBT-RDMA datagram bytes received                           :" + stats.bytesReceived());
				System.Console.Error.WriteLine("\tLBT-RDMA LBM messages received                             :" + stats.lbmMessagesReceived());
				System.Console.Error.WriteLine("\tLBT-RDMA LBM messages received with uninteresting topic    :" + stats.noTopicMessagesReceived());
				System.Console.Error.WriteLine("\tLBT-RDMA LBM requests received                             :" + stats.lbmRequestsReceived());
				break;

			default:
				System.Console.Error.WriteLine("Error: Unknown transport type received:" + stats.type());
				break;
		}
		_recycler.doneWithReceiverStatistics(stats);
	}

	public virtual void onReceive(LBMEventQueueStatistics stats)
	{
		System.Console.Error.Write("\nEvent queue statistics received");
		System.Console.Error.Write(" from " + stats.getApplicationSourceId());
		System.Console.Error.Write(" at " + stats.getSender().ToString());
		System.Console.Error.Write(", process ID=" + stats.getProcessId().ToString("x"));
		System.Console.Error.Write(", object ID=" + stats.getObjectId().ToString("x"));
		System.Console.Error.WriteLine(", sent " + stats.getTimestamp().ToLocalTime().ToString());
		System.Console.Error.WriteLine("\tData messages currently enqueued                : " + stats.dataMessages());
		System.Console.Error.WriteLine("\tTotal data messages enqueued                    : " + stats.dataMessagesTotal());
		System.Console.Error.WriteLine("\tData message service minimum time               : " + stats.dataMessagesMinimumServiceTime() + "us");
		System.Console.Error.WriteLine("\tData message service mean time                  : " + stats.dataMessagesMeanServiceTime() + "us");
		System.Console.Error.WriteLine("\tData message service maximum time               : " + stats.dataMessagesMaximumServiceTime() + "us");
		System.Console.Error.WriteLine("\tResponses currently enqueued                    : " + stats.responseMessages());
		System.Console.Error.WriteLine("\tTotal responses enqueued                        : " + stats.responseMessagesTotal());
		System.Console.Error.WriteLine("\tResponse service minimum time                   : " + stats.responseMessagesMinimumServiceTime() + "us");
		System.Console.Error.WriteLine("\tResponse service mean time                      : " + stats.responseMessagesMeanServiceTime() + "us");
		System.Console.Error.WriteLine("\tResponse service maximum time                   : " + stats.responseMessagesMaximumServiceTime() + "us");
		System.Console.Error.WriteLine("\tTopicless immediate messages currently enqueued : " + stats.topiclessImmediateMessages());
		System.Console.Error.WriteLine("\tTotal topicless immediate messages enqueued     : " + stats.topiclessImmediateMessagesTotal());
		System.Console.Error.WriteLine("\tTopicless immediate message service minimum time: " + stats.topiclessImmediateMessagesMinimumServiceTime() + "us");
		System.Console.Error.WriteLine("\tTopicless immediate message service mean time   : " + stats.topiclessImmediateMessagesMeanServiceTime() + "us");
		System.Console.Error.WriteLine("\tTopicless immediate message service maximum time: " + stats.topiclessImmediateMessagesMaximumServiceTime() + "us");
		System.Console.Error.WriteLine("\tWildcard messages currently enqueued            : " + stats.wildcardReceiverMessages());
		System.Console.Error.WriteLine("\tTotal wildcard messages enqueued                : " + stats.wildcardReceiverMessagesTotal());
		System.Console.Error.WriteLine("\tWildcard message service minimum time           : " + stats.wildcardReceiverMessagesMinimumServiceTime() + "us");
		System.Console.Error.WriteLine("\tWildcard message service mean time              : " + stats.wildcardReceiverMessagesMeanServiceTime() + "us");
		System.Console.Error.WriteLine("\tWildcard message service maximum time           : " + stats.wildcardReceiverMessagesMaximumServiceTime() + "us");
		System.Console.Error.WriteLine("\tI/O events currently enqueued                   : " + stats.ioEvents());
		System.Console.Error.WriteLine("\tTotal I/O events enqueued                       : " + stats.ioEventsTotal());
		System.Console.Error.WriteLine("\tI/O event service minimum time                  : " + stats.ioEventsMinimumServiceTime() + "us");
		System.Console.Error.WriteLine("\tI/O event service mean time                     : " + stats.ioEventsMeanServiceTime() + "us");
		System.Console.Error.WriteLine("\tI/O event service maximum time                  : " + stats.ioEventsMaximumServiceTime() + "us");
		System.Console.Error.WriteLine("\tTimer events currently enqueued                 : " + stats.timerEvents());
		System.Console.Error.WriteLine("\tTotal timer events enqueued                     : " + stats.timerEventsTotal());
		System.Console.Error.WriteLine("\tTimer event service minimum time                : " + stats.timerEventsMinimumServiceTime() + "us");
		System.Console.Error.WriteLine("\tTimer event service mean time                   : " + stats.timerEventsMeanServiceTime() + "us");
		System.Console.Error.WriteLine("\tTimer event service maximum time                : " + stats.timerEventsMaximumServiceTime() + "us");
		System.Console.Error.WriteLine("\tSource events currently enqueued                : " + stats.sourceEvents());
		System.Console.Error.WriteLine("\tTotal source events enqueued                    : " + stats.sourceEventsTotal());
		System.Console.Error.WriteLine("\tSource event service minimum time               : " + stats.sourceEventsMinimumServiceTime() + "us");
		System.Console.Error.WriteLine("\tSource event service mean time                  : " + stats.sourceEventsMeanServiceTime() + "us");
		System.Console.Error.WriteLine("\tSource event service maximum time               : " + stats.sourceEventsMaximumServiceTime() + "us");
		System.Console.Error.WriteLine("\tUnblock events currently enqueued               : " + stats.unblockEvents());
		System.Console.Error.WriteLine("\tTotal unblock events enqueued                   : " + stats.unblockEventsTotal());
		System.Console.Error.WriteLine("\tCancel events currently enqueued                : " + stats.cancelEvents());
		System.Console.Error.WriteLine("\tTotal cancel events enqueued                    : " + stats.cancelEventsTotal());
		System.Console.Error.WriteLine("\tCancel event service minimum time               : " + stats.cancelEventsMinimumServiceTime() + "us");
		System.Console.Error.WriteLine("\tCancel event service mean time                  : " + stats.cancelEventsMeanServiceTime() + "us");
		System.Console.Error.WriteLine("\tCancel event service maximum time               : " + stats.cancelEventsMaximumServiceTime() + "us");
		System.Console.Error.WriteLine("\tCallback events currently enqueued              : " + stats.callbackEvents());
		System.Console.Error.WriteLine("\tTotal callback events enqueued                  : " + stats.callbackEventsTotal());
		System.Console.Error.WriteLine("\tCallback event service minimum time             : " + stats.callbackEventsMinimumServiceTime() + "us");
		System.Console.Error.WriteLine("\tCallback event service mean time                : " + stats.callbackEventsMeanServiceTime() + "us");
		System.Console.Error.WriteLine("\tCallback event service maximum time             : " + stats.callbackEventsMaximumServiceTime() + "us");
		System.Console.Error.WriteLine("\tContext source events currently enqueued        : " + stats.contextSourceEvents());
		System.Console.Error.WriteLine("\tTotal context source events enqueued            : " + stats.contextSourceEventsTotal());
		System.Console.Error.WriteLine("\tContext source event service minimum time       : " + stats.contextSourceEventsMinimumServiceTime() + "us");
		System.Console.Error.WriteLine("\tContext source event service mean time          : " + stats.contextSourceEventsMeanServiceTime() + "us");
		System.Console.Error.WriteLine("\tContext source event service maximum time       : " + stats.contextSourceEventsMaximumServiceTime() + "us");
		System.Console.Error.WriteLine("\tEvents currently enqueued                       : " + stats.events());
		System.Console.Error.WriteLine("\tTotal events enqueued                           : " + stats.eventsTotal());
		System.Console.Error.WriteLine("\tEvent latency minimum time                      : " + stats.minimumAge() + "us");
		System.Console.Error.WriteLine("\tEvent latency mean time                         : " + stats.meanAge() + "us");
		System.Console.Error.WriteLine("\tEvent latency maximum time                      : " + stats.maximumAge() + "us");
		_recycler.doneWithEventQueueStatistics(stats);
	}

	public virtual void onReceive(LBMContextStatistics stats)
	{
		System.Console.Error.Write("\nContext statistics received");
		System.Console.Error.Write(" from " + stats.getApplicationSourceId());
		System.Console.Error.Write(" at " + stats.getSender().ToString());
		System.Console.Error.Write(", process ID=" + stats.getProcessId().ToString("x"));
		System.Console.Error.Write(", object ID=" + stats.getObjectId().ToString("x"));
		System.Console.Error.WriteLine(", sent " + stats.getTimestamp().ToLocalTime().ToString());
		System.Console.Error.WriteLine("\tTopic resolution datagrams sent                    : " + stats.topicResolutionDatagramsSent());
		System.Console.Error.WriteLine("\tTopic resolution datagram bytes sent               : " + stats.topicResolutionBytesSent());
		System.Console.Error.WriteLine("\tTopic resolution datagrams received                : " + stats.topicResolutionDatagramsReceived());
		System.Console.Error.WriteLine("\tTopic resolution datagram bytes received           : " + stats.topicResolutionBytesReceived());
		System.Console.Error.WriteLine("\tTopic resolution datagrams dropped (version)       : " + stats.topicResolutionDatagramsDroppedVersion());
		System.Console.Error.WriteLine("\tTopic resolution datagrams dropped (type)          : " + stats.topicResolutionDatagramsDroppedType());
		System.Console.Error.WriteLine("\tTopic resolution datagrams dropped (malformed)     : " + stats.topicResolutionDatagramsDroppedMalformed());
		System.Console.Error.WriteLine("\tTopic resolution send failures                     : " + stats.topicResolutionDatagramsSendFailed());
		System.Console.Error.WriteLine("\tTopics in source topic map                         : " + stats.topicResolutionSourceTopics());
		System.Console.Error.WriteLine("\tTopics in receiver topic map                       : " + stats.topicResolutionReceiverTopics());
		System.Console.Error.WriteLine("\tUnresolved topics in receiver topic map            : " + stats.topicResolutionUnresolvedReceiverTopics());
		System.Console.Error.WriteLine("\tUnknown LBT-RM datagrams received                  : " + stats.lbtrmUnknownMessagesReceived());
		System.Console.Error.WriteLine("\tUnknown LBT-RU datagrams received                  : " + stats.lbtruUnknownMessagesReceived());
		System.Console.Error.WriteLine("\tNumber of times message send blocked               : " + stats.sendBlocked());
		System.Console.Error.WriteLine("\tNumber of times message send returned EWOULDBLOCK  : " + stats.sendWouldBlock());
		System.Console.Error.WriteLine("\tNumber of times response send blocked              : " + stats.responseBlocked());
		System.Console.Error.WriteLine("\tNumber of times response send returned EWOULDBLOCK : " + stats.responseWouldBlock());
        System.Console.Error.WriteLine("\tNumber of duplicate UIM messages dropped           : " + stats.unicastImmediateMessageDuplicatesReceived());
        System.Console.Error.WriteLine("\tNumber of UIM messages received without stream info: " + stats.unicastImmediateMessageNoStreamReceived());
		_recycler.doneWithContextStatistics(stats);
	}

	public virtual void onReceive(LBMImmediateMessageSourceStatistics stats)
	{
		System.Console.Error.Write("\nImmediate message source statistics received");
		System.Console.Error.Write(" from " + stats.getApplicationSourceId());
		System.Console.Error.Write(" at " + stats.getSender().ToString());
		System.Console.Error.WriteLine(", sent " + stats.getTimestamp().ToLocalTime().ToString());
		System.Console.Error.WriteLine("Source: " + stats.source());
		System.Console.Error.WriteLine("Transport: " + stats.typeName());
		switch (stats.type())
		{
			case LBM.TRANSPORT_STAT_TCP:
				System.Console.Error.WriteLine("\tClients       : " + stats.numberOfClients());
				System.Console.Error.WriteLine("\tBytes buffered: " + stats.bytesBuffered());
				break;

			case LBM.TRANSPORT_STAT_LBTRM:
				System.Console.Error.WriteLine("\tLBT-RM datagrams sent                                 : " + stats.messagesSent());
				System.Console.Error.WriteLine("\tLBT-RM datagram bytes sent                            : " + stats.bytesSent());
				System.Console.Error.WriteLine("\tLBT-RM datagrams in transmission window               : " + stats.transmissionWindowMessages());
				System.Console.Error.WriteLine("\tLBT-RM datagram bytes in transmission window          : " + stats.transmissionWindowBytes());
				System.Console.Error.WriteLine("\tLBT-RM NAK packets received                           : " + stats.nakPacketsReceived());
				System.Console.Error.WriteLine("\tLBT-RM NAKs received                                  : " + stats.naksReceived());
				System.Console.Error.WriteLine("\tLBT-RM NAKs ignored                                   : " + stats.naksIgnored());
				System.Console.Error.WriteLine("\tLBT-RM NAKs shed                                      : " + stats.naksShed());
				System.Console.Error.WriteLine("\tLBT-RM NAKs ignored (retransmit delay)                : " + stats.naksIgnoredRetransmitDelay());
				System.Console.Error.WriteLine("\tLBT-RM retransmission datagrams sent                  : " + stats.retransmissionsSent());
				System.Console.Error.WriteLine("\tLBT-RM retransmission datagram bytes sent             : " + stats.retransmissionBytesSent());
				System.Console.Error.WriteLine("\tLBT-RM datagrams queued by rate control               : " + stats.messagesQueued());
				System.Console.Error.WriteLine("\tLBT-RM retransmission datagrams queued by rate control: " + stats.retransmissionsQueued());
				break;

			default:
				System.Console.Error.WriteLine("Error: Unknown transport type received:" + stats.type());
				break;
		}
		_recycler.doneWithImmediateMessageSourceStatistics(stats);
	}

	public virtual void onReceive(LBMImmediateMessageReceiverStatistics stats)
	{
		System.Console.Error.Write("\nImmediate message receiver statistics received");
		System.Console.Error.Write(" from " + stats.getApplicationSourceId());
		System.Console.Error.Write(" at " + stats.getSender().ToString());
		System.Console.Error.WriteLine(", sent " + stats.getTimestamp().ToLocalTime().ToString());
		System.Console.Error.WriteLine("Source: " + stats.source());
		System.Console.Error.WriteLine("Transport: " + stats.typeName());
		switch (stats.type())
		{

			case LBM.TRANSPORT_STAT_TCP:
				System.Console.Error.WriteLine("\tLBT-TCP bytes received                                    : " + stats.bytesReceived());
				System.Console.Error.WriteLine("\tLBM messages received                                     : " + stats.lbmMessagesReceived());
				System.Console.Error.WriteLine("\tLBM messages received with uninteresting topic            : " + stats.noTopicMessagesReceived());
				System.Console.Error.WriteLine("\tLBM requests received                                     : " + stats.lbmRequestsReceived());
				break;

			case LBM.TRANSPORT_STAT_LBTRM:
				System.Console.Error.WriteLine("\tLBT-RM datagrams received                                 : " + stats.messagesReceived());
				System.Console.Error.WriteLine("\tLBT-RM datagram bytes received                            : " + stats.bytesReceived());
				System.Console.Error.WriteLine("\tLBT-RM NAK packets sent                                   : " + stats.nakPacketsSent());
				System.Console.Error.WriteLine("\tLBT-RM NAKs sent                                          : " + stats.naksSent());
				System.Console.Error.WriteLine("\tLost LBT-RM datagrams detected                            : " + stats.lost());
				System.Console.Error.WriteLine("\tNCFs received (ignored)                                   : " + stats.ncfsIgnored());
				System.Console.Error.WriteLine("\tNCFs received (shed)                                      : " + stats.ncfsShed());
				System.Console.Error.WriteLine("\tNCFs received (retransmit delay)                          : " + stats.ncfsRetransmissionDelay());
				System.Console.Error.WriteLine("\tNCFs received (unknown)                                   : " + stats.ncfsUnknown());
				System.Console.Error.WriteLine("\tLoss recovery minimum time                                : " + stats.minimumRecoveryTime() + "ms");
				System.Console.Error.WriteLine("\tLoss recovery mean time                                   : " + stats.meanRecoveryTime() + "ms");
				System.Console.Error.WriteLine("\tLoss recovery maximum time                                : " + stats.maximumRecoveryTime() + "ms");
				System.Console.Error.WriteLine("\tMinimum transmissions per individual NAK                  : " + stats.minimumNakTransmissions());
				System.Console.Error.WriteLine("\tMean transmissions per individual NAK                     : " + stats.meanNakTransmissions());
				System.Console.Error.WriteLine("\tMaximum transmissions per individual NAK                  : " + stats.maximumNakTransmissions());
				System.Console.Error.WriteLine("\tDuplicate LBT-RM datagrams received                       : " + stats.duplicateMessages());
				System.Console.Error.WriteLine("\tLBT-RM datagrams unrecoverable (window advance)           : " + stats.unrecoveredMessagesWindowAdvance());
				System.Console.Error.WriteLine("\tLBT-RM datagrams unrecoverable (NAK generation expiration): " + stats.unrecoveredMessagesNakGenerationTimeout());
				System.Console.Error.WriteLine("\tLBT-RM LBM messages received                              : " + stats.lbmMessagesReceived());
				System.Console.Error.WriteLine("\tLBT-RM LBM messages received with uninteresting topic     : " + stats.noTopicMessagesReceived());
				System.Console.Error.WriteLine("\tLBT-RM LBM requests received                              : " + stats.lbmRequestsReceived());
				System.Console.Error.WriteLine("\tLBT-RM datagrams dropped (size)                           : " + stats.datagramsDroppedIncorrectSize());
				System.Console.Error.WriteLine("\tLBT-RM datagrams dropped (type)                           : " + stats.datagramsDroppedType());
				System.Console.Error.WriteLine("\tLBT-RM datagrams dropped (version)                        : " + stats.datagramsDroppedVersion());
				System.Console.Error.WriteLine("\tLBT-RM datagrams dropped (header)                         : " + stats.datagramsDroppedHeader());
				System.Console.Error.WriteLine("\tLBT-RM datagrams dropped (other)                          : " + stats.datagramsDroppedOther());
				System.Console.Error.WriteLine("\tLBT-RM datagrams received out of order                    : " + stats.outOfOrder());
				break;

			default:
				System.Console.Error.WriteLine("Error: Unknown transport type received:" + stats.type());
				break;
		}
		_recycler.doneWithImmediateMessageReceiverStatistics(stats);
	}
}
