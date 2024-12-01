using Blazored.SessionStorage;
using Famicom.Models;
using Microsoft.AspNetCore.Components;
using Models.Services;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Famicom.Components.Pages
{
    public partial class DashboardCleaner : ComponentBase
    {
        public bool IsCleaningMode { get; private set; }
        private bool IsProcessing { get; set; } = false;
        private string ProcessingMessage { get; set; } = string.Empty;
        private bool IsFunctionRunning { get; set; } = false;

        private CleanerService? cleanerService { get; set; }
        private CleanerModel? cleanerModel { get; set; }

        [Inject] IHttpClientFactory? httpClientFactory { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] ISessionStorageService SessionStorage { get; set; } = default!;
        [Inject] private UserPermissionService UserPermissionService { get; set; } = default!;
        [Inject] TableControllerService tableControllerService { get; set; } = default!;
        private UserModel userModel { get; set; } = new UserModel();

        protected override void OnInitialized()
        {
            var client = httpClientFactory!.CreateClient("default");
            cleanerService = new CleanerService(tableControllerService, client);
            cleanerModel = new CleanerModel(cleanerService);
            IsCleaningMode = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await Protect();
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task Protect()
        {
            var userid = await SessionStorage.GetItemAsync<int>("UserId");
            UserPermissionService.SetUser(userModel.GetUser(userId: userid)!);
            if (!UserPermissionService.RequireOne("CanAccess_CleanerPage"))
                NavigationManager.NavigateTo("/unauthorised");
        }

        public async Task ToggleCleaningMode()
        {
            if (IsFunctionRunning) return;

            IsFunctionRunning = true;
            IsProcessing = true;
            ProcessingMessage = IsCleaningMode
                ? "Deactivating Cleaning Mode, please wait..."
                : "Activating Cleaning Mode, please wait...";

            StateHasChanged();

            try
            {
                IsCleaningMode = !IsCleaningMode;

                if (IsCleaningMode && cleanerModel != null)
                {
                    await cleanerModel.UpdateAllTablesMaxHeight();
                }
                else if (!IsCleaningMode && cleanerModel != null)
                {
                    await cleanerModel.RevertAllTables();
                }

                Snackbar.Add(IsCleaningMode ? "Cleaning Mode activated!" : "Cleaning Mode deactivated!", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"An error occurred: {ex.Message}", Severity.Error);
            }
            finally
            {
                IsProcessing = false;
                IsFunctionRunning = false;
                StateHasChanged();
            }
        }
    }
}
