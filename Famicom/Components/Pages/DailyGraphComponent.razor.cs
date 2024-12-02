using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Models.Services;
using MudBlazor;
using static Famicom.Components.Pages.WeeklyGraphComponent;

namespace Famicom.Components.Pages
{
    public partial class DailyGraphComponent : ComponentBase
    {
        [Inject] ISessionStorageService SessionStorage { get; set; } = default!;

        private HealthService healthService = new HealthService();

        #region Today's Health Properties
        private DateTime? todaysDate { get; set; }
        private DateTime? todaysMorning { get; set; }
        private List<SharedModels.Health>? todayHealth { get; set; }
        private List<SharedModels.Health>? todaySitingTime { get; set; }
        private List<SharedModels.Health>? todayStandingTime { get; set; }
        public required double TotalSittingTime { get; set; }
        public required double TotalStandingTime { get; set; }

        #endregion

     
        #region Donut Chart Properties
        public int SelectedIndex { get; set; }
        public required double[] DailyData { get; set; }
        public required List<TodayTime>? DailyTime { get; set; } = new List<TodayTime>();
        public string[]? DailyLabels { get; set; } = { "Sitting Time", "Standing Time" };
        #endregion

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
            todayHealth = healthService.GetHealth(userId, todaysMorning);
            // Calculate total time spend on sitting and standing position for today
            CalculateTotalDailyTimeSpend();
            // Set the data for the donut chart
            DailyData = new double[] { TotalSittingTime, TotalStandingTime };
            DailyTime = new List<TodayTime>
            {
                new TodayTime()
                {
                    Position = "Sitting Time",
                    Time = TotalSittingTime
                },
                new TodayTime()
                {
                    Position = "Standing Time",
                    Time = TotalStandingTime
                }

            };
            StateHasChanged();
            await base.OnAfterRenderAsync(firstRender);
        }

        public string GetUserSittingStandingRatio()
        {
            if (TotalSittingTime == 0 && TotalStandingTime == 0) return "0:0";
            var ratio = TotalSittingTime / TotalStandingTime;
            return $"{Math.Round(ratio, 1)}:1";
        }

        public string GetUserSittingStandingAdviceText() {
            if (TotalSittingTime == 0 && TotalStandingTime == 0) return "";
            var ratio = Math.Round(TotalSittingTime / TotalStandingTime, 1);
            if(ratio > 3.5) return "You are sitting too much. You should stand up for a while.";
            if(ratio < 2.5) return "You are standing too much. You should sit down for a while.";
            return "";
        }

        public void CalculateTotalDailyTimeSpend()
        {
            if (todayHealth == null) return;
            TotalSittingTime = 0;
            TotalStandingTime = 0;
            for (int i = 1; i < todayHealth.Count; i++)
            {
                var previousHealth = todayHealth[i - 1];
                var currentHealth = todayHealth[i];
                var timeSpent = (currentHealth.Date - previousHealth.Date).TotalHours;
                if (previousHealth.Position < 1000 && currentHealth.Position < 1000)
                {
                    TotalSittingTime += timeSpent;
                }
                else if (previousHealth.Position > 1000 && currentHealth.Position > 1000)
                {
                    TotalStandingTime += timeSpent;
                }
                else if (previousHealth.Position < 1000 && currentHealth.Position > 1000)
                {
                    var sittingTime = (currentHealth.Date - previousHealth.Date).TotalHours * (1000 - previousHealth.Position) / 1000;
                    var standingTime = (currentHealth.Date - previousHealth.Date).TotalHours * (currentHealth.Position - 1000) / 1000;
                    TotalSittingTime += sittingTime;
                    TotalStandingTime += standingTime;
                }
                else if (previousHealth.Position > 1000 && currentHealth.Position < 1000)
                {
                    var standingTime = (currentHealth.Date - previousHealth.Date).TotalHours * (previousHealth.Position - 1000) / 1000;
                    var sittingTime = (currentHealth.Date - previousHealth.Date).TotalHours * (1000 - currentHealth.Position) / 1000;
                    TotalSittingTime += sittingTime;
                    TotalStandingTime += standingTime;
                }
            }

        }
        public class TodayTime()
        {
            public required string Position { get; set; }
            public required double Time { get; set; }
        }
    }
}

