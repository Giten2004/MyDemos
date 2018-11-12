using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.Apache.Qpid.Messaging;
using Org.Apache.Qpid.Messaging.SessionReceiver;

namespace ApacheQpidDemo
{
    class Program
    {

        class Listener : ISessionReceiver
        {
            // Callback method to be called when message arrives
            public void SessionReceiver(Receiver receiver, Message message)
            {
                Console.WriteLine("RECEIVED MESSAGE:");
                Console.WriteLine("#################");
                Console.WriteLine("Message text: ", message.GetContent());
                Console.WriteLine("Correlation ID: ", message.CorrelationId);
                receiver.Session.Acknowledge();
            }

            public void SessionException(Exception exception)
            {
                throw new NotImplementedException();
            }
        }

        static void Main(string[] args)
        {

            //RequestResponse();
            //return;

            //[[ The following two types of EnviromentVariable must set by manual, not here.
            //1)
            //System.Environment.SetEnvironmentVariable("QPID_SSL_CERT_STORE", "Personal", EnvironmentVariableTarget.Process);
            //System.Environment.SetEnvironmentVariable("QPID_SSL_CERT_NAME", "abcfr_abcfralmmacc1", EnvironmentVariableTarget.Process);


            //2)
            //System.Environment.SetEnvironmentVariable("QPID_LOG_ENABLE", "trace+", EnvironmentVariableTarget.Process);
            //System.Environment.SetEnvironmentVariable("QPID_SSL_CERT_FILENAME", @".\ABCFR_ABCFRALMMACC1.p12", EnvironmentVariableTarget.Process);
            //System.Environment.SetEnvironmentVariable("QPID_SSL_CERT_PASSWORD_FILE", @".\ABCFR_ABCFRALMMACC1.pwd", EnvironmentVariableTarget.Process);
            //System.Environment.SetEnvironmentVariable("QPID_SSL_CERT_NAME", "abcfr_abcfralmmacc1", EnvironmentVariableTarget.Process);
            //]]

            //string brokerAddr = args.Length > 0 ? args[0] : "amqp:ssl:192.168.34.11:11234";
            //shit, seems must use the host name
            string brokerAddr = args.Length > 0 ? args[0] : "amqp:ssl:chengdudev6:11234";
            string failBrokerAddr = args.Length > 1 ? args[1] : "amqp:ssl:chengdudev6:11234";

            string memberName = "ABCFR_ABCFRALMMACC1";
            string broadcastAddress = "broadcast." + memberName + ".TradeConfirmation; { node: { type: queue }, create: never, mode: consume, assert: always }";

            Connection connection = null;
            Session session;

            try
            {
                /*
                 * Step 1: Preparing the connection and session
                 */
                connection = new Connection(brokerAddr);
                connection.SetOption("reconnect", true);
                connection.SetOption("reconnect_limit", 2);
                connection.SetOption("reconnect_urls", failBrokerAddr);

                //shit, must set the username, a little different with Eurex's Demo code
                //shit, the username is case sensitive
                connection.SetOption("username", "ABCFR_ABCFRALMMACC1");

                connection.SetOption("transport", "ssl");
                connection.SetOption("sasl_mechanisms", "EXTERNAL");

                //connection.SetOption("heartbeat", 120);

                connection.Open();
                session = connection.CreateSession();
                /*
                 * Step 2: Create callback server and implicitly start it
                 */
                CallbackServer cbServer = new CallbackServer(session, new Listener());
                // The callback server is running and executing callbacks on a separate thread.
                /*
                 * Step 3: Creating message consumer
                 */
                Receiver receiver = session.CreateReceiver(broadcastAddress);
                receiver.Capacity = 100;

                Console.ReadLine();

                //System.Threading.Thread.Sleep(20 * 1000);   // in mS
                ///*
                // * Step 4: Stop the callback server and close receiver
                // */
                //cbServer.Close();
                //receiver.Close();
                //session.Sync();
            }
            catch (QpidException ex)
            {
                Console.WriteLine("QpidException caught: {0}", ex.Message);
                Console.WriteLine();
                Console.WriteLine("Press any key to continue!");
                Console.ReadLine();
            }
            finally
            {
                if (connection != null && connection.IsOpen)
                {
                    Console.WriteLine("Closing the connection.");
                    connection.Close();
                }
            }
        }

        private static void SendReceiveHelloWord()
        {

            //or start simpe qpid on windows : 
            //cmd: qpidd --port=60302 --no-data-dir --auth=no

            String broker = "chengdudev6:21234";
            String address = "amq.topic"; // queue "bxu.testBinding" was binded to this exchange
            Connection connection = null;
            try
            {
                connection = new Connection(broker);
                connection.SetOption("username", "admin");
                connection.SetOption("password", "admin");
                connection.Open();

                Session session = connection.CreateSession();
                Receiver receiver = session.CreateReceiver(address);

                Sender sender = session.CreateSender(address);
                sender.Send(new Message("<FIXML>........</FIXML>"));

                Message message = new Message();
                message = receiver.Fetch(DurationConstants.SECOND * 1);

                Console.WriteLine("{0}", message.GetContent());
                session.Acknowledge();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception {0}.", e);
                if (connection != null)
                    connection.Close();
            }
        }

        private static void SendReceiveHelloWord_SSL()
        {

            //or start simpe qpid on windows : 
            //cmd: qpidd --port=60302 --no-data-dir --auth=no

            String broker = "amqp:ssl:chengdudev6:11234";
            String address = "amq.topic"; // queue "bxu.testBinding" was binded to this exchange
            Connection connection = null;
            try
            {
                connection = new Connection(broker);
                connection.SetOption("username", "ABCFR_ABCFRALMMACC1");

                connection.SetOption("transport", "ssl");
                connection.SetOption("sasl_mechanisms", "EXTERNAL");
                connection.Open();

                Session session = connection.CreateSession();
                Receiver receiver = session.CreateReceiver(address);

                Sender sender = session.CreateSender(address);
                sender.Send(new Message("<FIXML>........</FIXML>"));

                Message message = new Message();
                message = receiver.Fetch(DurationConstants.SECOND * 1);

                Console.WriteLine("{0}", message.GetContent());
                session.Acknowledge();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception {0}.", e);
                if (connection != null)
                    connection.Close();
            }
        }

        private static void SendHelloWord_SSL_BR()
        {

            //or start simpe qpid on windows : 
            //cmd: qpidd --port=60302 --no-data-dir --auth=no

            String broker = "amqp:ssl:chengdudev6:11234";
            string memberName = "ABCFR_ABCFRALMMACC1";
            string broadcastAddress = "broadcast." + memberName + ".TradeConfirmation; { node: { type: queue }, create: never, mode: consume, assert: always }";
            Connection connection = null;
            try
            {
                connection = new Connection(broker);
                connection.SetOption("username", "ABCFR_ABCFRALMMACC1");

                connection.SetOption("transport", "ssl");
                connection.SetOption("sasl_mechanisms", "EXTERNAL");
                connection.Open();

                Session session = connection.CreateSession();

                Sender sender = session.CreateSender(broadcastAddress);
                sender.Send(new Message("<FIXML>........</FIXML>"));

                session.Sync();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception {0}.", e);
                if (connection != null)
                    connection.Close();
            }
        }

        private static void SendHelloWord_BR()
        {

            //or start simpe qpid on windows : 
            //cmd: qpidd --port=60302 --no-data-dir --auth=no

            String broker = "chengdudev6:21234";
            string memberName = "ABCFR_ABCFRALMMACC1";
            string broadcastAddress = "broadcast." + memberName + ".TradeConfirmation; { node: { type: queue }, create: never, mode: consume, assert: always }";
            //string broadcastAddress = "request.ABCFR_ABCFRALMMACC1";//"bxu.testBinding";//"amq.direct";//"broadcast";
            Connection connection = null;
            try
            {
                connection = new Connection(broker);
                connection.SetOption("username", "admin");
                connection.SetOption("password", "admin");
                connection.Open();

                Session session = connection.CreateSession();

                Sender sender = session.CreateSender(broadcastAddress);
                sender.Send(new Message("<FIXML>........</FIXML>"));

                session.Sync();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception {0}.", e);
                if (connection != null)
                    connection.Close();
            }
        }

        private static void SendReceive_Order_HelloWord()
        {

            //or start simpe qpid on windows : 
            //cmd: qpidd --port=60302 --no-data-dir --auth=no

            String broker = "chengdudev6:21234";
            String address = "amq.topic"; // queue "bxu.testBinding" was binded to this exchange
            Connection connection = null;
            try
            {
                connection = new Connection(broker);
                connection.SetOption("username", "admin");
                connection.SetOption("password", "admin");
                connection.Open();

                Session session = connection.CreateSession();
                Sender sender = session.CreateSender(address);
                sender.Send(new Message("<FIXML>........</FIXML>"));

                Receiver receiver = session.CreateReceiver(address);
                Message message = new Message();
                message = receiver.Fetch(DurationConstants.SECOND * 1);

                Console.WriteLine("{0}", message.GetContent());
                session.Acknowledge();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception {0}.", e);
                if (connection != null)
                    connection.Close();
            }
        }

        private static void SendReceiveBindingHelloWord()
        {

            //or start simpe qpid on windows : 
            //cmd: qpidd --port=60302 --no-data-dir --auth=no

            String broker = "chengdudev6:21234";
            String senderAddress = "amq.topic"; // queue "bxu.testBinding" was binded to this exchange
            string reseiverAddress = "bxu.testBinding";
            Connection connection = null;
            try
            {
                connection = new Connection(broker);
                connection.SetOption("username", "admin");
                connection.SetOption("password", "admin");
                connection.Open();

                Session session = connection.CreateSession();
                Receiver receiver = session.CreateReceiver(reseiverAddress);

                Sender sender = session.CreateSender(senderAddress);
                sender.Send(new Message("<FIXML>........</FIXML>"));

                Message message = new Message();
                message = receiver.Fetch(DurationConstants.SECOND * 1);

                Console.WriteLine("{0}", message.GetContent());
                session.Acknowledge();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception {0}.", e);
                if (connection != null)
                    connection.Close();
            }
        }

        private static void SendHelloWorld()
        {
            String broker = "chengdudev6:21234";
            String address = "amq.topic";
            Connection connection = null;
            try
            {
                connection = new Connection(broker);
                connection.SetOption("username", "admin");
                connection.SetOption("password", "admin");
                connection.Open();

                Session session = connection.CreateSession();
                Sender sender = session.CreateSender(address);
                var msg = new Message("<FIXML>........</FIXML>");
                msg.Subject = "bxu.testBinding";
                sender.Send(msg);

                //When sending the messages asynchronously, the session should be synchronized after every
                //few messages in order to make sure that the requests which were sent asynchronously were
                //delivered to the broker. 
                session.Sync();

                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception {0}.", e);
                if (connection != null)
                    connection.Close();
            }
        }

        private static void ReceiveHelloWorld()
        {
            String broker = "chengdudev6:21234";
            string reseiverAddress = "bxu.testBinding"; //bxu.testBinging was binded to exchange "amq.topic"
            Connection connection = null;
            try
            {
                connection = new Connection(broker);
                connection.SetOption("username", "admin");
                connection.SetOption("password", "admin");
                connection.Open();

                Session session = connection.CreateSession();
                Receiver receiver = session.CreateReceiver(reseiverAddress);

                Message message = new Message();
                message = receiver.Fetch(DurationConstants.SECOND * 1);

                Console.WriteLine("{0}", message.GetContent());

                //The message should be acknowledged after its processing is finished. 
                session.Acknowledge();

                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception {0}.", e);
                if (connection != null)
                    connection.Close();
            }
        }

        private static void Send()
        {
            string brokerAddr = "amqp:ssl:chengdudev6:11234";
            string failBrokerAddr = "amqp:ssl:chengdudev6:11234";

            string memberName = "ABCFR_ABCFRALMMACC1";
            string requestAddress = "request." + memberName + "; { node: { type: topic }, create: never }";
            string replyAddress = "response/response." + memberName + ".response_queue_1; { create: receiver, node: { type: topic } }";

            Connection connection = null;
            Session session;

            try
            {
                /*
                 * Step 1: Preparing the connection and session
                 */
                connection = new Connection(brokerAddr);
                connection.SetOption("reconnect", true);
                connection.SetOption("reconnect_limit", 1);
                connection.SetOption("reconnect_urls", failBrokerAddr);

                connection.SetOption("username", "ABCFR_ABCFRALMMACC1");

                connection.SetOption("transport", "ssl");
                connection.SetOption("sasl_mechanisms", "EXTERNAL");
                connection.Open();
                session = connection.CreateSession();
                /*
                 * Step 2: Creating message producer
                 */
                Sender sender = session.CreateSender(requestAddress);
                /*
                 * Step 3: Sending a message
                 */
                Message requestMsg = new Message("<FIXML>...</FIXML>");
                Address ra = new Address(replyAddress);
                requestMsg.ReplyTo = ra;
                sender.Send(requestMsg);
                Console.WriteLine("Request sent: {0}", requestMsg.GetContent());

                session.Sync();
                connection.Close();
            }
            catch (QpidException ex)
            {
                Console.WriteLine("QpidException caught: {0}", ex.Message);
            }
            finally
            {
                if (connection != null && connection.IsOpen)
                {
                    Console.WriteLine("Closing the connection.");
                    connection.Close();
                }
            }
        }

        private static void Reciever()
        {
            string brokerAddr = "amqp:ssl:chengdudev6:11234";
            string failBrokerAddr = "amqp:ssl:chengdudev6:11234";

            string memberName = "ABCFR_ABCFRALMMACC1";
            string responseAddress = "response." + memberName + ".response_queue_1; {create: receiver, assert: never," +
            "node: { type: queue, x-declare: { auto-delete: true, exclusive: false, arguments: {'qpid.policy_type': ring," +
            "'qpid.max_count': 1000, 'qpid.max_size': 1000000}}, x-bindings: [{exchange: 'response', queue: 'response." +
            memberName + ".response_queue_1', key: 'response." + memberName + ".response_queue_1'}]}}";

            Connection connection = null;
            Session session;

            try
            {
                /*
                 * Step 1: Preparing the connection and session
                 */
                connection = new Connection(brokerAddr);
                connection.SetOption("reconnect", true);
                connection.SetOption("reconnect_limit", 2);
                connection.SetOption("reconnect_urls", failBrokerAddr);

                connection.SetOption("username", "ABCFR_ABCFRALMMACC1");

                connection.SetOption("transport", "ssl");
                connection.SetOption("sasl_mechanisms", "EXTERNAL");
                connection.Open();
                session = connection.CreateSession();
                /*
                 * Step 2: Creating message consumer
                 */
                Receiver receiver = session.CreateReceiver(responseAddress);
                /*
                 * Step 3: Receiving a message
                 */
                Message msg = receiver.Fetch(DurationConstants.SECOND * 10);
                Console.WriteLine("RECEIVED MESSAGE:");
                Console.WriteLine("#################");
                Console.WriteLine(msg.GetContent());

                session.Acknowledge();
                session.Sync();
                connection.Close();
            }
            catch (QpidException ex)
            {
                Console.WriteLine("QpidException caught: {0}", ex.Message);
            }
            finally
            {
                if (connection != null && connection.IsOpen)
                {
                    Console.WriteLine("Closing the connection.");
                    connection.Close();
                }
            }
        }

        private static void RequestResponse()
        {
            string brokerAddr = "amqp:ssl:chengdudev6:11234";
            string failBrokerAddr = "amqp:ssl:chengdudev6:11234";

            string memberName = "ABCFR_ABCFRALMMACC1";
            string requestAddress = "request." + memberName + "; { node: { type: topic }, create: never }";
            string replyAddress = "response/response." + memberName + ".response_queue_1; { create: receiver, node: { type: topic } }";

            string responseAddress = "response." + memberName + ".response_queue_1; {create: receiver, assert: never," +
            "node: { type: queue, x-declare: { auto-delete: true, exclusive: false, arguments: {'qpid.policy_type': ring," +
            "'qpid.max_count': 1000, 'qpid.max_size': 1000000}}, x-bindings: [{exchange: 'response', queue: 'response." +
            memberName + ".response_queue_1', key: 'response." + memberName + ".response_queue_1'}]}}";

            Connection connection = null;
            Session session;

            try
            {
                /*
                 * Step 1: Preparing the connection and session
                 */
                connection = new Connection(brokerAddr);
                connection.SetOption("reconnect", true);
                connection.SetOption("reconnect_limit", 1);
                connection.SetOption("reconnect_urls", failBrokerAddr);

                connection.SetOption("username", "ABCFR_ABCFRALMMACC1");

                connection.SetOption("transport", "ssl");
                connection.SetOption("sasl_mechanisms", "EXTERNAL");
                connection.Open();
                session = connection.CreateSession();
                /*
                 * Step 2: Creating message consumer
                 */
                Receiver receiver = session.CreateReceiver(responseAddress);
                /*
                 * Step 2: Creating message producer
                 */
                Sender sender = session.CreateSender(requestAddress);
                /*
                 * Step 3: Sending a message
                 */
                Message requestMsg = new Message("<FIXML>...</FIXML>");
                Address ra = new Address(replyAddress);
                requestMsg.ReplyTo = ra;
                sender.Send(requestMsg);

                Console.WriteLine("Request sent: {0}", requestMsg.GetContent());

                session.Sync();


                /*
                 * Step 3: Receiving a message
                 */
                Message msg = receiver.Fetch(DurationConstants.SECOND * 10);
                Console.WriteLine("RECEIVED MESSAGE:");
                Console.WriteLine("#################");
                Console.WriteLine(msg.GetContent());

                session.Acknowledge();

                connection.Close();
            }
            catch (QpidException ex)
            {
                Console.WriteLine("QpidException caught: {0}", ex.Message);
            }
            finally
            {
                if (connection != null && connection.IsOpen)
                {
                    Console.WriteLine("Closing the connection.");
                    connection.Close();
                }
            }
        }
    }
}
