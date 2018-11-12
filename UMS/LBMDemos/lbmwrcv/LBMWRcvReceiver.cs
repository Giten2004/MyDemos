using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.latencybusters.lbm;

namespace LBMApplication
{
    class LBMWRcvReceiver
    {
        public long _dereg = 0;
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
        public LBMWildcardReceiver _wrcvr;

        bool _verbose = false;
        bool _end_on_eos = false;

        public void setWrcvr(LBMWildcardReceiver wrcvr)
        {
            _wrcvr = wrcvr;
        }

        public void setDereg(long dereg)
        {
            _dereg = dereg;
        }

        public LBMWRcvReceiver(bool verbose, bool end_on_eos)
        {
            _verbose = verbose;
            _end_on_eos = end_on_eos;
        }

        public int onReceiveImmediate(object cbArg, LBMMessage msg)
        {
            imsg_count++;
            return onReceive(cbArg, msg);
        }

        public int onReceive(object cbArg, LBMMessage msg)
        {
            // keep the sqn of a regular or hot failover sequence
            UInt64 sqn = (UInt64)msg.sequenceNumber();
            if (_verbose)
            {
                if ((msg.flags() & LBM.MSG_FLAG_HF_64) > 0)
                {
                    sqn = msg.hfSequenceNumber64();
                }
                else if ((msg.flags() & LBM.MSG_FLAG_HF_32) > 0)
                {
                    sqn = (UInt64)msg.hfSequenceNumber32();
                }
            }
            switch (msg.type())
            {
                case LBM.MSG_DATA:
                    msg_count++;
                    total_msg_count++;
                    subtotal_msg_count++;
                    byte_count += msg.length();

                    if ((total_msg_count == 100) && (_dereg == 1))
                    {
                        _wrcvr.umederegister();
                        _dereg = 0;
                    }

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
                        if (msg.channelInfo() != null)
                        {
                            Console.WriteLine("@{0}.{1:000000}[{2}{3}][{4}][{5}]{6}{7}{8}{9}{10}{11}{12}, {13} bytes",
                                    msg.timestampSeconds(), msg.timestampMicroseconds(), msg.topicName(),
                                    String.Empty, msg.source(), sqn,
                                    ((msg.flags() & LBM.MSG_FLAG_RETRANSMIT) != 0 ? "-RX" : String.Empty),
                                    ((msg.flags() & LBM.MSG_FLAG_OTR) != 0 ? "-OTR" : String.Empty),
                                    ((msg.flags() & LBM.MSG_FLAG_HF_64) != 0 ? "-HF64" : String.Empty),
                                    ((msg.flags() & LBM.MSG_FLAG_HF_32) != 0 ? "-HF32" : String.Empty),
                                    ((msg.flags() & LBM.MSG_FLAG_HF_DUPLICATE) != 0 ? "-HFDUP" : String.Empty),
                                    ((msg.flags() & LBM.MSG_FLAG_HF_PASS_THROUGH) != 0 ? "-PASS" : String.Empty),
                                    ((msg.flags() & LBM.MSG_FLAG_HF_OPTIONAL) != 0 ? "-HFOPT" : String.Empty),
                                    msg.length());
                        }
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
                    subtotal_msg_count = 0;
                    break;
                case LBM.MSG_UNRECOVERABLE_LOSS:
                    unrec_count++;
                    total_unrec_count++;
                    if (_verbose)
                    {
                        System.Console.Out.Write("[" + msg.topicName() + "][" + msg.source() + "][" + sqn + "],");
                        System.Console.Out.WriteLine(" LOST");
                    }
                    break;
                case LBM.MSG_UNRECOVERABLE_LOSS_BURST:
                    burst_loss++;
                    if (_verbose)
                    {
                        System.Console.Out.Write("[" + msg.topicName() + "][" + msg.source() + "][" + sqn + "],");
                        System.Console.Out.WriteLine(" LOST BURST");
                    }
                    break;
                case LBM.MSG_REQUEST:
                    msg_count++;
                    total_msg_count++;
                    subtotal_msg_count++;
                    byte_count += msg.length();
                    if (_verbose)
                    {
                        System.Console.Out.Write("Request ["
                            + msg.topicName()
                            + "]["
                            + msg.source()
                            + "]["
                            + msg.sequenceNumber()
                            + "], ");
                        System.Console.Out.WriteLine(msg.length() + " bytes");
                    }
                    break;
                case LBM.MSG_NO_SOURCE_NOTIFICATION:
                    System.Console.Out.WriteLine("["
                        + msg.topicName()
                        + "], no sources found for topic");
                    break;
                case LBM.MSG_UME_DEREGISTRATION_SUCCESS_EX:
                    System.Console.Out.Write("Received MSG_UME_DEREGISTRATION_SUCCESS_EX\n");
                    break;
                case LBM.MSG_UME_DEREGISTRATION_COMPLETE_EX:
                    System.Console.Out.Write("Received MSG_UME_DEREGISTRATION_COMPLETE_EX\n");
                    break;
                default:
                    System.Console.Out.WriteLine("Unknown lbm_msg_t type " + msg.type() + " [" + msg.topicName() + "][" + msg.source() + "]");
                    break;
            }
            msg.dispose();
            System.Console.Out.Flush();
            return 0;
        }

        private void end()
        {
            System.Console.Error.WriteLine("Quitting.... received "
                + total_msg_count
                + " messages");
            System.Environment.Exit(0);
        }
    }
}
