using FamicomBackend;
public class Cleaner : IUser
{
	public int UserID { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }

    public void CleaningMode()
    {
        //Add table cleaning logic here
    } 

}