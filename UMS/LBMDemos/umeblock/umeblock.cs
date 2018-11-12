//namespace LBMApplication
//{
//    using com.latencybusters.lbm;
//    using System;
//    using System.Runtime.InteropServices;
//    using System.Threading;

//    public static class umeblock
//    {
//        private static string confname;
//        private static int msglen = 10;
//        private static int msgs = 0x989680;
//        public static string purpose = "Purpose: Send messages on a single topic by using a blocking send call.";
//        private static string usage = "Usage umeblock [options] topic\nAvaible options\n  -c filname   use LBM configuration file FILE\n  -l num       send messages of NUM bytes\n  -M num       send NUM messages\n  -v           print additional info in verbose form\n";
//        private static bool verbose = false;

//        private static void logger(int loglevel, string message)
//        {
//            string str = string.Empty;
//            switch (loglevel)
//            {
//                case 1:
//                    str = "Emergency";
//                    break;

//                case 2:
//                    str = "Alert";
//                    break;

//                case 3:
//                    str = "Critical";
//                    break;

//                case 4:
//                    str = "Error";
//                    break;

//                case 5:
//                    str = "Warning";
//                    break;

//                case 6:
//                    str = "Note";
//                    break;

//                case 7:
//                    str = "Info";
//                    break;

//                case 8:
//                    str = "Debug";
//                    break;

//                default:
//                    str = "Unknown";
//                    break;
//            }
//            Console.Out.WriteLine(DateTime.Now.ToString() + " [" + str + "]: " + message);
//            Console.Out.Flush();
//        }

//        private static void Main(string[] args)
//        {
//            int length = args.Length;
//            ulong bytes = 0L;
//            int num3 = 0;
//            new LBM().setLogger(new LBMLogging(umeblock.logger));
//            int index = 0;
//            while (index < length)
//            {
//                try
//                {
//                    string str = args[index];
//                    if (str != null)
//                    {
//                        if (!(str == "-c"))
//                        {
//                            if (str == "-l")
//                            {
//                                goto Label_0096;
//                            }
//                            if (str == "-M")
//                            {
//                                goto Label_00B1;
//                            }
//                            if (str == "-v")
//                            {
//                                goto Label_00CC;
//                            }
//                            if (str == "-h")
//                            {
//                                goto Label_00DA;
//                            }
//                        }
//                        else
//                        {
//                            confname = args[++index];
//                            num3 += 2;
//                        }
//                    }
//                    goto Label_0107;
//                Label_0096:
//                    msglen = Convert.ToInt32(args[++index]);
//                    num3 += 2;
//                    goto Label_0107;
//                Label_00B1:
//                    msgs = Convert.ToInt32(args[++index]);
//                    num3 += 2;
//                    goto Label_0107;
//                Label_00CC:
//                    verbose = true;
//                    num3++;
//                    goto Label_0107;
//                Label_00DA:
//                    print_help_exit(0);
//                }
//                catch (Exception exception)
//                {
//                    Console.Error.WriteLine("umeblock: error\n" + exception.Message);
//                    print_help_exit(1);
//                }
//            Label_0107:
//                index++;
//            }
//            if ((length <= 0) || (num3 == length))
//            {
//                print_help_exit(1);
//            }
//            byte[] message = new byte[msglen];
//            if (confname != null)
//            {
//                LBM.setConfiguration(confname);
//            }
//            LBMContextAttributes lbmcattr = new LBMContextAttributes();
//            LBMSourceAttributes sattr = new LBMSourceAttributes();
//            LBMContext lbmctx = new LBMContext(lbmcattr);
//            LBMTopic lbmtopic = lbmctx.allocTopic(args[index - 1]);
//            UMESrcCb cb = new UMESrcCb(verbose);
//            UMEBlockSrc src = new UMEBlockSrc();
//            if (!src.createSource(lbmctx, lbmtopic, sattr, new LBMSourceEventCallback(cb.onSourceEvent), null, null))
//            {
//                Console.Write("Error creating blocking source!");
//                Environment.Exit(1);
//            }
//            Thread.Sleep(0x3e8);
//            long ticks = DateTime.Now.Ticks;
//            for (uint i = 0; i < msgs; i++)
//            {
//                try
//                {
//                    src.send(message, msglen, 0, null);
//                    bytes += msglen;
//                }
//                catch (LBMException exception2)
//                {
//                    Console.WriteLine("Error sending: " + exception2.ToString());
//                    Environment.Exit(1);
//                }
//                catch (Exception exception3)
//                {
//                    Console.WriteLine(exception3.ToString());
//                    Environment.Exit(1);
//                }
//            }
//            double sec = ((double) (DateTime.Now.Ticks - ticks)) / 10000000.0;
//            print_bw(sec, msgs, bytes);
//            Console.WriteLine("Lingering for 5 seconds...");
//            Thread.Sleep(0x1388);
//            src.close();
//            lbmctx.close();
//        }

//        private static void print_bw(double sec, int msgs, ulong bytes)
//        {
//            double num = 0.0;
//            double num2 = 0.0;
//            double num3 = 1000.0;
//            double num4 = 1000000.0;
//            char ch = 'K';
//            char ch2 = 'K';
//            num = ((double) msgs) / sec;
//            num2 = ((double) (bytes * ((ulong) 8L))) / sec;
//            if (num <= num4)
//            {
//                ch = 'K';
//                num /= num3;
//            }
//            else
//            {
//                ch = 'M';
//                num /= num4;
//            }
//            if (num2 <= num4)
//            {
//                ch2 = 'K';
//                num2 /= num3;
//            }
//            else
//            {
//                ch2 = 'M';
//                num2 /= num4;
//            }
//            Console.Out.WriteLine(string.Concat(new object[] { sec, " secs. ", num.ToString("0.000"), " ", ch, "msgs/sec. ", num2.ToString("0.000"), " ", ch2, "bps" }));
//            Console.Out.Flush();
//        }

//        private static void print_help_exit(int exit_value)
//        {
//            Console.Error.WriteLine(LBM.version());
//            Console.Error.WriteLine(purpose);
//            Console.Error.WriteLine(usage);
//            Environment.Exit(exit_value);
//        }

//        [DllImport("Kernel32.dll")]
//        public static extern int SetEnvironmentVariable(string name, string value);
//    }
//}

public class Program
{
    public static void Main(string[] args)
    {
    }
}