using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;
using Famicom.Models;

namespace Famicom.Components.Pages
{
    public partial class HealthBase : ComponentBase, IDisposable
    {
        protected HealthModel HealthModel { get; set; } = new HealthModel();
        public string CurrentMode { get; private set; } = "Sitting";
        public string SittingNotification { get; private set; } = "You are sitting comfortably.";
        public TimeSpan ElapsedTime => IsTimerRunning ? DateTime.Now - TimerStart : TimeSpan.Zero;

        private Timer? TimerUpdater;
        private DateTime TimerStart;
        private bool IsTimerRunning = false;
        private const int ModeSwitchInterval = 30; // Minutes

        protected override Task OnInitializedAsync()
        {
            StartAutomaticTimer();
            return Task.CompletedTask;
        }

        public void SavePresets()
        {
            // Save the desk height presets to the backend or local storage (mocked for now)
            StateHasChanged();
        }

        private void StartAutomaticTimer()
        {
            TimerUpdater = new Timer(_ =>
            {
                DetectMode();

                if (CurrentMode == "Sitting" && ElapsedTime.TotalMinutes >= ModeSwitchInterval)
                {
                    SittingNotification = "You’ve been sitting for 30 minutes. Consider switching to standing mode.";
                }
                else
                {
                    SittingNotification = string.Empty;
                }

                InvokeAsync(StateHasChanged);
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        private void DetectMode()
        {
            // Mock current desk height (In real application, fetch from hardware or input)
            int currentDeskHeight = 70; // Example mock value
            if (Math.Abs(currentDeskHeight - HealthModel.SittingHeightPreset) <= 2)
            {
                CurrentMode = "Sitting";
                if (!IsTimerRunning)
                {
                    TimerStart = DateTime.Now;
                    IsTimerRunning = true;
                }
            }
            else if (Math.Abs(currentDeskHeight - HealthModel.StandingHeightPreset) <= 2)
            {
                CurrentMode = "Standing";
                TimerStart = DateTime.Now;
                IsTimerRunning = false;
            }
        }

        public void Dispose()
        {
            TimerUpdater?.Dispose();
        }
    }
}
