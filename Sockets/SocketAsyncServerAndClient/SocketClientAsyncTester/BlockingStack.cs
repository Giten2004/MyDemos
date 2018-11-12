using System;
using System.Collections.Generic;
using System.Threading;

namespace SocketClientAsyncTester
{
    class BlockingStack<T>
    {
        private readonly Stack<T> stack;
        public BlockingStack(Stack<T> theStack)
        {
            this.stack = theStack;
        }

        public void Push(T item)
        {
            lock (stack)
            {
                stack.Push(item);
                if (stack.Count == 1)
                {
                    //This means we have gone from empty stack to stack with 1 item.
                    //So, wake Pop().
                    Monitor.PulseAll(stack);
                }
            }
        }

        public T Pop()
        {
            lock (stack)
            {
                if (stack.Count == 0)
                {
                    //Stack is empty. Wait until Pulse is received from Push().
                    Monitor.Wait(stack);
                }
                T item = stack.Pop();
                return item;
            }
        }
    }
}
