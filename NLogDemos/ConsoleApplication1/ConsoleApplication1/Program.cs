using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    /// <summary>
    /// https://stackoverflow.com/questions/710863/log4net-vs-nlog
    /// </summary>
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            //NP Q17 SWAP
            string[] split_leg = "NP Q17 SWAP".Split(' ');
            /*
         * Welcome to this NLog demo
         */
            Console.WriteLine("Greetings, some loggings is about to take place.");
            Console.Out.WriteLine("");


            Console.Out.WriteLine("Let's assume you're going to work, and using the bus to get there:");
            Console.Out.WriteLine("------------------------------------------------------------------");

            logger.Trace("Trace: The chatter of people on the street");
            logger.Debug("Debug: Where are you going and why?");
            logger.Info("Info: What bus station you're at.");
            logger.Warn("Warn: You're playing on the phone and not looking up for your bus");
            logger.Error("Error: You get on the wrong bus.");
            logger.Fatal("Fatal: You are run over by the bus.");

            /*
         * Closing app
         */
            Console.Out.WriteLine("");
            Console.Out.WriteLine("Done logging.");
            Console.Out.WriteLine("Hit any key to exit");
            Console.ReadKey();
        }
    }
}
