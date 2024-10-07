using FamicomBackend;
public class Employee : IUser
{
	public int UserID { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }

    public string Department { get; set; }
}