// © 2011 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;

using System.ServiceModel;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Security.Principal;
using System.Web.Security;
using System.ServiceModel.Description;
using ServiceModelEx;

/// <summary>
/// AspNetSqlProviderService implements the IRoleManager,IApplicationManager,IPasswordManager,IMembershipManager,and IUserManager interfaces. The service should be deployed over secure transports
/// </summary>
[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
class AspNetSqlProviderService : IRoleManager,IApplicationManager,IPasswordManager,IMembershipManager,IUserManager
{
   [OperationBehavior(TransactionScopeRequired = true)]
   string[] IApplicationManager.GetApplications()
   {
      ApplicationsTableAdapter adapter = new ApplicationsTableAdapter();

      Func<AspNetDbDataSet.aspnet_ApplicationsRow,string> converter = (row)=>
                                                                           {
                                                                              return row.ApplicationName;
                                                                           };
                                                                          
      string[] applications = adapter.GetData().ToArray(converter);
      if(applications == null)
      {
         applications =  new string[]{};
      }
      return applications;
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   void IApplicationManager.DeleteAllApplications()
   {
      AspNetDbTablesAdapter aspNetDbTablesAdapter = new AspNetDbTablesAdapter();
      aspNetDbTablesAdapter.DeleteAllApplications();
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   void IApplicationManager.DeleteApplication(string application)
   {
      AspNetDbTablesAdapter aspNetDbTablesAdapter = new AspNetDbTablesAdapter();
      aspNetDbTablesAdapter.DeleteApplication(application);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   void IMembershipManager.DeleteAllUsers(string application,bool deleteAllRelatedData)
   {
      IMembershipManager membershipManager = this;
      string[] users = membershipManager.GetAllUsers(application);

      Action<string> deleteUser = (user)=>
                                  {
                                     membershipManager.DeleteUser(application,user,deleteAllRelatedData);
                                  };
      Array.ForEach(users,deleteUser);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   MembershipCreateStatus IMembershipManager.CreateUser(string application,string userName,string password,string email,string passwordQuestion,string passwordAnswer,bool isApproved)
   {
      MembershipCreateStatus status = MembershipCreateStatus.UserRejected;

      Membership.ApplicationName = application;
      Membership.CreateUser(userName,password,email,passwordQuestion,passwordAnswer,isApproved,out status);
      
      return status;
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   bool IMembershipManager.DeleteUser(string application,string userName,bool deleteAllRelatedData)
   {
      Membership.ApplicationName = application;
      return Membership.DeleteUser(userName,deleteAllRelatedData);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   void IMembershipManager.UpdateUser(string application,string userName,string email,string oldAnswer,string newQuestion,string newAnswer,bool isApproved,bool isLockedOut)
   {
      Membership.ApplicationName = application;
      MembershipUser membershipUser = Membership.GetUser(userName);
      membershipUser.Email = email;
      membershipUser.IsApproved = isApproved;
      if(isLockedOut == false)
      {
         membershipUser.UnlockUser();
      }
      if(Membership.EnablePasswordRetrieval)
      {
         string password = membershipUser.GetPassword(oldAnswer);
         membershipUser.ChangePasswordQuestionAndAnswer(password,newQuestion,newAnswer);
      }
      Membership.UpdateUser(membershipUser);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   UserInfo IMembershipManager.GetUserInfo(string application,string userName)
   {
      Membership.ApplicationName = application;
      MembershipUser membershipUser = Membership.GetUser(userName);

      return new UserInfo(membershipUser.UserName,membershipUser.Email,membershipUser.PasswordQuestion,membershipUser.IsApproved,membershipUser.IsLockedOut);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   string[] IMembershipManager.GetAllUsers(string application)
   {
      Membership.ApplicationName = application;
      MembershipUserCollection collection = Membership.GetAllUsers();
      Converter<object,string> converter  = (obj)=>
                                            {
                                               MembershipUser membershipUser = obj as MembershipUser;
                                               return membershipUser.UserName;
                                            };
      return collection.UnsafeToArray(converter);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   int IMembershipManager.GetNumberOfUsersOnline(string application)
   {
      Membership.ApplicationName = application;
      return Membership.GetNumberOfUsersOnline();
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   int IMembershipManager.UserIsOnlineTimeWindow(string application)
   {
      Membership.ApplicationName = application;
      return Membership.UserIsOnlineTimeWindow;
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   string IMembershipManager.GetUserNameByEmail(string application,string email)
   {
      Membership.ApplicationName = application;
      return Membership.GetUserNameByEmail(email);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   bool IPasswordManager.EnablePasswordReset(string application)
   {
      Membership.ApplicationName = application;
      return Membership.EnablePasswordReset;
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   string IPasswordManager.ResetPassword(string application,string userName)
   {
      Membership.ApplicationName = application;
      if(Membership.EnablePasswordReset && !Membership.RequiresQuestionAndAnswer)
      {
         MembershipUser membershipUser = Membership.GetUser(userName);
         return membershipUser.ResetPassword();
      }
      return String.Empty;
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   string IPasswordManager.ResetPasswordWithQuestionAndAnswer(string application,string userName,string passwordAnswer)
   {
      Membership.ApplicationName = application;
      if(Membership.EnablePasswordReset)
      {
         MembershipUser membershipUser = Membership.GetUser(userName);
         return membershipUser.ResetPassword(passwordAnswer);
      }
      return String.Empty;
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   string IPasswordManager.GetPassword(string application,string userName,string passwordAnswer)
   {
      Membership.ApplicationName = application;
      Debug.Assert(Membership.EnablePasswordRetrieval);

      MembershipUser membershipUser = Membership.GetUser(userName);
      return membershipUser.GetPassword(passwordAnswer);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   string IPasswordManager.GetPasswordQuestion(string application,string userName)
   {
      Membership.ApplicationName = application;
      MembershipUser membershipUser = Membership.GetUser(userName);

      return membershipUser.PasswordQuestion;
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   void IPasswordManager.ChangePassword(string application,string userName,string newPassword)
   {
      Membership.ApplicationName = application;
      Debug.Assert(Membership.EnablePasswordRetrieval && !Membership.RequiresQuestionAndAnswer);

      MembershipUser membershipUser = Membership.GetUser(userName);
      membershipUser.ChangePassword(membershipUser.GetPassword(),newPassword);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   void IPasswordManager.ChangePasswordWithAnswer(string application,string userName,string passwordAnswer,string newPassword)
   {
      Membership.ApplicationName = application;
      Debug.Assert(Membership.EnablePasswordRetrieval);

      MembershipUser membershipUser = Membership.GetUser(userName);
      membershipUser.ChangePassword(membershipUser.GetPassword(passwordAnswer),newPassword);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   bool IPasswordManager.EnablePasswordRetrieval(string application)
   {
      Membership.ApplicationName = application;
      return Membership.EnablePasswordRetrieval;
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   string IPasswordManager.GeneratePassword(string application,int length,int numberOfNonAlphanumericCharacters)
   {
      Membership.ApplicationName = application;
      return Membership.GeneratePassword(length,numberOfNonAlphanumericCharacters);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   int IPasswordManager.GetMaxInvalidPasswordAttempts(string application)
   {
      Membership.ApplicationName = application;
      return Membership.MaxInvalidPasswordAttempts;
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   int IPasswordManager.GetMinRequiredNonAlphanumericCharacters(string application)
   {
      Membership.ApplicationName = application;
      return Membership.MinRequiredNonAlphanumericCharacters;
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   int IPasswordManager.GetMinRequiredPasswordLength(string application)
   {
      Membership.ApplicationName = application;
      return Membership.MinRequiredPasswordLength;
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   int IPasswordManager.GetPasswordAttemptWindow(string application)
   {
      Membership.ApplicationName = application;
      return Membership.PasswordAttemptWindow;
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   string IPasswordManager.GetPasswordStrengthRegularExpression(string application)
   {
      Membership.ApplicationName = application;
      return Membership.PasswordStrengthRegularExpression;
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   bool IPasswordManager.RequiresQuestionAndAnswer(string application)
   {
      Membership.ApplicationName = application;
      return Membership.RequiresQuestionAndAnswer;
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   void IRoleManager.DeleteAllRoles(string application,bool throwOnPopulatedRole)
   {
      IRoleManager roleManager = this;
      string[] roles = roleManager.GetAllRoles(application);

      Action<string> deleteRole = (role)=>
                                  {
                                     roleManager.DeleteRole(application,role,throwOnPopulatedRole);
                                  };
      Array.ForEach(roles,deleteRole);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   string[] IRoleManager.GetAllRoles(string application)
   {
      Roles.ApplicationName = application;
      return Roles.GetAllRoles();
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   string[] IRoleManager.GetRolesForUser(string application,string userName)
   {
      Roles.ApplicationName = application;
      return Roles.GetRolesForUser(userName);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   string[] IRoleManager.GetUsersInRole(string application,string role)
   {
      Roles.ApplicationName = application;
      return Roles.GetUsersInRole(role);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   bool IRoleManager.RoleExists(string application,string role)
   {
      Roles.ApplicationName = application;
      return Roles.RoleExists(role);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   void IRoleManager.AddUsersToRole(string application,string[] userNames,string role)
   {
      Roles.ApplicationName = application;
      Roles.AddUsersToRole(userNames,role);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   void IRoleManager.AddUsersToRoles(string application,string[] userNames,string[] roles)
   {
      Roles.ApplicationName = application;
      Roles.AddUsersToRoles(userNames,roles);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   void IRoleManager.AddUserToRole(string application,string userName,string role)
   {
      Roles.ApplicationName = application;
      Roles.AddUserToRole(userName,role);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   void IRoleManager.AddUserToRoles(string application,string userName,string[] roles)
   {
      Roles.ApplicationName = application;
      Roles.AddUserToRoles(userName,roles);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   void IRoleManager.CreateRole(string application,string role)
   {
      Roles.ApplicationName = application;
      Roles.CreateRole(role);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   bool IRoleManager.DeleteRole(string application,string role,bool throwOnPopulatedRole)
   {
      Roles.ApplicationName = application;
      return Roles.DeleteRole(role,throwOnPopulatedRole);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   bool IRoleManager.IsRolesEnabled(string application)
   {
      Roles.ApplicationName = application;
      return Roles.Enabled;
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   void IRoleManager.RemoveUserFromRole(string application,string userName,string roleName)
   {
      Roles.ApplicationName = application;
      Roles.RemoveUserFromRole(userName,roleName);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   void IRoleManager.RemoveUserFromRoles(string application,string userName,string[] roles)
   {
      Roles.ApplicationName = application;
      Roles.RemoveUserFromRoles(userName,roles);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   void IRoleManager.RemoveUsersFromRole(string application,string[] users,string roleName)
   {
      Roles.ApplicationName = application;
      Roles.RemoveUsersFromRole(users,roleName);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   void IRoleManager.RemoveUsersFromRoles(string application,string[] users,string[] roles)
   {
      Roles.ApplicationName = application;
      Roles.RemoveUsersFromRoles(users,roles);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   bool IUserManager.Authenticate(string application,string userName,string password)
   {
      Membership.ApplicationName = application;
      return Membership.ValidateUser(userName,password);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   bool IUserManager.IsInRole(string application,string userName,string role)
   {
      Roles.ApplicationName = application;
      return Roles.IsUserInRole(userName,role);
   }
   [OperationBehavior(TransactionScopeRequired = true)]
   string[] IUserManager.GetRoles(string application,string userName)
   {
      IRoleManager roleManager = this;
      return roleManager.GetRolesForUser(application,userName);
   }
}

