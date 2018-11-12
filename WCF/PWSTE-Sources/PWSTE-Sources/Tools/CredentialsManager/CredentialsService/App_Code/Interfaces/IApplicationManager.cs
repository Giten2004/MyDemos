// © 2011 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;

[ServiceContract]
interface IApplicationManager
{
   /// <summary>
   /// Removes all users and roles from the application and deletes the application.
   /// </summary>
   /// <param name="application"></param>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   void DeleteApplication(string application);

   /// <summary>
   /// Deletes all applications,and removes all users and roles from  all applications.
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   void DeleteAllApplications();
   
   /// <summary>
   /// Returns available applications.
   /// </summary>
   /// <returns></returns>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   string[] GetApplications();
}