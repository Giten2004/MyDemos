using System;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace ServiceModelEx
{
   partial class ServiceViewControl : NodeViewControl
   {
      string m_Address;
      public ServiceViewControl()
      {
         InitializeComponent();
      }
      public void Refresh(string address)
      {
         Thread.Sleep(0);
         Application.DoEvents();
         if(m_Address != address)
         {
            m_Address = address;
            m_WebBrowser.Navigate(m_Address);
         }
      }
      public string ExtractServiceName()
      {
         string title;

         while(m_WebBrowser.IsBusy)
         {
            Application.DoEvents();
            Thread.Sleep(0);
            title = m_WebBrowser.DocumentTitle;
            if(title == "The webpage cannot be displayed")
            {
               break;
            }
         }
         title = m_WebBrowser.DocumentTitle;
         if(title == "The webpage cannot be displayed" || title == "HTTP 400 Bad Request" || title == "Invalid syntax error" || title == "No page to display" || title == "Cannot find server" || title == "Navigation Canceled")
         {
            title = "Unknown";
         }
         if(String.IsNullOrEmpty(title))
         {
            return title;
         }
         return title.Split(new string[]{" Service"},StringSplitOptions.RemoveEmptyEntries)[0];
      }
   }
}
