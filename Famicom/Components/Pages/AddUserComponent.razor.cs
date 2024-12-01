using Microsoft.AspNetCore.Components;
using Models.Services;
using MudBlazor;
using SharedModels;
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
        public int UserType { get; set; }
        public string? ErrorMessage { get; set; }
        public required string fixedSalt { get; set; }

        public required List<UserTypes> UserTypes { get; set; }

        protected override void OnInitialized()
        {
            fixedSalt = userCredentialsService.GetFixedSalt();
            UserTypes = userRepo.GetUserType();
        }

        [Parameter]
        public EventCallback<bool> OnUserAdded { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; } = default!;

        public async Task Cancel()
        {
            await OnUserAdded.InvokeAsync(true);
        }

        public async Task AddUser()
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(UserEmail) || string.IsNullOrEmpty(UserPassword))
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

               
                userRepo.AddUser(UserName, UserEmail, UserType);
                   
                

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Snackbar.Add("An error occurred while adding the user", Severity.Error);
                await OnUserAdded.InvokeAsync(true);
                return;
            }



            Snackbar.Add("User added successfully", Severity.Success);
            await OnUserAdded.InvokeAsync(true);
            
        }

    }
}
