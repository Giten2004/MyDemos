using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;


namespace SocketClientAsyncTester
{
    
    internal sealed class SocketAsyncEventArgsPool    
    {      
        Stack<SocketAsyncEventArgs> pool;

        //just for assigning an ID so we can watch our objects while testing.
        private Int32 nextTokenId = 0;
        
                
        internal SocketAsyncEventArgsPool(Int32 capacity)
        {
            if (Program.watchProgramFlow == true)   //for testing
            {
                Program.testWriter.WriteLine("\r\nSocketAsyncEventArgsPool constructor");
            }

            this.pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        internal Int32 AssignTokenId()
        {
            Int32 tokenId = Interlocked.Increment(ref nextTokenId);
            return tokenId;
        }

        internal Int32 Count
        {
            get { return this.pool.Count; }
        }
        
        internal SocketAsyncEventArgs Pop()
        {
            lock (this.pool)
            {
                if (this.pool.Count > 0)
                {
                    return this.pool.Pop();
                }
                else
                    return null;
            }
        }

        internal void Push(SocketAsyncEventArgs item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Items added to a SocketAsyncEventArgsPool cannot be null");
            }
            lock (this.pool)
            {
                this.pool.Push(item);
            }
        }
    }
}
