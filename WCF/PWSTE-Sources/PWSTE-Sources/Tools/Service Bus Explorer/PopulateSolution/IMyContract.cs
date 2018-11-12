// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;


[ServiceContract]
interface IMyContract
{
   [OperationContract(IsOneWay = true)]
   void MyMethod();
}
