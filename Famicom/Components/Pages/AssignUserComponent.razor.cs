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
        public required List<IUser> Users { get; set; }
        public required List<ITable> Tables { get; set; }

        private UserService userService = new UserService();

        private TableService tableService = new TableService();

        #region Table Variables
        //Table Variables
        private string searchString = "";
        private MudTable<ITable>? mudTable;
        public int selectedRowNumber = -1;
        private ITable? selectedTable;
        //User Variables

        private string userSearchString = "";
        private MudTable<IUser>? mudUser;
        public int userSelectedRowNumber = -1;
        private IUser? selectedUser;

        #endregion

        [Parameter]
        public EventCallback OnUserAssigned { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; } = default!;

        protected override void OnInitialized()
        {
            Users = userService.GetAllUsersButCleaners();
            Tables = tableService.GetUserFreeTable();
            AddUserVisible = false;
        }



        public async Task Cancel()
        {
            await OnUserAssigned.InvokeAsync(null);
        }

        #region Table Methods
        private void RowClickEvent(TableRowClickEventArgs<ITable> args)
        {
            if (mudTable != null)
            {
                selectedTable = args.Item;
                StateHasChanged();
            }
        }

        private string SelectedRowClassFunc(ITable table, int rowNumber)
        {
            if (selectedTable != null && Tables.IndexOf(selectedTable) == rowNumber)
            {
                selectedRowNumber = rowNumber;
                return "selected";
            }
            return string.Empty;
        }
        #endregion

        #region Table Methods
        private void UserRowClickEvent(TableRowClickEventArgs<IUser> args)
        {
            if (mudTable != null)
            {
                selectedUser = args.Item;
                StateHasChanged();
            }
        }

        private string USerSelectedRowClassFunc(IUser user, int userRowNumber)
        {
            if (selectedUser != null && Users.IndexOf(selectedUser) == userRowNumber)
            {
                userSelectedRowNumber = userRowNumber;
                return "selected";
            }
            return string.Empty;
        }
        #endregion


        private async Task AssignTable()
        {
            try
            {
                tableService.AddTableUser(selectedUser!.UserID, selectedTable!.GUID);
                Snackbar.Add("Table assigned successfully", Severity.Success);
            }
            catch (Exception e)
            {
                Snackbar.Add("Something went wrong", Severity.Error);
                Debug.WriteLine(e.Message);
            }



            await OnUserAssigned.InvokeAsync(null);
        }

        #region Search For Tables
        public bool FilterFunc(ITable element)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.GUID.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Manufacturer.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

        public bool FilterFuncUser(IUser element)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.UserID.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        #endregion
    }
}
