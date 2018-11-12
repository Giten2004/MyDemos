// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.ServiceBus;
using ServiceModelEx.Properties;
using WinFormsEx;

namespace ServiceModelEx.ServiceBus
{
   partial class ExplorerForm : Form
   {
      const string DefaultIssuer = ServiceBusHelper.DefaultIssuer;

      public const int UnspecifiedNamespaceIndex   = 0;
      public const int NamespaceIndex              = 1;
      public const int RouterIndex              = 2;
      public const int BufferIndex              = 3;
      public const int EventEndpointIndex       = 4;
      public const int EndpointIndex            = 5;
      public const int ServiceError             = 6;
      
      NodeViewControl m_CurrentViewControl;
      public Dictionary<string,ServiceBusGraph> Graphs = new Dictionary<string,ServiceBusGraph>();

      ServiceBusTreeNode m_DraggedNode;

      public string ServiceNamespace
      {
         get
         {
            return m_NamespaceTextBox.Text;
         }
      }

      public ExplorerForm()
      {
         InitializeComponent();
         m_ServiceBusTree.ImageList = new ImageList();
         m_ServiceBusTree.ImageList.Images.Add(Resources.UnspecifiedSolution);
         m_ServiceBusTree.ImageList.Images.Add(Resources.Solution);
         m_ServiceBusTree.ImageList.Images.Add(Resources.Router);
         m_ServiceBusTree.ImageList.Images.Add(Resources.Queue);
         m_ServiceBusTree.ImageList.Images.Add(Resources.EventEndpoint);
         m_ServiceBusTree.ImageList.Images.Add(Resources.Endpoint);
         m_ServiceBusTree.ImageList.Images.Add(Resources.ServiceError);

         m_CurrentViewControl = m_BlankViewControl;
         DisplayBlankControl();

         TreeNode blank = new NamespaceTreeNode(this);
         m_ServiceBusTree.Nodes.Add(blank);

         SelectNamespaceTextBox();
      }


      void AddNodesToTree(TreeView tree,ServiceBusNode[] nodes)
      {
         string serviceNamespace = m_NamespaceTextBox.Text;

         if(tree.Nodes[0].Text == "Unspecified Namespace")
         {
            tree.Nodes.Clear();
         }
         else
         {
            foreach(TreeNode domianNode in tree.Nodes)
            {
               if(domianNode.Text == serviceNamespace)
               {
                  tree.Nodes.Remove(domianNode);
                  break;
               }
            }
         }
         TreeNode newNamespaceNode = new NamespaceTreeNode(this,serviceNamespace);
         tree.Nodes.Add(newNamespaceNode);

         tree.SelectedNode = newNamespaceNode;
         tree.Focus();

         foreach(ServiceBusNode node in nodes)
         {
            AddNode(newNamespaceNode,node);
         }
      }
      ServiceBusTreeNode MatchTreeNode(ServiceBusNode node)
      {
         if(node == null)
         {
            return new NamespaceTreeNode(this,m_NamespaceTextBox.Text);
         }
         if(node.Policy != null)
         {
            if(node.Policy is RouterPolicy)
            {
               return new RouterTreeNode(this,node,node.Name);
            }
            else
            {
               return new BufferTreeNode(this,node,node.Name);
            }
         }
         else
         {
            if(node.SubscribedTo != null)
            {
               return new RouterSubscriberTreeNode(this,node,node.Name);
            }
            else
            {
               return new EndpointTreeNode(this,node,node.Name);
            }
         }
      }      
                       
      void AddNode(TreeNode root,ServiceBusNode nodeToAdd)
      {
         TreeNode newTreeNode = MatchTreeNode(nodeToAdd);

         root.Nodes.Add(newTreeNode);
         if(nodeToAdd.Subscribers == null)
         {
            return;
         }
         foreach(ServiceBusNode subscriber in nodeToAdd.Subscribers)
         {
            AddNode(newTreeNode,subscriber);
         }
      }
      public ServiceBusTreeNode SelectedTreeNode
      {
         get
         {
            return m_ServiceBusTree.SelectedNode as ServiceBusTreeNode;
         }
         set
         {
            m_ServiceBusTree.SelectedNode = value;
            m_ServiceBusTree.Select();

            value.DisplayControl();
         }
      }
      public void OnExplore(object sender,EventArgs e)
      {
         Cursor currentCursor = Cursor.Current;
         Cursor.Current = Cursors.WaitCursor;

         m_ExploreToolStripMenuItem.Enabled = m_ExploreButton.Enabled = false;
         
         string serviceNamespace = m_NamespaceTextBox.Text;

         if(String.IsNullOrEmpty(serviceNamespace))
         {
            MessageBox.Show("You need to provide a service namespace","Service Bus Explorer",MessageBoxButtons.OK,MessageBoxIcon.Error);
            m_ExploreToolStripMenuItem.Enabled = m_ExploreButton.Enabled = true;

            Cursor.Current = currentCursor;

            return;
         }

         if(Graphs.ContainsKey(serviceNamespace.ToLower()) == false)
         {
            LogonDialog dialog = new LogonDialog(m_NamespaceTextBox.Text,DefaultIssuer);
            dialog.ShowDialog();

            if(dialog.Secret == null)
            {
               m_ExploreToolStripMenuItem.Enabled = m_ExploreButton.Enabled = true;
               Cursor.Current = currentCursor;

               return;
            }
            try
            {
               Graphs[serviceNamespace.ToLower()] = new ServiceBusGraph(serviceNamespace,dialog.Issuer,dialog.Secret);
            }
            catch(Exception exception)
            {
               MessageBox.Show("Invalid namespace name: " + exception.Message,"Service Bus Explorer",MessageBoxButtons.OK,MessageBoxIcon.Error);
               m_ExploreToolStripMenuItem.Enabled = m_ExploreButton.Enabled = true;
               Cursor.Current = currentCursor;
               return;
            }
         }
          
         SplashScreen splash = new SplashScreen(Resources.Progress);         

         try
         {
            Application.DoEvents();

            ServiceBusNode[] nodes = Graphs[serviceNamespace.ToLower()].Discover();

            AddNodesToTree(m_ServiceBusTree,nodes);

            DisplayNamespaceControl(serviceNamespace);
         }
         catch(Exception exception)
         {
            MessageBox.Show("Some error occurred discovering the service namespace: " + exception.Message,"Service Bus Explorer",MessageBoxButtons.OK,MessageBoxIcon.Error);

            for(int index = 0;index < m_ServiceBusTree.Nodes.Count;index++)
            {
               if(m_ServiceBusTree.Nodes[index].Text == serviceNamespace)
               {
                  m_ServiceBusTree.Nodes.Add(new NamespaceTreeNode(this,serviceNamespace,ServiceError));
                  m_ServiceBusTree.Nodes.RemoveAt(index);
                  break;
               }
            }
         }
         finally
         {
            splash.Close();
            m_ExploreToolStripMenuItem.Enabled = m_ExploreButton.Enabled = true;
            Cursor.Current = currentCursor;
         }
      } 
      void OnItemSelected(object sender,TreeViewEventArgs treeEventArgs)
      {
         ServiceBusTreeNode node = treeEventArgs.Node as ServiceBusTreeNode;

         if(node != null)
         {
            TreeNode namespaceNode = node;
            if(namespaceNode.Parent != null)
            {
               while(namespaceNode.Parent is NamespaceTreeNode == false)
               {
                  namespaceNode = namespaceNode.Parent;
               }

               m_NamespaceTextBox.Text = namespaceNode.Parent.Text;
            }
         }
         node.DisplayControl();
      }
      void DisplayControl(NodeViewControl control)
      {
         m_CurrentViewControl.Visible = false;
         control.Visible = true;
         m_CurrentViewControl = control;
      }
      internal void DisplayBlankControl()
      {
         DisplayControl(m_BlankViewControl);
      }

      internal void DisplayNamespaceControl(string serviceNamespace)
      {
         if(serviceNamespace == "Unspecified Namespace")
         {
            DisplayBlankControl();
            return;
         }
         m_NamespaceTextBox.Text = serviceNamespace;
         m_NamespaceViewControl.Refresh(serviceNamespace);
         DisplayControl(m_NamespaceViewControl);
      }
      internal void DisplayBufferControl(ServiceBusNode node,TransportClientEndpointBehavior credential)
      {
         m_BufferViewControl.Refresh(node,credential);
         DisplayControl(m_BufferViewControl);
      }
      internal void DisplayRouterSubscriberControl(ServiceBusNode node,TransportClientEndpointBehavior credential)
      {
         m_RouterSubscriberViewControl.Refresh(node,credential);
         DisplayControl(m_RouterSubscriberViewControl);
      }
      internal void DisplayEndpointControl(ServiceBusNode node,TransportClientEndpointBehavior credential)
      {
         m_EndpointViewControl.Refresh(node,credential);
         DisplayControl(m_EndpointViewControl);
      }
      internal void DisplayRouterControl(ServiceBusNode node,TransportClientEndpointBehavior credential)
      {
         m_RouterViewControl.Refresh(node,credential);
         DisplayControl(m_RouterViewControl);
      }
      void OnAbout(object sender,EventArgs e)
      {
         AboutBox about = new AboutBox();
         about.ShowDialog();
      }

      void OnLogon(object sender,EventArgs e)
      {
         LogonDialog dialog = new LogonDialog(m_NamespaceTextBox.Text,DefaultIssuer);
         dialog.ShowDialog();

         string serviceNamespace = m_NamespaceTextBox.Text;

         Graphs[serviceNamespace.ToLower()] = new ServiceBusGraph(serviceNamespace,dialog.Issuer,dialog.Secret);
      }

      void OnTimer(object sender,EventArgs e)
      {
         string serviceNamespace = m_NamespaceTextBox.Text;
         m_LogonMenuItem.Enabled = Graphs.ContainsKey(serviceNamespace.ToLower()) == false;

         //m_NewRouterMenuItem.Enabled = !m_LogonMenuItem.Enabled;
         m_NewBufferMenuItem.Enabled = !m_LogonMenuItem.Enabled;
         //m_DeleteAllRoutersMenuItem.Enabled = !m_LogonMenuItem.Enabled;
         m_DeleteAllBuffersMenuItem.Enabled = !m_LogonMenuItem.Enabled;
         //m_DeleteAllRutersAndBuffersMenuItem.Enabled = !m_LogonMenuItem.Enabled;
      }
      internal void SelectNamespaceTextBox()
      {
         m_NamespaceTextBox.Focus();
      }

      void OnNewRouter(object sender,EventArgs e)
      {
         string serviceNamespace = m_NamespaceTextBox.Text;
         NewRouterDialog dialog = new NewRouterDialog(serviceNamespace);
         dialog.ShowDialog();

         if(dialog.Client != null)
         {
            OnExplore(this,EventArgs.Empty);
         }
      }

      void OnNewBuffer(object sender,EventArgs e)
      {
         string serviceNamespace = m_NamespaceTextBox.Text;
         NewBufferDialog dialog = new NewBufferDialog(serviceNamespace);
         dialog.ShowDialog();

         if(dialog.Client != null)
         {
            OnExplore(this,EventArgs.Empty);
         }
      }

      void OnDeleteAllRouters(object sender,EventArgs e)
      {       
         DialogResult result = MessageBox.Show("Are you sure you want to delete all routers? You will also lose all subscribers","Service Bus Explorer",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
         if(result == DialogResult.No)
         {
            return;
         }
         DeleteAllRouters();
         OnExplore(this,EventArgs.Empty);
      }
      void DeleteAllRouters()
      {
         foreach(TreeNode node in m_ServiceBusTree.Nodes)
         {
            DeleteRouters(node as ServiceBusTreeNode);
         }
      }
      void DeleteRouters(ServiceBusTreeNode treeNode)
      {
         if(treeNode.ServiceBusNode != null)
         {
            if(treeNode.ServiceBusNode.Policy != null)
            {
               if(treeNode.ServiceBusNode.Policy is RouterPolicy)
               {
                  string nodeAddress = treeNode.ServiceBusNode.Address;
                  nodeAddress = nodeAddress.Replace(@"https://",@"sb://");
                  nodeAddress = nodeAddress.Replace(@"http://",@"sb://");

                  TransportClientEndpointBehavior credential = Graphs[ServiceNamespace.ToLower()].Credential;
                  Uri address = new Uri(nodeAddress);
                  try
                  {
                     RouterManagementClient.DeleteRouter(credential,address);
                  }
                  catch
                  {}
               }
            }
         }
         foreach(TreeNode node in treeNode.Nodes)
         {
            DeleteRouters(node as ServiceBusTreeNode);
         }
      }

      void OnDeleteAllBuffers(object sender,EventArgs e)
      {       
         DialogResult result = MessageBox.Show("Are you sure you want to delete all buffers?","Service Bus Explorer",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
         if(result == DialogResult.No)
         {
            return;
         }
         DeleteAllBuffers();
         OnExplore(this,EventArgs.Empty);
      }
      void DeleteAllBuffers()
      {
         foreach(TreeNode node in m_ServiceBusTree.Nodes)
         {
            DeleteBuffers(node as ServiceBusTreeNode);
         }
      }
      void DeleteBuffers(ServiceBusTreeNode treeNode)
      {
         if(treeNode.ServiceBusNode != null)
         {
            if(treeNode.ServiceBusNode.Policy != null)
            {
               if(treeNode.ServiceBusNode.Policy is MessageBufferPolicy)
               {
                  string nodeAddress = treeNode.ServiceBusNode.Address;
                  nodeAddress = nodeAddress.Replace(@"sb://",@"https://");

                  TransportClientEndpointBehavior credential = Graphs[ServiceNamespace.ToLower()].Credential;
                  Uri address = new Uri(nodeAddress);
                  try
                  {
                     MessageBufferClient.GetMessageBuffer(credential,address).DeleteMessageBuffer();
                  }
                  catch
                  {}
               }
            }
         }
         foreach(TreeNode node in treeNode.Nodes)
         {
            DeleteBuffers(node as ServiceBusTreeNode);
         }
      }

      void OnDeleteAllRoutersBuffers(object sender,EventArgs e)
      {
         DialogResult result = MessageBox.Show("Are you sure you want to delete all routers and buffers?","Service Bus Explorer",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
         if(result == DialogResult.No)
         {
            return;
         }
         DeleteAllRouters();
         DeleteAllBuffers();
         OnExplore(this,EventArgs.Empty);
      }
      void OnItemDrag(object sender,ItemDragEventArgs args)
      {
         m_DraggedNode = args.Item as ServiceBusTreeNode;

         if(m_DraggedNode != null)
         {
            if(m_DraggedNode.ServiceBusNode == null)
            {
               return;
            }
            if(m_DraggedNode.ServiceBusNode.Policy != null)//A router or a buffer
            {
               Cursor.Current = Cursors.Hand;
               DoDragDrop(m_DraggedNode,DragDropEffects.Link);
            }
         }
      }

      void OnDragDrop(object sender,DragEventArgs args)
      {           
         Cursor.Current = Cursors.Default;
         Debug.Assert(m_DraggedNode != null);

         ServiceBusTreeNode targetNode = GetTargetNode(args);
                                    
         if(targetNode.ServiceBusNode == null)
         {
            return;
         }
         if(targetNode.ServiceBusNode.Policy != null)//A router or a buffer
         {
            if(targetNode.ServiceBusNode.Policy is RouterPolicy)
            {
               Trace.WriteLine("Droped at: " + targetNode.Text);
               string draggedAddress = m_DraggedNode.ServiceBusNode.Address;
               draggedAddress = draggedAddress.Replace(@"https://",@"sb://");
               draggedAddress = draggedAddress.Replace(@"http://",@"sb://");

               string targetAddress = targetNode.ServiceBusNode.Address;
               targetAddress = targetAddress.Replace(@"https://",@"sb://");
               targetAddress = targetAddress.Replace(@"http://",@"sb://");
               
               TransportClientEndpointBehavior credential = Graphs[ServiceNamespace.ToLower()].Credential;

               Uri draggedUri = new Uri(draggedAddress);
               Uri targetUri = new Uri(targetAddress);

               try
               {
                  RouterClient targetClient  = RouterManagementClient.GetRouter(credential,targetUri);
                  if(m_DraggedNode.ServiceBusNode.Policy is RouterPolicy)
                  {
                     RouterClient draggedClient = RouterManagementClient.GetRouter(credential,draggedUri);

                     draggedClient.SubscribeToRouter(targetClient,TimeSpan.MaxValue);
                  }
                  else
                  {
                     MessageBufferClient draggedClient = MessageBufferClient.GetMessageBuffer(credential,draggedUri);
                     /* TODO Restore on next release
                     draggedClient.SubscribeToRouter(targetClient,TimeSpan.MaxValue);
                     */
                  }
                  OnExplore(this,EventArgs.Empty);

                  m_ServiceBusTree.SelectedNode = targetNode;
                  m_ServiceBusTree.Select();  
               }
               catch(Exception exception)
               {
                  MessageBox.Show("Unable to subscribe: " + exception.Message,"Service Bus Explorer",MessageBoxButtons.OK,MessageBoxIcon.Error);
               }
            }
         }
         m_DraggedNode = null;
		}

      void OnDragEnterOver(object sender,DragEventArgs args)
      {
         Debug.Assert(m_DraggedNode != null);
         
         ServiceBusTreeNode targetNode = GetTargetNode(args);

         if(targetNode == m_DraggedNode)
         {
            return;
         }
         if(targetNode.ServiceBusNode != null)
         {
            if(targetNode.ServiceBusNode.Policy != null)
            {
               if(targetNode.ServiceBusNode.Policy is RouterPolicy)
               {
                  foreach(ServiceBusNode node in targetNode.ServiceBusNode.Subscribers)
                  {
                     if(node.Address == m_DraggedNode.ServiceBusNode.Address)
                     {
                        Cursor.Current = Cursors.No;

                        return;
                     }
                  }
                  args.Effect = DragDropEffects.Link;
                  m_ServiceBusTree.SelectedNode = targetNode;
                  m_ServiceBusTree.Select();      
               }
            }
         }
      }
      ServiceBusTreeNode GetTargetNode(DragEventArgs args)
      {
         Point point = m_ServiceBusTree.PointToClient(new Point(args.X,args.Y));
			return m_ServiceBusTree.GetNodeAt(point) as ServiceBusTreeNode;
      }
   }
}