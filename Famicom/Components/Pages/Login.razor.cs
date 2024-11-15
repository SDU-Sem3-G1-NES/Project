using System.Text;
using Microsoft.AspNetCore.Components;
using Blazored.SessionStorage;
using Famicom.Models;
using Models.Services;

namespace Famicom.Components.Pages
{
    public partial class LoginBase : ComponentBase
    {
        [Inject]
        private NavigationManager? Navigation { get; set; }

        [Inject]
        private ISessionStorageService? SessionStorage { get; set; }

        private UserCredentialsService userCredentialsService = new UserCredentialsService();
        protected string? ErrorMessage { get; set; }
        protected readonly LoginModel loginModel = new LoginModel();
        private readonly string fixedSalt;

        public LoginBase()
        {
            fixedSalt = userCredentialsService.GetFixedSalt();
        }

        protected async Task OnSubmitButton()
        {
            // Hash the email and password
            string emailHash = BCrypt.Net.BCrypt.HashPassword(loginModel.Email, fixedSalt);
            string hashedEmailHex = ConvertToHex(emailHash);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(loginModel.Password, fixedSalt);
            string hashedPasswordHex = ConvertToHex(passwordHash);

            if (userCredentialsService.ValidateCredentials(hashedEmailHex, hashedPasswordHex))
            {
                string sessionID = Guid.NewGuid().ToString();
                // Store session ID in session storage
                if (SessionStorage != null)
                {
                    await SessionStorage.SetItemAsync("SessionId", sessionID);
                }
                else
                {
                    ErrorMessage = "Session Storage not found."
                    await InvokeAsync(StateHasChanged);
                }
                Navigation?.NavigateTo("/");
                
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                ErrorMessage = "Invalid email or password.";
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
