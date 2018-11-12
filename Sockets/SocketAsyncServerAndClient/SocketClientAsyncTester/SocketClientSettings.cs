using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SocketClientAsyncTester
{
    class SocketClientSettings
    {
        // total number of client connections that will be generated
        // by this test program.
        private Int32 totalNumberOfClientConnectionsToRun;
        
        // tells us how many objects to put in pool for connect operations
        private Int32 maxSimultaneousConnectOps;

        // tells us maximum number of open sockets
        private Int32 maxSimultaneousConnections;

        // number of SAEA objects to put in the pool. This just allows us to
        // specify some excess objects above the value of maxSimultaneousConnections,
        // if we wish.
        private Int32 numberOfSaeaForRecSend;

        // buffer size to use for each socket receive operation
        private Int32 bufferSize;

        // length of message prefix for receive ops
        private Int32 receivePrefixLength;

        // length of message prefix for send ops
        private Int32 sendPrefixLength;

        // See comments in buffer manager.
        private Int32 opsToPreAllocate;
        
        private IPEndPoint serverEndPoint;

        private Int32 numberOfMessagesPerConnection;

        public SocketClientSettings(IPEndPoint theServerEndPoint, Int32 totalNumberOfClientConnectionsToRun, Int32 numberOfMessages, Int32 maxSimultaneousConnectOps, Int32 theMaxConnections, Int32 receivePrefixLength, Int32 bufferSize, Int32 sendPrefixLength, Int32 opsToPreAlloc)
        {
            this.totalNumberOfClientConnectionsToRun = totalNumberOfClientConnectionsToRun;            
            this.maxSimultaneousConnectOps = maxSimultaneousConnectOps;
            this.maxSimultaneousConnections = theMaxConnections;
            this.numberOfSaeaForRecSend = theMaxConnections + 1;
            this.receivePrefixLength = receivePrefixLength;
            this.bufferSize = bufferSize;
            this.sendPrefixLength = sendPrefixLength;
            this.opsToPreAllocate = opsToPreAlloc;
            this.serverEndPoint = theServerEndPoint;
            this.numberOfMessagesPerConnection = numberOfMessages;
        }

        public Int32 ConnectionsToRun
        {
            get
            {
                return this.totalNumberOfClientConnectionsToRun;
            }
        }
        
        public Int32 MaxConnectOps
        {
            get
            {
                return this.maxSimultaneousConnectOps;
            }
        }

        public Int32 MaxConnections
        {
            get
            {
                return this.maxSimultaneousConnections;
            }
        }

        public Int32 NumberOfSaeaForRecSend
        {
            get
            {
                return this.numberOfSaeaForRecSend;
            }
        }

        public Int32 ReceivePrefixLength
        {
            get
            {
                return this.receivePrefixLength;
            }
        }
        public Int32 BufferSize
        {
            get
            {
                return this.bufferSize;
            }
        }
        public Int32 SendPrefixLength
        {
            get
            {
                return this.sendPrefixLength;
            }
        }
        public Int32 OpsToPreAllocate
        {
            get
            {
                return this.opsToPreAllocate;
            }
        }
                
        public IPEndPoint ServerEndPoint
        {
            get
            {
                return this.serverEndPoint;
            }
        }

        public Int32 NumberOfMessages
        {
            get
            {
                return this.numberOfMessagesPerConnection;
            }
        }                
    }
}
