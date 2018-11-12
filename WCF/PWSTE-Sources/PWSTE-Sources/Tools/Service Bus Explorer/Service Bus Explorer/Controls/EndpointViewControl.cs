// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Windows.Forms;
using System.ServiceModel;
using Microsoft.ServiceBus;

namespace ServiceModelEx.ServiceBus
{
   partial class EndpointViewControl : NodeViewControl
   {
      string m_Address;

      public EndpointViewControl()
      {
         InitializeComponent();
      }
      public override void Refresh(ServiceBusNode node,TransportClientEndpointBehavior credential)
      {
         base.Refresh(node,credential);

         m_Address = Address;

         //The feed does not show correct transport - always http/https, so remove all transports 
         m_Address = m_Address.Replace(@"sb://",@"[transport]://");

         m_AddressLabel.Text = m_Address;
         TrimLable(m_AddressLabel,69);
      }
      protected override void OnCopy(object sender,EventArgs e)
      {
         Clipboard.SetText(m_Address);
      }
   }
}
