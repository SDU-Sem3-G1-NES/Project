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
        public required List<DayValue>? DayValues { get; set; } = new List<DayValue>();
        private List<SharedModels.Health>? weeklyHealth { get; set; }
        private List<SharedModels.Health>? weeklySitingTime { get; set; }
        private List<SharedModels.Health>? weeklyStandingTime { get; set; }

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




        protected void CalculateDailyTimes()
        {
            if (weeklyHealth == null || weeklyHealth.Count == 0) return;

            var dailyTimes = Enum.GetValues(typeof(DaysOfWeek))
                .Cast<DaysOfWeek>()
                .ToDictionary(day => day, _ => (SittingTime: 0.0, StandingTime: 0.0));

            weeklyHealth.Sort((a, b) => a.Date.CompareTo(b.Date));

            for (int i = 0; i < weeklyHealth.Count - 1; i++)
            {
                var currentHealth = weeklyHealth[i];
                var nextHealth = weeklyHealth[i + 1];

                
                if ((nextHealth.Date - currentHealth.Date).TotalHours > 12)
                    continue;

                DaysOfWeek currentDay = (DaysOfWeek)currentHealth.Date.DayOfWeek;
                var timeSpent = (nextHealth.Date - currentHealth.Date).TotalHours;

                if (currentHealth.Position < 1000)
                {
                    dailyTimes[currentDay] = (
                        dailyTimes[currentDay].SittingTime + timeSpent,
                        dailyTimes[currentDay].StandingTime
                    );
                }
                else
                {
                    dailyTimes[currentDay] = (
                        dailyTimes[currentDay].SittingTime,
                        dailyTimes[currentDay].StandingTime + timeSpent
                    );
                }
            }

            // Handle the last entry of each day
            var lastHealth = weeklyHealth.Last();
            DaysOfWeek lastDay = (DaysOfWeek)lastHealth.Date.DayOfWeek;
            if (lastHealth.Position < 1000)
            {
                dailyTimes[lastDay] = (
                    dailyTimes[lastDay].SittingTime + 0.5,
                    dailyTimes[lastDay].StandingTime
                );
            }
            else
            {
                dailyTimes[lastDay] = (
                    dailyTimes[lastDay].SittingTime,
                    dailyTimes[lastDay].StandingTime + 0.5
                );
            }

            DayValues = dailyTimes.Select(d => new DayValue
            {
                Day = d.Key.ToString(),
                SittingTime = Math.Round(d.Value.SittingTime, 2),
                StandingTime = Math.Round(d.Value.StandingTime, 2)
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
