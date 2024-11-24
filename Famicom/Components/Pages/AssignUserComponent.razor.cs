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
        private List<Employee> Users { get; set; }
        public string selectedTable { get; set; }
        public int selectedUser { get; set; }
        private List<ITable> Tables { get; set; }

        private UserService userService = new UserService();

        private TableService tableService = new TableService();

        [Parameter]
        public EventCallback OnUserAssigned { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; } = default!;

        public AssignUserComponent()
        {
            ShouldAskQuestion = true;
            AddUserVisible = false;
            
        }



        public async Task Cancel()
        {
            await OnUserAssigned.InvokeAsync(null);
        }

        private void ChangeView()
        {
            ShouldAskQuestion = !ShouldAskQuestion;
            Users = userService.GetAllUsers();
            Tables = tableService.GetUserFreeTable();
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
