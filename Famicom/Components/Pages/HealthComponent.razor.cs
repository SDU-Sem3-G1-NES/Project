using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Models.Services;
using SharedModels;
using Microsoft.Extensions.Configuration.UserSecrets;
using MudBlazor.Extensions;
using System.Security.AccessControl;
using MudBlazor;

namespace Famicom.Components.Pages
{
    public partial class HealthComponent : ComponentBase
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

        #region Weekly Health Properties
        private DateTime StartOfWeek { get; set; }
        private DateTime EndOfWeek { get; set; }
        private List<DayValue>? dayValues { get; set; }
        private List<SharedModels.Health>? weeklyHealth { get; set; }
        private List<SharedModels.Health>? weeklySitingTime { get; set; }
        private List<SharedModels.Health>? weeklyStandingTime { get; set; }

        #endregion
        #region Donut Chart Properties
        public int SelectedIndex { get; set; }
        public required double[] DailyData { get; set; }
        public string[]? DailyLabels { get; set; } = { "Sitting Time", "Standing Time" };
        #endregion

        #region Bar Chart Properties

        public List<ChartSeries>? WeeklyData { get; set; }
        public string[] XAxisLabels = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

        #endregion

        private int userId { get; set; } = 0;
        protected override async Task OnInitializedAsync()
        {
            todaysDate = DateTime.Now;
            todaysMorning = DateTime.Today + new TimeSpan(6, 0, 0);
            StartOfWeek = GetStartOfWeek(todaysDate.Value);
            EndOfWeek = StartOfWeek.AddDays(6);
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            userId = await SessionStorage.GetItemAsync<int>("UserId");
            todayHealth = healthService.GetHealth(userId, todaysMorning);
            weeklyHealth = healthService.GetHealth(userId, StartOfWeek, EndOfWeek);
            CheckPosition();
            // Calculate daily time spend on sitting and standing position by Days of week
            CalculateDailyTimes();
            // Calculate total time spend on sitting and standing position for today
            CalculateTotalDailyTimeSpend();
            // Set the data for the donut chart
            DailyData = new double[] { TotalSittingTime, TotalStandingTime };
            // Set the data for the bar chart
            WeeklyData = new List<ChartSeries>
                {
                    new ChartSeries
                    {
                        Name = "Sitting Time",
                        Data = dayValues.Select(d => d.SittingTime).ToArray()
                    },
                    new ChartSeries
                    {
                        Name = "Standing Time",
                        Data = dayValues.Select(d => d.StandingTime).ToArray()
                    }
                };
            StateHasChanged();
            await base.OnAfterRenderAsync(firstRender);
        }

        public enum DaysOfWeek
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        }

        
        protected void CheckPosition()
        {
            if (todayHealth != null)
            {
                todaySitingTime = todayHealth.Where(x => x.Position < 1000).ToList();
                todayStandingTime = todayHealth.Where(x => x.Position > 1000).ToList();
            }
            if (weeklyHealth != null)
            {
                weeklySitingTime = weeklyHealth.Where(x => x.Position < 1000).ToList();
                weeklyStandingTime = weeklyHealth.Where(x => x.Position > 1000).ToList();
            }
        }

        protected DateTime GetStartOfWeek(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        protected void CalculateDailyTimes()
        {
            if (weeklyHealth == null) return;

            var dailyTimes = new Dictionary<DaysOfWeek, (double SittingTime, double StandingTime)>();

            for (int i = 1; i < weeklyHealth.Count; i++)
            {
                var previousHealth = weeklyHealth[i - 1];
                var currentHealth = weeklyHealth[i];

                var timeSpent = (currentHealth.Date - previousHealth.Date).TotalHours;
                DaysOfWeek day = (DaysOfWeek)Enum.Parse(typeof(DaysOfWeek), previousHealth.Date.DayOfWeek.ToString());

                if (!dailyTimes.ContainsKey(day))
                {
                    dailyTimes[day] = (0, 0);
                }

                if (previousHealth.Position < 1000 && currentHealth.Position < 1000)
                {
                    dailyTimes[day] = (dailyTimes[day].SittingTime + timeSpent, dailyTimes[day].StandingTime);
                }
                else if (previousHealth.Position > 1000 && currentHealth.Position > 1000)
                {
                    dailyTimes[day] = (dailyTimes[day].SittingTime, dailyTimes[day].StandingTime + timeSpent);
                }
                else if (previousHealth.Position < 1000 && currentHealth.Position > 1000)
                {
                    var sittingTime = (currentHealth.Date - previousHealth.Date).TotalHours * (1000 - previousHealth.Position) / 1000;
                    var standingTime = (currentHealth.Date - previousHealth.Date).TotalHours * (currentHealth.Position - 1000) / 1000;
                    dailyTimes[day] = (dailyTimes[day].SittingTime + sittingTime, dailyTimes[day].StandingTime + standingTime);
                }
                else if (previousHealth.Position > 1000 && currentHealth.Position < 1000)
                {
                    var standingTime = (currentHealth.Date - previousHealth.Date).TotalHours * (previousHealth.Position - 1000) / 1000;
                    var sittingTime = (currentHealth.Date - previousHealth.Date).TotalHours * (1000 - currentHealth.Position) / 1000;
                    dailyTimes[day] = (dailyTimes[day].SittingTime + sittingTime, dailyTimes[day].StandingTime + standingTime);
                }
            }

            dayValues = dailyTimes.Select(d => new DayValue
            {
                Day = d.Key.ToString(),
                SittingTime = (double)d.Value.SittingTime,
                StandingTime = (double)d.Value.StandingTime
            }).ToList();
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

        public class DayValue
        {
            public required string Day { get; set; }
            public required double SittingTime { get; set; }
            public required double StandingTime { get; set; }
        }
    }
}
