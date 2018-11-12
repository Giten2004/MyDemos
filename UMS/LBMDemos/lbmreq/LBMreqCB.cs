using System;
using System.Text;
using com.latencybusters.lbm;

namespace lbmreq
{
    class LBMreqCB
    {
        public int response_count;
        public int response_byte_count;
        int _verbose;

        public LBMreqCB(int verbose)
        {
            _verbose = verbose;
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
                        Console.Out.WriteLine("LBM.MSG_RESPONSE [" + msg.source() + "][" + msg.sequenceNumber() + "], " + msg.data().Length + " bytes");
                        if (_verbose > 1)
                            dump(msg);
                    }
                    break;
                default:
                    Console.Out.WriteLine("Unknown message type " + msg.type() + "[" + msg.source() + "]");
                    break;
            }
            msg.dispose();
            Console.Out.Flush();
            return 0;
        }

        public void onSourceEvent(object arg, LBMSourceEvent sourceEvent)
        {
            string clientname;

            switch (sourceEvent.type())
            {
                case LBM.SRC_EVENT_CONNECT:
                    clientname = sourceEvent.dataString();
                    Console.Out.WriteLine("Receiver connect " + clientname);
                    break;
                case LBM.SRC_EVENT_DISCONNECT:
                    clientname = sourceEvent.dataString();
                    Console.Out.WriteLine("Receiver disconnect " + clientname);
                    break;
                default:
                    break;
            }
            Console.Out.Flush();
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
                    b = data[(i << 4) + j] & 0xff;
                    sb.Append(b.ToString("X2"));
                    sb.Append(" ");
                }
                sb.Append("\t");
                sb.Append(encoding.GetString(data, i << 4, 16));
                Console.Out.WriteLine(sb.ToString());
            }
            j = size % 16;
            if (j > 0)
            {
                sb = new StringBuilder();
                for (i = 0; i < j; i++)
                {
                    b = data[size - j + i] & 0xff;
                    sb.Append(b.ToString("X2"));
                    sb.Append(" ");
                }
                for (i = j; i < 16; i++)
                {
                    sb.Append("   ");
                }
                sb.Append("\t");
                sb.Append(encoding.GetString(data, size - j, j));
                Console.Out.WriteLine(sb.ToString());
            }
            Console.Out.Flush();
        }
    }
}
