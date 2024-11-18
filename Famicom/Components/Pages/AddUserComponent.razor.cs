using Microsoft.AspNetCore.Components;
using Models.Services;
using MudBlazor;
using System.Diagnostics;

namespace Famicom.Components.Pages
{
    public partial class AddUserComponent: ComponentBase
    {
        UserService userRepo = new UserService();

        UserCredentialsService userCredentialsService = new UserCredentialsService();
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPassword { get; set; }
        public string? UserType { get; set; }
        public string? ErrorMessage { get; set; }

        private string fixedSalt { get; set; }

        protected override void OnInitialized()
        {
            fixedSalt = userCredentialsService.GetFixedSalt();
        }

        [Parameter]
        public EventCallback OnUserAdded { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; } = default!;

        public async Task Cancel()
        {
            await OnUserAdded.InvokeAsync(null);
        }
        public async Task AddUser()
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(UserEmail) || string.IsNullOrEmpty(UserPassword) || string.IsNullOrEmpty(UserType))
            {
                ErrorMessage = "Please fill in all fields.";
                return;
            }

            try
            {
                string emailHash = BCrypt.Net.BCrypt.HashPassword(UserEmail, fixedSalt);
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(UserPassword, fixedSalt);

                byte[] emailHashBytes = System.Text.Encoding.UTF8.GetBytes(emailHash);
                byte[] passwordHashBytes = System.Text.Encoding.UTF8.GetBytes(passwordHash);


                userCredentialsService.AddUserCredentials(emailHashBytes, passwordHashBytes);

                if (UserType == "Admin")
                {
                   userRepo.AddUser(UserName, UserEmail, 1);
                   
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                ErrorMessage = "An error occurred while adding the user.";
                Snackbar.Add("An error occurred while adding the user", Severity.Error);
                return;
            }



            Snackbar.Add("User added successfully", Severity.Success);
            await OnUserAdded.InvokeAsync(null);
        }

    }
}
