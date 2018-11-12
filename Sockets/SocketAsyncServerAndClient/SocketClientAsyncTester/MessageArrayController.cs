using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Globalization;

namespace SocketClientAsyncTester
{
    class MessageArrayController
    {
        string[] arrayOfMessages;
        
        Stack<OutgoingMessageHolder> stackOfOutgoingMessages;
        ConfigFileHandler configHandler;
        

        public MessageArrayController(ConfigFileHandler theConfigHandler)
        {
            this.configHandler = theConfigHandler;
            stackOfOutgoingMessages = new Stack<OutgoingMessageHolder>(this.configHandler.totalNumberOfClientConnectionsToRun);
        }
        
        internal Stack<OutgoingMessageHolder> CreateMessageStack()            
        {
            //We start by building all the messages
            //that we will send, and putting them in a jagged
            //array. This is just something we'll do for testing.
            //We don't want the message building to be a bottleneck
            //in the testing of our network app.
            //The top level size of the jagged array is equal
            //to the number of clients that we'll connect to the
            //server from this machine. The second level size
            //is equal to the number of messages that we'll send
            //from the client.
            string messageToSend = "";
            string tempMessageToSend = "";
            StringBuilder theStringBuilder = new StringBuilder();
            OutgoingMessageHolder outgoingMessageHolder;

            Console.WriteLine();
            Console.WriteLine("Creating arrays of messages to send to server from each client.");
            Console.WriteLine();
            string lastMessage = "";
            string myTempMessageToSend = "";
            //If you are running a test that is so long that it cannot be held 
            //in memory, then you can just send the same array of messages over 
            //and over, instead of having a separate array for each connection.
            if (Program.runLongTest == true)
            {                
                this.arrayOfMessages = new string[configHandler.numberOfSentMessagesPerConnection];
                
                for (int i = 0; i < configHandler.numberOfSentMessagesPerConnection; i++)
                {
                    myTempMessageToSend = "M" + (i + 1).ToString() + "-C0000";
                    this.arrayOfMessages[i] = myTempMessageToSend;
                }
                outgoingMessageHolder = new OutgoingMessageHolder(this.arrayOfMessages);
                //When Program.runLongTest == true there will be only one item in the stack
                this.stackOfOutgoingMessages.Push(outgoingMessageHolder);
            }
            else
            {
                Int64 count = Convert.ToInt64(configHandler.numberOfSentMessagesPerConnection * configHandler.totalNumberOfClientConnectionsToRun);
                if (count > 1000000)
                {
                    Console.WriteLine("runLongTest == false. So, all messages are put in memory first.\r\nIf you have a lot of messages to send, you'll use a lot of memory.");
                    Console.WriteLine("If you are running a test that is so long that it cannot\r\nbe held in memory, make Program.runLongTest = true.");
                }


                bool needToGetLastMessage = true;
                
                for (Int32 counter = this.configHandler.totalNumberOfClientConnectionsToRun; counter > 0; counter--)
                {                    
                    this.arrayOfMessages = new string[configHandler.numberOfSentMessagesPerConnection];

                    for (Int32 i = 0; i < configHandler.numberOfSentMessagesPerConnection; i++)
                    {
                        theStringBuilder.Length = 0; //clear the StringBuilder
                        theStringBuilder.Append("M");
                        theStringBuilder.Append((i + 1).ToString());
                        theStringBuilder.Append("-C");
                        
                        theStringBuilder.Append(counter.ToString());
                        tempMessageToSend = theStringBuilder.ToString();

                        //You can change messageType to 1, if you want to generate
                        //just a few messages for each client, and where each message
                        //will be different.
                        Int32 messageType = 0;
                        if (messageType == 0)
                        {
                            messageToSend = tempMessageToSend;
                        }
                        else
                        {
                            //Let's change the messages up by just appending the same message
                            //once for each loop. So, notice that this will make them longer
                            //and longer. So, you might want to do something else, if you need
                            //to test sending a lot of messages from one client.
                            for (int j = 0; j < i + 1; j++)
                            {
                                theStringBuilder.Append(tempMessageToSend);
                            }
                            messageToSend = theStringBuilder.ToString();
                            
                        }
                        
                        this.arrayOfMessages[i] = messageToSend;
                    }
                    outgoingMessageHolder = new OutgoingMessageHolder(this.arrayOfMessages);
                    this.stackOfOutgoingMessages.Push(outgoingMessageHolder);
                    if (needToGetLastMessage == true)
                    {                        
                        lastMessage = messageToSend;
                        needToGetLastMessage = false;
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("Arrays of messages built and put in stack.");
            if (Program.runLongTest == true)
            {
                Console.WriteLine("runLongTest == true. So, the same array of messages will be sent over and over, to conserve memory.");
                Console.WriteLine("First message starts on the next line:");
                Console.WriteLine(this.stackOfOutgoingMessages.Peek().arrayOfMessages[0]);
                Console.WriteLine("\r\nLast message starts on the next line:");
                Console.WriteLine(myTempMessageToSend); 
            }
            else
            {
                Console.WriteLine("First message of first client starts on the next line:");
                Console.WriteLine(this.stackOfOutgoingMessages.Peek().arrayOfMessages[0]);
                Console.WriteLine("\r\nLast message of last client starts on the next line:");                
                Console.WriteLine(lastMessage);
            }
            
            NumberFormatInfo numFormat = new CultureInfo(CultureInfo.CurrentCulture.ToString(), false).NumberFormat;
            numFormat.NumberDecimalDigits = 0;

            Console.WriteLine("\r\nNumber of connections to be started in this test = " + configHandler.totalNumberOfClientConnectionsToRun.ToString("n", numFormat));
            Console.WriteLine("Number of messages per connection = " + configHandler.numberOfSentMessagesPerConnection.ToString("n", numFormat));

            long totalMessages = Convert.ToInt64(configHandler.numberOfSentMessagesPerConnection)  * Convert.ToInt64(configHandler.totalNumberOfClientConnectionsToRun);
            Console.WriteLine("Total messages to be sent in this test = " + totalMessages.ToString("n", numFormat));
            Console.WriteLine();
            Console.WriteLine("Test is running ...");
            Console.WriteLine();
            return this.stackOfOutgoingMessages;
        }
    }
}
