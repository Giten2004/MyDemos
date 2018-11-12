using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.latencybusters.lbm;

namespace LBMApplication
{
    class LBMMRcvReceiver
    {
        public long imsg_count = 0;
        public long msg_count = 0;
        public long total_msg_count = 0;
        public long subtotal_msg_count = 0;
        public long byte_count = 0;
        public long unrec_count = 0;
        public long total_unrec_count = 0;
        public long burst_loss = 0;
        public long rx_msgs = 0;
        public long otr_msgs = 0;
        public LBMReceiver rcv = null;

        bool _verbose = false;
        LBMEventQueue _evq = null;

        public string saved_source = null;

        public LBMMRcvReceiver(LBMContext ctx, LBMTopic topic, LBMEventQueue evq, bool verbose)
        {
            rcv = new LBMReceiver(ctx, topic, new LBMReceiverCallback(onReceive), null, evq);
            _verbose = verbose;
            _evq = evq;
        }

        public LBMMRcvReceiver(LBMContext ctx, LBMTopic topic, bool verbose)
        {
            rcv = new LBMReceiver(ctx, topic, new LBMReceiverCallback(onReceive), null);
            _verbose = verbose;
        }

        protected int onReceive(object cbArg, LBMMessage msg)
        {
            switch (msg.type())
            {
                case LBM.MSG_DATA:
                    if (msg_count == 0)
                        saved_source = msg.source();
                    msg_count++;
                    total_msg_count++;
                    subtotal_msg_count++;
                    byte_count += msg.data().Length;

                    if ((msg.flags() & LBM.MSG_FLAG_RETRANSMIT) != 0)
                    {
                        rx_msgs++;
                    }
                    if ((msg.flags() & LBM.MSG_FLAG_OTR) != 0)
                    {
                        otr_msgs++;
                    }

                    if (_verbose)
                    {
                        System.Console.Out.Write("["
                            + msg.topicName()
                            + "]["
                            + msg.source()
                            + "]["
                            + msg.sequenceNumber()
                            + "], ");

                        if ((msg.flags() & LBM.MSG_FLAG_RETRANSMIT) != 0)
                        {
                            System.Console.Out.Write("-RX- ");
                        }
                        if ((msg.flags() & LBM.MSG_FLAG_OTR) != 0)
                        {
                            System.Console.Out.Write("-OTR- ");
                        }

                        System.Console.Out.WriteLine(msg.data().Length
                                                                + " bytes");
                    }
                    break;
                case LBM.MSG_BOS:
                    System.Console.Out.WriteLine("[" + msg.topicName() + "][" + msg.source() + "], Beginning of Transport Session");
                    break;
                case LBM.MSG_EOS:
                    System.Console.Out.WriteLine("[" + msg.topicName() + "][" + msg.source() + "], End of Transport Session");
                    subtotal_msg_count = 0;
                    break;
                case LBM.MSG_NO_SOURCE_NOTIFICATION:
                    if (_verbose)
                    {
                        System.Console.Out.WriteLine("[" + msg.topicName() + "], no sources found for topic");
                    }
                    break;
                case LBM.MSG_UNRECOVERABLE_LOSS:
                    unrec_count++;
                    total_unrec_count++;
                    if (_verbose)
                    {
                        System.Console.Out.Write("[" + msg.topicName() + "][" + msg.source() + "][" + msg.sequenceNumber() + "],");
                        System.Console.Out.WriteLine(" LOST");
                    }
                    break;
                case LBM.MSG_UNRECOVERABLE_LOSS_BURST:
                    burst_loss++;
                    if (_verbose)
                    {
                        System.Console.Out.Write("[" + msg.topicName() + "][" + msg.source() + "][" + msg.sequenceNumber() + "],");
                        System.Console.Out.WriteLine(" LOST BURST");
                    }
                    break;
                case LBM.MSG_REQUEST:
                    msg_count++;
                    total_msg_count++;
                    subtotal_msg_count++;
                    byte_count += msg.data().Length;
                    if (_verbose)
                    {
                        System.Console.Out.Write("Request ["
                            + msg.topicName()
                            + "]["
                            + msg.source()
                            + "]["
                            + msg.sequenceNumber()
                            + "], ");
                        System.Console.Out.WriteLine(msg.data().Length
                                                                + " bytes");
                    }
                    break;
                default:
                    System.Console.Out.WriteLine("Unknown lbm_msg_t type " + msg.type() + " [" + msg.topicName() + "][" + msg.source() + "]");
                    break;
            }
            System.Console.Out.Flush();
            msg.dispose();
            return 0;
        }

        private void end()
        {
            System.Environment.Exit(0);
        }
    }
}
