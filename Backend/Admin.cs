using FamicomBackend;
public class Admin : IUser
{
	public int UserID { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	IUserPermissions Permissions { get; set; }
}
