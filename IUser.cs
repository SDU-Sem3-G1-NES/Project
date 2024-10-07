using System;

namespace FamicomBackend
{
	public interface IUser
	{
		int UserID { get; set; }
		string Name { get; set; }
		string Email { get; set; }
		public void RetrieveUser();
	}
public void RetrieveUser()
    {
		//Add user retrival logic here
    }
}