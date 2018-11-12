// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using Microsoft.ServiceBus;
using System.Windows.Forms;
using System.Diagnostics;


namespace ServiceModelEx.ServiceBus
{
   partial class NewRouterDialog : Form
   {
      readonly string BaseAddress;
      public RouterClient Client
      {get;private set;}

      string ServiceNamespace
      {
         get
         {
            ExplorerForm form = Application.OpenForms[0] as ExplorerForm;
            return form.ServiceNamespace;
         }
      }

      TransportClientEndpointBehavior Credential
      {
         get
         {
            ExplorerForm form = Application.OpenForms[0] as ExplorerForm;
            return form.Graphs[ServiceNamespace.ToLower()].Credential;
         }
      }

      public NewRouterDialog(string serviceNamespace)
      {
         InitializeComponent();

         BaseAddress = ServiceBusEnvironment.CreateServiceUri("sb",serviceNamespace,"").AbsoluteUri;
         if(BaseAddress.EndsWith(@"/") == false)
         {
            BaseAddress += @"/";
         }

         m_AddressTextBox.Text = BaseAddress;
         OnTextChanged(this,EventArgs.Empty);


         RouterPolicy policy = new RouterPolicy();

         m_AllRadioButton.Checked = policy.MessageDistribution == MessageDistributionPolicy.AllSubscribers;
         m_OneRadioButton.Checked = policy.MessageDistribution == MessageDistributionPolicy.OneSubscriber;

         m_MaxSubscribersTextBox.Text = policy.MaxSubscribers.ToString();

         m_PushRetriesTextBox.Text = policy.PushDeliveryRetries.ToString();

         m_ExpirationTimePicker.Value = policy.ExpirationInstant;;

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
      }

      void OnCreate(object sender,EventArgs e)
      {
         Debug.Assert(m_AddressTextBox.Text != BaseAddress);

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
         if(m_AddressTextBox.Text.EndsWith(@"/") == false)
         {
            m_AddressTextBox.Text += @"/";
         }
         try
         {
            Client = RouterManagementClient.CreateRouter(Credential,new Uri(m_AddressTextBox.Text),policy);
         }
         catch(Exception exception)
         {
            MessageBox.Show("Unable to create router: " + exception.Message,"Service Bus Explorer",MessageBoxButtons.OK,MessageBoxIcon.Error);
            return;
         }

         Close();
      }

      void OnTextChanged(object sender,EventArgs e)
      {
         m_CreateButton.Enabled = m_AddressTextBox.Text.StartsWith(BaseAddress) && m_AddressTextBox.Text.Length >= BaseAddress.Length+1;
      }
   }
}
