using System;
using Library;

namespace MVP
{
	/// <summary>
	/// Summary description for UserModel.
	/// </summary>
	public class UserModel : IUserModel
	{
		private string _email;
		private string _userName;

		public UserModel()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public bool Save()
		{
			throw new NotImplementedException();
		}

		public string UserName
		{
			get { return _userName; }
			set { _userName = value; }
		}

		public string Email
		{
			get { return _email; }
			set { _email = value; }
		}
	}
}
