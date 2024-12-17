
namespace SharedModels;
public class Employee : IUser
{
	public int UserID { get; set; }
	public required string Name { get; set; }
	public required string Email { get; set; }
    public required List<UserPermissions> Permissions { get; set; }

    public bool HasPermission(UserPermissions permission)
    {
        if (Permissions.Contains(permission))
        {
            return true;
        }
        return false;
    }

    public IUser RetrieveUser()
    {
        return this;
    }
}