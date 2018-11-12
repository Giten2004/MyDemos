using System;
using System.Windows.Forms;
using System.ServiceModel.Description;
using System.Net.Security;
using ServiceModelEx.Properties;
using System.ServiceModel;

namespace ServiceModelEx
{
   partial class OperationViewControl : NodeViewControl
   {
      const int FaultIndex = 0;
      public OperationViewControl()
      {
         InitializeComponent();

          m_FaultsListView.SmallImageList = new ImageList();
          m_FaultsListView.SmallImageList.Images.Add(Resources.error);
      }
      public void Refresh(OperationDescription operation)
      {
         m_NameLabel.Text = "Name: " + operation.Name;
         if(operation.IsOneWay)
         {
            m_OneWayLabel.Text = "One Way: Yes";
         }
         else
         {
            m_OneWayLabel.Text = "One Way: No";
         }
         AddFaults(operation);
         AddKnownTypes(operation);

         m_NonProtectionRadioButton.Enabled = false;
         m_SignedRadioButton.Enabled = false;
         m_EncryptRadioButton.Enabled = false;

         if(operation.HasProtectionLevel)
         {
            SetProtectionLevel(operation.ProtectionLevel);
         }
         SetTransactionFlow(operation);
         SetFormat(operation);
      }

      void SetFormat(OperationDescription operation)
      {
         foreach(IOperationBehavior behavior in operation.Behaviors)
         {
            if(behavior is DataContractSerializerOperationBehavior)
            {
               DataContractSerializerOperationBehavior serializerBehavior = behavior as DataContractSerializerOperationBehavior;
               m_FormatStyleLabel.Text = "Format: " + serializerBehavior.DataContractFormatAttribute.Style;
               break;
            }
         }
      }

      void SetTransactionFlow(OperationDescription operation)
      {
         TransactionFlowOption option = TransactionFlowOption.NotAllowed;
         foreach(IOperationBehavior behavior in operation.Behaviors)
         {
            if(behavior is TransactionFlowAttribute)
            {
               TransactionFlowAttribute attribute = behavior as TransactionFlowAttribute;
               option = attribute.Transactions;
               break;
            }
         }
         m_AllowedRadioButton.Checked = option == TransactionFlowOption.Allowed;
         m_NotAllowedRadioButton.Checked = option == TransactionFlowOption.NotAllowed;
         m_MandatoryRadioButton.Checked = option == TransactionFlowOption.Mandatory;
      }

      void AddKnownTypes(OperationDescription operation)
      {
         m_KnownTypesListView.Items.Clear();
         foreach(Type type in operation.KnownTypes)
         {
            string typeName = ExplorerForm.ExtractTypeName(type);
            m_KnownTypesListView.Items.Add(typeName);
         }
      }

      void AddFaults(OperationDescription operation)
      {
         m_FaultsListView.Items.Clear();
         foreach(FaultDescription fault in operation.Faults)
         {
            ListViewItem item = new ListViewItem(fault.Name,FaultIndex);
            m_FaultsListView.Items.Add(item);
         }
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
   }
}
