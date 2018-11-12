// © 2011 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.ServiceModel;

namespace Client
{
   public partial class MyClient : Form
   {
      public MyClient()
      {
         InitializeComponent();
      }

      void OnCall(object sender,EventArgs e)
      {
         MyContractClient proxy = new MyContractClient("TCP");

         try
         {
            proxy.MethodWithError();
         }
         catch(FaultException<DivideByZeroException> exception)
         {
            Trace.WriteLine("First call: " + exception.GetType() + " " + exception.Message);
         }
         catch(FaultException exception)
         {
            Trace.WriteLine("First call: " + exception.GetType() + " " + exception.Message);
         }
         catch(CommunicationException exception)
         {
            Trace.WriteLine("First call: " + exception.GetType() + " " + exception.Message);
         }

         try
         {
            proxy.MethodWithoutError();
         }
         catch(Exception exception)
         {
            Trace.WriteLine("Second call: " + exception.GetType() + " " + exception.Message);
         }
         proxy.Close();
      }
   }
}



