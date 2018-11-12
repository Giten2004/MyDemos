using System;
using System.Windows.Forms;
using System.ServiceModel.Description;

namespace ServiceModelEx
{
   partial class EndpointViewControl : NodeViewControl
   {
      public EndpointViewControl()
      {
         InitializeComponent();
      }
      public void Refresh(ServiceEndpoint endpoint)
      {
         m_NameLabel.Text = "Name: " + endpoint.Name;
         m_ExplicitRadioButton.Checked = endpoint.ListenUriMode == ListenUriMode.Explicit;
         m_UniqueRadioButton.Checked = endpoint.ListenUriMode == ListenUriMode.Unique;
         m_ListeningTextBox.Text = endpoint.ListenUri.AbsoluteUri;
      }
   }
}
