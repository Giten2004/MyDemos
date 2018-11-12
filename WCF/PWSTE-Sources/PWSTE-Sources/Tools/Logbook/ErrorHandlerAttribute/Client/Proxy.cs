// © 2011 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

[ServiceContract]
public interface IMyContract
{
   [OperationContract]
   [FaultContract(typeof(DivideByZeroException))]
   void MethodWithError();

   [OperationContract]
   void MethodWithoutError();
}

public partial class MyContractClient : ClientBase<IMyContract>,IMyContract
{
   public MyContractClient()
   {}

   public MyContractClient(string endpointConfigurationName) : base(endpointConfigurationName)
   {}

   public void MethodWithError()
   {
      Channel.MethodWithError();
   }

   public void MethodWithoutError()
   {
      Channel.MethodWithoutError();
   }
}
