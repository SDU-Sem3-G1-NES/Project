using System.ComponentModel.DataAnnotations;

namespace Famicom.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        protected internal string Email { get; set; } = string.Empty;

        [Required]
        protected internal string Password { get; set; } = string.Empty;
    }
}