﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using SharedModels;
using Famicom.Models;
using static MudBlazor.Colors;
using Models.Services;
using System.Diagnostics;
using DotNetEnv;
using Blazored.SessionStorage;

namespace Famicom.Components.Pages
{
    public partial class AdminBase : ComponentBase
    {
       
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        [Inject] ISessionStorageService SessionStorage { get; set; } = default!;

        [Inject] private UserPermissionService UserPermissionService { get; set; } = default!;

        [Inject] public ISnackbar Snackbar { get; set; } = default!;

        public string? PanelTitle { get; set; }

        private TableService tableService = new TableService();

        private UserService userService = new UserService();
        private UserModel userModel { get; set; } = new UserModel();
        public required List<ITable> Table { get; set; }
        public bool IsTableOverlayActivated { get; set; } = false;
        public bool IsUserOverlayActivated { get; set; } = false;
        public bool IsAssignOverlayActivated { get; set; } = false;
        public string searchString = "";
        public required List<IUser> Users { get; set; }

        #region Edit Table Variables  
        
        public bool tableReadOnly = true;
        public ITable? selectedItem1 = null;
        public LinakTable? elementBeforeEdit;
        public HashSet<ITable> selectedItems1 = new HashSet<ITable>();
        public TableApplyButtonPosition applyButtonPosition = TableApplyButtonPosition.End;
        public TableEditButtonPosition editButtonPosition = TableEditButtonPosition.End;
        public TableEditTrigger editTrigger = TableEditTrigger.RowClick;
        public IEnumerable<ITable> Elements = new List<ITable>();

        public void BackupItem(object element)
        {
            elementBeforeEdit = new(
                ((ITable)element).Name,
                ((ITable)element).GUID,
                ((ITable)element).Manufacturer
                );
            
        }

        public void ItemHasBeenCommitted(object element)
        {
            try
            {
                tableService.UpdateTableName(((ITable)element).GUID,((ITable)element).Name);
                tableService.UpdateTableManufacturer(((ITable)element).GUID, ((ITable)element).Manufacturer);
                Table = tableService.GetAllTables();
                Snackbar.Add("Table has been changed succesfuly", Severity.Success);
                elementBeforeEdit = null;
            }
            catch (Exception e)
            {
                ResetItemToOriginalValues(element);
                Debug.WriteLine(e.Message);
                Snackbar.Add("An error occurred while updating the table", Severity.Error);
            }
        }

        public void ResetItemToOriginalValues(object element)
        {
            ((ITable)element).Name = elementBeforeEdit!.Name;
            ((ITable)element).Manufacturer = elementBeforeEdit!.Manufacturer;
        }

        #endregion


        protected override async Task OnInitializedAsync()
        {

            tableService = new TableService();
            Table = tableService.GetAllTables();
            Users = userService.GetAllUsers();
            PanelTitle = GetUserType();
            await base.OnInitializedAsync();
        }
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await Protect();
            await base.OnAfterRenderAsync(firstRender);
        }

        private string GetUserType()
        {
            string userType = userModel.GetUserType();
            if (userType == "Admin")
            {
                return "Admin Panel";
            }
            return "User Panel";
        }


        public void RefreshPage()
        {
            NavigationManager?.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }


        public void AddTableOverlay(bool value)
        {

            IsTableOverlayActivated = value;

        }

        public void AddUserOverlay(bool value)
        {
            IsUserOverlayActivated = value;
        }


        public void AssignOverlay(bool value)
        {
            IsAssignOverlayActivated = value;
        }

        #region Methods for closing overlay from component
        public async Task HandleUserAdded(bool isCancelled)
        {
            IsUserOverlayActivated = false;
            await InvokeAsync(StateHasChanged);
            Users = userService.GetAllUsers();
        }

        public async Task HandleTableAdded(bool isCancelled)
        {
            IsTableOverlayActivated = false;
            await InvokeAsync(StateHasChanged);
            Table = tableService.GetAllTables();



        }

        public async Task HandleUserAssigned()
        {
            IsAssignOverlayActivated = false;
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region For table
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

        public string CheckAssignedTable(int id)
        {
            return userService.GetUserAssignedTable(id);
        }

        private async Task Protect()
        {
            var userid = await SessionStorage.GetItemAsync<int>("UserId");
            UserPermissionService.SetUser(userModel.GetUser(userId: userid)!);
            if (!UserPermissionService.RequireOne("CanAccess_AdminPage")) NavigationManager.NavigateTo("/unauthorised");
        }
    }
}