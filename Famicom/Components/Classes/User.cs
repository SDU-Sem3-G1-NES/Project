namespace Famicom.Components.Classes
{
    public class User
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public int UserTypeId { get; set; }
        public string? UserTypeName { get; set; }
        public string? Permisions { get; set; }

        public User()
        {
        }
    }
}
