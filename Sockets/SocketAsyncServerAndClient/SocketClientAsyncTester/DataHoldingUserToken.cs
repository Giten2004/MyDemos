using System;
using System.Net.Sockets;
using System.Text;

namespace SocketClientAsyncTester
{
    class DataHoldingUserToken
    {
        internal DataHolder theDataHolder;

        private Int32 idOfThisObject; //for testing only

        internal readonly Int32 sendPrefixLength;
        internal readonly Int32 receivePrefixLength;
        internal Int32 receivedPrefixBytesDoneCount = 0;
        internal Int32 receivedMessageBytesDoneCount = 0;
        internal Byte[] byteArrayForPrefix;
        internal Int32 receiveMessageOffset;
        internal Int32 recPrefixBytesDoneThisOp = 0;
        internal Int32 lengthOfCurrentIncomingMessage;
        internal readonly Int32 bufferOffsetReceive;
        internal readonly Int32 permanentReceiveMessageOffset;
        internal readonly Int32 bufferOffsetSend;
        internal Byte[] dataToSend;
        internal Int32 sendBytesRemaining;
        internal Int32 bytesSentAlready;

        public DataHoldingUserToken(Int32 rOffset, Int32 sOffset, Int32 receivePrefixLength, Int32 sendPrefixLength, Int32 identifier)
        {
            this.idOfThisObject = identifier;
            this.bufferOffsetReceive = rOffset;
            this.bufferOffsetSend = sOffset;
            this.receivePrefixLength = receivePrefixLength;
            this.sendPrefixLength = sendPrefixLength;
            this.receiveMessageOffset = rOffset + receivePrefixLength;
            this.permanentReceiveMessageOffset = this.receiveMessageOffset;           
        }

        public Int32 TokenId
        {
            get
            {
                return idOfThisObject;
            }
        }

        internal void CreateNewDataHolder()
        {
            theDataHolder = new DataHolder();
        }

        public void Reset()
        {
            this.receivedPrefixBytesDoneCount = 0;
            this.receivedMessageBytesDoneCount = 0;
            this.recPrefixBytesDoneThisOp = 0;
            this.receiveMessageOffset = this.permanentReceiveMessageOffset;
        }
    }
}
