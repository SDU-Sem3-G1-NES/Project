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

        [Inject] IHttpClientFactory? ClientFactory { get; set; }

        [Inject] TableControllerService? TableControllerService { get; set; }

        [Parameter] public string[] Guid { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            var tc = await TableControllerService!.GetTableController("cd:fb:1a:53:fb:e6", ClientFactory!.CreateClient("default"));
            Guid = await tc.GetAllTableIds();
        }
    }
}
