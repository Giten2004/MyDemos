using System;

namespace Library
{
	/// <summary>
	/// Summary description for IModel.
	/// </summary>
	public interface IUserModel
	{
		bool Save();
		string UserName {get;set;}
		string Email {get;set;}
	}
}
