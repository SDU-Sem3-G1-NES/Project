using Blazored.SessionStorage;
using Famicom.Models;
using Microsoft.AspNetCore.Components;
using Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Famicom.Components.Pages
{
    public partial class DashboardCleaner : ComponentBase
    {
        public bool IsCleaningMode { get; private set; }
        private CleanerService? cleanerService { get; set; }
        private CleanerModel? cleanerModel { get; set; }

        [Inject] IHttpClientFactory? httpClientFactory { get; set; } 
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] ISessionStorageService SessionStorage { get; set; } = default!;
        [Inject] private UserPermissionService UserPermissionService { get; set; } = default!;
        private UserModel userModel { get; set; } = new UserModel();

        protected override void OnInitialized()
        {
            var tableControllerService = new TableControllerService();
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

            IsCleaningMode = !IsCleaningMode;
            if (IsCleaningMode && cleanerModel != null)
            {
                await cleanerModel.UpdateAllTablesMaxHeight();
            }
            else if (!IsCleaningMode && cleanerModel != null)
            {
                await cleanerModel.RevertAllTables();
            }
        }
    }
}
