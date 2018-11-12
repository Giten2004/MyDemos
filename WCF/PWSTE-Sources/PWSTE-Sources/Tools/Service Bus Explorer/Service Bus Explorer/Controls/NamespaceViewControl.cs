// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Windows.Forms;
using System.Threading;
using Microsoft.ServiceBus;

namespace ServiceModelEx.ServiceBus
{
   partial class NamespaceViewControl : NodeViewControl
   {
      public NamespaceViewControl()
      {
         InitializeComponent();
      }
      public void Refresh(string ns)
      {
         string address = @"https://netservices.azure.com/";
         m_WebBrowser.Navigate(address);
      }
   }
}
