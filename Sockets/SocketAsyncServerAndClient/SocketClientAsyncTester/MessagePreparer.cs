using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace SocketClientAsyncTester
{
    class MessagePreparer
    {
        internal void GetDataToSend(SocketAsyncEventArgs e)
        {            
            DataHoldingUserToken theUserToken = (DataHoldingUserToken)e.UserToken;
            DataHolder dataHolder = theUserToken.theDataHolder;

            //In this example code, we will  
            //prefix the message with the length of the message. So we put 2 
            //things into the array.
            // 1) prefix,
            // 2) the message.

            //Determine the length of the message that we will send.
            Int32 lengthOfCurrentOutgoingMessage = dataHolder.arrayOfMessagesToSend[dataHolder.NumberOfMessagesSent].Length;

            //convert the message to byte array
            Byte[] arrayOfBytesInMessage = Encoding.ASCII.GetBytes(dataHolder.arrayOfMessagesToSend[dataHolder.NumberOfMessagesSent]);

            //So, now we convert the length integer into a byte array.
            //Aren't byte arrays wonderful? Maybe you'll dream about byte arrays tonight!
            Byte[] arrayOfBytesInPrefix = BitConverter.GetBytes(lengthOfCurrentOutgoingMessage);

            //Create the byte array to send.
            theUserToken.dataToSend = new Byte[theUserToken.sendPrefixLength + lengthOfCurrentOutgoingMessage];

            //Now copy the 2 things to the theUserToken.dataToSend.
            Buffer.BlockCopy(arrayOfBytesInPrefix, 0, theUserToken.dataToSend, 0, theUserToken.sendPrefixLength);
            Buffer.BlockCopy(arrayOfBytesInMessage, 0, theUserToken.dataToSend, theUserToken.sendPrefixLength, lengthOfCurrentOutgoingMessage);

            theUserToken.sendBytesRemaining = theUserToken.sendPrefixLength + lengthOfCurrentOutgoingMessage;
            theUserToken.bytesSentAlready = 0;
        }
    }
}
