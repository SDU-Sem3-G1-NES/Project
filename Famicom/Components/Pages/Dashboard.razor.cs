using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using SharedModels;
using Famicom.Models;
using System.Diagnostics;
using Models.Services;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Famicom.Components.Pages
{
    public partial class DashboardBase : ComponentBase
    {
        [CascadingParameter]
        public string? Email { get; set; }

        [CascadingParameter]
        public int? UserId { get; set; }

        [Inject] NavigationManager? Navigation { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if(Navigation != null && Navigation.Uri.Replace(Navigation.BaseUri, "/") == "/") {
                Navigation.NavigateTo("/Dashboard");
                return;
            }
            await base.OnInitializedAsync();
        }
    }
}
