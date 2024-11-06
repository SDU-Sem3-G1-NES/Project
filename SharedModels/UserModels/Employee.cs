
namespace SharedModels;
public class Employee : IUser
{
	public int UserID { get; set; }
	public required string Name { get; set; }
	public required string Email { get; set; }
    public List<UserPermissions> Permissions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool HasPermission(UserPermissions permission)
    {
        throw new NotImplementedException();
    }

    public IUser RetrieveUser()
    {
        return this;
    }
}