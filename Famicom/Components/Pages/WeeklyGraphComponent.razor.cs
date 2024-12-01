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
    public partial class WeeklyGraphComponent : ComponentBase
    {
        [Inject] ISessionStorageService SessionStorage { get; set; } = default!;

        private HealthService healthService = new HealthService();

        #region Weekly Health Properties
        private bool isEmpty { get; set; }
        private DateTime? todaysDate { get; set; }
        private DateTime? todaysMorning { get; set; }
        private DateTime StartOfWeek { get; set; }
        private DateTime EndOfWeek { get; set; }
        public required List<DayValue>? dayValues { get; set; }
        private List<SharedModels.Health>? weeklyHealth { get; set; }
        private List<SharedModels.Health>? weeklySitingTime { get; set; }
        private List<SharedModels.Health>? weeklyStandingTime { get; set; }

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
            weeklyHealth = healthService.GetHealth(userId, StartOfWeek, EndOfWeek);
            // Calculate daily time spend on sitting and standing position by Days of week
            CalculateDailyTimes();
            // Set the data for the bar chart
            WeeklyData = new List<ChartSeries>
                {
                    new ChartSeries
                    {
                        Name = "Sitting Time",
                        Data = dayValues!.Select(d => d.SittingTime).ToArray()
                    },
                    new ChartSeries
                    {
                        Name = "Standing Time",
                        Data = dayValues!.Select(d => d.StandingTime).ToArray()
                    }
                };
            isEmpty = IsNullOrEmpty();
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


        protected DateTime GetStartOfWeek(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        private bool IsNullOrEmpty() => WeeklyData!.All(series => series.Data.All(value => value > 0));

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


        public class DayValue
        {
            public required string Day { get; set; }
            public required double SittingTime { get; set; }
            public required double StandingTime { get; set; }
        }
    }
}
