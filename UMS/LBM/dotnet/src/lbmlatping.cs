
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

#define RunUnsafeMode          // runs faster in unsafe mode
//#undef RunUnsafeMode         // slower but if safe mode is required

using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using com.latencybusters.lbm;

namespace LBMApplication
{
    class lbmlatping
    {
        [DllImport("Kernel32.dll")]
        public static extern int SetEnvironmentVariable(string name, string value);

        /* Send each message around a large number of times -- needed because the clocks
         * are not accurate enough to measure one round trip. */
        private const int NUM_ROUNDTRIPS_PER_MSG = 250;

        /* Ignore the results for the first two thousand messages (things still getting
         * set up). */
        private const int NUM_MSGS_IGNORED = 2000;

        /* Total messages sent = fifty thousand, plus the amount ignored */
        private const int NUM_MSGS = (50 * 1000) + NUM_MSGS_IGNORED;

        // the payload to send -- and the components needed to prevent the GC from moving it
        private byte[] msgbuf = null;
        private GCHandle msgbufPinner;
        private IntPtr msgbufPtr;

        private LBMSource ping_src = null;
        private long usBusyWaitPause = 0;
        private int cpu = -1;

        private static string usage =
          "Usage: lbmlatping [options]\n"
        + "Available options:\n"
        + "  -a procmask = set cpu affinity mask.\n"
        + "               Available processors bitmask: " +
                    String.Format("0x{0:X}", (ulong)Process.GetCurrentProcess().ProcessorAffinity) + "\n"
        + "  -c filename = Use LBM configuration file filename.\n"
        + "                Multiple config files are allowed.\n"
        + "                Example:  '-c file1.cfg -c file2.cfg'\n"
        + "  -h = help\n"
        + "  -l len = use len length messages\n"
        + "  -P usec = pause after each send usec microseconds (busy wait only)\n"
        ;

        public lbmlatping()
            : this(new string[0])
        {
        }

        public lbmlatping(string[] args)
        {
            processCommandline(args);
            if (msgbuf == null) generatePayload(16);
        }

        private void generatePayload(int msglen)
        {
            if ((msgbuf != null) && (msgbuf.Length == msglen)) return;

            if (msgbuf != null) msgbufPinner.Free();
            char c = 'A';
            msgbuf = new byte[msglen];
            for (int i = 0; i < msglen; i++)
            {
                msgbuf[i] = (byte) c++;
                if (c > 'Z') c = 'A';
            }
            msgbufPinner = GCHandle.Alloc(msgbuf, GCHandleType.Pinned);
            msgbufPtr = msgbufPinner.AddrOfPinnedObject();
        }

        private void processCommandline(string[] args)
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
                        case "-a":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            cpu = Convert.ToInt32(args[i]);
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
                                System.Console.Error.WriteLine("lbmpong error: " + Ex.Message);
                                error = true;
                            }
                            break;
                        case "-h":
                            printHelpExit(1);
                            break;
                        case "-l":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            int msglen = Convert.ToInt32(args[i]);
                            generatePayload(msglen);
                            break;
                        case "-P":
                            if (++i >= n)
                            {
                                error = true;
                                break;
                            }
                            usBusyWaitPause = Convert.ToInt32(args[i]);
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
                    System.Console.Error.WriteLine("lbmlatping: error\n" + e.Message + "\n");
                    printHelpExit(1);
                }
            }
        }

        private static void printHelpExit(int exit_value)
        {
            System.Console.Out.WriteLine(LBM.version());
            System.Console.Out.WriteLine(usage);
            Environment.Exit(exit_value);
        }

        public static int Main(string[] args)
        {
            lbmlatping latping = null;

            LBMContext ctx = null;
            LBMContextAttributes ctx_attr = null;

            LBMTopic ping_src_topic = null;
            LBMSourceAttributes ping_src_topic_attr = null;
            LBMSource ping_src = null;

            LBMTopic pong_rcv_topic = null;
            LBMReceiverAttributes pong_rcv_topic_attr = null;
            lbmlatpingreceiver pong_rcv = null;

            latping = new lbmlatping(args);
            if (latping.cpu >= 0)
            {
                Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(latping.cpu);
            }
            try
            {
                /* Create the context. */
                ctx_attr = new LBMContextAttributes();
                ctx_attr.setValue("resolver_cache", "0");
                ctx_attr.setValue("operational_mode", "sequential");
                ctx_attr.setValue("request_tcp_port_high", "50000");
                ctx = new LBMContext(ctx_attr);
                ctx_attr.dispose();

                /* Create the ping source. */
                ping_src_topic_attr = new LBMSourceAttributes();
                ping_src_topic_attr.setValue("resolver_advertisement_sustain_interval", "0");
                ping_src_topic_attr.setValue("transport", "lbtsmx");
                ping_src_topic = new LBMTopic(ctx, "lbmlat-ping", ping_src_topic_attr);
                ping_src_topic_attr.dispose();
                ping_src = new LBMSource(ctx, ping_src_topic);
                latping.ping_src = ping_src;
             
                /* Perform some configuration validation */
                const int smx_header_size = 16;
                int max_payload_size =
                        Convert.ToInt32(ping_src.getAttributeValue("transport_lbtsmx_datagram_max_size")) + smx_header_size;
                if (latping.msgbuf.Length > max_payload_size)
                {
                    /* The SMX transport doesn't fragment, so payload must be within maximum size limits */
                    System.Console.WriteLine("Error: Message size requested is larger than configured SMX datagram size.");
                    System.Environment.Exit(1);
                }

                /* Create the pong receiver. */
                pong_rcv_topic_attr = new LBMReceiverAttributes();
                pong_rcv_topic_attr.enableSingleReceiverCallback(true);
                pong_rcv_topic = new LBMTopic(ctx, "lbmlat-pong", pong_rcv_topic_attr);
                pong_rcv_topic_attr.dispose();
                pong_rcv = new lbmlatpingreceiver(ctx, pong_rcv_topic, latping);

                /* Run the context just long enough to advertise. */
                ctx.processEvents(1000);

                /* The ponger kicks things off as soon as he's discovered our ping source. */
                while (true)
                {
                    System.Threading.Thread.Sleep(1000000);
                    //ctx.processEvents(1000000);
                }
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine(e.ToString());
            }
            return 0;
        }

#if RunUnsafeMode
        unsafe
#endif
        class lbmlatpingreceiver
        {
            [DllImport("msvcrt.dll", SetLastError = false)]
            static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);

            private lbmlatping latping;
            private long[] tsstart = new long[lbmlatping.NUM_MSGS + 2];
            private long[] tsend = new long[lbmlatping.NUM_MSGS + 1];

            private int bounce_count = NUM_ROUNDTRIPS_PER_MSG; 
            private int rcvd_msgs = 0;
            private LBMSource source;
            private LBMReceiver receiver;
            private byte[] buffer = null;
            private IntPtr bufferptr = (IntPtr)0;

            private IntPtr bufferAcquired = (IntPtr)0;
            private int bufferAcquiredSize = -1;

            public lbmlatpingreceiver(LBMContext lbmctx, LBMTopic lbmtopic, lbmlatping mylatping)
            {
                latping = mylatping;
                source = latping.ping_src;
                buffer = latping.msgbuf;
                bufferptr = latping.msgbufPtr;

                if (source.buffAcquire(out bufferAcquired, (uint)buffer.Length, 0) == 0)
                {
                    bufferAcquiredSize = buffer.Length;
                }
                receiver = new LBMReceiver(lbmctx, lbmtopic, new LBMReceiverCallback(onReceive), null, null);
            }
#if RunUnsafeMode
            static unsafe void dqwordBlockCopy(void* dest, void* src, int lenBytes)
            {
                int dqwordBlocks = lenBytes >> 4;

                if (dqwordBlocks > 0)
                {
                    long* pDest = (long*)dest;
                    long* pSrc = (long*)src;

                    for (int i = 0; i < dqwordBlocks; i++)
                    {
                        pDest[0] = pSrc[0];
                        pDest[1] = pSrc[1];
                        pDest += 2;
                        pSrc += 2;
                    }
                    dest = pDest;
                    src = pSrc;
                    lenBytes = lenBytes - (dqwordBlocks << 4);
                }

                if (lenBytes > 0)
                {
                    byte* pDestB = (byte*)dest;
                    byte* pSrcB = (byte*)src;
                    for (int i = 0; i < lenBytes; i++)
                    {
                        *pDestB++ = *pSrcB++;
                    }
                }
            }
#endif
            protected int onReceive(object cbArg, LBMMessage msg)
            {
                switch (msg.type())
                {
                    case LBM.MSG_DATA:
                        try 
                        {
                            if (++bounce_count < NUM_ROUNDTRIPS_PER_MSG)
                            {
                                // Bounce the message -- no need to size check
                                IntPtr messagePtr = msg.dataPointerSafe();
#if RunUnsafeMode
                                dqwordBlockCopy(bufferAcquired.ToPointer(), messagePtr.ToPointer(), (int)msg.length());
#else
                                memcpy(bufferAcquired, messagePtr, (int) msg.length());
#endif
                                source.buffsCompleteAndAcquire(out bufferAcquired, (uint)buffer.Length, 0);
                            }
                            else
                            {
                                tsend[rcvd_msgs] = getTicks();

                                bounce_count = 0;
                                if (rcvd_msgs > lbmlatping.NUM_MSGS)
                                {   // still doing statistics report (throw away the extraneous message)
                                    break;
                                }
                                if (latping.usBusyWaitPause > 0)
                                {
                                    // Busy wait for at least requested microseconds.
                                    long tstarget = tsend[rcvd_msgs] + nanosToTicks(latping.usBusyWaitPause * 1000);
                                    while (tstarget > getTicks()) ;
                                }

                                tsstart[rcvd_msgs + 1] = getTicks();
#if RunUnsafeMode
                                fixed (void* buff = buffer)
                                {
                                    dqwordBlockCopy(bufferAcquired.ToPointer(), buff, buffer.Length);
                                }
#else
                                memcpy(bufferAcquired, bufferptr, (int)buffer.Length);
#endif
                                source.buffsCompleteAndAcquire(out bufferAcquired, (uint)buffer.Length, 0);

                                if (rcvd_msgs++ == lbmlatping.NUM_MSGS)
                                {
                                    int i;
                                    double min = Double.MaxValue, max = 0;
                                    int max_idx = -1;
                                    double[] elapsed_ts = new double[lbmlatping.NUM_MSGS - lbmlatping.NUM_MSGS_IGNORED];

                                    System.Console.Out.WriteLine("Successfully sent & received " + lbmlatping.NUM_MSGS + " total "
                                        + buffer.Length + "-byte messages, ignoring the first "
                                        + lbmlatping.NUM_MSGS_IGNORED + " messages.");
                                    System.Console.Out.WriteLine("Round-trip times in nanoseconds.");

                                    for (i = lbmlatping.NUM_MSGS_IGNORED; i < lbmlatping.NUM_MSGS; i++)
                                    {
                                        elapsed_ts[i - lbmlatping.NUM_MSGS_IGNORED] =
                                            ((double)ticksToNanos(tsend[i] - tsstart[i])) / NUM_ROUNDTRIPS_PER_MSG;
                                        if (elapsed_ts[i - lbmlatping.NUM_MSGS_IGNORED] < min)
                                            min = elapsed_ts[i - lbmlatping.NUM_MSGS_IGNORED];
                                        else if (elapsed_ts[i - lbmlatping.NUM_MSGS_IGNORED] > max)
                                        {
                                            max = elapsed_ts[i - lbmlatping.NUM_MSGS_IGNORED];
                                            max_idx = i - lbmlatping.NUM_MSGS_IGNORED;
                                        }
                                    }
                                    /* Now calculate some summary statistics. */
                                    lbmStatistics stats = new lbmStatistics();
                                    Array.Sort(elapsed_ts);
                                    stats.calcSummaryStats(elapsed_ts);

                                    string output = String.Format("Min: {0:0.00}, Max {1:0.00}", min, max);
                                    System.Console.Out.WriteLine(output);

                                    output = String.Format("Mean: {0:0.00}, Median: {1:0.00}, Standard Dev: {2:0.00}",
                                                stats.mean, stats.data[stats.data.Length / 2], stats.sample_sd);
                                    System.Console.Out.WriteLine(output);

                                    output = String.Format("99.9%: {0:#.00}, 99%: {1:0.00}, 95%: {2:0.00}, 90%: {3:0.00}, 80%: {4:0.00}",
                                                            stats.data[(stats.data.Length * 999) / 1000],
                                                            stats.data[(stats.data.Length * 99) / 100],
                                                            stats.data[(stats.data.Length * 95) / 100],
                                                            stats.data[(stats.data.Length * 9) / 10],
                                                            stats.data[(stats.data.Length * 8) / 10]);
                                    System.Console.Out.WriteLine(output);

                                    /* We're done. */
                                    Environment.Exit(0);
                                }
                            }
                        }   
                        catch (Exception e)
                        {
                            System.Console.Out.WriteLine(e.ToString());
                        }
                        break;
                    case LBM.MSG_BOS:
                        System.Console.Error.WriteLine("[" + msg.topicName() + "][" + msg.source()
                                              + "], Beginning of Transport Session");
                        break;
                    case LBM.MSG_EOS:
                        System.Console.Error.WriteLine("[" + msg.topicName() + "][" + msg.source()
                                              + "], End of Transport Session");
                        break;
                    case LBM.MSG_UNRECOVERABLE_LOSS:
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
                return 0;
            }

            private static readonly long pspertick = (1000L * 1000L * 1000L * 1000L)	// picoseconds per tick
                                                            / System.Diagnostics.Stopwatch.Frequency;
            private static readonly long nspertick = pspertick / 1000L;
            private static double ticksToNanos(long ticks)
            {
                return (double) ticks * pspertick / 1000.0;
            }

            private static long nanosToTicks(long nanos)
            {   // We are working with pico seconds (instead of nanos) because nanos per tick may be too small
                // to hold in a long.
                return nanos * 1000L / pspertick;
            }

            private static long getTicks()
            {
                return System.Diagnostics.Stopwatch.GetTimestamp();
            }

//            private long nanoTime()
//            {
//                return System.Diagnostics.Stopwatch.GetTimestamp() * pspertick / 1000L;
//            }
        }
    }
}
