// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Windows.Forms;
using System.ServiceModel;
using Microsoft.ServiceBus;

namespace ServiceModelEx.ServiceBus
{
   partial class RouterViewControl : NodeViewControl
   {
      DateTime m_Expiration;

      public RouterViewControl()
      {
         InitializeComponent();
      }
      //TODO Restore on next release
      public override void Refresh(ServiceBusNode node,TransportClientEndpointBehavior credential)
      {
         RouterPolicy policy = node.Policy as RouterPolicy;

         m_AllRadioButton.Checked = policy.MessageDistribution == MessageDistributionPolicy.AllSubscribers;
         m_OneRadioButton.Checked = policy.MessageDistribution == MessageDistributionPolicy.OneSubscriber;

         m_MaxSubscribersTextBox.Text = policy.MaxSubscribers.ToString();

         m_PushRetriesTextBox.Text = policy.PushDeliveryRetries.ToString();

         m_Expiration = policy.ExpirationInstant;
         m_ExpirationTimePicker.Value = m_Expiration;

         m_BufferLengthTextBox.Text = policy.MaxBufferLength.ToString();

         int overflowIndex = 0;
         switch(policy.OverflowPolicy)
         {
            case OverflowPolicy.RejectIncomingMessage:
            {
               overflowIndex = 0;
               break;
            }
            /* TODO Restore on next release
            case OverflowPolicy.DiscardIncomingMessage:
            {
               overflowIndex = 1;
               break;
            }
            case OverflowPolicy.DiscardExistingMessage:
            {
               overflowIndex = 2;
               break;
            }
            */
         }
         m_OverflowComboBox.Text = m_OverflowComboBox.Items[overflowIndex] as string;

         m_PurgeButton.Enabled = node.SubscribersCount > 0;

         base.Refresh(node,credential);
      }
      
      bool IsDirty(RouterPolicy policy)
      {
         if(m_AllRadioButton.Checked && policy.MessageDistribution == MessageDistributionPolicy.OneSubscriber)
         {
            return true;
         }
         if(m_OneRadioButton.Checked && policy.MessageDistribution == MessageDistributionPolicy.AllSubscribers)
         {
            return true;
         }

         if(m_MaxSubscribersTextBox.Text != "")
         {
            if(Convert.ToInt32(m_MaxSubscribersTextBox.Text) != policy.MaxSubscribers)
            {
               return true;
            }
         }
         if(m_PushRetriesTextBox.Text != "")
         {
            if(Convert.ToInt32(m_PushRetriesTextBox.Text) != policy.PushDeliveryRetries)
            {
               return true;
            }
         }

         if(m_ExpirationTimePicker.Value != m_Expiration)
         {
            return true;
         }

         if(m_BufferLengthTextBox.Text != "")
         {
            if(Convert.ToInt32(m_BufferLengthTextBox.Text) != policy.MaxBufferLength)
            {
               return true;
            }
         }

         switch(m_OverflowComboBox.Text)
         {
            case "Reject":
            {
               if(policy.OverflowPolicy != OverflowPolicy.RejectIncomingMessage)
               {
                  return true;
               }
               break;
            }            
            /* TODO Restore on next release

            case "Discard Incoming":
            {
               if(policy.Overflow != OverflowPolicy.DiscardIncomingMessage)
               {
                  return true;
               }
               break;
            }
            case "Discard Existing":
            {
               if(policy.Overflow != OverflowPolicy.DiscardExistingMessage)
               {
                  return true;
               }
               break;
            }
            */
         }
         return false;
      }
      void OnCopyAddress(object sender,EventArgs e)
      {         
         Clipboard.SetText(RealAddress.AbsoluteUri);
      }

      void RestoreSubscribedTo()
      {
         if(Node.SubscribedTo == null)
         {
            return;
         }

         RouterPolicy policy = Node.Policy as RouterPolicy;
         RouterClient client = RouterManagementClient.GetRouter(Credential,RealAddress);

         foreach(ServiceBusNode router in Node.SubscribedTo)
         {
            Uri address = GetRealAddress(router.Address);
            RouterClient parentRouter = RouterManagementClient.GetRouter(Credential,address);
            TimeSpan TimeSpan = DateTime.UtcNow - policy.ExpirationInstant;
            client.SubscribeToRouter(parentRouter,TimeSpan);
         }
      }

      void ApplyPolicy(RouterPolicy policy)
      {
         try
         {
            RouterClient client = RouterManagementClient.GetRouter(Credential,RealAddress);
            client.DeleteRouter();
            RouterManagementClient.CreateRouter(Credential,RealAddress,policy);
            RestoreSubscribedTo();
            Explore();
         }
         catch(Exception exception)
         {
            MessageBox.Show("Error applying change: " + exception.Message,"Service Bus Explorer",MessageBoxButtons.OK,MessageBoxIcon.Error);
         }
      }
      void OnPurge(object sender,EventArgs e)
      {
         DialogResult result = MessageBox.Show("Are you sure you want to remove all subscribers?","Service Bus Explorer",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
         if(result == DialogResult.No)
         {
            return;
         }
         RouterPolicy policy = Node.Policy as RouterPolicy;

         ApplyPolicy(policy);
      }

      void OnReset(object sender,EventArgs e)
      {
         DialogResult result = MessageBox.Show("Are you sure you want to reset the policy to it's default? you will also lose all subscribers","Service Bus Explorer",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
         if(result == DialogResult.No)
         {
            return;
         }
         ApplyPolicy(new RouterPolicy());  
      }

      void OnTimerTick(object sender,EventArgs e)
      {
         try
         {
            if(Node == null)
            {
               return;
            }

            RouterPolicy policy = Node.Policy as RouterPolicy;
            m_UpdateButton.Enabled = IsDirty(policy);
            m_ResetButton.Enabled = IsDirty(new RouterPolicy());
         }
         catch
         {}
      }

      void OnUpdate(object sender,EventArgs e)
      {
         DialogResult result = MessageBox.Show("Are you sure you want to update the policy? you will also lose all subscribers","Service Bus Explorer",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
         if(result == DialogResult.No)
         {
            return;
         }

         RouterPolicy policy = new RouterPolicy();

         if(m_AllRadioButton.Checked)
         {
            policy.MessageDistribution = MessageDistributionPolicy.AllSubscribers;
         }
         else
         {
            policy.MessageDistribution = MessageDistributionPolicy.OneSubscriber;
         }

         if(m_MaxSubscribersTextBox.Text != "")
         {
            policy.MaxSubscribers = Convert.ToInt32(m_MaxSubscribersTextBox.Text);
         }
         if(m_PushRetriesTextBox.Text != "")
         {
            policy.PushDeliveryRetries = Convert.ToInt32(m_PushRetriesTextBox.Text);
         }

         policy.ExpirationInstant = m_ExpirationTimePicker.Value;

         if(m_BufferLengthTextBox.Text != "")
         {
            policy.MaxBufferLength = Convert.ToInt32(m_BufferLengthTextBox.Text);
         }

         switch(m_OverflowComboBox.Text)
         {
            case "Reject":
            {
               policy.OverflowPolicy = OverflowPolicy.RejectIncomingMessage;
               break;
            }   
            /* TODO Restore on next release
            case "Discard Incoming":
            {
               policy.Overflow = OverflowPolicy.DiscardIncomingMessage;
               break;
            }
            case "Discard Existing":
            {
               policy.Overflow = OverflowPolicy.DiscardExistingMessage;
               break;
            }
            */
         }
         ApplyPolicy(policy);  
      }

      void OnDelete(object sender,EventArgs e)
      {
         DialogResult result = MessageBox.Show("Are you sure you want to delete the router?","Service Bus Explorer",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
         if(result == DialogResult.No)
         {
            return;
         }

         try
         {
            RouterClient client = RouterManagementClient.GetRouter(Credential,RealAddress);
            client.DeleteRouter();
            Explore();
         }
         catch(Exception exception)
         {
            MessageBox.Show("Error deleting router: " + exception.Message,"Service Bus Explorer",MessageBoxButtons.OK,MessageBoxIcon.Error);
         }
      }
   }
}
