using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.latencybusters.lbm;

namespace ReqRep
{
    class LBMRespReceiver
    {
        public long imsg_count = 0;
        public long request_count = 0;
        public LBMMessage lbmReqMessage = null;

        int _verbose = 0;
        bool _end_on_eos = false;
        LBMEventQueue _evq = null;
        private LBMReceiver _rcv = null;

        public LBMRespReceiver(LBMContext ctx, LBMTopic topic, LBMEventQueue evq, int verbose, bool end_on_eos)
        {
            _verbose = verbose;
            _evq = evq;
            _end_on_eos = end_on_eos;
            _rcv = new LBMReceiver(ctx, topic, new LBMReceiverCallback(onReceive), null, evq);
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
                        System.Console.Out.Write("LBM.MSG_DATA topicName:{0}, source: {1}, sequenceNumber:{2}, ", msg.topicName(), msg.source(), msg.sequenceNumber());
                        System.Console.Out.WriteLine(msg.data().Length + " bytes");

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
                    bool skipped = lbmReqMessage != null;

                    if (_verbose > 0)
                    {
                        System.Console.Out.Write("LBM.MSG_REQUEST topicName:{0}, source: {1}, sequenceNumber:{2}, ", msg.topicName(), msg.source(), msg.sequenceNumber());
                        System.Console.Out.WriteLine(msg.data().Length + " bytes" + (skipped ? " (ignored)" : ""));

                        if (_verbose > 1)
                            dump(msg);
                    }
                    if (!skipped)
                    {
                        lbmReqMessage = msg;
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
            byte[] data = msg.data();
            int size = msg.data().Length;
            StringBuilder sb;
            int b;
            ASCIIEncoding encoding = new ASCIIEncoding();

            sb = new StringBuilder();
            for (i = 0; i < (size >> 4); i++)
            {
                for (j = 0; j < 16; j++)
                {
                    b = ((int)data[(i << 4) + j]) & 0xff;
                    sb.Append(b.ToString("X2"));
                    sb.Append(" ");
                }
                sb.Append("\t");
                sb.Append(encoding.GetString(data, i << 4, 16));
                System.Console.Out.WriteLine(sb.ToString());
            }
            j = size % 16;
            if (j > 0)
            {
                sb = new StringBuilder();
                for (i = 0; i < j; i++)
                {
                    b = ((int)data[size - j + i]) & 0xff;
                    sb.Append(b.ToString("X2"));
                    sb.Append(" ");
                }
                for (i = j; i < 16; i++)
                {
                    sb.Append("   ");
                }
                sb.Append("\t");
                sb.Append(encoding.GetString(data, size - j, j));
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
