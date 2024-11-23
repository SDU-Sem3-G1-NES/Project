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
        public string HealthData => HealthModel.HealthData;

        private Timer? TimerUpdater;
        private DateTime TimerStart;

        protected bool IsTimerRunning { get; private set; } = false;
        protected bool IsHeightLocked { get; set; } = true;
        public string SittingNotification { get; private set; } = "You are sitting comfortably.";
        public string HeightWarning { get; private set; } = string.Empty;
        public TimeSpan ElapsedTime => IsTimerRunning ? DateTime.Now - TimerStart : TimeSpan.Zero;

        private const int DeskMinHeight = 68; // Min desk height
        private const int DeskMaxHeight = 132; // Max desk height

        public async Task FetchHealthDataAsync()
        {
            await HealthModel.FetchHealthDataAsync();

            var minHeight = HealthModel.CalculateMinDeskHeight(HealthModel.UserHeightInCm);
            var maxHeight = HealthModel.CalculateMaxDeskHeight(HealthModel.UserHeightInCm);

            if (minHeight < DeskMinHeight || maxHeight > DeskMaxHeight)
            {
                HeightWarning = $"Recommended height range exceeds desk capabilities ({DeskMinHeight} cm - {DeskMaxHeight} cm).";
            }
            else
            {
                HeightWarning = string.Empty;
            }

            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            await FetchHealthDataAsync();
            StartAutomaticTimer();
        }

        protected void ToggleHeightLock()
        {
            IsHeightLocked = !IsHeightLocked;
        }

        private void StartAutomaticTimer()
        {
            TimerUpdater = new Timer(_ =>
            {
                // Simulate reading the current desk height (mock value for now)
                var currentDeskHeight = 70; // Mock value

                var minHeight = HealthModel.CalculateMinDeskHeight(HealthModel.UserHeightInCm);
                var maxHeight = HealthModel.CalculateMaxDeskHeight(HealthModel.UserHeightInCm);

                if (currentDeskHeight < minHeight)
                {
                    SittingNotification = "Desk height too low for sitting.";
                }
                else if (currentDeskHeight > maxHeight)
                {
                    SittingNotification = "Desk height too high for standing.";
                }
                else
                {
                    SittingNotification = "Desk height is in the recommended range.";
                }

                InvokeAsync(StateHasChanged);
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        public void Dispose()
        {
            TimerUpdater?.Dispose();
        }
    }
}
