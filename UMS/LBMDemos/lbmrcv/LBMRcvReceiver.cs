using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.latencybusters.lbm;
using com.latencybusters.lbm.sdm;

namespace LBMApplication
{
    class LBMRcvReceiver
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

        public long data_start_time = 0;
        public long data_end_time = 0;

        public int stotal_msg_count = 0;
        public long total_byte_count = 0;

        LBMSDMessage SDMsg;

        private bool _verbose = false;
        private bool _end_on_eos = false;
        private bool _summary = false;
        private LBMObjectRecyclerBase _recycler = null;

        public LBMRcvReceiver(bool verbose, bool end_on_eos, bool summary, LBMObjectRecyclerBase objRec)
        {
            _verbose = verbose;
            _end_on_eos = end_on_eos;
            _summary = summary;
            LBMReceiverCallback cb = new LBMReceiverCallback(onReceive);
            SDMsg = new LBMSDMessage();
            _recycler = objRec;
        }

        // This immediate-mode receiver is *only* used for topicless
        // immediate-mode sends.  Immediate sends that use a topic
        // are received with normal receiver objects.
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
                    if (stotal_msg_count == 0)
                        data_start_time = Environment.TickCount;
                    else
                        data_end_time = Environment.TickCount;

                    msg_count++;
                    total_msg_count++;
                    stotal_msg_count++;
                    subtotal_msg_count++;
                    byte_count += msg.length();
                    total_byte_count += msg.length();

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
                        Console.WriteLine("@{0}.{1:000000}[{2}{3}][{4}][{5}]{6}{7}{8}{9}{10}{11}{12}, {13} bytes",
                                msg.timestampSeconds(), msg.timestampMicroseconds(), msg.topicName(),
                                String.Empty, msg.source(), sqn,
                                ((msg.flags() & LBM.MSG_FLAG_RETRANSMIT) != 0 ? "-RX" : String.Empty),
                                ((msg.flags() & LBM.MSG_FLAG_HF_64) != 0 ? "-HF64" : String.Empty),
                                ((msg.flags() & LBM.MSG_FLAG_HF_32) != 0 ? "-HF32" : String.Empty),
                                ((msg.flags() & LBM.MSG_FLAG_HF_DUPLICATE) != 0 ? "-HFDUP" : String.Empty),
                                ((msg.flags() & LBM.MSG_FLAG_HF_PASS_THROUGH) != 0 ? "-PASS" : String.Empty),
                                ((msg.flags() & LBM.MSG_FLAG_HF_OPTIONAL) != 0 ? "-HFOPT" : String.Empty),
                                ((msg.flags() & LBM.MSG_FLAG_OTR) != 0 ? "-OTR" : String.Empty),
                                msg.length());

                        if (lbmrcv.verifiable)
                        {
                            int rc = VerifiableMessage.verifyMessage(msg.data(), msg.data().Length, lbmrcv.verbose);
                            if (rc == 0)
                            {
                                System.Console.WriteLine("Message sqn " + sqn + " does not verify!");
                            }
                            else if (rc == -1)
                            {
                                System.Console.Error.WriteLine("Message sqn " + sqn + " is not a verifiable message.");
                                System.Console.Error.WriteLine("Use -V option on source and restart receiver.");
                            }
                            else
                            {
                                if (lbmrcv.verbose)
                                {
                                    System.Console.WriteLine("Message sqn " + sqn + " verfies");
                                }
                            }
                        }
                        else if (lbmrcv.sdm)
                        {
                            try
                            {
                                SDMsg.parse(msg.data());

                                LBMSDMField f = SDMsg.locate("Sequence Number");

                                long recvdseq = ((LBMSDMFieldInt64)f).get();
                                System.Console.Out.WriteLine("SDM Message contains " + SDMsg.count() + " fields and Field Sequence Number == " + recvdseq);
                            }
                            catch (LBMSDMException sdme)
                            {
                                System.Console.Out.WriteLine("Error occurred processing received SDM Message: " + sdme);
                            }
                        }
                    }
                    break;
                case LBM.MSG_BOS:
                    System.Console.Out.WriteLine("[" + msg.topicName() + "][" + msg.source() + "], Beginning of Transport Session");
                    break;
                case LBM.MSG_EOS:
                    //data_end_time = System.DateTime.Now.Ticks;
                    System.Console.Out.WriteLine("[" + msg.topicName() + "][" + msg.source() + "], End of Transport Session");
                    if (_end_on_eos)
                    {
                        if (_summary)
                            print_summary();
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
                    if (stotal_msg_count == 0)
                        data_start_time = Environment.TickCount;
                    else
                        data_end_time = Environment.TickCount;
                    msg_count++;
                    stotal_msg_count++;
                    subtotal_msg_count++;
                    byte_count += msg.data().Length;
                    total_byte_count += msg.data().Length;
                    if (_verbose)
                    {
                        System.Console.Out.Write("Request ["
                            + msg.topicName()
                            + "]["
                            + msg.source()
                            + "]["
                            + sqn
                            + "], ");
                        System.Console.Out.WriteLine(msg.data().Length
                                                                + " bytes");
                    }
                    break;
                case LBM.MSG_HF_RESET:
                    if (_verbose)
                    {
                        Console.WriteLine("[{0}][{1}][{2}]{3}{4}{5}{6}-RESET\n", msg.topicName(), msg.source(), sqn,
                                ((msg.flags() & LBM.MSG_FLAG_RETRANSMIT) != 0 ? "-RX" : String.Empty),
                                ((msg.flags() & LBM.MSG_FLAG_OTR) != 0 ? "-OTR" : String.Empty),
                                ((msg.flags() & LBM.MSG_FLAG_HF_64) != 0 ? "-HF64" : String.Empty),
                                ((msg.flags() & LBM.MSG_FLAG_HF_32) != 0 ? "-HF32" : String.Empty));
                    }
                    break;
                default:
                    System.Console.Out.WriteLine("Unknown lbm_msg_t type " + msg.type() + " [" + msg.topicName() + "][" + msg.source() + "]");
                    break;
            }
            msg.dispose();
            return 0;
        }

        private void print_summary()
        {
            double total_time_sec, mps, bps;

            total_time_sec = 0.0;
            mps = 0.0;
            bps = 0.0;

            long bits_received = total_byte_count * 8;
            long total_time = Math.Abs(data_end_time - data_start_time);

            total_time_sec = total_time / 1000.0;

            if (total_time_sec > 0)
            {
                mps = stotal_msg_count / total_time;
                bps = bits_received / total_time / 1000.0;
            }

            System.Console.Out.WriteLine("\nTotal time         : "
                       + total_time_sec.ToString("0.000")
                       + "  sec");
            System.Console.Out.WriteLine("Messages received  : "
                       + stotal_msg_count);
            System.Console.Out.WriteLine("Bytes received     : "
                       + total_byte_count);
            System.Console.Out.WriteLine("Avg. throughput    : "
                       + mps.ToString("0.000")
                       + " Kmsgs/sec, "
                       + bps.ToString("0.000")
                       + " Mbps\n\n");

        }

        private void end()
        {
            System.Console.Out.WriteLine("Quitting.... received "
                + total_msg_count
                + " messages");
            System.Environment.Exit(0);
        }
    }
}