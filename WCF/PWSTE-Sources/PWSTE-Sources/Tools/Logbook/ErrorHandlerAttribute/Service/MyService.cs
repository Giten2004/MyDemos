// © 2011 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;
using System.Windows.Forms;
using System.Diagnostics;
using ServiceModelEx;


[ServiceContract]
interface IMyContract
{
   [OperationContract]
   [FaultContract(typeof(DivideByZeroException))]
   void MethodWithError();

   [OperationContract]
   void MethodWithoutError();
}

[ErrorHandlerBehavior]
class MyService : IMyContract
{
   public void MethodWithError()
   {
      throw new DivideByZeroException("Test of exception promotion and logging");
   }
   public void MethodWithoutError()
   {}
}

