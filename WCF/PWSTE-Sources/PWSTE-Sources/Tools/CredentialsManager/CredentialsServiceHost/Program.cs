// © 2011 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net


using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ServiceModel;

namespace CredentialsServiceHost
{
   static class Program
   {
      [STAThread]
      static void Main()
      {
         ServiceHost host = new ServiceHost(typeof(AspNetSqlProviderService),new Uri("http://localhost:8000"));
         host.Open();
         
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.Run(new HostForm());

         host.Close();
      }
   }
}