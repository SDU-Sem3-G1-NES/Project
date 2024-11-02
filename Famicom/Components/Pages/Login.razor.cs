using System.Text;
using Microsoft.AspNetCore.Components;
using DataAccess;
using Famicom.Models;

namespace Famicom.Components.Pages
{
    public partial class LoginBase : ComponentBase
    {
        protected string? ErrorMessage { get; set; }

        protected LoginModel loginModel = new LoginModel();

        private readonly string fixedSalt = "$2b$12$VKSaJPoloZwgNhNqMJFxfu"; // Fixed salt for consistent hashing

        [Inject]
        private NavigationManager? Navigation { get; set; }

        [Inject]
        private UserRepository? userRepository { get; set; }

        protected async Task HandleLogin()
        {
            try
            {

                // Hash the email and password
                string emailHash = BCrypt.Net.BCrypt.HashPassword(loginModel.Email, fixedSalt);
                string hashedEmailHex = ConvertToHex(emailHash);
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(loginModel.Password, fixedSalt);
                string hashedPasswordHex = ConvertToHex(passwordHash);

                // Retrieve password from database.
                string? storedPasswordHashHex = userRepository?.GetHashedPassword(hashedEmailHex);

                if (storedPasswordHashHex != null && hashedPasswordHex.Equals(storedPasswordHashHex, StringComparison.OrdinalIgnoreCase))
                {
                    // Successful login, navigate to home
                    Navigation?.NavigateTo("/");
                }
                else
                {
                    ErrorMessage = "Invalid email or password.";
                }

                await InvokeAsync(StateHasChanged); // Update the UI
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                await InvokeAsync(StateHasChanged);
            }
        }

        // For converting the hashed password to hex for database querying.
        private static string ConvertToHex(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
    }
}
