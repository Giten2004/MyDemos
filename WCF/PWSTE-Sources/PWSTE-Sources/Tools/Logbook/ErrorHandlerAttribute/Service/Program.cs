// © 2011 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ServiceModel;
using ServiceModelEx;

namespace Host
{
   static class Program
   {
      static void Main()
      {
         ServiceHost<MyService> host = new ServiceHost<MyService>("http://localhost:8000/");
         host.Open();

         Application.EnableVisualStyles();
         Application.Run(new HostForm());

         host.Close();
      }
   }
}