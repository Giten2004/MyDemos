// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;

namespace ServiceModelEx.ServiceBus
{
   [ServiceContract]
   public interface IServiceBusDiscovery
   {
      [OperationContract(IsOneWay = true)]
      void OnDiscoveryRequest(string contractName,string contractNamespace,Uri[] scopesToMatch,Uri responseAddress);
   }
}
   
 