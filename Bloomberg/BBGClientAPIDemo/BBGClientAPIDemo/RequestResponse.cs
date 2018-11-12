using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Bloomberglp.Blpapi;


namespace BBGClientAPIDemo
{
    public class RequestResponse
    {
        public const string ServiceAddress = "//blp/refdata";

        private string[] _securities;
        private string[] _fields;

        private SessionOptions _sessionOptions;
        private Session _session;

        public RequestResponse(string[] securities, string[] fields)
        {
            _securities = securities;
            _fields = fields;

            _sessionOptions = new SessionOptions();
            _sessionOptions.ServerHost = "127.0.0.1";
            _sessionOptions.ServerPort = 8194;

            _session = new Session(_sessionOptions);
        }

        public void Run()
        {
            if (!_session.Start())
            {
                Console.WriteLine("Could ont start session.");
                return;
            }

            if (!_session.OpenService(ServiceAddress))
            {
                Console.WriteLine("Could not open service...");
                return;
            }

            CorrelationID requestID = new CorrelationID(1);
            Service refDataSvc = _session.GetService(ServiceAddress);
            Request request = refDataSvc.CreateRequest("ReferenceDataRequest");

            foreach (var security in _securities)
            {
                request.GetElement("securities").AppendValue(security);
            }
            foreach (var field in _fields)
            {
                request.GetElement("fields").AppendValue(field);
            }

            _session.SendRequest(request, requestID);

            bool continueToLoop = true;
            while (continueToLoop)
            {
                Event nEvent = _session.NextEvent();
                var eventType = nEvent.Type;

                switch (eventType)
                {
                    case Event.EventType.RESPONSE:
                        continueToLoop = false;
                        HandleResponseEvent(nEvent);
                        break;
                    case Event.EventType.PARTIAL_RESPONSE:
                        HandleResponseEvent(nEvent);
                        break;
                    default:
                        HandleOtherEvent(nEvent);
                        break;
                }
            }
        }

        private void HandleOtherEvent(Event nEvent)
        {
            Console.WriteLine("EventType= {0}", nEvent.Type);

            foreach (Message message in nEvent.GetMessages())
            {
                Console.WriteLine("correlationID={0}", message.CorrelationID);
                Console.WriteLine("MessageType={0}", message.MessageType);

                Console.Write(message.ToString());

                if (nEvent.Type == Event.EventType.SESSION_STATUS &&
                    message.MessageType.ToString() == "SessionTerminated")
                {
                    Console.WriteLine("SessionTerminated: {0}", message.MessageType);
                    return;
                }

                Console.WriteLine();
            }
        }

        private void HandleResponseEvent(Event nEvent)
        {
            Console.WriteLine("EventType= {0}", nEvent.Type);

            foreach (Message message in nEvent.GetMessages())
            {
                Console.WriteLine("correlationID={0}", message.CorrelationID);
                Console.WriteLine("MessageType={0}", message.MessageType);

                Console.Write(message.ToString());

                Console.WriteLine();
            }
        }
    }
}
