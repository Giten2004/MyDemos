// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net


using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Discovery;
using Microsoft.ServiceBus;
using ServiceModelEx.Properties;
using ServiceModelEx.ServiceBus;
using WinFormsEx;


namespace ServiceModelEx
{
   partial class ExplorerForm 
   {      
      Dictionary<string,TokenProvider> m_NamespaceCredentials = new Dictionary<string,TokenProvider>();
      Dictionary<string,string> m_AnnouncementsPaths = new Dictionary<string,string>();
      Dictionary<string,string> m_DisoveryPaths = new Dictionary<string,string>();
      Dictionary<string,ServiceBusAnnouncementSink<IMetadataExchange>> m_ServiceBusAnnouncementSinks = new Dictionary<string,ServiceBusAnnouncementSink<IMetadataExchange>>();

      public static bool IsServiceBusAddress(string address)
      {
         address = address.ToLower();

         return address.Contains("servicebus.windows.net");
      }
      void OnServiceBusLogOn(object sender,EventArgs e)
      {
         string serviceNamespace = "";

         if(IsServiceBusAddress(m_MexAddressTextBox.Text))
         {
            serviceNamespace = ServiceBusHelper.ExtractNamespace(new Uri(m_MexAddressTextBox.Text));
         }

         LOGIN:
         LogonDialog dialog = new LogonDialog(serviceNamespace,ServiceBusHelper.DefaultIssuer);
         dialog.ShowDialog();

         if(IsServiceBusAddress(m_MexAddressTextBox.Text) == false)
         {
            try
            {
               m_MexAddressTextBox.Text = ServiceBusEnvironment.CreateServiceUri("sb",dialog.ServiceNamespace,"").AbsoluteUri;
            }
            catch
            {
               System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Invalid service namespace","MEX Explorer",System.Windows.Forms.MessageBoxButtons.RetryCancel,System.Windows.Forms.MessageBoxIcon.Error);
               if(result == System.Windows.Forms.DialogResult.Retry)
               {
                  goto LOGIN;
               }
               else
               {
                  return;
               }
            }
         }


         TransportClientEndpointBehavior credentials = new TransportClientEndpointBehavior();
         credentials.TokenProvider = TokenProvider.CreateSharedSecretTokenProvider(dialog.Issuer,dialog.Secret);

         m_NamespaceCredentials[dialog.ServiceNamespace] = credentials.TokenProvider;         
      }  
    
      ServiceEndpoint[] GetServiceBusEndpoints()
      {
         string serviceNamespace = ServiceBusHelper.ExtractNamespace(new Uri(m_MexAddressTextBox.Text));
        
         return ServiceBusMetadataHelper.GetEndpoints(m_MexAddressTextBox.Text,m_NamespaceCredentials[serviceNamespace]);
      }

      void ExploreServiceBus()
      {                  
         string mexAddress = m_MexAddressTextBox.Text;

         if(IsServiceBusAddress(mexAddress))
         {
            if(m_NamespaceCredentials.ContainsKey(ServiceBusHelper.ExtractNamespace(new Uri(mexAddress))) == false)
            {
               OnServiceBusLogOn(this,EventArgs.Empty);
            }
         }

         SplashScreen splash = new SplashScreen(Resources.Progress);         
         try
         {
            ExploreServiceBus(mexAddress);
         }
         finally
         {
            splash.Close();
            m_ExploreButton.Enabled = true;
         }       
      }
      
      void ExploreServiceBus(string mexAddress)
      {
         ServiceNode existingNode = null;

         try
         {
            Uri address = new Uri(mexAddress);
            ServiceEndpoint[] endpoints = null;

            //Find if tree already contain this address
            foreach(ServiceNode node in m_MexTree.Nodes)
            {
               if(node.MexAddress == mexAddress)
               {
                  if(node.Text == "Unspecified Base Address" || node.Text == "Invalid Address")
                  {
                     node.ImageIndex = node.SelectedImageIndex = ServiceIndex;
                  }
                  existingNode = node;
                  break;
               }
            }

            endpoints = GetServiceBusEndpoints();

            ProcessMetaData(existingNode,mexAddress,endpoints);
         }
         catch
         {
            if(existingNode == null)
            {
               CurrentNode = new ServiceNode(mexAddress,this,"Invalid Address",ServiceError,ServiceError);
               m_MexTree.Nodes.Add(CurrentNode);
            }
            else
            {
               CurrentNode.Text = "Invalid Address";
               CurrentNode.Nodes.Clear();
               CurrentNode.ImageIndex = CurrentNode.SelectedImageIndex = ServiceError;
            }
         }
      }

      void OnConfigureDiscovery(object sender,EventArgs e)
      {
         string serviceNamespace = "";

         if(IsServiceBusAddress(m_MexAddressTextBox.Text))
         {
            serviceNamespace = ServiceBusHelper.ExtractNamespace(new Uri(m_MexAddressTextBox.Text));
         }

         bool announcementsEnables = m_ServiceBusAnnouncementSinks.ContainsKey(serviceNamespace);

         if(m_DisoveryPaths.ContainsKey(serviceNamespace) == false)
         {
            m_DisoveryPaths[serviceNamespace] = DiscoverableServiceHost.DiscoveryPath;
         }
         
         if(m_AnnouncementsPaths.ContainsKey(serviceNamespace) == false)
         {
            m_AnnouncementsPaths[serviceNamespace] = DiscoverableServiceHost.AnnouncementsPath;
         }

         DiscoveryDialog dialog = new DiscoveryDialog(serviceNamespace,m_DisoveryPaths[serviceNamespace],announcementsEnables,m_AnnouncementsPaths[serviceNamespace]);
         dialog.ShowDialog();
                
         serviceNamespace = dialog.ServiceNamespace;

         m_MexAddressTextBox.Text = ServiceBusEnvironment.CreateServiceUri("sb",serviceNamespace,"").AbsoluteUri;        

         m_DisoveryPaths[serviceNamespace] = dialog.DiscoveryPath;

         if(String.IsNullOrWhiteSpace(dialog.AnnouncementsPath) == false)
         {
            m_AnnouncementsPaths[serviceNamespace] = dialog.AnnouncementsPath;

            if(m_NamespaceCredentials.ContainsKey(ServiceBusHelper.ExtractNamespace(new Uri(m_MexAddressTextBox.Text))) == false)
            {
               OnServiceBusLogOn(this,EventArgs.Empty);
            }
               
            Uri newAnouncementsAddress = ServiceBusEnvironment.CreateServiceUri("sb",serviceNamespace,m_AnnouncementsPaths[serviceNamespace]);

            if(m_ServiceBusAnnouncementSinks.ContainsKey(serviceNamespace))
            {

               if(m_ServiceBusAnnouncementSinks[serviceNamespace].AnnouncementsAddress.AbsoluteUri != newAnouncementsAddress.AbsoluteUri)
               {
                  m_ServiceBusAnnouncementSinks[serviceNamespace].Close();
                  m_ServiceBusAnnouncementSinks.Remove(serviceNamespace);
               }
               else
               {
                  return;
               }
            }
            
            TokenProvider tokenPorvider = m_NamespaceCredentials[serviceNamespace];

            m_ServiceBusAnnouncementSinks[serviceNamespace] = new ServiceBusAnnouncementSink<IMetadataExchange>(serviceNamespace,tokenPorvider);
            m_ServiceBusAnnouncementSinks[serviceNamespace].AnnouncementsAddress = newAnouncementsAddress;

            m_ServiceBusAnnouncementSinks[serviceNamespace].OnlineAnnouncementReceived += OnHelloNotice;
            m_ServiceBusAnnouncementSinks[serviceNamespace].OfflineAnnouncementReceived   += OnByeNotice;
 
            m_ServiceBusAnnouncementSinks[serviceNamespace].Open();
         }
         else
         {
            if(m_ServiceBusAnnouncementSinks.ContainsKey(serviceNamespace))
            {
               m_ServiceBusAnnouncementSinks[serviceNamespace].Close();
               m_ServiceBusAnnouncementSinks.Remove(serviceNamespace);
            }
         }
      }
      
      void DiscoverServiceBus()
      {
         foreach(string serviceNamespace in m_NamespaceCredentials.Keys)
         {
            string[] addresses = DiscoverServiceBusMexAddresses(serviceNamespace);
            foreach(string address in addresses)
            {
               m_MexAddressTextBox.Text = address;
               ExploreServiceBus(address);
            }
         }
      }

      string[] DiscoverServiceBusMexAddresses(string serviceNamespace)
      {
         NetOnewayRelayBinding binding = new NetOnewayRelayBinding();
         if(m_DisoveryPaths.ContainsKey(serviceNamespace) == false)
         {
            m_DisoveryPaths[serviceNamespace] = DiscoverableServiceHost.DiscoveryPath;
         }
         EndpointAddress address = new EndpointAddress(ServiceBusEnvironment.CreateServiceUri("sb",serviceNamespace,m_DisoveryPaths[serviceNamespace]));

         ServiceBusDiscoveryClient discoveryClient = new ServiceBusDiscoveryClient(binding,address);
         TransportClientEndpointBehavior creds = new TransportClientEndpointBehavior(m_NamespaceCredentials[serviceNamespace]);

         IServiceBusProperties properties = discoveryClient as IServiceBusProperties;
         properties.Credential = creds;

         FindResponse discovered = discoveryClient.Find(FindCriteria.CreateMetadataExchangeEndpointCriteria());

         return discovered.Endpoints.Select(mexEndpoint => mexEndpoint.Address.Uri.AbsoluteUri).ToArray();
      }
   }
}