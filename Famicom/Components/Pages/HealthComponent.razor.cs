using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Models.Services;
using SharedModels;
using Microsoft.Extensions.Configuration.UserSecrets;
using MudBlazor.Extensions;
using System.Security.AccessControl;

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
        #endregion

        #region Weekly Health Properties
        private DateTime StartOfWeek { get; set; }
        private DateTime EndOfWeek { get; set; }
        private List<DayValue>? dayValues { get; set; }
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
            todayHealth = healthService.GetHealth(userId, todaysMorning);
            weeklyHealth = healthService.GetHealth(userId, StartOfWeek, EndOfWeek);
            CheckPosition();
            CalculateDailyTimes();
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

            var dailyTimes = new Dictionary<DaysOfWeek, (int SittingTime, int StandingTime)>();

            foreach (var health in weeklyHealth)
            {
                DaysOfWeek day = (DaysOfWeek)Enum.Parse(typeof(DaysOfWeek), health.Date.DayOfWeek.ToString());
                if (dailyTimes.ContainsKey(day))
                {
                    if (health.Position < 1000)
                    {
                        dailyTimes[day] = (dailyTimes[day].SittingTime + health.Position, dailyTimes[day].StandingTime);
                    }
                    else
                    {
                        dailyTimes[day] = (dailyTimes[day].SittingTime, dailyTimes[day].StandingTime + health.Position);
                    }
                }
                else
                {
                    if (health.Position < 1000)
                    {
                        dailyTimes[day] = (health.Position, 0);
                    }
                    else
                    {
                        dailyTimes[day] = (0, health.Position);
                    }
                }
            }

            dayValues = dailyTimes.Select(d => new DayValue
            {
                Day = d.Key.ToString(),
                SittingTime = d.Value.SittingTime,
                StandingTime = d.Value.StandingTime
            }).ToList();


        }

        public class DayValue
        {
            public required string Day { get; set; }
            public required int SittingTime { get; set; }
            public required int StandingTime { get; set; }
        }
    }
}
