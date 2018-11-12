using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace SocketClientAsyncTester
{
    internal sealed class SocketPool
    {
        // Pool of reusable Socket objects.        
        ////Stack<Socket> pool;

        // initializes the object pool to the specified size.
        // "capacity" = Maximum number of Socket objects
        ////internal SocketPool(Int32 capacity)
        ////{
        ////    this.pool = new Stack<Socket>(capacity);
        ////}

        // The number of Socket instances in the pool.         
        internal Int32 Count
        {
            get { return this.poolOfSockets.Count; }
        }

        ////// Removes a Socket instance from the pool.
        ////// returns Socket removed from the pool.
        ////internal Socket Pop()
        ////{
        ////    lock (this.pool)
        ////    {
        ////        return this.pool.Pop();
        ////    }
        ////}

        // Add a Socket instance to the pool. 
        // "theSocket" = Socket instance to add to the pool.
        ////internal void Push(Socket theSocket)
        ////{
        ////    if (theSocket == null)
        ////    {
        ////        throw new ArgumentNullException("Items added to a SocketPool cannot be null");
        ////    }
        ////    lock (this.pool)
        ////    {
        ////        this.pool.Push(theSocket);
        ////    }
        ////}

        Queue<Socket> poolOfSockets;

        internal SocketPool(Int32 capacity)
        {
            this.poolOfSockets = new Queue<Socket>(capacity);
        }

        // Add a Socket instance to the pool. 
        // "theSocket" = Socket instance to add to the pool.
        internal void Enqueue(Socket theSocket)
        {
            if (theSocket == null)
            {
                throw new ArgumentNullException("Items added to a SocketPool cannot be null");
            }
            lock (this.poolOfSockets)
            {
                this.poolOfSockets.Enqueue(theSocket);
            }
        }



        // Removes a Socket instance from the pool.
        // returns Socket removed from the pool.
        internal Socket Dequeue()
        {
            lock (this.poolOfSockets)
            {
                if (this.poolOfSockets.Count > 0)
                {
                    return this.poolOfSockets.Dequeue();
                }
                else
                {
                    return null;
                }
            }
        }

        internal Socket[] ToArray()
        {
            lock (this.poolOfSockets)
            {
                return this.poolOfSockets.ToArray();
            }
        }
    }
}
