using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using SharedModels;
using Famicom.Models;
using System.Diagnostics;
using Models.Services;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using Famicom.Components.Pages;

namespace Famicom.Components.Pages
{
    public partial class DashboardBase : ComponentBase
    {
        [Inject] ISessionStorageService SessionStorage { get; set; } = default!;
        [Inject] NavigationManager Navigation { get; set; } = default!;
        [Inject] ISnackbar Snackbar { get; set; } = default!;
        private UserModel userModel = new UserModel();

        public ITable _table { get; set; } = null!;
        private TableModel tableModel { get; set; } = null!;
        private int userId { get; set; }
        public RenderFragment? _content;
        [Inject] IHttpClientFactory clientFactory { get; set; } = default!;
        [Inject] ISessionStorageService sessionStorage { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            tableModel = new TableModel(clientFactory);
            if(Navigation != null && Navigation.Uri.Replace(Navigation.BaseUri, "/") == "/") {
                Navigation.NavigateTo("/Dashboard");
                return;
            }
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var isLoggedIn = await SessionStorage.GetItemAsync<bool>("IsLoggedIn");
            if(!isLoggedIn || _content != null) return;
            var email = await SessionStorage.GetItemAsync<string>("Email");
            var user = userModel.GetUser(email);

            switch (user!.GetType().Name)
            {
                case "Employee":
                case "Admin":
                    _content = builder =>
                    {
                        builder.OpenComponent(0, typeof(DashboardEmployee));
                        builder.CloseComponent();
                    };
                    StateHasChanged();
                    break;
                case "Cleaner":
                    Navigation.NavigateTo("/Cleaning");
                    StateHasChanged();
                    break;
                default:
                    _content = builder =>
                    {
                        builder.OpenComponent(0, typeof(Unauthorised));
                        builder.CloseComponent();
                    };
                    StateHasChanged();
                    break;
            }
            
        }
    }
}
