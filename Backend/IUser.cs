using System;

namespace FamicomBackend
{
	public interface IUser
	{
		int UserID { get; set; }
		string Name { get; set; }
		string Email { get; set; }
		IUserPermissions Permissions { get; set; }
		public void RetrieveUser();
	}
}