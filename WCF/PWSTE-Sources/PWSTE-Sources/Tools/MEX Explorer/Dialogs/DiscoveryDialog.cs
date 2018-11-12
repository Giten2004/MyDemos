// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Windows.Forms;
using System.Diagnostics;


namespace ServiceModelEx
{
   partial class DiscoveryDialog : Form
   {
      public string ServiceNamespace
      {
         get
         {
            return m_NamespaceTextbox.Text;
         }
         set
         {
            if(ExplorerForm.IsServiceBusAddress(value))
            {
               m_NamespaceTextbox.Text = value;
            }
         }
      }
      public string AnnouncementsPath
      {
         get
         {
            if(m_AnnouncementsEnabledCheckbox.Checked)
            {
               return m_AnnouncementsTextBox.Text;
            }
            return String.Empty;
         }
      }

      public string DiscoveryPath
      {
         get
         {
            return m_DiscoveryTextbox.Text;
         }
      }
          
      public DiscoveryDialog(string serviceNamespace,string discoveryUri,bool announcementsEnabled,string announcementsUri)
      {
         InitializeComponent();

         if(String.IsNullOrEmpty(serviceNamespace))
         {
            serviceNamespace = "Enter service namespace";
         }

         m_NamespaceTextbox.Text = serviceNamespace;

         m_DiscoveryTextbox.Text = discoveryUri;
         m_AnnouncementsEnabledCheckbox.Checked = announcementsEnabled;
         m_AnnouncementsTextBox.Text = announcementsUri;

         OnEnableChanged(this,EventArgs.Empty);
         OnNamespaceTextChanged(this,EventArgs.Empty);
      }
      void OnDone(object sender,EventArgs e)
      {
         Close();
      }

      void OnEnableChanged(object sender,EventArgs e)
      {
         m_AnnouncementsTextBox.Enabled = m_AnnouncementsEnabledCheckbox.Checked;
      }
      void OnNamespaceTextChanged(object sender,EventArgs e)
      {
         m_DoneButton.Enabled = m_NamespaceTextbox.Text.Contains(" ") == false;
      }
   }
}
