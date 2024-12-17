using Blazored.SessionStorage;
using Famicom.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.Services;

namespace Famicom.Components.Pages
{
    public partial class DashboardEmployee : ComponentBase
    {
        [Inject] ISessionStorageService _sessionStorage { get; set; } = default!;
        [Inject] HttpClient HttpClient { get; set; } = default!;
        [Inject] TableControllerService _tableControllerService { get; set; } = default!;
        [Inject] TableService tableService {get; set; } = default!;
        private TableModel _tableModel { get; set; } = null!;
        public TableComponent _tableComponent { get; set; } = null!;
        private ITable _table { get; set; } = null!; 
        private int userId { get; set; } = 0;

        protected override async Task OnInitializedAsync()
        {
            _tableModel = new TableModel(HttpClient, _tableControllerService, tableService);
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            userId = await _sessionStorage.GetItemAsync<int>("UserId");
            _table = _tableModel.GetTable(userId)!;
            StateHasChanged();
        }
    }
}
