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
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        [Inject] ISessionStorageService SessionStorage { get; set; } = default!;

        [Inject] private UserPermissionService UserPermissionService { get; set; } = default!;
        private UserModel userModel { get; set; } = new UserModel();


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await Protect();
            await base.OnAfterRenderAsync(firstRender);
        }
        
        private async Task Protect()
        {
            var userid = await SessionStorage.GetItemAsync<int>("UserId");
            UserPermissionService.SetUser(userModel.GetUser(userId: userid)!);
            if (!UserPermissionService.RequireOne("CanAccess_CleanerPage")) NavigationManager.NavigateTo("/unauthorised");
        }
    }
}
