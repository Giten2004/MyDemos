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

      public LogonDialog(string serviceNamespace,string issuer)
      {
         InitializeComponent();

         Debug.Assert(String.IsNullOrEmpty(serviceNamespace) == false);

         m_CertNValueTextBox.Text = serviceNamespace;
         m_FindValueComboBox.Text = m_FindValueComboBox.Items[0] as string;
         m_StoreLoctionComboBox.Text = m_StoreLoctionComboBox.Items[0] as string;
         m_StoreNameComboBox.Text = m_StoreNameComboBox.Items[0] as string;

         m_IssuerTextBox.Text = issuer;

         OnTextChanged(this,EventArgs.Empty);
      }

      void OnLogon(object sender,EventArgs e)
      {
         Debug.Assert(String.IsNullOrEmpty(m_SecretTextBox.Text) == false);
         Secret = m_SecretTextBox.Text;
         Issuer = m_IssuerTextBox.Text;

         Close();
      }

      void OnTextChanged(object sender,EventArgs e)
      {
         m_LogonButton.Enabled = String.IsNullOrEmpty(m_SecretTextBox.Text) == false;
      }
   }
}
