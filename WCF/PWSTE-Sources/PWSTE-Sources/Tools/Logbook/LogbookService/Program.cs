// © 2011 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;
using System.Windows.Forms;
using ServiceModelEx;

namespace LogbookService
{
   static class Program
   {
      static void Main()
      {
         ServiceHost host = new ServiceHost(typeof(LogbookManager),new Uri("http://localhost:8005/"));

         host.Open();

         Application.Run(new LogbookHostForm());

         host.Close();
      }
   }
}