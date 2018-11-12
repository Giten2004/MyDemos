using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace SocketClientAsyncTester
{
    class ConnectOpUserToken
    {
        internal OutgoingMessageHolder outgoingMessageHolder;
        
        private Int32 id; //for testing only
        
        
        public ConnectOpUserToken(Int32 identifier)
        {
            id = identifier;
        }

        public Int32 TokenId
        {
            get
            {
                return id;
            }
        }
    }
}
