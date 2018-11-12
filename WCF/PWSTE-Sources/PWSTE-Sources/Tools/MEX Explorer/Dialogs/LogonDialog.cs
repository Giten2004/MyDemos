// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Windows.Forms;
using System.Diagnostics;


namespace ServiceModelEx
{
   partial class LogonDialog : Form
   {
      public string Secret
      {get;private set;}

      public string Issuer
      {get;private set;}

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
      
      public LogonDialog(string serviceNamespace,string issuer)
      {
         InitializeComponent();

         if(String.IsNullOrEmpty(serviceNamespace))
         {
            serviceNamespace = "Enter service namespace";
            m_LogonButton.Enabled = false;
         }

         m_NamespaceTextbox.Text = serviceNamespace;
         m_IssuerTextBox.Text = issuer;

         OnSecretTextChanged(this,EventArgs.Empty);
         OnNamespaceTextChanged(this,EventArgs.Empty);
      }

      void OnLogon(object sender,EventArgs e)
      {
         Debug.Assert(String.IsNullOrEmpty(m_SecretTextBox.Text) == false);
         Secret = m_SecretTextBox.Text;
         Issuer = m_IssuerTextBox.Text;

         Close();
      }

      void OnSecretTextChanged(object sender,EventArgs e)
      {
         m_LogonButton.Enabled = String.IsNullOrEmpty(m_SecretTextBox.Text) == false;
      }

      void OnNamespaceTextChanged(object sender,EventArgs e)
      {
         m_LogonButton.Enabled = m_NamespaceTextbox.Text.Contains(" ") == false;
      }
   }
}
