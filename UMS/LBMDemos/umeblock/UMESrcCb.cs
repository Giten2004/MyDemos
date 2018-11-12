namespace LBMApplication
{
    using com.latencybusters.lbm;
    using System;

    public class UMESrcCb
    {
        private bool _verbose;

        public UMESrcCb(bool v)
        {
            this._verbose = v;
        }

        public void onSourceEvent(object arg, LBMSourceEvent sourceEvent)
        {
            switch (sourceEvent.type())
            {
                case 5:
                    this.print("Registration error: " + sourceEvent.dataString());
                    break;

                case 7:
                    this.print("Stable ACK arrived");
                    break;

                case 9:
                    this.print("Store unresponsive: " + sourceEvent.dataString());
                    break;

                case 10:
                    this.print("Reclaimed");
                    break;

                case 13:
                {
                    UMESourceEventAckInfo info = sourceEvent.ackInfo();
                    this.print("Stable EX ACK. UME store " + Convert.ToString(info.storeIndex()) + ": " + Convert.ToString(info.store()) + " message stable. SQN " + Convert.ToString(info.sequenceNumber()) + ". Flags " + Convert.ToString(info.flags()) + " ", false);
                    if ((info.flags() & 1) != 0)
                    {
                        this.print("IA ", false);
                    }
                    if ((info.flags() & 1) != 0)
                    {
                        this.print("IR ", false);
                    }
                    if ((info.flags() & 4) != 0)
                    {
                        this.print("STABLE ", false);
                    }
                    if ((info.flags() & 8) != 0)
                    {
                        this.print("STORE ", false);
                    }
                    this.print(" ");
                    break;
                }
                case 15:
                {
                    LBMSourceEventSequenceNumberInfo info2 = sourceEvent.sequenceNumberInfo();
                    this.print("Sequence number info. first: " + Convert.ToString(info2.firstSequenceNumber()) + " last: " + Convert.ToString(info2.lastSequenceNumber()));
                    break;
                }
                case 30:
                    this.print("Reclaimed");
                    break;

                default:
                    this.print("callback event: " + Convert.ToString(sourceEvent.type()));
                    break;
            }
            Console.Out.Flush();
        }

        private void print(string msg)
        {
            this.print(msg, true);
        }

        private void print(string msg, bool newline)
        {
            if (this._verbose)
            {
                if (newline)
                {
                    Console.Error.WriteLine(msg);
                }
                else
                {
                    Console.Error.Write(msg);
                }
            }
        }
    }
}

