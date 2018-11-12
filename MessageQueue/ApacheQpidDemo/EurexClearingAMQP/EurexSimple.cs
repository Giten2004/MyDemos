using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.Apache.Qpid.Messaging;
using Org.Apache.Qpid.Messaging.SessionReceiver;

namespace EurexClearingAMQP
{
    public class EurexSimple
    {
        private static readonly log4net.ILog _Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISessionReceiver _sessionReceiver;
        private Connection _connection;
        private Session _session;
        private CallbackServer _callbackServer;
        private Receiver _receiver;

        private string _brokerAddress = "amqp:ssl:chengdudev6:11234";
        private string _failBrokerAddress = "amqp:ssl:chengdudev6:11234";
        private string _memberName = "ABCFR_ABCFRALMMACC1";
        private string _broadcastAddress = "broadcast.ABCFR_ABCFRALMMACC1.TradeConfirmation; { node: { type: queue}, create: never, mode: consume, assert: always}";

        public void ConnectAndReceive()
        {
            try
            {
                _sessionReceiver = new TradeConfirmationReceiver();
                /*
                   * Step 1: Preparing the connection and session
                   */
                _connection = new Connection(_brokerAddress);
                _connection.SetOption("reconnect", true);
                _connection.SetOption("reconnect_limit", 2);
                _connection.SetOption("reconnect_urls", _failBrokerAddress);

                //must set the username, a little different with Eurex's Demo code
                //the username is case sensitive
                _connection.SetOption("username", _memberName);
                _connection.SetOption("transport", "ssl");
                _connection.SetOption("sasl_mechanisms", "EXTERNAL");
                _connection.SetOption("heartbeat", 60);//unit:seconds

                _connection.Open();

                _session = _connection.CreateSession();
                /*
                 * Step 2: Create callback server and implicitly start it
                 */
                // The callback server is running and executing callbacks on a separate thread.
                _callbackServer = new CallbackServer(_session, _sessionReceiver);
                /*
                 * Step 3: Creating message consumer
                 */
                _receiver = _session.CreateReceiver(_broadcastAddress);
                _receiver.Capacity = 100;
            }
            catch (QpidException ex)
            {
                _Log.Error("Make connection to AMQP broker failed!", ex);
                throw;
            }
        }
    }
}
