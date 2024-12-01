using System.ComponentModel.DataAnnotations;

namespace Famicom.Models
{
    public class SettingsModel
    {
        [Required]
        protected internal string CurrentPassword { get; set; } = string.Empty;
        [Required]
        protected internal string NewPassword { get; set; } = string.Empty;

        [Required]
        protected internal string ConfirmedPassword { get; set; } = string.Empty;
    }
}