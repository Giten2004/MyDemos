// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;
using Microsoft.ServiceBus;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Timers;
using System.ServiceModel.Description;
using System.Collections.ObjectModel;


namespace ServiceModelEx.ServiceBus
{
   public static partial class ServiceBusHelper
   {
      internal const string DefaultIssuer = "owner";
            
      internal static void SetBehavior(Collection<ServiceEndpoint> endpoints,TransportClientEndpointBehavior credential)
      {
         foreach(ServiceEndpoint endpoint in endpoints)
         {
            if(endpoint.Binding is NetTcpRelayBinding ||
               endpoint.Binding is WSHttpRelayBinding   ||
               endpoint.Binding is NetOnewayRelayBinding)
            {
               endpoint.Behaviors.Add(credential);
            }
         }
      }
      public static void DeleteBuffer(string bufferAddress,string secret)
      {
         if(bufferAddress.EndsWith("/") == false)
         {
            bufferAddress += "/";
         }         
         
         Uri address = new Uri(bufferAddress);

         TransportClientEndpointBehavior credential = new TransportClientEndpointBehavior();
         credential.TokenProvider = TokenProvider.CreateSharedSecretTokenProvider(DefaultIssuer,secret);

         if(BufferExists(address,credential))
         {
            MessageBufferClient client = MessageBufferClient.GetMessageBuffer(credential,address);
            client.DeleteMessageBuffer();
         }  
      }
      public static void CreateBuffer(string bufferAddress,string secret)
      {
         CreateBuffer(bufferAddress,ServiceBusHelper.DefaultIssuer,secret);
      }
      public static void CreateBuffer(string bufferAddress,string issuer,string secret)
      {
         TransportClientEndpointBehavior creds = new TransportClientEndpointBehavior();
         creds.TokenProvider = TokenProvider.CreateSharedSecretTokenProvider(issuer,secret);

         CreateBuffer(bufferAddress,creds);
      }
      static void CreateBuffer(string bufferAddress,TransportClientEndpointBehavior credential)
      {
         MessageBufferPolicy policy = CreateBufferPolicy();
         CreateBuffer(bufferAddress,policy,credential);
      }
      static internal MessageBufferPolicy CreateBufferPolicy()
      {
         MessageBufferPolicy policy = new MessageBufferPolicy();                
         policy.Discoverability = DiscoverabilityPolicy.Public;
         policy.ExpiresAfter = TimeSpan.FromMinutes(10);
         policy.MaxMessageCount = 50;

         return policy;
      }
      public static void VerifyBuffer(string bufferAddress,string secret)
      {
         VerifyBuffer(bufferAddress,ServiceBusHelper.DefaultIssuer,secret);
      }
      public static void VerifyBuffer(string bufferAddress,string issuer,string secret)
      {
         TransportClientEndpointBehavior creds = new TransportClientEndpointBehavior();
         creds.TokenProvider = TokenProvider.CreateSharedSecretTokenProvider(issuer,secret);

         VerifyBuffer(bufferAddress,creds);
      }
      internal static void VerifyBuffer(string bufferAddress,TransportClientEndpointBehavior credential)
      {
         if(BufferExists(bufferAddress,credential))
         {
            return;
         }
         CreateBuffer(bufferAddress,credential);
      }
      public static void PurgeBuffer(Uri bufferAddress,TransportClientEndpointBehavior credential)
      {
         Debug.Assert(BufferExists(bufferAddress,credential));

         MessageBufferClient client = MessageBufferClient.GetMessageBuffer(credential,bufferAddress);
         MessageBufferPolicy policy = client.GetPolicy();
         client.DeleteMessageBuffer();
         MessageBufferClient.CreateMessageBuffer(credential,bufferAddress,policy);
      }
      //Helpers
      internal static bool BufferExists(string bufferAddress,TransportClientEndpointBehavior credential)
      {
         return BufferExists(new Uri(bufferAddress),credential);
      }
      internal static bool BufferExists(Uri bufferAddress,TransportClientEndpointBehavior credential)
      {
         try
         {
            MessageBufferClient client = MessageBufferClient.GetMessageBuffer(credential,bufferAddress);
            MessageBufferPolicy policy  = client.GetPolicy();
            if(policy.TransportProtection != TransportProtectionPolicy.AllPaths)
            {
               throw new InvalidOperationException("Buffer must be configured for transport protection");
            }
            return true;
         }
         catch(FaultException exception)
         {
            Debug.Assert(exception.Message == "Policy could not be retrieved: ContentType is incorrect");
         }
         
         return false;
      }
      static void CreateBuffer(string bufferAddress,MessageBufferPolicy policy,TransportClientEndpointBehavior credential)
      {
         if(bufferAddress.EndsWith("/") == false)
         {
            bufferAddress += "/";
         }         
         
         Uri address = new Uri(bufferAddress);

         if(BufferExists(address,credential))
         {
            MessageBufferClient client = MessageBufferClient.GetMessageBuffer(credential,address);
            client.DeleteMessageBuffer();
         }  
         MessageBufferClient.CreateMessageBuffer(credential,address,policy);
      }
   }
}






