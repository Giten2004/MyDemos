// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Windows.Forms;
using System.ServiceModel;
using Microsoft.ServiceBus;

namespace ServiceModelEx.ServiceBus
{
   partial class BufferViewControl : NodeViewControl
   {
      public BufferViewControl()
      {
         InitializeComponent();
      }
      
      public override void Refresh(ServiceBusNode node,TransportClientEndpointBehavior credential)
      {
         MessageBufferPolicy policy = node.Policy as MessageBufferPolicy;

         int overflowIndex = 0;
         switch(policy.OverflowPolicy)
         {
            case OverflowPolicy.RejectIncomingMessage:
            {
               overflowIndex = 0;
               break;
            }
            default:
            {
               throw new InvalidOperationException("Unrecognized overflow value");
            }
         }
         m_OverflowComboBox.Text = m_OverflowComboBox.Items[overflowIndex] as string;

         m_ExpirationTime.Text = policy.ExpiresAfter.TotalMinutes.ToString();

         m_CountTextBox.Text = policy.MaxMessageCount.ToString();

         base.Refresh(node,credential);
      }
      
      bool IsDirty(MessageBufferPolicy policy)
      {
         if(m_CountTextBox.Text != "")
         {
            if(Convert.ToInt32(m_CountTextBox.Text) != policy.MaxMessageCount)
            {
               return true;
            }
         }

         if(m_ExpirationTime.Text != policy.ExpiresAfter.TotalMinutes.ToString())
         {
            return true;
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
            default:
            {
               throw new InvalidOperationException("Unrecognized overflow value");
            }
         }
         return false;
      }

      void OnTimerTick(object sender,EventArgs e)
      {
         try
         {
            if(Node == null)
            {
               return;
            }
            MessageBufferPolicy policy = Node.Policy as MessageBufferPolicy;
            m_UpdateButton.Enabled = IsDirty(policy);
            m_ResetButton.Enabled = IsDirty(new MessageBufferPolicy());
         }
         catch
         {}
      }

      void OnUpdate(object sender,EventArgs e)
      {
         MessageBufferPolicy policy = new MessageBufferPolicy();
         policy.Discoverability = DiscoverabilityPolicy.Public;

         if(m_ExpirationTime.Text != "")
         {
            policy.ExpiresAfter = TimeSpan.FromMinutes(Convert.ToInt32(m_ExpirationTime.Text));
         }
         if(m_CountTextBox.Text != "")
         {
            policy.MaxMessageCount = Convert.ToInt32(m_CountTextBox.Text);
         }

         switch(m_OverflowComboBox.Text)
         {
            case "Reject":
            {
               policy.OverflowPolicy = OverflowPolicy.RejectIncomingMessage;
               break;
            }            
            default:
            {
               throw new InvalidOperationException("Unrecognized overflow value");
            }
         }
         ApplyPolicy(policy);  
      }
      void ApplyPolicy(MessageBufferPolicy policy)
      {
         try
         {
            Uri address = new Uri(RealAddress.AbsoluteUri.Replace(@"sb://",@"https://"));

            MessageBufferClient client = MessageBufferClient.GetMessageBuffer(Credential,address);
            client.DeleteMessageBuffer();
            MessageBufferClient.CreateMessageBuffer(Credential,address,policy);
            Explore();
         }
         catch(Exception exception)
         {
            MessageBox.Show("Error applying change: " + exception.Message,"Service Bus Explorer",MessageBoxButtons.OK,MessageBoxIcon.Error);
         }
      }
      void OnReset(object sender,EventArgs e)
      {
         DialogResult result = MessageBox.Show("Are you sure you want to reset the buffer to it's default?","Service Bus Explorer",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
         if(result == DialogResult.No)
         {
            return;
         }
         MessageBufferPolicy policy = new MessageBufferPolicy();
         policy.Discoverability = DiscoverabilityPolicy.Public;

         ApplyPolicy(policy);  
      }
      void OnPurge(object sender,EventArgs e)
      {
         DialogResult result = MessageBox.Show("Are you sure you want to remove all messages?","Service Bus Explorer",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
         if(result == DialogResult.No)
         {
            return;
         }
         try
         {
            Uri address = new Uri(RealAddress.AbsoluteUri.Replace(@"sb://",@"https://"));

            MessageBufferClient client = MessageBufferClient.GetMessageBuffer(Credential,address);
            MessageBufferPolicy policy = client.GetPolicy();
            ApplyPolicy(policy);
         }
         catch(Exception exception)
         {
            MessageBox.Show("Error purging buffer: " + exception.Message,"Service Bus Explorer",MessageBoxButtons.OK,MessageBoxIcon.Error);
         }
      }
      void OnDelete(object sender,EventArgs e)
      {
         DialogResult result = MessageBox.Show("Are you sure you want to delete the buffer?","Service Bus Explorer",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
         if(result == DialogResult.No)
         {
            return;
         }
         try
         {
            Uri address = new Uri(RealAddress.AbsoluteUri.Replace(@"sb://",@"https://"));
            MessageBufferClient client = MessageBufferClient.GetMessageBuffer(Credential,address);
            client.DeleteMessageBuffer();
            Explore();
         }
         catch(Exception exception)
         {
            MessageBox.Show("Error deleting buffer: " + exception.Message,"Service Bus Explorer",MessageBoxButtons.OK,MessageBoxIcon.Error);
         }
      }
   }
}
