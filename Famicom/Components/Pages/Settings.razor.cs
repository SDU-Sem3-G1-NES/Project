using Microsoft.AspNetCore.Components;
using Blazored.SessionStorage;
using Famicom.Models;
using Models.Services;

namespace Famicom.Components;
public partial class SettingsBase : ComponentBase
{
    [Inject]
    private ISessionStorageService? SessionStorage { get; set; }
    private UserService userService = new UserService();
    private UserCredentialsService userCredentialsService = new UserCredentialsService();
    protected SettingsModel settingsModel = new SettingsModel();
    protected bool isSubmitting = false;
    protected string? Message { get; set; }
    protected string? SuccessMessage { get; set; }
    protected string? ConfirmMessage { get; set; }
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
        Message = string.Empty;
        ConfirmMessage = string.Empty;
        SuccessMessage = string.Empty;
        await GetSessionStorage();
        isSubmitting = true;
        StateHasChanged();
        try
        {
            string CurrentPassword = settingsModel.CurrentPassword;
            string NewPassword = settingsModel.NewPassword;
            string ConfirmedPassword = settingsModel.ConfirmedPassword;

            if (NewPassword != ConfirmedPassword)
            {
                ConfirmMessage = "Passwords do not match.";
                return;
            }

            // Check if the current password is correct
            string hashedEmail = userCredentialsService.ConvertToHex(BCrypt.Net.BCrypt.HashPassword(userEmail, fixedSalt));
            string hashedCurrentPassword = userCredentialsService.ConvertToHex(BCrypt.Net.BCrypt.HashPassword(CurrentPassword, fixedSalt));

            if (userCredentialsService.ValidateCredentials(hashedEmail, hashedCurrentPassword) == false)
            {
                Message = "Current password is incorrect.";
                return;
            }

            // Change Password
            string hashedNewPassword = userCredentialsService.ConvertToHex(BCrypt.Net.BCrypt.HashPassword(NewPassword, fixedSalt));
            userService.UpdateHashPass(hashedEmail, hashedNewPassword);
            hashedNewPassword = hashedCurrentPassword;

            Message = string.Empty;
            SuccessMessage = "Password updated successfully.";
        }
        catch (Exception ex)
        {
            Message = $"An error occurred: {ex.Message}";
        }
        finally
        {
            settingsModel.NewPassword = string.Empty;
            settingsModel.ConfirmedPassword = string.Empty;
            settingsModel.CurrentPassword = string.Empty;
            isSubmitting = false;
        }
    }

}