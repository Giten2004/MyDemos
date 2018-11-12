// © 2011 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;

[ServiceContract]
public interface IUserManager
{
   /// <summary>
   /// Authenticates the user.
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   bool Authenticate(string applicationName,string userName,string password);

   /// <summary>
   /// Verifies user role's membership.
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   bool IsInRole(string applicationName,string userName,string role);

   /// <summary>
   /// Returns all roleList the user is a member of.
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   string[] GetRoles(string applicationName,string userName);
}