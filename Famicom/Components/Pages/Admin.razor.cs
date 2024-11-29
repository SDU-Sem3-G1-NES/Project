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
        public string? PanelTitle { get; set; }

        private TableService tableService = new TableService();
        private UserModel userModel { get; set; } = new UserModel();
        public required List<ITable> Table { get; set; }
        public bool IsTableOverlayActivated { get; set; } = false;
        public bool IsUserOverlayActivated { get; set; } = false;
        public bool IsAssignOverlayActivated { get; set; } = false;
        public string searchString = "";

        #region Properties for Search, Filter and Sorting
        public string? orderValue { get; set; }
        public string? selectedRoom { get; set; }
        public string? selectedTable { get; set; }
        public bool coerceValue { get; set; }
        public bool resetValueOnEmptyText { get; set; }
        public bool coerceText { get; set; }

        //Mock data for rooms and tables
        public List<string> roomNames = new List<string>() { "None", "Room 1", "Room 2", "Room 3", "Room 4", "Room 5" };

        public List<string> tableNames = new List<string>() { "None", "Table 1", "Table 2", "Table 3", "Table 4", "Table 5", "Julka", "Hulk", "SpiderMan", "America", "Razor" };

        #endregion


        protected override void OnInitialized()
        {

            tableService = new TableService();
            Table = tableService.GetAllTables();
            PanelTitle = GetUserType();
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

        public async Task<IEnumerable<string>> Search1(string value, CancellationToken token)
        {

            await Task.Delay(5, token);

            if (string.IsNullOrEmpty(value))
                return tableNames;
            return tableNames.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
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
        #endregion

        private async Task Protect()
        {
            var userid = await SessionStorage.GetItemAsync<int>("UserId");
            UserPermissionService.SetUser(userModel.GetUser(userId: userid)!);
            if (!UserPermissionService.RequireOne("CanAccess_AdminPage")) NavigationManager.NavigateTo("/unauthorised");
        }
    }
}