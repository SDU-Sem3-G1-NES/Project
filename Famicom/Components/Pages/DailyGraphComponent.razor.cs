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
            if (todayHealth == null || todayHealth.Count < 2) return;

            TotalSittingTime = 0;
            TotalStandingTime = 0;

            
            var todayDate = DateTime.Today;
            var todayData = todayHealth.Where(h => h.Date.Date == todayDate).ToList();

            if (todayData.Count == 0) return;

            todayData.Sort((a, b) => a.Date.CompareTo(b.Date));

            for (int i = 0; i < todayData.Count - 1; i++)
            {
                var currentHealth = todayData[i];
                var nextHealth = todayData[i + 1];

                
                if ((nextHealth.Date - currentHealth.Date).TotalHours > 12)
                    continue;

                var timeSpent = (nextHealth.Date - currentHealth.Date).TotalHours;

                if (currentHealth.Position < 1000)
                {
                    TotalSittingTime += timeSpent;
                }
                else
                {
                    TotalStandingTime += timeSpent;
                }
            }

            
            var lastHealth = todayData.Last();
            if (lastHealth.Position < 1000)
            {
                TotalSittingTime += 0.5;
            }
            else
            {
                TotalStandingTime += 0.5;
            }

            
            TotalSittingTime = Math.Round(TotalSittingTime, 2);
            TotalStandingTime = Math.Round(TotalStandingTime, 2);
        }




        public string FormatTime(decimal hours)
        {
            var h = (int)hours;
            var m = (int)((hours - h) * 60);
            return $"{h}h {m}m";
        }


        public class TodayTime()
        {
            public required string Position { get; set; }
            public required double Time { get; set; }
        }
    }
}

