using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
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

        protected override async Task OnInitializedAsync()
        {
            StartAutomaticTimer();
            
            var presets = await FetchPresetsFromBackend();
            if (presets != null)
            {
                HealthModel.SittingHeightPreset = presets.SittingHeight;
                HealthModel.StandingHeightPreset = presets.StandingHeight;
            }
        }

        public async void SavePresets()
        {
            await SavePresetsToBackend();
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

            // Save the current mode to the backend
            SaveCurrentModeToBackend();
        }

        private async Task<HealthModel?> FetchPresetsFromBackend()
        {
            try
            {
                var healthData = await BackendService.GetHealthByUser(1, null); // Replace 1 with actual user ID
                if (healthData != null && healthData.Count > 0)
                {
                    var latestData = healthData[0]; // Assuming the latest data is the first in the list
                    return new HealthModel
                    {
                        SittingHeightPreset = latestData.SittingHeightPreset,
                        StandingHeightPreset = latestData.StandingHeightPreset
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching health presets: {ex.Message}");
            }
            return null;
        }

        private async Task SavePresetsToBackend()
        {
            try
            {
                await BackendService.InsertHealth(1, null, 0); // Replace with proper arguments
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving presets: {ex.Message}");
            }
        }

        private void SaveCurrentModeToBackend()
        {
            // You can implement a similar logic to store the CurrentMode in the backend
            // For now, this is just a placeholder
            Console.WriteLine($"Current Mode '{CurrentMode}' has been logged to the backend.");
        }

        public void Dispose()
        {
            TimerUpdater?.Dispose();
        }
    }
}
