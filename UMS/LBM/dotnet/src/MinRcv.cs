/*file: MinRcv.cs - minimal receiver program.
 *
 * Copyright (c) 2005-2013 Informatica Corporation. All Rights Reserved.
 * Permission is granted to licensees to use
 * or alter this software for any purpose, including commercial applications,
 * according to the terms laid out in the Software License Agreement.
 -
 - This source code example is provided by Informatica for educational
 - and evaluation purposes only.
 -
 - THE SOFTWARE IS PROVIDED "AS IS" AND INFORMATICA DISCLAIMS ALL WARRANTIES
 - EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION, ANY IMPLIED WARRANTIES OF
 - NON-INFRINGEMENT, MERCHANTABILITY OR FITNESS FOR A PARTICULAR
 - PURPOSE.  INFORMATICA DOES NOT WARRANT THAT USE OF THE SOFTWARE WILL BE
 - UNINTERRUPTED OR ERROR-FREE.  INFORMATICA SHALL NOT, UNDER ANY CIRCUMSTANCES, BE
 - LIABLE TO LICENSEE FOR LOST PROFITS, CONSEQUENTIAL, INCIDENTAL, SPECIAL OR
 - INDIRECT DAMAGES ARISING OUT OF OR RELATED TO THIS AGREEMENT OR THE
 - TRANSACTIONS CONTEMPLATED HEREUNDER, EVEN IF INFORMATICA HAS BEEN APPRISED OF
 - THE LIKELIHOOD OF SUCH DAMAGES.
 -
 - As you will see, there is no error handling in this example program.  A
 - production program would want to have better error handling, but for the
 - purposes of a minimal example, it would just be a distraction.  Also, a
 - production program would want to support a configuration file to override
 - default values on options.
 -
 - Build/run notes - Windows command-line
 -   1. Make sure the "csc.exe" C# compiler is in your path.  It probably
 -          will be if you've run a "vsvars32.bat" or similar batch file
 -          included with your installation of the Microsoft tools.
 -   2. Make sure C:\Program Files\29West\LBM_<VERS>\Win2k-i386\bin
 -           is in your path.
 -   3. Copy "lbmcs.dll" from C:\Program Files\29West\LBM_<VERS>\Win2k-i386\bin\dotnet
 -          to your program's directory.
 -   3. Compile using a command like:
 -          csc /reference:lbmcs.dll MinRcv.cs
 -   4. Run simply by executing MinRcv.exe:
 -          MinRcv.exe
 -
 - Build/run notes - Windows Visual Studio
 -   1. Create a new project and import the MinRcv.cs source file.
 -   2. Add a reference to lbmcs.dll, which is located in
 -          C:\Program Files\29West\LBM_<VERS>\Win2k-i386\bin\dotnet
 -   3. Build and run.
 */


using System;
using System.Collections.Generic;
using System.Text;
using com.latencybusters.lbm;

namespace MinRcv
{
    class MinRcv
    {
        /*
         * A class variable is used to communicate from the receiver callback
         * to the other class methods. It keeps track of the number of data
         * messages that have been received.
         */
        private static int messagesReceived = 0;

        static void Main(string[] args)
        {
            /*
             * Create a context object.  A context is an environment in which
             * LBM functions.  Each context can have its own attributes and
             * configuration settings, which can be specified in an
             * LBMContextAttributes object.  In this example program, we'll
             * just use the default attributes.
             */
            LBMContext myContext = new LBMContext();

            /*
             * Create a topic object.  A topic object is little more than a
             * string (the topic name).  During operation, LBM keeps some state
             * information in the topic object as well.  The topic is bound to
             * the containing context, and will also be bound to a receiver
             * object.  Topics can also have a set of attributes; these are
             * specified using either an LBMReceiverAttributes object or an
             * LBMSourceAttributes object.  The simplest LBMTopic constructor
             * makes a Receiver topic with the default set of attributes.  That
             * is the constructor we will call here; note that "Greeting" is
             * the topic string.
             */
            LBMTopic myTopic = new LBMTopic(myContext, "Greeting");

	    MinRcvReceiverCallback myReceiverCallback = new MinRcvReceiverCallback();

            /*
             * Create the receiver object and bind it to a topic.  Receivers
             * must be associated with a context.
             */
            LBMReceiver myReceiver = new LBMReceiver(myContext, myTopic, myReceiverCallback.onReceive, null, null);


            /*
                * Wait until we've received some messages. While the main thread
             * of the example program sleeps, LBM will receive messages and
             * call the "onReceive" method of our MinRcvReceiverCallback class.
             */
            while (messagesReceived == 0)
            {
                System.Threading.Thread.Sleep(1000);
            }

            /*
             * We've received and processed some messages.  Now let's close
             * things down.
             */

            /*
             * Close the receiver, which means we quit listening for messages
             * on our receiver's topic.
             */
            myReceiver.close();

            /*
             * Notice that we don't "close" the topic.  LBM keeps track of
             * topics for you.
             */


            /*
             * Now, we close the context itself.  Always close a context that
             * is no longer in use and won't be used again.
             */
            myContext.close();
        }  /* main */


        /* LBM passes received messages to the application by means of a
         * callback.  I.e. the LBM context thread reads the network socket,
         * performs its higher-level protocol functions, and then calls an
         * application-level function that was set up during initialization.
         * This callback function has some limitations placed upon it.  It
         * should execute quickly; any potentially blocking calls it might make
         * may interfere with the proper execution of the LBM context thread.
         * One common desire is for the receive function to send an LBM message
         * (via LBMSource.send()), however this has the potential to produce a
         * deadlock condition.  If it is desired for the receive callback
         * function to call LBM or other potentially blocking functions, it is
         * strongly advised to make use of an event queue, which causes the
         * callback to be executed from an application thread.  See the example
         * tool lbmrcvq.cs for an example of using a receiver event queue.
         *
         * LBM receiver callbacks in C# are simply methods with a prototype of:
         * public int onReceive(Object cbArgs, LBMMessage theMessage)
         */
        class MinRcvReceiverCallback
        {

            public int onReceive(Object cbArgs, LBMMessage theMessage)
            {
                /* There are several different events that can cause the
                 *  receiver callbackto be called.  Decode the event that
                 * caused this.  */
                switch (theMessage.type())
                {
                    case LBM.MSG_DATA:

                        /* NOTE:  Normally it would be a bad idea to do
                         * something as slow as a print statement in the
                         * callback function itself.  In this example, we'll
                         * probably only receive one message, so it doesn't
                         * matter.
                         */
                        System.Console.WriteLine(
                                        "Received "
                                        + theMessage.length()
                                        + " bytes on topic "
                                        + theMessage.topicName()
                                        + ": '"
                                        + System.Text.Encoding.GetEncoding("ISO-8859-1").GetString(theMessage.data())
                                        + "'");


                        /* Increment the number of messages we've received.*/
                        messagesReceived++;
                        break;

		    case LBM.MSG_BOS:
			System.Console.WriteLine("[" + theMessage.topicName() + "][" + theMessage.source() + "], Beginning of Transport Session");
			break;

		    case LBM.MSG_EOS:
			System.Console.WriteLine("[" + theMessage.topicName() + "][" + theMessage.source() + "], End of Transport Session");
			break;

                    default:
                        System.Console.WriteLine("unexpected event: " + theMessage.type());
                        System.Environment.Exit(1);
                        break;
                }
				theMessage.dispose();
                /*
                 * Return 0 if there were no errors. Returning a non-zero value will
                 * cause LBM to log a generic error message.
                 */
                return 0;
            }  /* onReceive */
        }  /* MinRcvReceiverCallback */
    }  /* class MinRcv */
}
