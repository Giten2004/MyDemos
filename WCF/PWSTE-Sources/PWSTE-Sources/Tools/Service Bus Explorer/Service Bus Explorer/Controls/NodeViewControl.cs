// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.ServiceBus;

namespace ServiceModelEx.ServiceBus
{
   partial class NodeViewControl : UserControl
   {
      protected string Address
      {get;set;}

      protected ServiceBusNode Node
      {get;set;}

      protected TransportClientEndpointBehavior Credential
      {get;set;}

      protected void Explore()
      {
         ExplorerForm form = Application.OpenForms[0] as ExplorerForm;
         form.OnExplore(this,EventArgs.Empty);
      }
      public NodeViewControl()
      {
         InitializeComponent();
         Visible = false;
      }
      public virtual void Refresh(ServiceBusNode node,TransportClientEndpointBehavior credential)
      {
         Node = node;
         Credential = credential;
         Address = GetRealAddress(node.Address).AbsoluteUri;

         m_ItemNameLabel.Text = node.Name;
         m_AddressLabel.Text = GetRealAddress(node.Address).AbsoluteUri;

         TrimLable(m_ItemNameLabel,34);
         TrimLable(m_AddressLabel,69);
      }

      protected virtual void OnCopy(object sender,EventArgs e)
      {
         Clipboard.SetText(Address);
      }
      protected Uri RealAddress
      {
         get
         {
            string address = Address;

            //The feed does not show correct transport - always http/https, so remove all transports 
            address = address.Replace(@"https://",@"sb://");
            address = address.Replace(@"http://",@"sb://");

            return new Uri(address);
         }
      }

      protected static Uri GetRealAddress(string address)
      {
         //The feed does not show correct transport - always http/https, so remove all transports 
         address = address.Replace(@"https://",@"sb://");
         address = address.Replace(@"http://",@"sb://");

         return new Uri(address);
      }

      protected static void TrimLable(Label label,int maxlength)
      {
         if(label.Text.Length > maxlength)
         {
            label.Text = label.Text.Substring(0,maxlength) + "...";
         }
      }
   }
}
