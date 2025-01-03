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

        [Inject] 
        private LoginStateService LoginStateService { get; set; } = default!;

        [Inject]
        private UserPermissionService userPermissionService { get; set; } = default!;
        private UserCredentialsService userCredentialsService = new UserCredentialsService();
        private UserService userService = new UserService();
        protected string? ErrorMessage { get; set; }
        protected readonly LoginModel loginModel = new LoginModel();
        private readonly string fixedSalt;
        private bool isInitialized;
        protected bool isSubmitting = false;

        public LoginBase()
        {
            fixedSalt = userCredentialsService.GetFixedSalt();
        }
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !isInitialized)
            {
                await CheckSessionStorage();
                isInitialized = true;
            }
        }
        private async Task CheckSessionStorage()
        {
            if (SessionStorage == null || Navigation == null)
            {
                throw new Exception("SessionStorage or NavigationManager not found.");
            }

            try
            {
                string? sessionID = await SessionStorage.GetItemAsync<string>("SessionId");

                if (!string.IsNullOrEmpty(sessionID))
                {
                    Navigation.NavigateTo("/");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking session storage: {ex.Message}");
            }
        }
        
        protected async Task OnSubmitButton()
        {
            isSubmitting = true;
            try
            {
                // Hash the email and password
                string emailHash = BCrypt.Net.BCrypt.HashPassword(loginModel.Email, fixedSalt);
                string hashedEmailHex = userCredentialsService.ConvertToHex(emailHash);
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(loginModel.Password, fixedSalt);
                string hashedPasswordHex = userCredentialsService.ConvertToHex(passwordHash);

                if (userCredentialsService.ValidateCredentials(hashedEmailHex, hashedPasswordHex) && userService.GetUser(loginModel.Email) != null)
                {
                    var userId = userService.GetUser(loginModel.Email)!.UserID;
                    string sessionID = Guid.NewGuid().ToString();
                    // Store session ID in session storage
                    if (SessionStorage != null)
                    {
                        await SessionStorage.SetItemAsync("SessionId", sessionID);
                        await SessionStorage.SetItemAsync("UserId", userId);
                        await SessionStorage.SetItemAsync("Email", loginModel.Email);
                        await SessionStorage.SetItemAsync("IsLoggedIn", true);

                        LoginStateService.IsLoggedIn = true;
                        var user = userService.GetUser(loginModel.Email);
                        userPermissionService.SetUser(user!);

                        Navigation?.NavigateTo("/");
                    }
                    else
                    {
                        ErrorMessage = "Session Storage not found.";
                        await InvokeAsync(StateHasChanged);
                    }
                }
                else
                {
                    ErrorMessage = "Invalid email or password.";
                    await InvokeAsync(StateHasChanged);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                await InvokeAsync(StateHasChanged);
            }
            finally
            {
                isSubmitting = false;
            }
        }
        
        
    }
}
