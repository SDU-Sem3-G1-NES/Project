using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading;
using System.Threading.Tasks;
using Famicom.Models;

namespace Famicom.Components.Pages
{
    public partial class HealthBase : ComponentBase, IDisposable
    {
        protected HealthModel HealthModel { get; set; } = new HealthModel();
        public string HealthData => HealthModel.HealthData;

        private Timer? SittingTimer;
        private Timer? ElapsedTimeUpdater;
        private DateTime TimerStart;

        protected bool IsTimerRunning { get; private set; } = false;

        public string SittingNotification { get; private set; } = "You are sitting comfortably.";
        public TimeSpan ElapsedTime => IsTimerRunning ? DateTime.Now - TimerStart : TimeSpan.Zero;

        public async Task FetchHealthDataAsync()
        {
            await HealthModel.FetchHealthDataAsync();
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            await FetchHealthDataAsync();
        }

        protected void StartSittingTimer()
        {
            if (IsTimerRunning) return;

            TimerStart = DateTime.Now;
            IsTimerRunning = true;

            SittingTimer = new Timer(async _ => await NotifySittingTimerElapsed(), null, TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(30));
            ElapsedTimeUpdater = new Timer(_ => InvokeAsync(StateHasChanged), null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        protected void StopSittingTimer()
        {
            IsTimerRunning = false;

            SittingTimer?.Dispose();
            SittingTimer = null;

            ElapsedTimeUpdater?.Dispose();
            ElapsedTimeUpdater = null;

            SittingNotification = "Timer stopped.";
        }

        protected async Task NotifySittingTimerElapsed()
        {
            SittingNotification = "Time to stand up and stretch!";
            await InvokeAsync(StateHasChanged);
        }

        protected void ResetSittingTimer()
        {
            StopSittingTimer();
            StartSittingTimer();
            SittingNotification = "Timer reset. Keep working comfortably.";
        }

        public void Dispose()
        {
            SittingTimer?.Dispose();
            ElapsedTimeUpdater?.Dispose();
        }
    }
}
