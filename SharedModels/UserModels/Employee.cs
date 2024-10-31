namespace SharedModels;
public class Employee : IUser
{
	public int UserID { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public IUserPermissions Permissions { get; set; }
    public string Department { get; set; }

    public void RetrieveUser()
    {
        throw new NotImplementedException();
    }
}