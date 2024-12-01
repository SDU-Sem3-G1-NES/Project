using Microsoft.AspNetCore.Components;
using Blazored.SessionStorage;
using Famicom.Models;
using Models.Services;


public partial class SettingsBase : ComponentBase
{
    [Inject]
    private ISessionStorageService? SessionStorage { get; set; }

    [Inject]

    private NavigationManager? Navigation { get; set; }
    private UserService userService = new UserService();
    private UserCredentialsService userCredentialsService = new UserCredentialsService();
    protected SettingsModel settingsModel = new SettingsModel();
    protected bool isSubmitting = false;
    protected string? ErrorMessage { get; set; }
    private string? userEmail;
    private readonly string fixedSalt;

    public SettingsBase()
    {
        fixedSalt = userCredentialsService.GetFixedSalt();
    }
    private async Task GetSessionStorage()
    {
        if (SessionStorage == null)
        {
            throw new Exception("SessionStorage not found.");
        }

        try
        {
            userEmail = await SessionStorage.GetItemAsync<string>("Email");

            if (userEmail == null)
            {
                throw new Exception("Email not found in session storage.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting session storage: {ex.Message}");
        }
        
    }
    protected async Task OnChangePassword()
    {
        await GetSessionStorage();
        isSubmitting = true;
        try
        {
            string CurrentPassword = settingsModel.CurrentPassword;
            string NewPassword = settingsModel.NewPassword;
            string ConfirmedPassword = settingsModel.ConfirmedPassword;

            if (NewPassword != ConfirmedPassword)
            {
                ErrorMessage = "Passwords do not match.";
                return;
            }

            // Check if the current password is correct
            string hashedEmail = userCredentialsService.ConvertToHex(BCrypt.Net.BCrypt.HashPassword(userEmail, fixedSalt));
            string hashedCurrentPassword = userCredentialsService.ConvertToHex(BCrypt.Net.BCrypt.HashPassword(CurrentPassword, fixedSalt));

            if (userCredentialsService.ValidateCredentials(hashedEmail, hashedCurrentPassword) == false)
            {
                ErrorMessage = "Current password is incorrect.";
                return;
            }

            // Change Password
            string hashedNewPassword = userCredentialsService.ConvertToHex(BCrypt.Net.BCrypt.HashPassword(NewPassword, fixedSalt));
            userService.UpdateHashPass(hashedEmail, hashedNewPassword);
            hashedNewPassword = hashedCurrentPassword;
            
            Navigation?.NavigateTo("/");
            ErrorMessage = "Password updated successfully.";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"An error occurred: {ex.Message}";
        }
        finally
        {
            settingsModel.NewPassword = string.Empty;
            settingsModel.ConfirmedPassword = string.Empty;
            settingsModel.CurrentPassword = string.Empty;
            isSubmitting = false;
            await InvokeAsync(StateHasChanged);
        }
    }

}