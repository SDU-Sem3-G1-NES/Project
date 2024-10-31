namespace SharedModels;
public class Admin : IUser
{
	public int UserID { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public IUserPermissions Permissions { get; set; }

	public void RetrieveUser() {
		throw new NotImplementedException();
	}
}
