// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Discovery;
using System.Threading;
using System.Windows.Forms;
using ServiceModelEx.Properties;
using ServiceModelEx.ServiceBus;
using WinFormsEx;

namespace ServiceModelEx
{
   partial class ExplorerForm : Form
   {
      const int MessageMultiplier = 5;

      const int AddressIndex = 0;
      const int BindingIndex = 1;
      const int ContractIndex = 2;
      const int EndpointIndex = 3;
      const int OperationIndex = 4;
      const int ServiceIndex = 5;
      const int ServiceError = 6;
      const int AddressUnspecified = 7;
      
      public ServiceNode CurrentNode
      {get;set;}

      NodeViewControl m_CurrentViewControl;

      AnnouncementSink<IMetadataExchange> m_AnnouncementSink;
      SynchronizationContext m_SynchronizationContext;

      public ExplorerForm()
      {
         m_SynchronizationContext = SynchronizationContext.Current;

         InitializeComponent();
         m_MexTree.ImageList = new ImageList();
         m_MexTree.ImageList.Images.Add(Resources.Address);
         m_MexTree.ImageList.Images.Add(Resources.Binding);
         m_MexTree.ImageList.Images.Add(Resources.Contract);
         m_MexTree.ImageList.Images.Add(Resources.PieEndpoint);
         m_MexTree.ImageList.Images.Add(Resources.Operation);
         m_MexTree.ImageList.Images.Add(Resources.Service);
         m_MexTree.ImageList.Images.Add(Resources.ServiceError);
         m_MexTree.ImageList.Images.Add(Resources.AddressUnspecified);

         m_CurrentViewControl = m_BlankViewControl;
         DisplayBlankControl();

         m_AnnouncementSink = new AnnouncementSink<IMetadataExchange>();
         m_AnnouncementSink.OnlineAnnouncementReceived  += OnHelloNotice;
         m_AnnouncementSink.OfflineAnnouncementReceived += OnByeNotice;

         m_AnnouncementSink.Open();
      }

      void OnByeNotice(string mexAddress,Uri[] scopes)
      {
         if(SynchronizationContext.Current != m_SynchronizationContext)
         {
            SendOrPostCallback notice = (state)=>
                                        {
                                           string address = state as string;
                                           OnByeNotice(address,scopes);
                                        };
            m_SynchronizationContext.Post(notice,mexAddress);
            return;
         }
         foreach(ServiceNode service in m_MexTree.Nodes)
         {
            if(service.MexAddress == mexAddress)
            {
               m_MexTree.Nodes.Remove(service);
               break;
            }
         }
         if(m_MexTree.Nodes.Count == 0)
         {
            DisplayBlankControl();
         }
         else
         {
            CurrentNode = m_MexTree.Nodes[0] as ServiceNode;
            m_MexAddressTextBox.Text = CurrentNode.MexAddress; 
            DisplayServiceControl();
         }
      }
      void OnHelloNotice(string mexAddress,Uri[] scopes)
      {
         if(SynchronizationContext.Current != m_SynchronizationContext)
         {
            SendOrPostCallback notice = (state)=>
                                        {
                                           string address = state as string;
                                           OnHelloNotice(address,scopes);
                                        };
            m_SynchronizationContext.Post(notice,mexAddress);
            return;
         }
         m_MexAddressTextBox.Text = mexAddress;

         OnExplore(this,EventArgs.Empty);         

         foreach(ServiceNode node in m_MexTree.Nodes)
         {
            if(node.MexAddress == mexAddress)
            {
               CurrentNode = node;
               m_MexAddressTextBox.Text = mexAddress;
               m_MexTree.SelectedNode = node;
               m_MexTree.Focus();
            }
         }
      }
      public void SetMexAddress(string address)
      {
         m_MexAddressTextBox.Text = address;
      }
      void Explore(string mexAddress)
      {     
         ServiceNode existingNode = null;

         try
         {
            Uri address = new Uri(mexAddress);
            ServiceEndpointCollection endpoints = null;            

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
            if(address.Scheme == "http")
            {
               HttpTransportBindingElement httpBindingElement = new HttpTransportBindingElement();
               httpBindingElement.MaxReceivedMessageSize *= MessageMultiplier;

               //Try the HTTP MEX Endpoint
               try
               {
                  endpoints = GetEndpoints(httpBindingElement);
               }
               catch
               {}
               //Try over HTTP-GET
               if(endpoints == null)
               {
                  string httpGetAddress = mexAddress;
                  if(mexAddress.EndsWith("?wsdl") == false)
                  {
                     httpGetAddress += "?wsdl";
                  }
                  CustomBinding binding = new CustomBinding(httpBindingElement);
                  MetadataExchangeClient MEXClient = new MetadataExchangeClient(binding);
                  MetadataSet metadata = MEXClient.GetMetadata(new Uri(httpGetAddress),MetadataExchangeClientMode.HttpGet);
                  MetadataImporter importer = new WsdlImporter(metadata);
                  endpoints = importer.ImportAllEndpoints();
               }
            }
            if(address.Scheme == "https")
            {
               HttpsTransportBindingElement httpsBindingElement = new HttpsTransportBindingElement();
               httpsBindingElement.MaxReceivedMessageSize *= MessageMultiplier;

               //Try the HTTPS MEX Endpoint
               try
               {
                  endpoints = GetEndpoints(httpsBindingElement);
               }
               catch
               {
               }
               //Try over HTTP-GET
               if(endpoints == null)
               {
                  string httpsGetAddress = mexAddress;
                  if(mexAddress.EndsWith("?wsdl") == false)
                  {
                     httpsGetAddress += "?wsdl";
                  }
                  CustomBinding binding = new CustomBinding(httpsBindingElement);
                  MetadataExchangeClient MEXClient = new MetadataExchangeClient(binding);
                  MetadataSet metadata = MEXClient.GetMetadata(new Uri(httpsGetAddress),MetadataExchangeClientMode.HttpGet);
                  MetadataImporter importer = new WsdlImporter(metadata);
                  endpoints = importer.ImportAllEndpoints();
               }
            }
            if(address.Scheme == "net.tcp")
            {
               TcpTransportBindingElement tcpBindingElement = new TcpTransportBindingElement();
               tcpBindingElement.MaxReceivedMessageSize *= MessageMultiplier;
               endpoints = GetEndpoints(tcpBindingElement);
            }
            if(address.Scheme == "net.pipe")
            {
               NamedPipeTransportBindingElement ipcBindingElement = new NamedPipeTransportBindingElement();
               ipcBindingElement.MaxReceivedMessageSize *= MessageMultiplier;
               endpoints = GetEndpoints(ipcBindingElement);
            }
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
      


      void ProcessMetaData(ServiceNode existingNode,string mexAddress,ServiceEndpointCollection endpoints)
      {
         ProcessMetaData(existingNode,mexAddress,endpoints.ToArray());
      }
      void ProcessMetaData(ServiceNode existingNode,string mexAddress,ServiceEndpoint[] endpoints)
      {
         if(existingNode == null)
         {
            if(endpoints.Length == 0)
            {
               CurrentNode = new ServiceNode(mexAddress,this,"Service has no endpoints",ServiceIndex,ServiceIndex);
               m_MexTree.Nodes.Add(CurrentNode);
               return;
            }
            else
            {
               CurrentNode = new ServiceNode(mexAddress,this,"Exploring...",ServiceIndex,ServiceIndex);
               m_MexTree.Nodes.Add(CurrentNode);
            }
         }
         else
         {
            CurrentNode = existingNode;
            
            if(endpoints.Length == 0)
            {
               CurrentNode.Text = "Service has no endpoints";
               return;
            }
            else
            {
               CurrentNode.Text = "Exploring...";
               CurrentNode.Nodes.Clear();
            }
         }
         int index = 1;
         foreach(ServiceEndpoint endpoint in endpoints)
         {
            AddEndPoint(endpoint,"Endpoint"+index);
            index++;
         }
         DisplayServiceControl();
      }

      static string[] DiscoverMexAddresses()
      {
         DiscoveryClient discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint());
         FindResponse discovered = discoveryClient.Find(FindCriteria.CreateMetadataExchangeEndpointCriteria());

         return discovered.Endpoints.Select(mexEndpoint => mexEndpoint.Address.Uri.AbsoluteUri).ToArray();
      }
      void OnDiscover(object sender,EventArgs e)
      {
         m_DiscoverButton.Enabled = false;

         DisplayBlankControl();

         string currentAddress = m_MexAddressTextBox.Text;
         
         SplashScreen splash = new SplashScreen(Resources.Progress);
         try
         {
            DiscoverIntranet();
            DiscoverServiceBus();
         }
         finally
         {
            m_MexAddressTextBox.Text = currentAddress;
            splash.Close();
            m_DiscoverButton.Enabled = true;
         }
      }

      void DiscoverIntranet()
      {
         string[] addresses = DiscoverMexAddresses();
         foreach(string address in addresses)
         {
            if(IsServiceBusAddress(address))
            {
               continue;
            }
            m_MexAddressTextBox.Text = address;
            Explore(address);
         }
      }

      void OnExplore(object sender,EventArgs e)
      {
         m_ExploreButton.Enabled = false;

         string mexAddress = m_MexAddressTextBox.Text;
         if(String.IsNullOrEmpty(mexAddress))
         {
            return;
         }
         DisplayBlankControl();

         
         if(IsServiceBusAddress(mexAddress))
         {
            ExploreServiceBus();
            return;
         }
         
         SplashScreen splash = new SplashScreen(Resources.Progress);         
         try
         {
            Explore(mexAddress);
         }
         finally
         {
            splash.Close();
            m_ExploreButton.Enabled = true;
         }
      }
      ServiceEndpointCollection GetEndpoints(BindingElement bindingElement)
      {
         CustomBinding binding = new CustomBinding(bindingElement);

         MetadataExchangeClient MEXClient = new MetadataExchangeClient(binding);
         MetadataSet metadata = MEXClient.GetMetadata(new EndpointAddress(m_MexAddressTextBox.Text));
         MetadataImporter importer = new WsdlImporter(metadata);
         return importer.ImportAllEndpoints();
      }

       void AddContract(TreeNode endpointNode,ContractDescription contract)
      {
         TreeNode contractNode = new ContractNode(this,contract,contract.Name,ContractIndex,ContractIndex);
         endpointNode.Nodes.Add(contractNode);

         foreach(OperationDescription operation in contract.Operations)
         {
            AddOperation(contractNode,operation);
         }
      }
      void AddCallbackContract(TreeNode endpointNode,ContractDescription contract)
      {
      }
      internal static string ExtractTypeName(Type type)
      {
         string typeName = type.ToString();
         string typeNamespace = type.Namespace;

         if(typeNamespace == null)
         {
            return typeName;
         }

         return typeName.Substring(typeNamespace.Length+1,typeName.Length-typeNamespace.Length-1);
      }
      void AddBinding(TreeNode endpointNode,System.ServiceModel.Channels.Binding binding)
      {
         string bindingName;

         if(IsServiceBusAddress(m_MexAddressTextBox.Text))
         {
            if(binding.GetType() == typeof(CustomBinding))
            {
               bindingName = (binding.Name.Split('_'))[0];
            }
            else
            {
               bindingName = ExtractTypeName(binding.GetType());
            }
         }
         else
         {
            bindingName = ExtractTypeName(binding.GetType());
         }

         TreeNode bindingNode = new BindingNode(this,binding,bindingName,BindingIndex,BindingIndex);
         endpointNode.Nodes.Add(bindingNode);
      }
      void AddAddress(TreeNode endpointNode,EndpointAddress address)
      {
         TreeNode addressNode = new AddressNode(this,address,address.Uri.AbsoluteUri,AddressIndex,AddressIndex);
         endpointNode.Nodes.Add(addressNode);
      }
      void AddOperation(TreeNode contractNode,OperationDescription operation)
      {
         TreeNode operationNode = new OperationNode(this,operation,operation.Name,OperationIndex,OperationIndex);
         contractNode.Nodes.Add(operationNode);
      }
      void AddEndPoint(ServiceEndpoint endpoint,string name)
      {
         TreeNode endpointNode = new EndpointNode(this,endpoint,name,EndpointIndex,EndpointIndex);

         AddAddress(endpointNode,endpoint.Address);
         AddBinding(endpointNode,endpoint.Binding);
         AddContract(endpointNode,endpoint.Contract);

         CurrentNode.Nodes.Add(endpointNode);
      }

      void OnItemSelected(object sender,TreeViewEventArgs treeEventArgs)
      {
         MexNode node = treeEventArgs.Node as MexNode;
         node.DisplayControl();
      }
      void DiplayControl(NodeViewControl control)
      {
         m_CurrentViewControl.Visible = false;
         control.Visible = true;
         m_CurrentViewControl = control;
      }
      internal void DisplayEndpointConrol(ServiceEndpoint endpoint)
      {
         m_EndpointViewControl.Refresh(endpoint);
         DiplayControl(m_EndpointViewControl);
      }
      internal void DisplayBlankControl()
      {
         DiplayControl(m_BlankViewControl);
      }
      internal void DisplayServiceControl()
      {
         if(CurrentNode.Text == "Unspecified Base Address" || CurrentNode.Text == "Invalid Address")
         {
            DisplayBlankControl();
            return;
         }
         m_ServiceViewControl.Refresh(m_MexAddressTextBox.Text);
         DiplayControl(m_ServiceViewControl);
         string serviceName = "";
         while(serviceName == "")
         {
            Application.DoEvents();
            {
               serviceName = m_ServiceViewControl.ExtractServiceName();
            }
         }
         if(serviceName == "Internet Explorer cannot display the webpage")
         {
            serviceName = "Unknown";
         }
         CurrentNode.Text = serviceName;
         if(CurrentNode.Text == "Unknown")
         {
            DisplayBlankControl();
         }
      }

      internal void DisplayBindingConrol(System.ServiceModel.Channels.Binding binding)
      {
         m_BindingViewControl.Refresh(binding);
         DiplayControl(m_BindingViewControl);
      }

      internal void DisplayOperationConrol(OperationDescription operation)
      {
         m_OperationViewControl.Refresh(operation);
         DiplayControl(m_OperationViewControl);
      }

      internal void DisplayContractConrol(ContractDescription contract)
      {
         m_ContractViewControl.Refresh(contract);
         DiplayControl(m_ContractViewControl);
      }

      internal void DisplayAddressConrol(EndpointAddress address)
      {
         m_AddressViewControl.Refresh(address);
         DiplayControl(m_AddressViewControl);
      }

      void OnGenerateProxy(object sender,EventArgs e)
      {
         string currentDirectoty = Directory.GetCurrentDirectory();
         string arguments =  m_MexAddressTextBox.Text + @"  /Out:Proxy.cs /noconfig";
         try
         {
            Process.Start(@"C:\Program Files\Microsoft SDKs\Windows\v6.0\Bin\SvcUtil.exe",arguments);
            BringToFront();
         }
         catch
         {
            MessageBox.Show("Cannot Find SvcUtil.exe","Metadata Explorer",MessageBoxButtons.OK,MessageBoxIcon.Error);
         }
      }

      void OnAbout(object sender,EventArgs e)
      {
         AboutBox about = new AboutBox();
         about.ShowDialog();
      }

      void OnClosed(object sender,FormClosedEventArgs e)
      {
         m_AnnouncementSink.Close();

         foreach(string serviceNamespace in m_ServiceBusAnnouncementSinks.Keys)
         {
            m_ServiceBusAnnouncementSinks[serviceNamespace].Close();
         }
      }
   }
}