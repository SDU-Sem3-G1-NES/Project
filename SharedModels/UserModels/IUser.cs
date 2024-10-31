namespace SharedModels;

public interface IUser
{
	int UserID { get; set; }
	string Name { get; set; }
	string Email { get; set; }
	List<UserPermissions> Permissions { get; set; }
	public IUser RetrieveUser();
	public bool HasPermission(UserPermissions permission);
}
