// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using Microsoft.ServiceBus;
using System.Windows.Forms;
using System.Diagnostics;


namespace ServiceModelEx.ServiceBus
{
   partial class NewBufferDialog : Form
   {
      readonly string BaseAddress;

      public NewBufferDialog(string serviceNamespace)
      {
         InitializeComponent();

         BaseAddress = ServiceBusEnvironment.CreateServiceUri("https",serviceNamespace,"").AbsoluteUri;
         if(BaseAddress.EndsWith(@"/") == false)
         {
            BaseAddress += @"/";
         }

         m_AddressTextBox.Text = BaseAddress;
         OnTextChanged(this,EventArgs.Empty);


         MessageBufferPolicy policy = new MessageBufferPolicy();

         m_ExpirationTime.Text = policy.ExpiresAfter.TotalMinutes.ToString();
         m_CountTextBox.Text = policy.MaxMessageCount.ToString();

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
      }

      public MessageBufferClient Client
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


      void OnCreate(object sender,EventArgs e)
      {
         Debug.Assert(m_AddressTextBox.Text != BaseAddress);

         MessageBufferPolicy policy = new MessageBufferPolicy();
         policy.Discoverability = DiscoverabilityPolicy.Public;

         if(m_CountTextBox.Text != "")
         {
            policy.MaxMessageCount = Convert.ToInt32(m_CountTextBox.Text);
         }

         if(m_ExpirationTime.Text != "")
         {
            policy.ExpiresAfter = TimeSpan.FromMinutes(Convert.ToInt32(m_ExpirationTime.Text));
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
               throw new InvalidOperationException("Unknown overflow value");
            }            
         }
         if(m_AddressTextBox.Text.EndsWith(@"/") == false)
         {
            m_AddressTextBox.Text += @"/";
         }
         try
         {
            Client = MessageBufferClient.CreateMessageBuffer(Credential,new Uri(m_AddressTextBox.Text),policy);
         }
         catch(Exception exception)
         {
            MessageBox.Show("Unable to create buffer: " + exception.Message,"Service Bus Explorer",MessageBoxButtons.OK,MessageBoxIcon.Error);
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
