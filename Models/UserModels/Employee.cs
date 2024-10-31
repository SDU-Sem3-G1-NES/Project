namespace Models;
public class Employee : IUser
{
	public int UserID { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	IUserPermissions Permissions { get; set; }
    public string Department { get; set; }
}