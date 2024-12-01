using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using SharedModels;
using Famicom.Models;
using System.Diagnostics;
using Models.Services;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Mvc;

namespace Famicom.Components.Pages
{
    public partial class DashboardBase : ComponentBase
    {
        [CascadingParameter]
        public string? Email { get; set; }

        [CascadingParameter]
        public int? UserId { get; set; }

        public ITable _table { get; set; } = null!;
        private TableModel tableModel { get; set; } = null!;
        private int userId { get; set; }
        [Inject] NavigationManager? Navigation { get; set; }
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
            
            userId = await sessionStorage.GetItemAsync<int>("UserId");
            _table = tableModel.GetTable(userId)!;
            StateHasChanged();
        }
    }
}
