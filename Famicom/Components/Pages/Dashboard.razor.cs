using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using SharedModels;
using Famicom.Models;
using System.Diagnostics;
using Models.Services;
using Blazored.SessionStorage;

namespace Famicom.Components.Pages
{
    public partial class DashboardBase : ComponentBase
    {
        [CascadingParameter]
        public string? Email { get; set; }

        [CascadingParameter]
        public int? UserId { get; set; }

        [CascadingParameter]
        public bool? isLoggedIn { get; set; }
    }
}
