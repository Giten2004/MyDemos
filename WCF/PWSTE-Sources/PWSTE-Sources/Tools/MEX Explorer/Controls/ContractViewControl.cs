using System;
using System.Windows.Forms;
using System.ServiceModel.Description;
using System.Net.Security;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.ServiceModel;

namespace ServiceModelEx
{
   partial class ContractViewControl : NodeViewControl
   {
      public ContractViewControl()
      {
         InitializeComponent();
      }
      public void Refresh(ContractDescription contract)
      {
         m_NameLabel.Text = "Name: " + contract.Name;
         m_NamespaceLabel.Text = "Namespace: " + contract.Namespace;

         m_NonProtectionRadioButton.Enabled = false;
         m_SignedRadioButton.Enabled = false;
         m_EncryptRadioButton.Enabled = false;
         if(contract.HasProtectionLevel)
         {
            SetProtectionLevel(contract.ProtectionLevel);
         }
         SetSessionMode(contract.SessionMode);
      }
      void SetProtectionLevel(ProtectionLevel protectionLevel)
      {
         m_NonProtectionRadioButton.Enabled = true;
         m_SignedRadioButton.Enabled = true;
         m_EncryptRadioButton.Enabled = true;

         m_NonProtectionRadioButton.Checked = protectionLevel == ProtectionLevel.None;
         m_SignedRadioButton.Checked = protectionLevel == ProtectionLevel.Sign;
         m_EncryptRadioButton.Checked = protectionLevel == ProtectionLevel.EncryptAndSign;
      }
      void SetSessionMode(SessionMode mode)
      {
         m_AllowedRadioButton.Checked = mode == SessionMode.Allowed;
         m_NotAllowedRadioButton.Checked = mode == SessionMode.NotAllowed;
         m_RequiredRadioButton.Checked = mode == SessionMode.Required;
      }
   }
}
