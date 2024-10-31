
namespace SharedModels;
public class Employee : IUser
{
	public int UserID { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
    public string Department { get; set; }
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