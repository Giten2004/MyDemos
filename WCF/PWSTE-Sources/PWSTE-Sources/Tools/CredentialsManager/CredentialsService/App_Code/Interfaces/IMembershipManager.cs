// © 2011 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Web.Security;
using System.ServiceModel;
using System.Runtime.Serialization;


[DataContract]
public class UserInfo
{
   string m_Name;
   string m_Email;
   string m_PasswordQuestion;
   bool m_IsApproved;
   bool m_IsLockedOut;

   [DataMember]
   public string Email
   {
      get
      {
         return m_Email;
      }
      set
      {
         m_Email = value;
      }
   }
   [DataMember]
   public string PasswordQuestion
   {
      get
      {
         return m_PasswordQuestion;
      }
      set
      {
         m_PasswordQuestion = value;
      }
   }
   [DataMember]
   public bool IsApproved
   {
      get
      {
         return m_IsApproved;
      }
      set
      {
         m_IsApproved = value;
      }
   }
  [DataMember]
   public bool IsLockedOut
   {
      get
      {
         return m_IsLockedOut;
      }
      set
      {
         m_IsLockedOut = value;
      }
   } 
   [DataMember]
   public string Name
   {
      get
      {
         return m_Name;
      }
      set
      {
         m_Name = value;
      }
   }
   public UserInfo(string name,string email,string passwordQuestion,bool isApproved,bool isLockedOut)
   {
      Name = name;
      Email = email;
      PasswordQuestion = passwordQuestion;
      IsApproved = isApproved;
      IsLockedOut = isLockedOut;
   }
   public UserInfo()
   {}
}

[ServiceContract]
interface IMembershipManager
{
   /// <summary>
   /// Creates a new user.
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   MembershipCreateStatus CreateUser(string application,string userName,string password,string email,string passwordQuestion,string passwordAnswer,bool isApproved);

   /// <summary>
   /// Deletes the specified user.
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   bool DeleteUser(string application,string userName,bool deleteAllRelatedData);

   /// <summary>
   /// Deletes all users,and optionally removed all relevant data
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   void DeleteAllUsers(string application,bool deleteAllRelatedData);

   /// <summary>
   /// Returns the user matching the specified email.
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   string GetUserNameByEmail(string application,string email);

   /// <summary>
   /// Gets the number of users currently accessing an application
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   int GetNumberOfUsersOnline(string application);

   /// <summary>
   /// Updates the user record and status. May require a password answer.
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   void UpdateUser(string application,string userName,string email,string oldAnswer,string newQuestion,string newAnswer,bool isApproved,bool isLockedOut);

   /// <summary>
   /// ]Specifies the number of minutes after the last activity date-time stamp for a user during which the user is considered on-line. Returns the number of minutes after the last activity date-time stamp for a user during which the user is considered on-line
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   int UserIsOnlineTimeWindow(string application);

   /// <summary>
   /// Returns all of the users in the database.
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   string[] GetAllUsers(string application);

   /// <summary>
   /// Gets the information from the data source for the specified user.
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   UserInfo GetUserInfo(string application,string userName);
}
