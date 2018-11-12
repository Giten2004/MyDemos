using System;
using System.Windows.Forms;
using System.ServiceModel;

namespace ServiceModelEx
{
   partial class AddressViewControl : NodeViewControl
   {
      public AddressViewControl()
      {
         InitializeComponent();
      }
      public void Refresh(EndpointAddress address)
      {
         if(address.Identity != null)
         {
            m_IdentityLabel.Text = "Identity: " + address.Identity.ToString();
         }
         else
         {
            m_IdentityLabel.Text = "Identity: None";
         }

         if(address.IsNone)
         {
            m_IsNoneLabel.Text = "Is None: Yes";
         }
         else
         {
            m_IsNoneLabel.Text = "Is None: No";
         }
         if(address.IsAnonymous)
         {
            m_IsAnonymousLabel.Text = "Is Anonymous: Yes";
         }
         else
         {
            m_IsAnonymousLabel.Text = "Is Anonymous: No";
         }
         m_LocalPathTextBox.Text = address.Uri.AbsolutePath;
         m_AbsoluteURITextBox.Text = address.Uri.AbsoluteUri;
         m_AuthorityLabel.Text = "Authority: " + address.Uri.Authority;
         m_DnsSafeHostLabel.Text = "DNS Safe Host: " +address.Uri.DnsSafeHost;
         m_HostLabel.Text = "Host: " + address.Uri.Host;
         m_HostNameTypeLabel.Text = "Host Name Type:" + address.Uri.HostNameType.ToString();
         if(address.Uri.IsAbsoluteUri)
         {
            m_IsAbsoluteUriLabel.Text = "Absolute URI: Yes";
         }
         else
         {
            m_IsAbsoluteUriLabel.Text = "Absolute URI: No";
         }
         if(address.Uri.IsLoopback)
         {
            m_IsLoopbackLabel.Text = "Loopback: Yes";
         }
         else
         {
            m_IsLoopbackLabel.Text = "Loopback: No";
         }
         m_LocalPathTextBox.Text = address.Uri.LocalPath;
         m_PortLabel.Text = "Port: " + address.Uri.Port.ToString();
         m_SchemaLabel.Text = "Schema: " + address.Uri.Scheme;
      }
   }
}
