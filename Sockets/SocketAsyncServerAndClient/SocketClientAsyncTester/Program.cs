using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Globalization;


namespace SocketClientAsyncTester
{
    /// <summary>
    /// http://www.codeproject.com/Articles/83102/C-SocketAsyncEventArgs-High-Performance-Socket-Cod
    /// </summary>
    public static class Program
    {
        //****Tested on .NET 3.5, Windows XP Prof and Windows 7 Prof.


        //If this is true, then stuff about which method is being 
        //called will print to log.
        public static bool watchProgramFlow = true;

        //this is relevant only if watchProgramFlow == false
        public static readonly bool writeErrorsToLog = true;

        //If you make this true, then connect/disconnect info will print to log.
        public static readonly bool showConnectAndDisconnect = true;

        //If you make this true, then some receive/send data 
        //will print to log.        
        public static readonly bool watchData = true;

        //Only use this is you are running a test with so many messages that they
        //are using too much memory. You can see how much memory is being used by just
        //looking in Windows Task Manager.
        public static readonly bool runLongTest = false;

        //If you make this true, then stuff about which method is being 
        //called will print to Console. Don't use this unless you are trying to
        //solve a problem.
        public static readonly bool consoleWatch = false;

        //If you make this true, it will simulate some delay between
        //connections. You need a little delay between the connections. Otherwise
        //it calls GetMessages() just as fast as a for loop can loop. It uses the value
        //in tickDelayBeforeNextConn.
        public static readonly bool useDelayBetweenConnections = true;

        //This is in ticks. So a value of 10000 = 1 millisecond delay.     
        //This probably needs to have a value of at least 1000.
        public const Int64 tickDelayBeforeNextConn = 10000;

        public static readonly bool playSoundOnCompletion = true;

        //tells what size the message prefix will be for messages that
        //the client sends. don't change it unless you change the code.
        public const Int32 sendPrefixLength = 4;

        //tells what size the message prefix will be for messages that
        //the client receives. don't change it unless you change the code.
        public const Int32 receivePrefixLength = 4;

        //tells us how many objects to put in pool for connect operations
        public const Int32 maxSimultaneousConnectOps = 10;

        // 1 for receive, 1 for send. used in BufferManager
        public const Int32 opsToPreAlloc = 2;    

        //Used in SocketClient.ProcessConnectError(). If this is true, then when the server
        //does not accept the connection, the client will retry until it connects.
        public static readonly bool continuallyRetryConnectIfSocketError = true;

        public static TestFileWriter testWriter;

        //These strings are just for console interaction.
        public const string checkString = "C";
        public const string stopString = "S";
        public const string closeString = "Z";
        public const string completedString = "Test Completed";
        public const string wpf = "T";
        public const string wpfNo = "F";        
        public static string wpfTrueString = "";
        public static string wpfFalseString = "";

        public static void Main(string[] args)
        {
            // Before the app starts, let's build strings for you to use the console.
            BuildStringsForClientConsole();

            // Get settings and connection info.
            ConfigFileHandler configHandler = new ConfigFileHandler();
            
            // Start the log writer.
            testWriter = new TestFileWriter(configHandler.saveDirectory);
            
            // Create one object with a lot of settings, to pass to SocketClient.
            SocketClientSettings socketClientSettings = new SocketClientSettings(configHandler.remoteEndPoint, configHandler.totalNumberOfClientConnectionsToRun, configHandler.numberOfReceivedMessagesPerConnection, Program.maxSimultaneousConnectOps, configHandler.maximumNumberOfSimultaneousClientConnections, Program.receivePrefixLength, configHandler.testBufferSize, Program.sendPrefixLength, Program.opsToPreAlloc);

            // Build the arrays of messages that will be sent, and pass them
            // to the SocketClient object in a stack of arrays.            
            MessageArrayController messageArrayController = new MessageArrayController(configHandler);
            Stack<OutgoingMessageHolder> stackOfOutgoingMessages = messageArrayController.CreateMessageStack();

            // Create the object that will do most of the work.
            SocketClient socketClient = new SocketClient(socketClientSettings);
            socketClient.GetMessages(stackOfOutgoingMessages);
            
            // Interact with the console.
            HandleConsoleInteraction(configHandler, socketClient);
        }

        //____________________________________________________________________________
        static void BuildStringsForClientConsole()
        {
            StringBuilder sb = new StringBuilder();

            // Make the string to write if test is NOT complete.
            sb.Append("\r\n");
            sb.Append("\r\n");
            sb.Append("'");
            sb.Append(completedString);
            sb.Append("' will display here when test completes.\r\n\r\nBefore the test completes to take any of the following\r\nactions type the corresponding letter below and press Enter.\r\n");
            sb.Append(stopString);
            sb.Append(")  to stop the program before it completes.\r\n");
            sb.Append(checkString);
            sb.Append(")  to check current status\r\n");
            string tempString = sb.ToString();
            sb.Length = 0;

            // string when watchProgramFlow == true 
            sb.Append(wpfNo);
            sb.Append(")  to quit writing program flow. (ProgramFlow is being logged now.)\r\n");
            wpfTrueString = tempString + sb.ToString();
            sb.Length = 0;

            // string when watchProgramFlow == false
            sb.Append(wpf);
            sb.Append(")  to start writing program flow. (ProgramFlow is NOT being logged.)\r\n");
            wpfFalseString = tempString + sb.ToString();
        }

        //____________________________________________________________________________
        static void HandleConsoleInteraction(ConfigFileHandler configHandler, SocketClient socketClient)
        {
            string stringToCompare = "";
            string theEntry = "";
            Int32 count = 0;

            // Let's put a little wait here to let the clients finish, so that we
            // will not write irrelevant info to Console if it finishes fast.
            Thread.Sleep(1000);

            while (stringToCompare != closeString)
            {
                if (socketClient.finished == true)
                {
                    if ((count > 0) && (socketClient.abortTest == false))
                    {                        
                        Console.WriteLine("\r\n\r\n*********   " + Program.completedString + "   *********\r\nTo close the program and write log, type '" + Program.closeString + "' and press Enter");
                    }
                }
                else
                {
                    if (watchProgramFlow == true)
                    {
                        Console.WriteLine(wpfTrueString); 
                    }
                    else
                    {
                        Console.WriteLine(wpfFalseString);
                    }
                }

                theEntry = Console.ReadLine().ToUpper(CultureInfo.InvariantCulture);

                switch (theEntry)
                {
                    case checkString:
                        GetPercentageCompleted(configHandler, socketClient);
                        break;
                    case wpf:
                        if ((Program.watchProgramFlow == false) && (socketClient.finished == false))
                        {
                            Program.watchProgramFlow = true;
                            Console.WriteLine("Changed watchProgramFlow to true.");
                            Program.testWriter.WriteLine("\r\nStart logging program flow.\r\n");
                        }
                        else
                        {
                            Console.WriteLine("Program flow was already being logged.");
                        }
                        
                        break;
                    case wpfNo:
                        if ((Program.watchProgramFlow == true) && (socketClient.finished == false))
                        {
                            Program.watchProgramFlow = false;
                            Console.WriteLine("Changed watchProgramFlow to false.");
                            Program.testWriter.WriteLine("\r\nStopped logging program flow.\r\n");
                        }
                        else
                        {
                            Console.WriteLine("Program flow was already NOT being logged.");
                        }                        
                        break;
                    case stopString:
                        if (socketClient.finished == false)
                        {
                            socketClient.abortTest = true;                            
                            Console.WriteLine("test stopped");
                            stringToCompare = closeString;
                        }                                                
                        break;
                    case closeString:
                        stringToCompare = closeString;                        
                        break;
                    default:
                        Console.WriteLine("Unrecognized entry");
                        break;
                }
                count++;
            }
            CloseProgram(configHandler, socketClient);            
        }

        //____________________________________________________________________________
        static void CloseProgram(ConfigFileHandler configHandler, SocketClient socketClient)
        {
            CalculateTimeRelated(configHandler, socketClient);

            //Write final message to log.
            if (socketClient.completedAllTests == true)
            {
                Program.testWriter.WriteLine("\r\n\r\nTest successful.  All messages sent.");
            }
            else
            {
                Program.testWriter.WriteLine("\r\n\r\n**********  Test finished with aborted messages  **********");
            }

            //Close log.
            try
            {
                testWriter.Close();

            }
            catch
            {
                Console.WriteLine("Could not close log properly.");
            }
            socketClient.CleanUpOnExit();

            Int32 exitCode = 0;
            if (socketClient.abortTest == true)
            {
                exitCode = 1;
            }
            Environment.Exit(exitCode);
        }

        //____________________________________________________________________________
        static void CalculateTimeRelated(ConfigFileHandler configHandler, SocketClient socketClient)
        {
            long stopTime = 0;
                        
            if (socketClient.completedAllTests == true)
            {
                stopTime = socketClient.stopTime;                
            }
            else
            {
                stopTime = DateTime.Now.Ticks;
            }
            
            if (socketClient.totalCountOfMessagesSent > 0)
            {
                //Display some performance numbers. But when writing to Console, they
                //are useless. So we won't display them then.
                if (Program.consoleWatch == false)
                {
                    long elapsedTicks = stopTime - socketClient.startTime;
                    TimeSpan elapsedTime = new TimeSpan(elapsedTicks);
                    decimal ticksPerMessage = (decimal)elapsedTicks / (decimal)socketClient.totalCountOfMessagesSent;
                    decimal msPerMessage = ticksPerMessage / (decimal)10000;
                    msPerMessage = Math.Round(msPerMessage, 2, MidpointRounding.AwayFromZero);

                    GetPercentageCompleted(configHandler, socketClient);
                    testWriter.WriteLine("Time taken for test was " + elapsedTime);
                    testWriter.WriteLine("ms per message = " + msPerMessage.ToString("#0.00"));
                }
            }            
            else
            {
                Console.WriteLine("Something is wrong. No messages sent. Maybe firewall is blocking.");
            }
        }

        //____________________________________________________________________________
        static void GetPercentageCompleted(ConfigFileHandler configHandler, SocketClient socketClient)
        {
            Double sentCount = (Double)socketClient.totalCountOfMessagesSent;
            Double totalMessagesToBeSent = (Double)configHandler.numberOfSentMessagesPerConnection * (Double)configHandler.totalNumberOfClientConnectionsToRun;
            Double percentOfMessages = (sentCount / totalMessagesToBeSent) * (Double)100;
            Int64 totalCountOfMessagesAborted = ((Int64)configHandler.numberOfSentMessagesPerConnection * (Int64)configHandler.totalNumberOfClientConnectionsToRun) - socketClient.totalCountOfMessagesSent;

            NumberFormatInfo numFormat = new CultureInfo(CultureInfo.CurrentCulture.ToString(), false).NumberFormat;
            numFormat.NumberDecimalDigits = 0;

            //We'll only write this to log if we are finished.
            if ((socketClient.finished == true) || (socketClient.abortTest == true))
            {
                Program.testWriter.WriteLine("\r\nMessages sent = " + sentCount.ToString("n", numFormat) + ", which is " + percentOfMessages.ToString("#0.00") + "% of total messages to be sent.\r\n# of connection retries = " + socketClient.totalNumberOfConnectionRetries + "\r\nAborted messages =  " + totalCountOfMessagesAborted + "\r\nMax simultaneous connections were " + socketClient.maxSimultaneousClientsThatWereConnected);
            }
            else //write to Console when user types value of checkString
            {
                Console.WriteLine("\r\nMessages sent = " + sentCount.ToString("n", numFormat) + ", which is " + percentOfMessages.ToString("#0.00") + "% of total messages to be sent.\r\n# of connection retries = " + socketClient.totalNumberOfConnectionRetries + "\r\nClient connections finished = " + socketClient.numberOfConnectionsFinishedSuccessfully + ".\r\n" + socketClient.clientsNowConnectedCount + " clients connected to server from this machine.\r\nTest continuing now.\r\n");
            }
        }        
    }
}
