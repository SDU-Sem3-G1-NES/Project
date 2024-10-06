using System;

public interface IUser
{
	int UserID { get; set; }
	string Name { get; set; }
	string Email { get; set; }
	string Address { get; set; }
}
public class Admin : IUser
{
	public int UserID { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public string Address { get; set; }

	public bool ManageUsers { get; set; }
}

public class Employee : IUser
{
	public int UserID { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public string Address { get; set; }

	public string Department { get; set; }
}