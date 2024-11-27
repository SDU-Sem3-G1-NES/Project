using Microsoft.AspNetCore.Components;
using SharedModels;
using Models.Services;
using MudBlazor;
using System.Diagnostics;

namespace Famicom.Components.Pages
{
    public partial class AssignUserComponent : ComponentBase
    {
        private bool AddUserVisible { get; set; }

        private bool ShouldAskQuestion { get; set; }
        public required List<IUser> Users { get; set; }
        public required string selectedTable { get; set; }
        public int selectedUser { get; set; }
        public required List<ITable> Tables { get; set; }

        private UserService userService = new UserService();

        private TableService tableService = new TableService();

        [Parameter]
        public EventCallback OnUserAssigned { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; } = default!;

        protected override void OnInitialized()
        {
            ShouldAskQuestion = true;
            Users = userService.GetAllUsers();
            Tables = tableService.GetUserFreeTable();
            AddUserVisible = false;
        }



        public async Task Cancel()
        {
            await OnUserAssigned.InvokeAsync(null);
        }

        private void ChangeView()
        {
            ShouldAskQuestion = !ShouldAskQuestion;
        }



        private async Task AssignTable()
        {
            try
            {
                tableService.AddTableUser(selectedUser, selectedTable);
                Snackbar.Add("Table assigned successfully", Severity.Success);
                ShouldAskQuestion = true;
            }
            catch (Exception e)
            {
                Snackbar.Add("Something went wrong", Severity.Error);
                Debug.WriteLine(e.Message);
            }



            await OnUserAssigned.InvokeAsync(null);
        }
    }
}
