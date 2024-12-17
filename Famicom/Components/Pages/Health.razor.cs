using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using SharedModels;
using Famicom.Models;
using System.Diagnostics;
using Blazored.SessionStorage;
using Models.Services;
using System.Runtime.CompilerServices;

namespace Famicom.Components.Pages
{
    public partial class HealthBase : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] ISessionStorageService SessionStorage { get; set; } = default!;
        [Inject] private UserPermissionService UserPermissionService { get; set; } = default!;
        private UserModel userModel { get; set; } = new UserModel();
        private HealthModel HealthModel { get; set; } = new HealthModel();

        public string HealthData => HealthModel.HealthData;

        public void FetchHealthData()
        {
            HealthModel.FetchHealthData();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            FetchHealthData();
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
            if(!UserPermissionService.RequireOne("CanAccess_HealthPage")) NavigationManager.NavigateTo("/unauthorised");
        }
    }
}
