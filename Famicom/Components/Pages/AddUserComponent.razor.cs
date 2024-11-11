using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Famicom.Components.Pages
{
    public partial class AddUserComponent: ComponentBase
    {
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPassword { get; set; }
        public string? UserType { get; set; }
        public string? ErrorMessage { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; } = default!;

        [Parameter]
        public EventCallback OnUserAdded { get; set; }

        public async Task AddUser()
        {
            // Add user logic here

            Snackbar.Add("User added successfully", Severity.Success);
            await OnUserAdded.InvokeAsync(null);
        }

    }
}
