namespace Models;
public class Cleaner : IUser
{
	public int UserID { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
    IUserPermissions Permissions { get; set; }
    public void InitialiseCleaningMode()
    {
        throw new NotImplementedException();
    } 

}