using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;
using Famicom.Models;

namespace Famicom.Components.Pages
{
    public partial class HealthBase : ComponentBase
    {
        protected HealthModel HealthModel { get; set; } = new HealthModel();

        public string HealthData => HealthModel.HealthData;

        public async Task FetchHealthDataAsync()
        {
            await HealthModel.FetchHealthDataAsync();
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            await FetchHealthDataAsync();
        }
    }
}
