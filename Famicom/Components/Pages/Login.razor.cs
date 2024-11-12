using System.Text;
using Microsoft.AspNetCore.Components;
using Famicom.Models;
using Models.Services;

namespace Famicom.Components.Pages
{
    public partial class LoginBase : ComponentBase
    {
        [Inject]
        private NavigationManager? Navigation { get; set; }
        private UserCredentialsService userCredentialsService = new UserCredentialsService();
        protected string? ErrorMessage { get; set; }
        protected readonly LoginModel loginModel = new LoginModel();
        private readonly string fixedSalt;

        public LoginBase()
        {
            fixedSalt = userCredentialsService.GetFixedSalt(); // Fixed salt for consistent hashing
        }

        protected async Task OnSubmitButton()
        {

            // Hash the email and password
            string emailHash = BCrypt.Net.BCrypt.HashPassword(loginModel.Email, fixedSalt);
            string hashedEmailHex = userCredentialsService.ConvertToHex(emailHash);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(loginModel.Password, fixedSalt);
            string hashedPasswordHex = userCredentialsService.ConvertToHex(passwordHash);

            if(userCredentialsService.ValidateCredentials(hashedEmailHex, hashedPasswordHex))
            {
                Navigation?.NavigateTo("/");
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                ErrorMessage = "Invalid email or password.";
                await InvokeAsync(StateHasChanged);
            }
            
        }

        
        
    }
}
