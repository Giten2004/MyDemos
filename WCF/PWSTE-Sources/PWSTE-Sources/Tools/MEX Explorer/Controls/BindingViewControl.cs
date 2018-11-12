using System;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Net.Security;
using Microsoft.ServiceBus;

namespace ServiceModelEx
{
   partial class BindingViewControl : NodeViewControl
   {
      enum SecurityModeEx
      {
         None,
         Transport,
         Message,
         Mixed,
         Both
      }
      enum CredentialTypeEx
      {
         None,
         Windows,
         Username,
         Certificate,
         Token
      }
      public BindingViewControl()
      {
         InitializeComponent();
      }
      public void Refresh(Binding binding)
      {
         ConfigureSecurityMode(binding);
         ConfigureCredentialsType(binding);
         ConfigureProtectionLevel(binding);
         ConfigureReliability(binding);
         ConfigureTransactions(binding);
         ConfigureStreaming(binding);
      }

      static CredentialTypeEx ConvertCredentials(PeerTransportCredentialType credentials)
      {
         switch(credentials)
         {
            case PeerTransportCredentialType.Certificate:
            {
               return CredentialTypeEx.Certificate;
            }
            case PeerTransportCredentialType.Password:
            {
               return CredentialTypeEx.Username;
            }
            default:
            {
               throw new InvalidOperationException("Unknown credentials type");
            }
         }
      }
      static CredentialTypeEx ConvertCredentials(BasicHttpMessageCredentialType credentials)
      {
         switch(credentials)
         {
            case BasicHttpMessageCredentialType.Certificate:
            {
               return CredentialTypeEx.Certificate;
            }
            case BasicHttpMessageCredentialType.UserName:
            {
               return CredentialTypeEx.Username;
            }
            default:
            {
               throw new InvalidOperationException("Unknown credentials type");
            }
         }
      }

      static CredentialTypeEx ConvertCredentials(MsmqAuthenticationMode credentials)
      {
         switch(credentials)
         {
            case MsmqAuthenticationMode.Certificate:
            {
               return CredentialTypeEx.Certificate;
            }
            case MsmqAuthenticationMode.None:
            {
               return CredentialTypeEx.None;
            }
            case MsmqAuthenticationMode.WindowsDomain:
            {
               return CredentialTypeEx.Windows;
            }
            default:
            {
               throw new InvalidOperationException("Unknown credentials type");
            }
         }
      }
      static CredentialTypeEx ConvertCredentials(TcpClientCredentialType credentials)
      {
         switch(credentials)
         {
            case TcpClientCredentialType.Certificate:
            {
               return CredentialTypeEx.Certificate;
            }
            case TcpClientCredentialType.None:
            {
               return CredentialTypeEx.None;
            }
            case TcpClientCredentialType.Windows:
            {
               return CredentialTypeEx.Windows;
            }
            default:
            {
               throw new InvalidOperationException("Unknown credentials type");
            }
         }
      }

      static CredentialTypeEx ConvertCredentials(HttpClientCredentialType credentials)
      {
         switch(credentials)
         {
            case HttpClientCredentialType.Basic:
            case HttpClientCredentialType.Digest:
            {
               return CredentialTypeEx.Username;
            }
            case HttpClientCredentialType.Certificate:
            {
               return CredentialTypeEx.Certificate;
            }
            case HttpClientCredentialType.None:
            {
               return CredentialTypeEx.None;
            }
            case HttpClientCredentialType.Ntlm:
            case HttpClientCredentialType.Windows:
            {
               return CredentialTypeEx.Windows;
            }
            default:
            {
               throw new InvalidOperationException("Unknown credentials type");
            }
         }
      }
      static CredentialTypeEx ConvertCredentials(MessageCredentialType credentials)
      {
         switch(credentials)
         {
            case MessageCredentialType.Certificate:
            {
               return CredentialTypeEx.Certificate;
            }
            case MessageCredentialType.IssuedToken:
            {
               return CredentialTypeEx.Token;
            }
            case MessageCredentialType.None:
            {
               return CredentialTypeEx.None;
            }
            case MessageCredentialType.UserName:
            {
               return CredentialTypeEx.Username;
            }
            case MessageCredentialType.Windows:
            {
               return CredentialTypeEx.Windows;
            }
            default:
            {
               throw new InvalidOperationException("Unknown credentials type");
            }
         }
      }
      
      void SetProtectionLevel(ProtectionLevel protectionLevel)
      {
         m_NonProtectionRadioButton.Enabled = true;
         m_SignedRadioButton.Enabled = true;
         m_EncryptRadioButton.Enabled = true;

         m_NonProtectionRadioButton.Checked = protectionLevel == ProtectionLevel.None;
         m_SignedRadioButton.Checked = protectionLevel == ProtectionLevel.Sign;
         m_EncryptRadioButton.Checked = protectionLevel == ProtectionLevel.EncryptAndSign;
      }

      void ConfigureCredentialsType(Binding binding)
      {
         CredentialTypeEx credentialType = CredentialTypeEx.None;
         bool enabled = false;

         if(binding is NetTcpBinding)
         {
            NetTcpBinding tcpBinding = binding as NetTcpBinding;
            switch(tcpBinding.Security.Mode)
            {
               case SecurityMode.Message:
               {
                  credentialType = ConvertCredentials(tcpBinding.Security.Message.ClientCredentialType);
                  break;
               }
               case SecurityMode.None:
               {
                  credentialType = CredentialTypeEx.None;
                  break;
               }
               case SecurityMode.Transport:
               {
                  credentialType = ConvertCredentials(tcpBinding.Security.Transport.ClientCredentialType);
                  break;
               }
            }
            enabled = true;
         }
         if(binding is NetNamedPipeBinding)
         {
            NetNamedPipeBinding pipeBinding = binding as NetNamedPipeBinding;
            switch(pipeBinding.Security.Mode)
            {
               case NetNamedPipeSecurityMode.None:
               {
                  credentialType = CredentialTypeEx.None;
                  break;
               }
               case NetNamedPipeSecurityMode.Transport:
               {
                  credentialType = CredentialTypeEx.Windows;
                  break;
               }
            }
            enabled = true;
         }
         if(binding is NetMsmqBinding)
         {
            NetMsmqBinding msmqBinding = binding as NetMsmqBinding;
            switch(msmqBinding.Security.Mode)
            {
               case NetMsmqSecurityMode.Both:
               case NetMsmqSecurityMode.Message:
               {
                  credentialType =  ConvertCredentials(msmqBinding.Security.Message.ClientCredentialType);
                  break;
               }
               case NetMsmqSecurityMode.None:
               {
                  credentialType = CredentialTypeEx.None;
                  break;
               }
               case NetMsmqSecurityMode.Transport:
               {
                  credentialType =  ConvertCredentials(msmqBinding.Security.Transport.MsmqAuthenticationMode);
                  break;
               }
            }
            enabled = true;
         }
         if(binding is BasicHttpBinding)
         {
            BasicHttpBinding basicBinding = binding as BasicHttpBinding;
            switch(basicBinding.Security.Mode)
            {
               case BasicHttpSecurityMode.Message:
               case BasicHttpSecurityMode.TransportWithMessageCredential:
                  {
                  credentialType = ConvertCredentials(basicBinding.Security.Message.ClientCredentialType);
                  break;
               }
               case BasicHttpSecurityMode.None:
               {
                  credentialType = CredentialTypeEx.None;
                  break;
               }
               case BasicHttpSecurityMode.Transport:
               case BasicHttpSecurityMode.TransportCredentialOnly:
               {
                  credentialType = ConvertCredentials(basicBinding.Security.Transport.ClientCredentialType);
                  break;
               }
            }
            enabled = true;
         }
         if(binding is WSHttpBinding)
         {
            WSHttpBinding wsBinding = binding as WSHttpBinding;
            switch(wsBinding.Security.Mode)
            {
               case SecurityMode.TransportWithMessageCredential:
               case SecurityMode.Message:
               {
                  credentialType = ConvertCredentials(wsBinding.Security.Message.ClientCredentialType);
                  break;
               }
               case SecurityMode.None:
               {
                  credentialType= CredentialTypeEx.None;
                  break;
               }
               case SecurityMode.Transport:
               {
                  credentialType = ConvertCredentials(wsBinding.Security.Transport.ClientCredentialType);
                  break;
               }
            }
            enabled = true;
         }

         if(binding is WSDualHttpBinding)
         {
            WSDualHttpBinding wsDualBinding = binding as WSDualHttpBinding;
            switch(wsDualBinding.Security.Mode)
	         {
               case WSDualHttpSecurityMode.Message:
               {
                  credentialType = ConvertCredentials(wsDualBinding.Security.Message.ClientCredentialType);
                  break;
               }
               case WSDualHttpSecurityMode.None:
               {
                  credentialType  = CredentialTypeEx.None;
                  break;
               }
	         }
            enabled = true;
         }
         if(binding is NetPeerTcpBinding)
         {
            NetPeerTcpBinding peerBinding = binding as NetPeerTcpBinding;
            switch (peerBinding.Security.Mode)
            {
               case SecurityMode.None:
               {
                  credentialType = CredentialTypeEx.None;
                  break;
               }
               case SecurityMode.Transport:
               {
                  credentialType = ConvertCredentials(peerBinding.Security.Transport.CredentialType);
                  break;
               }
            }
            enabled = true;
         }
         if(binding is WSFederationHttpBinding)
         {
            credentialType = CredentialTypeEx.Token;
            enabled = true;
         }
                  
         if(binding is NetTcpRelayBinding)
         {
            credentialType = CredentialTypeEx.Token;
             enabled = true;
         }
                           
         if(binding is BasicHttpRelayBinding)
         {
            credentialType = CredentialTypeEx.Token;
            enabled = true;
         }

         m_NoCredentialsRadioButton.Checked = credentialType == CredentialTypeEx.None;
         m_WindowsCredentialsRadioButton.Checked = credentialType == CredentialTypeEx.Windows;
         m_UsernameCredentialsRadioButton.Checked = credentialType == CredentialTypeEx.Username;
         m_CertificateCredentialsRadioButton.Checked = credentialType == CredentialTypeEx.Certificate;
         m_TokenRadioButton.Checked = credentialType == CredentialTypeEx.Token;

         m_NoCredentialsRadioButton.Enabled = enabled;
         m_WindowsCredentialsRadioButton.Enabled = enabled;
         m_UsernameCredentialsRadioButton.Enabled = enabled;
         m_CertificateCredentialsRadioButton.Enabled = enabled;
         m_TokenRadioButton.Enabled = enabled;

      }

      void ConfigureSecurityMode(Binding binding)
      {
         SecurityModeEx securityMode = SecurityModeEx.None;

         bool enabled = false;

         if(binding is NetTcpBinding)
         {
            NetTcpBinding tcpBinding = binding as NetTcpBinding;
            switch(tcpBinding.Security.Mode)
            {
               case SecurityMode.Message:
               {
                  securityMode = SecurityModeEx.Message;
                  break;
               }
               case SecurityMode.None:
               {
                  securityMode = SecurityModeEx.None;
                  break;
               }
               case SecurityMode.Transport:
               {
                  securityMode = SecurityModeEx.Transport;
                  break;
               }
               case SecurityMode.TransportWithMessageCredential:
               {
                  securityMode = SecurityModeEx.Mixed;
                  break;
               }
            }
            enabled = true;
         }
         if(binding is NetNamedPipeBinding)
         {
            NetNamedPipeBinding pipeBinding = binding as NetNamedPipeBinding;
            switch(pipeBinding.Security.Mode)
            {
               case NetNamedPipeSecurityMode.None:
               {
                  securityMode = SecurityModeEx.None;
                  break;
               }
               case NetNamedPipeSecurityMode.Transport:
               {
                  securityMode = SecurityModeEx.Transport;
                  break;
               }
            }
            enabled = true;
         }
         if(binding is NetMsmqBinding)
         {
            NetMsmqBinding msmqBinding = binding as NetMsmqBinding;
            switch(msmqBinding.Security.Mode)
            {
               case NetMsmqSecurityMode.Both:
               {
                  securityMode = SecurityModeEx.Both;
                  break;
               }
               case NetMsmqSecurityMode.Message:
               {
                  securityMode = SecurityModeEx.Message;
                  break;
               }
               case NetMsmqSecurityMode.None:
               {
                  securityMode = SecurityModeEx.None;
                  break;
               }
               case NetMsmqSecurityMode.Transport:
               {
                  securityMode = SecurityModeEx.Transport;
                  break;
               }
            }
            enabled = true;
         }
         if(binding is BasicHttpBinding)
         {
            BasicHttpBinding basicBinding = binding as BasicHttpBinding;
            switch(basicBinding.Security.Mode)
            {
               case BasicHttpSecurityMode.Message:
               {
                  securityMode = SecurityModeEx.Message;
                  break;
               }
               case BasicHttpSecurityMode.None:
               {
                  securityMode = SecurityModeEx.None;
                  break;
               }
               case BasicHttpSecurityMode.Transport:
               {
                  securityMode = SecurityModeEx.Transport;
                  break;
               }
               case BasicHttpSecurityMode.TransportCredentialOnly:
               {
                  securityMode = SecurityModeEx.Transport;
                  break;
               }
               case BasicHttpSecurityMode.TransportWithMessageCredential:
               {
                 securityMode = SecurityModeEx.Mixed;
                 break;
               }
            }
            enabled = true;
         }
         if(binding is WSHttpBinding)
         {
            WSHttpBinding wsBinding = binding as WSHttpBinding;
            switch(wsBinding.Security.Mode)
            {
               case SecurityMode.Message:
               {
                  securityMode = SecurityModeEx.Message;
                  break;
               }
               case SecurityMode.None:
               {
                  securityMode = SecurityModeEx.None;
                  break;
               }
               case SecurityMode.Transport:
               {
                  securityMode = SecurityModeEx.Transport;
                  break;
               }
               case SecurityMode.TransportWithMessageCredential:
               {
                  securityMode = SecurityModeEx.Mixed;
                  break;
               }
            }
            enabled = true;
         }

         if(binding is WSDualHttpBinding)
         {
            WSDualHttpBinding wsDualBinding = binding as WSDualHttpBinding;
            switch(wsDualBinding.Security.Mode)
	         {
               case WSDualHttpSecurityMode.Message:
               {
                  securityMode = SecurityModeEx.Message;
                  break;
               }
               case WSDualHttpSecurityMode.None:
               {
                  securityMode = SecurityModeEx.None;
                  break;
               }
	         }
            enabled = true;
         }
         if(binding is NetPeerTcpBinding)
         {
            NetPeerTcpBinding peerBinding = binding as NetPeerTcpBinding;
            switch (peerBinding.Security.Mode)
            {
               case SecurityMode.Message:
               {
                  securityMode = SecurityModeEx.Message;
                  break;
               }
               case SecurityMode.None:
               {
                  securityMode = SecurityModeEx.None;
                  break;
               }
               case SecurityMode.Transport:
               {
                  securityMode = SecurityModeEx.Transport;
                  break;
               }
               case SecurityMode.TransportWithMessageCredential:
               {
                  securityMode = SecurityModeEx.Transport;
                  break;
               }
            }
            enabled = true;
         }
         if(binding is WSFederationHttpBinding)
         {
            WSFederationHttpBinding federatedBinding = binding as WSFederationHttpBinding;
            switch (federatedBinding.Security.Mode)
            {
               case WSFederationHttpSecurityMode.Message:
               {
                  securityMode = SecurityModeEx.Message;
                  break;
               }
               case WSFederationHttpSecurityMode.None:
               {
                  securityMode = SecurityModeEx.None;
                  break;
               }
               case WSFederationHttpSecurityMode.TransportWithMessageCredential:
               {
                  securityMode = SecurityModeEx.Mixed;
                  break;
               }
            }
            enabled = true;
         }

         if(binding is NetTcpRelayBinding)
         {
            NetTcpRelayBinding tcpRelayBinding = binding as NetTcpRelayBinding;
            switch(tcpRelayBinding.Security.Mode)
            {
               case EndToEndSecurityMode.Message:
               {
                  securityMode = SecurityModeEx.Message;
                  break;
               }
               case EndToEndSecurityMode.None:
               {
                  securityMode = SecurityModeEx.None;
                  break;
               }
               case EndToEndSecurityMode.Transport:
               {
                  securityMode = SecurityModeEx.Transport;
                  break;
               }
               case EndToEndSecurityMode.TransportWithMessageCredential:
               {
                  securityMode = SecurityModeEx.Mixed;
                  break;
               }
            }
            enabled = true;
         }
         
         if(binding is BasicHttpRelayBinding)
         {
            BasicHttpRelayBinding basicRelayBinding = binding as BasicHttpRelayBinding;
            switch(basicRelayBinding.Security.Mode)
            {
               case EndToEndBasicHttpSecurityMode.Message:
               {
                  securityMode = SecurityModeEx.Message;
                  break;
               }
               case EndToEndBasicHttpSecurityMode.None:
               {
                  securityMode = SecurityModeEx.None;
                  break;
               }
               case EndToEndBasicHttpSecurityMode.Transport:
               {
                  securityMode = SecurityModeEx.Transport;
                  break;
               }
               case EndToEndBasicHttpSecurityMode.TransportWithMessageCredential:
               {
                  securityMode = SecurityModeEx.Mixed;
                  break;
               }
            }
            enabled = true;
         }

         if(binding is NetOnewayRelayBinding)
         {
            NetOnewayRelayBinding onewayRelayBinding = binding as NetOnewayRelayBinding;
            switch(onewayRelayBinding.Security.Mode)
            {
               case EndToEndSecurityMode.Message:
               {
                  securityMode = SecurityModeEx.Message;
                  break;
               }
               case EndToEndSecurityMode.None:
               {
                  securityMode = SecurityModeEx.None;
                  break;
               }
               case EndToEndSecurityMode.Transport:
               {
                  securityMode = SecurityModeEx.Transport;
                  break;
               }
               case EndToEndSecurityMode.TransportWithMessageCredential:
               {
                  securityMode = SecurityModeEx.Mixed;
                  break;
               }
            }
            enabled = true;
         }

         if(binding is CustomBinding)
         {         
            enabled = false;
         }

         m_NoneRadioButton.Checked = securityMode == SecurityModeEx.None;
         m_TransportRadioButton.Checked = securityMode == SecurityModeEx.Transport;
         m_MessageRadioButton.Checked = securityMode == SecurityModeEx.Message;
         m_MixedRadioButton.Checked = securityMode == SecurityModeEx.Mixed;
         m_BothRadioButton.Checked = securityMode == SecurityModeEx.Both;

         m_NoneRadioButton.Enabled = enabled;
         m_TransportRadioButton.Enabled = enabled;
         m_MessageRadioButton.Enabled = enabled;
         m_MixedRadioButton.Enabled = enabled;
         m_BothRadioButton.Enabled = enabled;
      }
      void ConfigureProtectionLevel(Binding binding)
      {
         m_NonProtectionRadioButton.Checked = false;
         m_SignedRadioButton.Checked = false;
         m_EncryptRadioButton.Checked = false;

         m_NonProtectionRadioButton.Enabled = false;
         m_SignedRadioButton.Enabled = false;
         m_EncryptRadioButton.Enabled = false;


         if(binding is NetTcpBinding)
         {
            NetTcpBinding tcpBinding = binding as NetTcpBinding;
            if(tcpBinding.Security.Mode == SecurityMode.Transport)
            {
               SetProtectionLevel(tcpBinding.Security.Transport.ProtectionLevel);
            }
         }
         if(binding is NetNamedPipeBinding)
         {
            NetNamedPipeBinding pipeBinding = binding as NetNamedPipeBinding;
            if(pipeBinding.Security.Mode == NetNamedPipeSecurityMode.Transport)
            {
               SetProtectionLevel(pipeBinding.Security.Transport.ProtectionLevel);
            }
         }
         if(binding is NetMsmqBinding)
         {
            NetMsmqBinding msmqBinding = binding as NetMsmqBinding;
            if(msmqBinding.Security.Mode == NetMsmqSecurityMode.Transport || msmqBinding.Security.Mode == NetMsmqSecurityMode.Both)
            {
               SetProtectionLevel(msmqBinding.Security.Transport.MsmqProtectionLevel);
            }
         }

         if(binding is NetTcpRelayBinding)
         {
            NetTcpRelayBinding tcpRealyBinding = binding as NetTcpRelayBinding;
            if(tcpRealyBinding.Security.Mode == EndToEndSecurityMode.Transport)
            {
               SetProtectionLevel(tcpRealyBinding.Security.Transport.ProtectionLevel);
            }
         }
      }

      void ConfigureReliability(Binding binding)
      {
         if(binding is NetTcpBinding)
         {
            NetTcpBinding tcpBinding = binding as NetTcpBinding;
            if(tcpBinding.ReliableSession.Enabled)
            {
               m_ReliabilityEnabledLabel.Text = "Enabled";
               if(tcpBinding.ReliableSession.Ordered)
               {
                  m_OrderedLabel.Text = "Ordered";
               }
               else
               {
                  m_OrderedLabel.Text = "Unordered";
               }
            }
            else
            {
               m_ReliabilityEnabledLabel.Text = "Disabled";
               m_OrderedLabel.Text = "Unordered";
            }
         }
         if(binding is NetNamedPipeBinding)
         {
            m_ReliabilityEnabledLabel.Text = "Enabled";
            m_OrderedLabel.Text = "Ordered";
         }
         if(binding is NetMsmqBinding)
         {
            m_ReliabilityEnabledLabel.Text = "Reliability: N/A";
            m_OrderedLabel.Text = "Ordered: N/A";
         }

         if(binding is BasicHttpRelayBinding)
         {
            m_ReliabilityEnabledLabel.Text = "Reliability: N/A";
            m_OrderedLabel.Text = "Ordered: N/A";
         }
         
         if(binding is CustomBinding)
         {
            m_ReliabilityEnabledLabel.Text = "Reliability: N/A";
            m_OrderedLabel.Text = "Ordered: N/A";
         }
         if(binding is BasicHttpBinding || binding is NetPeerTcpBinding)
         {
            m_ReliabilityEnabledLabel.Text = "Disabled";
            m_OrderedLabel.Text = "Unordered";
         }
         if(binding is WSHttpBinding)
         {
            WSHttpBinding wsBinding = binding as WSHttpBinding;
            if(wsBinding.ReliableSession.Enabled)
            {
               m_ReliabilityEnabledLabel.Text = "Enabled";
               if(wsBinding.ReliableSession.Ordered)
               {
                  m_OrderedLabel.Text = "Ordered";
               }
               else
               {
                  m_OrderedLabel.Text = "Unordered";
               }
            }
            else
            {
               m_ReliabilityEnabledLabel.Text = "Disabled";
               m_OrderedLabel.Text = "Unordered";
            }
         }
         if(binding is WSDualHttpBinding)
         {
            m_ReliabilityEnabledLabel.Text = "Enabled";
            WSDualHttpBinding wsDualBinding = binding as WSDualHttpBinding;
            if(wsDualBinding.ReliableSession.Ordered)
            {
               m_OrderedLabel.Text = "Ordered";
            }
            else
            {
               m_OrderedLabel.Text = "Unordered";
            }            
         }
         if(binding is WSFederationHttpBinding)
         {
            WSFederationHttpBinding federatedBinding = binding as WSFederationHttpBinding;
            if(federatedBinding.ReliableSession.Enabled)
            {
               m_ReliabilityEnabledLabel.Text = "Enabled";
               if(federatedBinding.ReliableSession.Ordered)
               {
                  m_OrderedLabel.Text = "Ordered";
               }
               else
               {
                  m_OrderedLabel.Text = "Unordered";
               }
            }
            else
            {
               m_ReliabilityEnabledLabel.Text = "Disabled";
               m_OrderedLabel.Text = "Unordered";
            }
         }
   
         if(binding is NetTcpRelayBinding)
         {
            NetTcpRelayBinding tcpRelayBinding = binding as NetTcpRelayBinding;
            if(tcpRelayBinding.ReliableSession.Enabled)
            {
               m_ReliabilityEnabledLabel.Text = "Enabled";
               if(tcpRelayBinding.ReliableSession.Ordered)
               {
                  m_OrderedLabel.Text = "Ordered";
               }
               else
               {
                  m_OrderedLabel.Text = "Unordered";
               }
            }
            else
            {
               m_ReliabilityEnabledLabel.Text = "Disabled";
               m_OrderedLabel.Text = "Unordered";
            }
         }
      }
      
      void ConfigureTransactions(Binding binding)
      {
         if(binding is NetTcpBinding)
         {
            NetTcpBinding tcpBinding = binding as NetTcpBinding;
            if(tcpBinding.TransactionFlow)
            {
               m_TransactionFlowLabel.Text = "Flow: Enabled";
               if(tcpBinding.TransactionProtocol == TransactionProtocol.OleTransactions)
               {
                  m_TransactionProtocol.Text = "Protocol: OleTx";
               }
               else
               {
                  m_TransactionProtocol.Text = "Protocol: WSAT";
               }
            }
            else
            {
               m_TransactionFlowLabel.Text = "Flow: Disabled";
               m_TransactionProtocol.Text = "Protocol: -";
            }
         }
         if(binding is NetNamedPipeBinding)
         {
            NetNamedPipeBinding pipeBinding = binding as NetNamedPipeBinding;
            if(pipeBinding.TransactionFlow)
            {
               m_TransactionFlowLabel.Text = "Flow: Enabled";
               if(pipeBinding.TransactionProtocol == TransactionProtocol.OleTransactions)
               {
                  m_TransactionProtocol.Text = "Protocol: OleTx";
               }
               else
               {
                  m_TransactionProtocol.Text = "Protocol: WSAT";
               }
            }
            else
            {
               m_TransactionFlowLabel.Text = "Flow: Disabled";
               m_TransactionProtocol.Text  = "Protocol: -";
            }
         }
                  
         if(binding is BasicHttpRelayBinding)
         {
            m_TransactionFlowLabel.Text = "Flow: N/A";
            m_TransactionProtocol.Text = "Protocol: -";
         }
         
         if(binding is CustomBinding)
         {
            m_TransactionFlowLabel.Text = "Flow: N/A";
            m_TransactionProtocol.Text = "Protocol: -";
         }
         if(binding is NetMsmqBinding)
         {
            m_TransactionFlowLabel.Text = "Flow: N/A";
            m_TransactionProtocol.Text = "Protocol: -";
         }
         if(binding is BasicHttpBinding || binding is NetPeerTcpBinding)
         {
            m_TransactionFlowLabel.Text = "Flow: Disabled";
            m_TransactionProtocol.Text = "Protocol: -";
         }
         if(binding is WSHttpBinding)
         {
            WSHttpBinding wsBinding = binding as WSHttpBinding;
            if(wsBinding.TransactionFlow)
            {
               m_TransactionFlowLabel.Text = "Flow: Enabled";
               m_TransactionProtocol.Text = "Protocol: WSAT";
            }
            else
            {
               m_TransactionFlowLabel.Text = "Flow: Disabled";
               m_TransactionProtocol.Text = "Protocol: -";
            }
         }
         if(binding is WSDualHttpBinding)
         {
            WSDualHttpBinding wsDualBinding = binding as WSDualHttpBinding;
            if(wsDualBinding.TransactionFlow)
            {
               m_TransactionFlowLabel.Text = "Flow: Enabled";
               m_TransactionProtocol.Text = "Protocol: WSAT";
            }
            else
            {
               m_TransactionFlowLabel.Text = "Flow: Disabled";
               m_TransactionProtocol.Text = "Protocol: -";
            }
         }
         if(binding is WSFederationHttpBinding)
         {
            WSFederationHttpBinding federatedBinding = binding as WSFederationHttpBinding;
            if(federatedBinding.TransactionFlow)
            {
               m_TransactionFlowLabel.Text = "Flow: Enabled";
               m_TransactionProtocol.Text = "Protocol: WSAT";
            }
            else
            {
               m_TransactionFlowLabel.Text = "Flow: Disabled";
               m_TransactionProtocol.Text = "Protocol: -";
            }
         }
      }
      void ConfigureStreaming(Binding binding)
      {
         m_StreamingEnabledLabel.Text = "N/A";

         if(binding is BasicHttpBinding)
         {
            BasicHttpBinding basicBinding = binding as BasicHttpBinding;
            if(basicBinding.TransferMode == TransferMode.Streamed)
            {
               m_StreamingEnabledLabel.Text = "Enabled";
            }
            else
            {
               m_StreamingEnabledLabel.Text = "Disabled";
            }
         }
         if(binding is BasicHttpRelayBinding)
         {
            BasicHttpRelayBinding basicRelayBinding = binding as BasicHttpRelayBinding;
            if(basicRelayBinding.TransferMode == TransferMode.Streamed)
            {
               m_StreamingEnabledLabel.Text = "Enabled";
            }
            else
            {
               m_StreamingEnabledLabel.Text = "Disabled";
            }
         }
      }
   }
}
