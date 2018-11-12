using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace SocketClientAsyncTester
{
    public class DataHolder
    {
        private Int32 numberOfMessagesSent = 0;
        
        //We'll just send a string message. And have one or more messages, so
        //we need an array.
        internal string[] arrayOfMessagesToSend;

        internal Byte[] dataMessageReceived;
     
        //Since we are creating a List<T> of message data, we'll
        //need to decode it later, if we want to read a string.
        internal List<byte[]> listOfMessagesReceived = new List<byte[]>();

        public DataHolder()
        {
        }

        public Int32 NumberOfMessagesSent
        {
            get
            {
                return this.numberOfMessagesSent;
            }
            set
            {
                this.numberOfMessagesSent = value;
            }
        }
    
        //write the array of messages to send
        internal void PutMessagesToSend(string[] theArrayOfMessagesToSend)
        {
            this.arrayOfMessagesToSend = theArrayOfMessagesToSend;            
        }
    }
}
