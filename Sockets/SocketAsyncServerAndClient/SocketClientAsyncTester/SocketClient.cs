using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketClientAsyncTester
{
    internal sealed class SocketClient
    {
        //__variables for testing ____________________________________________        
        internal Int32 numberOfConnectionsFinishedSuccessfully = 0;
        internal Int32 totalNumberOfConnectionRetries = 0;
        internal Int32 totalNumberOfConnectionsFinished = 0;        
        internal Int32 maxSimultaneousClientsThatWereConnected = 0;
        private object lockerForConnectionCount = new object();
        internal bool finished = false;
        internal Int64 totalCountOfMessagesSent = 0;        
        internal bool completedAllTests = false;
        internal bool abortTest = false;
        internal long startTime;
        internal long stopTime;
        internal Int32 clientsNowConnectedCount = 0;
        TimeSpan connectDelayTimeSpan = new TimeSpan(Program.tickDelayBeforeNextConn);
        OutgoingMessageHolder outgoingMessageHolderForLongTest;
        Semaphore counterForLongTest;
        
        
        //__variables for real app____________________________________________
        
        //Create a large reusable set of buffers for all socket operations.
        BufferManager bufferManager;
        
        // allows us to set the maximum number of client connections (that is, 
        // ports/sockets to open simultaneously.        
        Semaphore theMaxConnectionsEnforcer;

        private SocketClientSettings socketClientSettings;
        private BlockingStack<OutgoingMessageHolder> stackOfOutgoingMessages;
        
        PrefixHandler prefixHandler;
        MessageHandler messageHandler;

        // Pool of reusable SocketAsyncEventArgs objects
        SocketAsyncEventArgsPool poolOfConnectEventArgs;
        // pool of reusable SocketAsyncEventArgs objects for receive and send socket operations
        SocketAsyncEventArgsPool poolOfRecSendEventArgs;
        

        MessagePreparer messagePreparer;

        //__END variables for real app____________________________________________

       

        //____________________________________________________________________________
        // Create uninitialized SocketClient instance.  

        public SocketClient(SocketClientSettings theSocketClientSettings)
        {
            if (Program.watchProgramFlow == true)   //for testing
            {
                Program.testWriter.WriteLine("SocketClient constructor");
            }
            this.socketClientSettings = theSocketClientSettings;
            this.prefixHandler = new PrefixHandler();
            this.messageHandler = new MessageHandler();                     
            this.messagePreparer = new MessagePreparer();
            this.bufferManager = new BufferManager(this.socketClientSettings.BufferSize * this.socketClientSettings.NumberOfSaeaForRecSend * this.socketClientSettings.OpsToPreAllocate, this.socketClientSettings.BufferSize * this.socketClientSettings.OpsToPreAllocate);
            this.poolOfRecSendEventArgs = new SocketAsyncEventArgsPool(this.socketClientSettings.NumberOfSaeaForRecSend);
            this.poolOfConnectEventArgs = new SocketAsyncEventArgsPool(this.socketClientSettings.MaxConnectOps);
            
            this.theMaxConnectionsEnforcer = new Semaphore(this.socketClientSettings.MaxConnections, this.socketClientSettings.MaxConnections);
            this.counterForLongTest = new Semaphore(this.socketClientSettings.ConnectionsToRun, this.socketClientSettings.ConnectionsToRun);
            Init();
        }

        //____________________________________________________________________________
        // Initializes the client by preallocating reusable buffers and 
        // context objects (SocketAsyncEventArgs objects).  
        // It is NOT mandatory that you preallocate them or reuse them. 
        // In fact, you would probably NOT normally do this on a client.
        // But, for testing we want to be able to throw a lot of connections at
        // our socket server from a LAN. And this is a way to do it.
        //
        private void Init()
        {
            if (Program.watchProgramFlow == true)   //for testing
            {
                Program.testWriter.WriteLine("Init method");
                Program.testWriter.WriteLine("Creating connect SocketAsyncEventArgs pool");
                
            }

            SocketAsyncEventArgs connectEventArg;
            // preallocate pool of SocketAsyncEventArgs objects for connect operations
            for (int i = 0; i < this.socketClientSettings.MaxConnectOps; i++)
            {
                connectEventArg = CreateNewSaeaForConnect(poolOfConnectEventArgs);

                // add SocketAsyncEventArg to the pool
                this.poolOfConnectEventArgs.Push(connectEventArg);
            }

            if (Program.watchProgramFlow == true)   //for testing
            {
                Program.testWriter.WriteLine("");
            }

            // Allocate one large byte buffer block, which all I/O operations will use a piece of.
            //This gaurds against memory fragmentation.
            this.bufferManager.InitBuffer();


            //The pool that we built ABOVE is for SocketAsyncEventArgs objects that do
            // connect operations. Now we will build a separate pool for 
            //SocketAsyncEventArgs objects that do receive/send operations. 
            //One reason to separate them is that connect
            //operations do NOT need a buffer, but receive/send operations do. 
            //You can see that is true by looking at the
            //methods in the .NET Socket class on the Microsoft website. 
            //ReceiveAsync and SendAsync take
            //a parameter for buffer size in SocketAsyncEventArgs.Buffer.
            
            //Preallocate pool of SocketAsyncEventArgs for receive/send operations.
            if (Program.watchProgramFlow == true)   //for testing
            {
                Program.testWriter.WriteLine("Creating receive/send SocketAsyncEventArgs pool");
            }

            SocketAsyncEventArgs eventArgObjectForPool;
            for (int i = 0; i < this.socketClientSettings.NumberOfSaeaForRecSend; i++)
            {

                //If you have different needs for the send versus the receive sockets, then
                //you might want to allocate a separate pool of SocketAsyncEventArgs for send
                //operations instead of having SocketAsyncEventArgs that do both receiving 
                //and sending operations.

                //Allocate the SocketAsyncEventArgs object.
                eventArgObjectForPool = new SocketAsyncEventArgs();

                // assign a byte buffer from the buffer block to 
                //this particular SocketAsyncEventArg object
                this.bufferManager.SetBuffer(eventArgObjectForPool);
                //Since we assigned a buffer like that, you can NOT just add more of
                //these send/receive SAEA objects if you run out of them.

                //Attach the receive/send-operation-SocketAsyncEventArgs object 
                //to its event handler. Since this SocketAsyncEventArgs object is used 
                //for both receive and send operations, whenever either of those 
                //completes, the IO_Completed method will be called.
                eventArgObjectForPool.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                
                DataHoldingUserToken receiveSendToken = new DataHoldingUserToken(eventArgObjectForPool.Offset, eventArgObjectForPool.Offset + this.socketClientSettings.BufferSize, this.socketClientSettings.ReceivePrefixLength, this.socketClientSettings.SendPrefixLength, (this.poolOfRecSendEventArgs.AssignTokenId() + 1000000));

                //Create an object that we can write data to, and remove as an object
                //from the UserToken, if we wish.
                receiveSendToken.CreateNewDataHolder();

                eventArgObjectForPool.UserToken = receiveSendToken;
                // add this SocketAsyncEventArg object to the pool.
                this.poolOfRecSendEventArgs.Push(eventArgObjectForPool);
            }
            Console.WriteLine("Object pools built.");
        }

        //____________________________________________________________________________
        // This method is called when we need to create a new SAEA object to do
        //connect operations. The reason to put it in a separate method is so that
        //we can easily add more objects to the pool if we need to.
        //You can do that if you do NOT use a buffer in the SAEA object that does
        //the connect operations.
        private SocketAsyncEventArgs CreateNewSaeaForConnect(SocketAsyncEventArgsPool pool)
        {
            //Allocate the SocketAsyncEventArgs object. 
            SocketAsyncEventArgs connectEventArg = new SocketAsyncEventArgs();            

            //Attach the event handler.  Since we'll be using this 
            //SocketAsyncEventArgs object to process connect ops,
            //what this does is cause the calling of the ConnectEventArg_Completed
            //object when the connect op completes.
            connectEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);

            ConnectOpUserToken theConnectingToken = new ConnectOpUserToken(pool.AssignTokenId() + 10000);
            connectEventArg.UserToken = theConnectingToken;

            return connectEventArg;

            //You may wonder about the buffer of this object. If you
            //decide to use objects from one pool to do connect operations, and
            //a separate pool of SAEA objects to do send/receive, then
            //there is NO NEED to assign a buffer to this SAEA object for connects.
            //But, if you want to 
            //use this SAEA object to do connect, send, receive, and disconnect,
            //then you will need a buffer for this object.
            //Working with the buffer is different in the client vs the server, due
            //to the way that ConnectAsync works.
            //You would need to think about whether to do the initial call of
            //BufferManager.SetBuffer here, or do it in ProcessConnect method.            
            //If a SocketAsyncEventArg object has a buffer already set before
            //the ConnectAsync method is called, then the contents of the buffer will 
            //be sent immediately upon connecting, without your calling StartSend().
            //Read that sentence again.
            //So you could only call BufferManager.SetBuffer here if you are sure the
            //client will always do a send operation first, and the data will be
            //ready when the connection is made. If you want to have the
            //option of doing a receive operation first, then wait and set the buffer
            //after the connect operation completes. 
            //If you decide to use that design, then you will need to call the 
            //BufferManager's FreeBuffer method, to
            //null the buffer again before putting it back in the pool. FreeBuffer would
            //probably need to be called in the ProcessDisconnectAndCloseSocket method. 
        }

        //____________________________________________________________________________
        // This method is called when an operation is completed on a socket 
        //
        private void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            // determine which type of operation just completed and call the associated handler
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    if (Program.watchProgramFlow == true)   //for testing
                    {
                        ConnectOpUserToken theConnectingToken = (ConnectOpUserToken)e.UserToken;
                        Program.testWriter.WriteLine("\r\nIO_Completed method In Connect, connect id = " + theConnectingToken.TokenId);
                    }

                    ProcessConnect(e);
                    break;

                case SocketAsyncOperation.Receive:
                    if (Program.watchProgramFlow == true)   //for testing
                    {
                        DataHoldingUserToken receiveSendToken = (DataHoldingUserToken)e.UserToken;
                        Program.testWriter.WriteLine("\r\nIO_Completed method In Receive, id = " + receiveSendToken.TokenId);
                    }

                    ProcessReceive(e);
                    break;

                case SocketAsyncOperation.Send:
                    if (Program.watchProgramFlow == true)   //for testing
                    {
                        DataHoldingUserToken receiveSendToken = (DataHoldingUserToken)e.UserToken;
                        Program.testWriter.WriteLine("\r\nIO_Completed method In Send, id = " + receiveSendToken.TokenId);
                    }

                    ProcessSend(e);
                    break;

                case SocketAsyncOperation.Disconnect:
                    if (Program.watchProgramFlow == true)   //for testing
                    {
                        DataHoldingUserToken receiveSendToken = (DataHoldingUserToken)e.UserToken;
                        Program.testWriter.WriteLine("\r\nIO_Completed method In Disconnect, id = " + receiveSendToken.TokenId);
                    }
                    ProcessDisconnectAndCloseSocket(e);                    
                    break;


                default:
                    {
                        DataHoldingUserToken receiveSendToken = (DataHoldingUserToken)e.UserToken;
                        if (Program.watchProgramFlow == true)   //for testing
                        {
                            Program.testWriter.WriteLine("\r\nError in I/O Completed, id = " + receiveSendToken.TokenId);
                        }

                        throw new ArgumentException("\r\nError in I/O Completed, id = " + receiveSendToken.TokenId);                       
                    }                    
            }
        }

        //____________________________________________________________________________
        // 
        internal void GetMessages(Stack<OutgoingMessageHolder> theStackOfOutgoingMessages)
        {
            if (Program.watchProgramFlow == true)   //for testing
            {
                Program.testWriter.WriteLine("GetMessages");
            }
            this.stackOfOutgoingMessages = new BlockingStack<OutgoingMessageHolder>(theStackOfOutgoingMessages);

            //In this case the stack contains only one OutgoingMessageHolder which will be reused
            //by the CheckStack method.
            if (Program.runLongTest == true)
            {
                this.outgoingMessageHolderForLongTest = this.stackOfOutgoingMessages.Pop();
            }
            
            this.startTime = DateTime.Now.Ticks;
            Thread t = new Thread(CheckStack);
            t.Start();
        }

        //____________________________________________________________________________
        //
        private void CheckStack()
        {
            if (Program.watchProgramFlow == true)   //for testing
            {
                Program.testWriter.WriteLine("CheckStack");
            }

            OutgoingMessageHolder outgoingMessageHolder;
            if (Program.runLongTest == true)
            {
                this.counterForLongTest.WaitOne();
                //When runLongTest == true we reuse the same outgoingMessageHolder
                outgoingMessageHolder = this.outgoingMessageHolderForLongTest;                
            }
            else
            {
                outgoingMessageHolder = this.stackOfOutgoingMessages.Pop();
            }
            
            //only relevant when the test has finished.
            if (outgoingMessageHolder == null)
                return;
            
            if (Program.useDelayBetweenConnections == true)
            {
                Thread.Sleep(connectDelayTimeSpan);
            }

            this.theMaxConnectionsEnforcer.WaitOne();
            
            //We got outgoingMessageHolder. Pass it along.
            PushMessageArray(outgoingMessageHolder);            
        }

        //____________________________________________________________________________
        //
        private void PushMessageArray(OutgoingMessageHolder theOutgoingMessageHolder)
        {
            SocketAsyncEventArgs connectEventArgs;

            //Get a SocketAsyncEventArgs object to connect with.
            
            //Get it from the pool if there is more than one.
            connectEventArgs = this.poolOfConnectEventArgs.Pop();
            //or make a new one.            
            if (connectEventArgs == null)
                {
                    connectEventArgs = CreateNewSaeaForConnect(poolOfConnectEventArgs);
                }

            ConnectOpUserToken theConnectingToken = (ConnectOpUserToken)connectEventArgs.UserToken;

            if (Program.watchProgramFlow == true)   //for testing
            {
                Program.testWriter.WriteLine("PushMessageArray, connect id = " + theConnectingToken.TokenId);
            }

            theConnectingToken.outgoingMessageHolder = theOutgoingMessageHolder;

            StartConnect(connectEventArgs);
            //Loop back to get message(s) for next connection.           
            CheckStack();

            
        }


        //____________________________________________________________________________
        // Connect to the host.        
        // 
        private void StartConnect(SocketAsyncEventArgs connectEventArgs)        
        {
            //Cast SocketAsyncEventArgs.UserToken to our state object.
            ConnectOpUserToken theConnectingToken = (ConnectOpUserToken)connectEventArgs.UserToken;
            if (Program.watchProgramFlow == true)   //for testing
            {                
                Program.testWriter.WriteLine("StartConnect, connect id = " + theConnectingToken.TokenId);
            }

            //SocketAsyncEventArgs object that do connect operations on the client
            //are different from those that do accept operations on the server.
            //On the server the listen socket had EndPoint info. And that info was
            //passed from the listen socket to the SocketAsyncEventArgs object 
            //that did the accept operation.
            //But on the client there is no listen socket. The connect socket 
            //needs the info on the Remote Endpoint.
            connectEventArgs.RemoteEndPoint = this.socketClientSettings.ServerEndPoint;

            connectEventArgs.AcceptSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Post the connect operation on the socket.
            //A local port is assigned by the Windows OS during connect op.            
            bool willRaiseEvent = connectEventArgs.AcceptSocket.ConnectAsync(connectEventArgs);
            if (!willRaiseEvent)
            {
                if (Program.watchProgramFlow == true)   //for testing
                {                    
                    Program.testWriter.WriteLine("StartConnect method if (!willRaiseEvent), id = " + theConnectingToken.TokenId);
                }

                ProcessConnect(connectEventArgs);
            }
        }

        //____________________________________________________________________________
        // Pass the connection info from the connecting object to the object
        // that will do send/receive. And put the connecting object back in the pool.
        //
        private void ProcessConnect(SocketAsyncEventArgs connectEventArgs)
        {
            ConnectOpUserToken theConnectingToken = (ConnectOpUserToken)connectEventArgs.UserToken;

            if (connectEventArgs.SocketError == SocketError.Success)
            {
                lock (this.lockerForConnectionCount)
                {
                    this.clientsNowConnectedCount++;
                    if (this.clientsNowConnectedCount > this.maxSimultaneousClientsThatWereConnected)
                    {
                        this.maxSimultaneousClientsThatWereConnected++;
                    }
                }

                SocketAsyncEventArgs receiveSendEventArgs = this.poolOfRecSendEventArgs.Pop();
                receiveSendEventArgs.AcceptSocket = connectEventArgs.AcceptSocket;

                //Earlier, in the UserToken of connectEventArgs we put an array 
                //of messages to send. Now we move that array to the DataHolder in
                //the UserToken of receiveSendEventArgs.
                DataHoldingUserToken receiveSendToken = (DataHoldingUserToken)receiveSendEventArgs.UserToken;
                receiveSendToken.theDataHolder.PutMessagesToSend(theConnectingToken.outgoingMessageHolder.arrayOfMessages);

                if (Program.showConnectAndDisconnect == true)
                {
                    Program.testWriter.WriteLine("ProcessConnect connect id " + theConnectingToken.TokenId + " socket info now passing to\r\n   sendReceive id " + receiveSendToken.TokenId + ", local endpoint = " + IPAddress.Parse(((IPEndPoint)connectEventArgs.AcceptSocket.LocalEndPoint).Address.ToString()) + ": " + ((IPEndPoint)connectEventArgs.AcceptSocket.LocalEndPoint).Port.ToString() + ". Clients connected to server from this machine = " + this.clientsNowConnectedCount);
                }
                
                messagePreparer.GetDataToSend(receiveSendEventArgs);
                StartSend(receiveSendEventArgs);

                //release connectEventArgs object back to the pool.
                connectEventArgs.AcceptSocket = null;
                this.poolOfConnectEventArgs.Push(connectEventArgs);

                if (Program.watchProgramFlow == true)   //for testing
                {
                    Program.testWriter.WriteLine("back to pool for connection object " + theConnectingToken.TokenId);
                }
            }

                //This else statement is when there was a socket error
            else
            {
                ProcessConnectionError(connectEventArgs);
            }
        }

        //____________________________________________________________________________
        internal void ProcessConnectionError(SocketAsyncEventArgs connectEventArgs)
        {
            ConnectOpUserToken theConnectingToken = (ConnectOpUserToken)connectEventArgs.UserToken;

            if (Program.watchProgramFlow == true)   //for testing
            {
                Program.testWriter.WriteLine("ProcessConnectionError() id = " + theConnectingToken.TokenId + ". ERROR: " + connectEventArgs.SocketError.ToString());
            }
            else if (Program.writeErrorsToLog == true)
            {
                Program.testWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + " ProcessConnectionError() id = " + theConnectingToken.TokenId + ". ERROR: " + connectEventArgs.SocketError.ToString());
            }

            Interlocked.Increment(ref this.totalNumberOfConnectionRetries);
            
            // If connection was refused by server or timed out or not reachable, then we'll keep this socket.
            // If not, then we'll destroy it.
            if ((connectEventArgs.SocketError != SocketError.ConnectionRefused) && (connectEventArgs.SocketError != SocketError.TimedOut)  && (connectEventArgs.SocketError != SocketError.HostUnreachable))
            {
                CloseSocket(connectEventArgs.AcceptSocket);
            }            
            
            if (Program.continuallyRetryConnectIfSocketError == true)
            {
                // Since we did not send the messages, let's put them back in the stack.
                // We cannot leave them in the SAEA for connect ops, because the SAEA 
                // could get pushed down in the stack and not reached.

                // If runLongTest == true, we reuse the same array of messages. So in that case
                // we do NOT need to put the array back in the BlockingStack.
                // But if runLongTest == false, we need to put the array of messages back in 
                // the blocking stack, so that it will be taken out and sent.
                if (Program.runLongTest == true)
                {
                    this.counterForLongTest.Release();
                }
                else
                {
                    this.stackOfOutgoingMessages.Push(theConnectingToken.outgoingMessageHolder);
                    this.poolOfConnectEventArgs.Push(connectEventArgs);                    
                }                
            }
            else
            {     
                //it is time to release connectEventArgs object back to the pool.
                this.poolOfConnectEventArgs.Push(connectEventArgs);              

                if (Program.watchProgramFlow == true)   //for testing
                {
                    Program.testWriter.WriteLine("back to pool for socket and SAEA " + theConnectingToken.TokenId);
                }

                Interlocked.Increment(ref this.totalNumberOfConnectionsFinished);                
                //If we are not retrying the failed connections, then we need to
                //account for them here, when deciding whether we have finished
                //the test.
                if (this.totalNumberOfConnectionsFinished == this.socketClientSettings.ConnectionsToRun)
                {

                    FinishTest();
                }
            }
            this.theMaxConnectionsEnforcer.Release();
        }
                

        //____________________________________________________________________________
        //set the send buffer and post a send op
        private void StartSend(SocketAsyncEventArgs receiveSendEventArgs)
        {
            if (abortTest == true)
            {
                FinishTest();
                return;
            }

            DataHoldingUserToken receiveSendToken = (DataHoldingUserToken)receiveSendEventArgs.UserToken;            
            if (Program.watchProgramFlow == true)   //for testing
            {
                Program.testWriter.WriteLine("\r\nStartSend, id = " + receiveSendToken.TokenId);
            }

            if (receiveSendToken.sendBytesRemaining <= this.socketClientSettings.BufferSize)
            {
                receiveSendEventArgs.SetBuffer(receiveSendToken.bufferOffsetSend, receiveSendToken.sendBytesRemaining);
                //Copy the bytes to the buffer associated with this SAEA object.
                Buffer.BlockCopy(receiveSendToken.dataToSend, receiveSendToken.bytesSentAlready, receiveSendEventArgs.Buffer, receiveSendToken.bufferOffsetSend, receiveSendToken.sendBytesRemaining);
            }
            else
            {
                //We cannot try to set the buffer any larger than its size.
                //So since receiveSendToken.sendBytesRemaining > its size, we just
                //set it to the maximum size, to send the most data possible.
                receiveSendEventArgs.SetBuffer(receiveSendToken.bufferOffsetSend, this.socketClientSettings.BufferSize);
                //Copy the bytes to the buffer associated with this SAEA object.
                Buffer.BlockCopy(receiveSendToken.dataToSend, receiveSendToken.bytesSentAlready, receiveSendEventArgs.Buffer, receiveSendToken.bufferOffsetSend, this.socketClientSettings.BufferSize);

                //We'll change the value of sendUserToken.sendBytesRemaining
                //in the ProcessSend method.
            }
            
            //post the send
            bool willRaiseEvent = receiveSendEventArgs.AcceptSocket.SendAsync(receiveSendEventArgs);
            if (!willRaiseEvent)
            {
                Program.testWriter.WriteLine(" StartSend in if (!willRaiseEvent), id = " + receiveSendToken.TokenId);
                ProcessSend(receiveSendEventArgs);
            }
        }


        //____________________________________________________________________________
        private void ProcessSend(SocketAsyncEventArgs receiveSendEventArgs)
        {
            DataHoldingUserToken receiveSendToken = (DataHoldingUserToken)receiveSendEventArgs.UserToken;

            if (Program.watchProgramFlow == true)   //for testing
            {
                Program.testWriter.WriteLine("\r\nProcessSend, id = " + receiveSendToken.TokenId);
            }

            if (receiveSendEventArgs.SocketError == SocketError.Success)
            {
                receiveSendToken.sendBytesRemaining = receiveSendToken.sendBytesRemaining - receiveSendEventArgs.BytesTransferred;
                // If this if statement is true, then we have sent all of the
                // bytes in the message. Otherwise, at least one more send
                // operation will be required to send the data.
                if (receiveSendToken.sendBytesRemaining == 0)
                {
                    //increment total number of messages sent in this test
                    Interlocked.Increment(ref this.totalCountOfMessagesSent);

                    //incrementing count of messages sent on this connection                
                    receiveSendToken.theDataHolder.NumberOfMessagesSent++;
                    StartReceive(receiveSendEventArgs);                    
                }
                else
                {
                    // So since (receiveSendToken.sendBytesRemaining == 0) is false,
                    // we have more bytes to send for this message. So we need to 
                    // call StartSend, so we can post another send message.
                    receiveSendToken.bytesSentAlready += receiveSendEventArgs.BytesTransferred;
                    StartSend(receiveSendEventArgs);
                }
            }
            else
            {
                //If we are in this else-statement, there was a socket error.
                if (Program.watchProgramFlow == true)   //for testing
                {
                    Program.testWriter.WriteLine("ProcessSend ERROR, id " + receiveSendToken.TokenId + "\r\n");
                }
                else if (Program.writeErrorsToLog == true)
                {
                    Program.testWriter.WriteLine("ProcessSend ERROR, id " + receiveSendToken.TokenId);
                }

                // We'll just close the socket if there was a
                // socket error when receiving data from the client.
                receiveSendToken.Reset();
                StartDisconnect(receiveSendEventArgs);
            }            
        }         


        //____________________________________________________________________________
        // Set the receive buffer and post a receive op.
        private void StartReceive(SocketAsyncEventArgs receiveSendEventArgs)
        {
            DataHoldingUserToken receiveSendToken = (DataHoldingUserToken)receiveSendEventArgs.UserToken;
            //Set buffer for receive.          
            receiveSendEventArgs.SetBuffer(receiveSendToken.bufferOffsetReceive, this.socketClientSettings.BufferSize);

            if (Program.watchProgramFlow == true)   //for testing
            {
                Program.testWriter.WriteLine("\r\nStartReceive, id = " + receiveSendToken.TokenId);               
            }            

            bool willRaiseEvent = receiveSendEventArgs.AcceptSocket.ReceiveAsync(receiveSendEventArgs);
            if (!willRaiseEvent)
            {
                Program.testWriter.WriteLine("StartReceive in if (!willRaiseEvent), id = " + receiveSendToken.TokenId);
                ProcessReceive(receiveSendEventArgs);
            }

        }

        //____________________________________________________________________________
        private void ProcessReceive(SocketAsyncEventArgs receiveSendEventArgs)
        {
            
            DataHoldingUserToken receiveSendToken = (DataHoldingUserToken)receiveSendEventArgs.UserToken;
            // If there was a socket error, close the connection.
            if (receiveSendEventArgs.SocketError != SocketError.Success)
            {
                if (Program.watchProgramFlow == true)   //for testing
                {
                    Program.testWriter.WriteLine("ProcessReceive ERROR " + receiveSendEventArgs.SocketError.ToString() + ", id " + receiveSendToken.TokenId);
                }
                else if (Program.writeErrorsToLog == true)
                {
                    Program.testWriter.WriteLine("ProcessReceive ERROR " + receiveSendEventArgs.SocketError.ToString() + ", id " + receiveSendToken.TokenId);
                }

                receiveSendToken.Reset();
                StartDisconnect(receiveSendEventArgs);
                return;
            }

            //If no data was received, close the connection.
            if (receiveSendEventArgs.BytesTransferred == 0)
            {
                if (Program.watchProgramFlow == true)   //for testing
                {
                    Program.testWriter.WriteLine("ProcessReceive NO DATA, id " + receiveSendToken.TokenId);
                }
                receiveSendToken.Reset();
                StartDisconnect(receiveSendEventArgs);
                return;
            }

            if (Program.watchProgramFlow == true)   //for testing
            {
                Program.testWriter.WriteLine("\r\nProcessReceive, id " + receiveSendToken.TokenId);
            }

            
            Int32 remainingBytesToProcess = receiveSendEventArgs.BytesTransferred;

            if (Program.watchProgramFlow == true)   //for testing
            {
                Program.testWriter.WriteLine("ProcessReceive() if Success, id " + receiveSendToken.TokenId + ". Bytes read this op = " + receiveSendEventArgs.BytesTransferred + ".");
            }
            if (Program.watchData == true)
            {
                //This only gives us a readable string if it is operating on 
                //string data.
                string tempString = Encoding.ASCII.GetString(receiveSendEventArgs.Buffer, receiveSendToken.bufferOffsetReceive, receiveSendEventArgs.BytesTransferred);

                Program.testWriter.WriteLine(receiveSendToken.TokenId + " data received = " + tempString);
            }
            

            // If we have not got all of the prefix then we need to work on it. 
            // receivedPrefixBytesDoneCount tells us how many prefix bytes were
            // processed during previous receive ops which contained data for 
            // this message. (In normal use, usually there will NOT have been any 
            // previous receive ops here. So receivedPrefixBytesDoneCount would be 0.)
            if (receiveSendToken.receivedPrefixBytesDoneCount < this.socketClientSettings.ReceivePrefixLength)
            {
                remainingBytesToProcess = prefixHandler.HandlePrefix(receiveSendEventArgs, receiveSendToken, remainingBytesToProcess);
                                
                if (remainingBytesToProcess == 0)
                {                    
                    // We need to do another receive op, since we do not have
                    // the message yet.
                    StartReceive(receiveSendEventArgs);

                    //Jump out of the method, since there is no more data.
                    return;
                }
            }

            // If we have processed the prefix, we can work on the message now.
            // We'll arrive here when we have received enough bytes to read
            // the first byte after the prefix.
            bool incomingTcpMessageIsReady = messageHandler.HandleMessage(receiveSendEventArgs, receiveSendToken, remainingBytesToProcess);            

            if (incomingTcpMessageIsReady == true)
            {
                //In the design of our SocketClient used for testing the
                //DataHolder can contain data for multiple messages. That is 
                //different from the server design, where we have one DataHolder
                //for one message.

                if (Program.watchData == true)
                {                    
                    string messageString = AssembleMessage(receiveSendToken);
                    Program.testWriter.WriteLine("id " + receiveSendToken.TokenId + ", data = " + messageString);
                }

                //If we have set runLongTest to true, then we will assume that
                //we cannot put the data in memory, because there would be too much
                //data. So we'll just skip writing that data, in that case. We
                //write it when runLongTest == false.
                if (Program.runLongTest == false)
                {
                    //Write to DataHolder.
                    receiveSendToken.theDataHolder.listOfMessagesReceived.Add(receiveSendToken.theDataHolder.dataMessageReceived);
                }

                //null out the byte array, for the next message
                receiveSendToken.theDataHolder.dataMessageReceived = null;

                //Reset the variables in the UserToken, to be ready for the
                //next message that will be received on the socket in this
                //SAEA object.
                receiveSendToken.Reset();
                
                //If we have not sent all the messages, get the next message, and
                //loop back to StartSend.
                if (receiveSendToken.theDataHolder.NumberOfMessagesSent < this.socketClientSettings.NumberOfMessages)
                {
                    //No need to reset the buffer for send here.
                    //It is reset in the StartSend method.
                    messagePreparer.GetDataToSend(receiveSendEventArgs);
                    StartSend(receiveSendEventArgs);
                }
                else
                {
                    //Since we have sent all the messages that we planned to send,
                    //time to disconnect.                    
                    StartDisconnect(receiveSendEventArgs);
                }
            }
            else
            {
                // Since we have NOT gotten enough bytes for the whole message,
                // we need to do another receive op. Reset some variables first.

                // All of the data that we receive in the next receive op will be
                // message. None of it will be prefix. So, we need to move the 
                // receiveSendToken.receiveMessageOffset to the beginning of the 
                // buffer space for this SAEA.
                receiveSendToken.receiveMessageOffset = receiveSendToken.bufferOffsetReceive;

                // Do NOT reset receiveSendToken.receivedPrefixBytesDoneCount here.
                // Just reset recPrefixBytesDoneThisOp.
                receiveSendToken.recPrefixBytesDoneThisOp = 0;                

                StartReceive(receiveSendEventArgs);
            }
        }

        //____________________________________________________________________________
        private string AssembleMessage(DataHoldingUserToken receiveSendToken)
        {
            //The server sent back its receivedTransmissionId value and the message that we sent to it.
            //So the first 4 bytes represent an Int32.
            Int32 transMissionIdOfServer = BitConverter.ToInt32(receiveSendToken.theDataHolder.dataMessageReceived, 0);
            string messageString = transMissionIdOfServer.ToString();
            string moreString = Encoding.ASCII.GetString(receiveSendToken.theDataHolder.dataMessageReceived, 4, receiveSendToken.theDataHolder.dataMessageReceived.Length - 4);
            messageString = messageString + moreString;
            return messageString;
        }
        
        //____________________________________________________________________________
        // Disconnect from the host.        
        private void StartDisconnect(SocketAsyncEventArgs receiveSendEventArgs)
        {
            DataHoldingUserToken receiveSendToken = (DataHoldingUserToken)receiveSendEventArgs.UserToken;
            if (Program.watchProgramFlow == true)
            {
                Program.testWriter.WriteLine("\r\nStartDisconnect(), id = " + receiveSendToken.TokenId);
            }

            receiveSendEventArgs.AcceptSocket.Shutdown(SocketShutdown.Both);
            bool willRaiseEvent = receiveSendEventArgs.AcceptSocket.DisconnectAsync(receiveSendEventArgs);
            if (!willRaiseEvent)
            {
                ProcessDisconnectAndCloseSocket(receiveSendEventArgs);
            }
        }


        //____________________________________________________________________________
        private void ProcessDisconnectAndCloseSocket(SocketAsyncEventArgs receiveSendEventArgs)
        {
            DataHoldingUserToken receiveSendToken = (DataHoldingUserToken)receiveSendEventArgs.UserToken;

            if (Program.watchProgramFlow == true)
            {
                Program.testWriter.WriteLine("\r\nProcessDisconnect(), id = " + receiveSendToken.TokenId);
            }

            if (receiveSendEventArgs.SocketError != SocketError.Success)
            {
                if (Program.watchProgramFlow == true)   //for testing
                {
                    Program.testWriter.WriteLine("ProcessDisconnect ERROR, id " + receiveSendToken.TokenId);
                }
                else if (Program.writeErrorsToLog == true)
                {
                    Program.testWriter.WriteLine("ProcessDisconnect ERROR, id " + receiveSendToken.TokenId);
                }
            }

            if (Program.watchData == true)
            {
                Program.testWriter.WriteLine(ShowData(receiveSendToken));
            }
            
            //This method closes the socket and releases all resources, both
            //managed and unmanaged. It internally calls Dispose.
            receiveSendEventArgs.AcceptSocket.Close();
                       
            //for testing
            Int32 sCount = receiveSendToken.theDataHolder.NumberOfMessagesSent;

            //create an object that we can write data to.
            receiveSendToken.CreateNewDataHolder();

            // It is time to release this SAEA object.
            this.poolOfRecSendEventArgs.Push(receiveSendEventArgs);

            //Count down the number of connected clients as they disconnect.
            Interlocked.Decrement(ref this.clientsNowConnectedCount);

            Interlocked.Increment(ref this.numberOfConnectionsFinishedSuccessfully);
            Interlocked.Increment(ref this.totalNumberOfConnectionsFinished);
            this.theMaxConnectionsEnforcer.Release();

            if (Program.showConnectAndDisconnect == true)
            {
                Program.testWriter.WriteLine(receiveSendToken.TokenId + " id disconnected. " + sCount + " = sent message count. " + this.clientsNowConnectedCount + " client connections to server from this machine. " + this.totalNumberOfConnectionsFinished + " clients finished.\r\n");

            }
            //If all of the clients have finished sending
            //and receiving all of their messages, then we
            //need to finish.
            if (this.totalNumberOfConnectionsFinished == this.socketClientSettings.ConnectionsToRun)
                {                    
                    this.completedAllTests = true;
                    FinishTest();
                }
        }

        //____________________________________________________________________________
        private string ShowData(DataHoldingUserToken receiveSendToken)
        {
            Int32 count = receiveSendToken.theDataHolder.listOfMessagesReceived.Count;
            Int32 lengthOfMessage = 0;

            StringBuilder sb = new StringBuilder();
            sb.Append("id ");
            sb.Append(receiveSendToken.TokenId);
            sb.Append(" received ");
            sb.Append(count);
            sb.Append(" messages:\r\n");
            for (int i = 0; i < count; i++)
            {
                lengthOfMessage = receiveSendToken.theDataHolder.listOfMessagesReceived[i].Length;
                //The server sent back its receivedTransmissionId value.
                //It is Int32, which is 4 bytes.
                Int32 transMissionIdOfServer = BitConverter.ToInt32(receiveSendToken.theDataHolder.listOfMessagesReceived[i], 0);
                sb.Append(transMissionIdOfServer.ToString());
                sb.Append(", ");
                sb.Append(Encoding.ASCII.GetString(receiveSendToken.theDataHolder.listOfMessagesReceived[i], 4, lengthOfMessage - 4));
                sb.Append("\r\n");
            }
            sb.Append("\r\n");
            return sb.ToString();
        }

        //____________________________________________________________________________
        private void FinishTest()
        {
            if (this.finished == false)
            {
                this.finished = true;
                stopTime = DateTime.Now.Ticks;
                //free the blocking stack by pushing null, which will be handled
                //by CheckStack()
                this.stackOfOutgoingMessages.Push(null);

                if (this.abortTest == false)
                {
                    Console.WriteLine("\r\n\r\n*********   " + Program.completedString + "   *********\r\nTo close the program and write log type '" + Program.closeString + "' and press Enter");
                    if (Program.playSoundOnCompletion == true)
                    {
                        PlaySoundOnCompletion();
                    }
                }
            }
        }

        //____________________________________________________________________________
        internal void CleanUpOnExit()
        {
            DisposeAllSaeaObjects();
        }

        //____________________________________________________________________________
        private void DisposeAllSaeaObjects()
        {
            SocketAsyncEventArgs eventArgs;
            while (this.poolOfConnectEventArgs.Count > 0)
            {
                eventArgs = poolOfConnectEventArgs.Pop();
                eventArgs.Dispose();
            }
            while (this.poolOfRecSendEventArgs.Count > 0)
            {
                eventArgs = poolOfRecSendEventArgs.Pop();
                eventArgs.Dispose();
            }
        }

        //____________________________________________________________________________
        private void CloseSocket(Socket theSocket)
        {
            try
            {
                theSocket.Shutdown(SocketShutdown.Both);
            }
            catch
            {
            }
            theSocket.Close();
        }
        
        //____________________________________________________________________________
        //This is just to alert you when a test finishes, so that you don't have to
        //watch it continually.
        private void PlaySoundOnCompletion()
        {            
            Console.Beep(220, 100);
            Console.Beep(247, 100);
            Console.Beep(262, 100);
            Console.Beep(294, 100);
            Console.Beep(330, 100);
            Console.Beep(349, 100);
            Console.Beep(392, 100);
            // Console.Beep may not work on x64. So added next line
            System.Media.SystemSounds.Beep.Play();
        }
    }
}
 


