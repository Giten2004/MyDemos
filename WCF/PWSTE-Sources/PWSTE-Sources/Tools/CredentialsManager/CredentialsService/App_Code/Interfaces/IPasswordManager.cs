// © 2011 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;

[ServiceContract]
interface IPasswordManager
{
   /// <summary>
   /// Gets a value indicating whether the current membership provider is configured to allow users to reset their passwords. Returns true if the membership provider supports password reset; otherwise false
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   bool EnablePasswordReset(string application);

   /// <summary>
   /// Gets a value indicating whether the current membership provider is configured to allow users to retrieve their passwords. Returns true if the membership provider supports password retrieval; otherwise false
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   bool EnablePasswordRetrieval(string application);

   /// <summary>
   /// Geenrates a password according to the regular expression and other restrictions
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   string GeneratePassword(string application,int length,int numberOfNonAlphanumericCharacters);

   /// <summary>
   /// Returns the maximum allowed of password retries before locking out the user
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   int GetMaxInvalidPasswordAttempts(string application);

   /// <summary>
   /// Returns the minimum required number of non alphanumeric characters
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   int GetMinRequiredNonAlphanumericCharacters(string application);

   /// <summary>
   /// Returns the minimum required password length
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   int GetMinRequiredPasswordLength(string application);

   /// <summary>
   /// Gets the number of minutes in which a maximum number of invalid password or password answer attempts are allowed before the membership user is locked out
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   int GetPasswordAttemptWindow(string application);

   /// <summary>
   /// Returns the regular expression for the password
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   string GetPasswordStrengthRegularExpression(string application);

   /// <summary>
   /// Gets a value indicating whether the default membership provider requires the user to answer a password question for password reset and retrieval
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   bool RequiresQuestionAndAnswer(string application);

   /// <summary>
   /// Resets a user's password to a new,automatically generated password. Returns the new password for the membership user. Requires the application configuration file to not require password answer
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   string ResetPassword(string application,string userName);

   /// <summary>
   /// Resets a user's password to a new,automatically generated password. Returns the new password for the membership user. Requires the password answer
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   string ResetPasswordWithQuestionAndAnswer(string application,string userName,string passwordAnswer);

   /// <summary>
   /// Gets the password for the membership user from the membership data store. Requires the password answer for the membership user
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   string GetPassword(string application,string userName,string passwordAnswer);

   /// <summary>
   /// Gets the password question for the user.
   /// </summary>
   [TransactionFlow(TransactionFlowOption.Allowed)]
   [OperationContract]
   string GetPasswordQuestion(string application,string userName);

   /// <summary>
   /// Updates the password for the membership user in the membership data store. Returns true if the update was successful; otherwise,false. Requires the application configuration file to not require password answer
   /// </summary>
   [TransactionFlow(TransactionFlowOption.Allowed)]
   [OperationContract]
   void ChangePassword(string application,string userName,string newPassword);

   /// <summary>
   /// Updates the password for the membership user in the membership data store. Returns true if the update was successful; otherwise,false.
   /// </summary>
   [OperationContract]
   [TransactionFlow(TransactionFlowOption.Allowed)]
   void ChangePasswordWithAnswer(string application,string userName,string passwordAnswer,string newPassword);
}
