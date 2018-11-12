// © 2011 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Web.Services.Protocols;

namespace CredentialsManagerClient
{
   partial class ResetWithQuestionDialog : Form
   {
      string m_Url;
      string m_Application;
      PasswordManagerProxy m_PasswordManager;

      public ResetWithQuestionDialog(string url,string application,string user)
      {
         InitializeComponent();

         m_Url = url;
         m_Application = application;
         m_UserNameTextBox.Text = user;

         m_PasswordManager = new PasswordManagerProxy(m_Url);
         m_PasswordQuestionTextBox.Text = m_PasswordManager.GetPasswordQuestion(application,user);
      }
      void OnReset(object sender,EventArgs e)
      {
         if(m_PasswordAnswerTextBox.Text == String.Empty && m_PasswordAnswerTextBox.Enabled)
         {
            m_Validator.SetError(m_PasswordAnswerTextBox,"Password answer cannot be empty");
            return;
         }
         m_Validator.Clear();

         string newPassword = null;
         try
         {
            newPassword = m_PasswordManager.ResetPasswordWithQuestionAndAnswer(m_Application,m_UserNameTextBox.Text,m_PasswordAnswerTextBox.Text);
         }
         catch(SoapException exception)
         {
            if(exception.Message.Contains("The password-answer supplied is wrong"))
            {
               MessageBox.Show("The password-answer supplied is wrong. Please try agian.","Credentials Manager",MessageBoxButtons.OK,MessageBoxIcon.Error);
               return;
            }
            if(exception.Message.Contains("The user account has been locked out"))
            {
               MessageBox.Show("The user account has been locked out","Credentials Manager",MessageBoxButtons.OK,MessageBoxIcon.Error);
               Close();
               return;
            }
            
            throw;
         }
         Clipboard.SetText(newPassword);

         MessageBox.Show("Generated password: " + newPassword + " " + Environment.NewLine + "The password is avaiable on the clipboard as well.","Credentials Manager",MessageBoxButtons.OK,MessageBoxIcon.Information);

         Close();
      }

      void OnClosed(object sender,FormClosedEventArgs e)
      {
         m_PasswordManager.Close();
      }
   }
}