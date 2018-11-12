using System;
using System.Text;
using Amqp;
using Amqp.Sasl;
using Amqp.Framing;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Threading.Tasks;

namespace amqpnetliteDemo
{
    public class BroadcastReceiver
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string certFile;
        static BroadcastReceiver()
        {
            certFile = System.IO.Path.Combine(Environment.CurrentDirectory, @"cert\eurex_test1_public.v2.crt");

            Log.DebugFormat("certFile: {0}", certFile);
        }

        private ConnectionFactory _factory;
        private Connection _connection;
        private Session _session;
        private ReceiverLink _receiver;

        public bool ValidateServerCertificate(object sender,
                                                     X509Certificate certificate,
                                                     X509Chain chain,
                                                     SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Log.ErrorFormat("Certificate error: {0}", sslPolicyErrors);

            return false;
        }

        public X509Certificate CertificateSelectionCallback(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            return X509Certificate.CreateFromCertFile(certFile);
        }

        public async void Start()
        {
            try
            {
                _factory = new ConnectionFactory();

                _factory.SSL.RemoteCertificateValidationCallback = ValidateServerCertificate;
                _factory.SSL.LocalCertificateSelectionCallback = CertificateSelectionCallback;
                _factory.SSL.ClientCertificates.Add(X509Certificate.CreateFromCertFile(certFile));

                _factory.AMQP.MaxFrameSize = 64 * 1024;
                //factory.AMQP.ContainerId = "fixml-client";
                _factory.AMQP.IdleTimeout = 60 * 1000;
               

                _factory.SASL.Profile = SaslProfile.External;

                Trace.TraceLevel = TraceLevel.Frame;
                Trace.TraceListener = (l, f, a) =>
                {
                    System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("[hh:mm:ss.fff]") + " " + String.Format(f, a));

                    Log.DebugFormat(f, a);
                };

                Connection.DisableServerCertValidation = false;

                Address brokerAddress = new Address("amqps://ecag-fixml-prod1.deutsche-boerse.com:10070");

                _connection = await _factory.CreateAsync(brokerAddress);

                _session = new Session(_connection);

                _receiver = new ReceiverLink(_session, "broadcast-receiver", "broadcast.LCMLO_LIQSPALBBTEST1.TradeConfirmation");
                _receiver.Closed += ReceiverClosedCallback;

                _receiver.Start(5, EurexMessageCallback);

                Log.DebugFormat("_receiver.Start(5, EurexMessageCallback);");
            }
            catch (Exception e)
            {
                Log.Fatal("Exception", e);
            }
        }

        public void EurexMessageCallback(IReceiverLink receiver, Message message)
        {
            Log.DebugFormat("EurexMessageCallback");

            if (message == null)
            {
                Log.WarnFormat("receiver.Receive(600000) is timeout.");
                return;
            }

            Amqp.Framing.Data payload = (Amqp.Framing.Data)message.BodySection;
            String payloadText = Encoding.UTF8.GetString(payload.Binary);

            Log.DebugFormat("Received message: {0}", payloadText);
            receiver.Accept(message);
        }

        public void ReceiverClosedCallback(IAmqpObject sender, Error error)
        {
            Log.FatalFormat("error.Description:{0}, error.Info:{1}, error:{2}", error.Description, error.Info, error);
        }
    }
}