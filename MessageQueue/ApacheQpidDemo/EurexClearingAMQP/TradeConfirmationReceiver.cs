using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using Org.Apache.Qpid.Messaging;
using Org.Apache.Qpid.Messaging.SessionReceiver;

namespace EurexClearingAMQP
{
    public class TradeConfirmationReceiver : ISessionReceiver
    {
        private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Implement methods of interface ISessionReceiver

        public void SessionException(Exception exception)
        {
            _Log.Error("SessionException happend.", exception);
        }

        public void SessionReceiver(Receiver receiver, Message message)
        {
            try
            {
                var tradeConfirmationMsg = message.GetContent();
                _Log.WarnFormat("Received message : {0}", tradeConfirmationMsg);

                var tradeConfirmationDomainModel = TradeConfirmationDM.ParseXML(tradeConfirmationMsg);
                if (tradeConfirmationDomainModel.TrdHandlInst != TradeHandlingInstr.Trade_Confirmation)
                {
                    //the received message is not trade confirmation message, log and ingored it
                    _Log.WarnFormat("The received message is not trade confirmation message, log and ingored it: {0}", tradeConfirmationMsg);
                    //remove the message from the broadcast queue
                    //receiver.Session.Acknowledge();
                }
            }
            catch (Exception ex)
            {
                _Log.Error(string.Format("Process the recived message failed. Message text:{0}, CorreclationID:{1}", message.GetContent(), message.CorrelationId), ex);
            }
        }

        #endregion
    }
}
