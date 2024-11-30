using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Models.Services;
using SharedModels;
using Microsoft.Extensions.Configuration.UserSecrets;
using MudBlazor.Extensions;

namespace Famicom.Components.Pages
{
    public partial class HealthComponent : ComponentBase
    {
        [Inject] ISessionStorageService SessionStorage { get; set; } = default!;
        private HealthService healthService = new HealthService();
        private DateTime? todaysDate { get; set; }
        private DateTime? todaysMorning { get; set; }
        private List<SharedModels.Health>? todayHealth { get; set; }
        private int userId { get; set; } = 0;
        protected override async Task OnInitializedAsync()
        {
            todaysDate = DateTime.Now;
            todaysMorning = DateTime.Today + new TimeSpan(6, 0, 0);
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            userId = await SessionStorage.GetItemAsync<int>("UserId");
            todayHealth = healthService.GetHealth(userId);
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
