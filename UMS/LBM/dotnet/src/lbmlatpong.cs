
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
    class lbmlatpong
    {
        private volatile bool found_pinger = false;
        private LBMSource pong_src = null;
        private int cpu = -1;

        private static string usage =
          "Usage: lbmlatpong [options]\n"
        + "Available options:\n"
        + "  -a procmask = set cpu affinity mask.\n"
        + "               Available processors bitmask: " +
                    String.Format("0x{0:X}", (ulong)Process.GetCurrentProcess().ProcessorAffinity) + "\n"
        + "  -c filename = Use LBM configuration file filename.\n"
        + "                Multiple config files are allowed.\n"
        + "                Example:  '-c file1.cfg -c file2.cfg'\n"
        + "  -h = help\n"
        ;

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

        public lbmlatpong()
            : this(new string[0])
        {
        }

        public lbmlatpong(string[] args)
        {
            processCommandline(args);
        }

        public static int Main(string[] args)
        {
            lbmlatpong latpong = null;

            LBMContext ctx = null;
            LBMContextAttributes ctx_attr = null;

            LBMTopic pong_src_topic = null;
            LBMSourceAttributes pong_src_topic_attr = null;
            LBMSource pong_src = null;

            LBMTopic ping_rcv_topic = null;
            LBMReceiverAttributes ping_rcv_topic_attr = null;
            lbmlatpongreceiver ping_rcv = null;

            latpong = new lbmlatpong(args);
            if (latpong.cpu >= 0)
            {
                Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(latpong.cpu);
            }

            /* Create the context. */
            ctx_attr = new LBMContextAttributes();
            ctx_attr.setValue("resolver_cache", "0");
            ctx_attr.setValue("operational_mode", "sequential");
            ctx_attr.setValue("request_tcp_port_high", "50000");
            ctx_attr.setValue("transport_lbtipc_receiver_thread_behavior", "busy_wait");
            ctx = new LBMContext(ctx_attr);
            ctx_attr.dispose();

            /* Create the pong source. */
            pong_src_topic_attr = new LBMSourceAttributes();
            pong_src_topic_attr.setValue("resolver_advertisement_sustain_interval", "0");
            pong_src_topic_attr.setValue("transport", "lbtsmx");
            pong_src_topic = new LBMTopic(ctx, "lbmlat-pong", pong_src_topic_attr);
            pong_src_topic_attr.dispose();
            pong_src = new LBMSource(ctx, pong_src_topic);
            latpong.pong_src = pong_src;

            /* Create the ping receiver. */
            ping_rcv_topic_attr = new LBMReceiverAttributes();
            ping_rcv_topic_attr.enableSingleReceiverCallback(true);
            ping_rcv_topic_attr.setSourceNotificationCallbacks(
                        new LBMSourceCreationCallback(latpong.onNewSource),
                        new LBMSourceDeletionCallback(latpong.onSourceDelete),
                        null);
            ping_rcv_topic = new LBMTopic(ctx, "lbmlat-ping", ping_rcv_topic_attr);
            ping_rcv_topic_attr.dispose();
            ping_rcv = new lbmlatpongreceiver(ctx, ping_rcv_topic, latpong, pong_src);

            /* Wait a bit for things to get set up. */
            System.Threading.Thread.Sleep(1000);

            /* Run the context until we've discovered the pinger's source. */
            while (!latpong.found_pinger)
            {
                ctx.processEvents(1000);
            }

            /* Wait a bit for things to get set up. */
            System.Threading.Thread.Sleep(1000);

            /* Send in a dummy pong message to kick things off. */
            IntPtr writeBuff;
            if (pong_src.buffAcquire(out writeBuff, (uint)16, 0) == 0)
            {
                Marshal.WriteInt64(writeBuff, 0, 1234567890);
                pong_src.buffsComplete();
            }

            /* Wait forever. */
            while (true)
            {
                System.Threading.Thread.Sleep(1000000);
                // ctx.processEvents(1000000);
            }
        }
#if RunUnsafeMode
        unsafe
#endif
        class lbmlatpongreceiver
        {
            [DllImport("msvcrt.dll", SetLastError = false)]
            static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);

            private lbmlatpong latpong;
            private LBMSource source;
            private LBMReceiver receiver;

            private IntPtr bufferAcquired = (IntPtr) 0;
            private int bufferAcquiredSize = -1;

            public lbmlatpongreceiver(LBMContext lbmctx, LBMTopic lbmtopic, lbmlatpong mylatpong, LBMSource src)
            {
                latpong = mylatpong;
                source = src;
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
                        IntPtr messagePtr = msg.dataPointerSafe();

                        if (bufferAcquiredSize != msg.length())
                        {
                            if (bufferAcquiredSize >= 0) source.buffsCancel();

                            // assume that the payload size is correctly checked on the pinger size
                            bufferAcquiredSize =
                                (source.buffAcquire(out bufferAcquired, (uint) msg.length(), 0) == 0)
                                            ? (int) msg.length() : -1;
                        }
#if RunUnsafeMode
                        dqwordBlockCopy(bufferAcquired.ToPointer(), messagePtr.ToPointer(), (int) msg.length());
#else
                        memcpy(bufferAcquired, messagePtr, (int) msg.length());
#endif
                        source.buffsCompleteAndAcquire(out bufferAcquired, (uint) bufferAcquiredSize, 0);
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
        }

        public int onSourceDelete(string source, object cbObj, object sourceCbObj)
        {
            return 0;
        }

        public Object onNewSource(string source, object cbObj)
        {
            found_pinger = true;
            return null;
        }
    }
}
