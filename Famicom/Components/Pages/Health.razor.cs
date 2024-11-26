using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using SharedModels;
using Famicom.Models;
using System.Diagnostics;

namespace Famicom.Components.Pages
{
    public partial class HealthBase : ComponentBase
    {
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
    }
}
