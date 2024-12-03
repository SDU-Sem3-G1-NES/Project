using Famicom.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Diagnostics;
using Blazored.SessionStorage;
using Models.Services;


namespace Famicom.Components.Pages
{
    public partial class HealthPresetComponent : ComponentBase
    {
        private PresetsModel? presetsModel { get; set; }
        private int UserHeight { get; set; }
        private double SittingPosition { get; set; }
        private double StandingPosition { get; set; }
        private string? ErrorMessage { get; set; }
        private int UserId { get; set; }

        [Inject]
        ISessionStorageService SessionStorage { get; set; } = default!;

        [Inject]
        ISnackbar Snackbar { get; set; } = default!;

        [Inject]
        IHttpClientFactory ClientFactory { get; set; } = default!;
        
        [Inject]
        TableControllerService TableControllerService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                presetsModel = new PresetsModel(ClientFactory, TableControllerService);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            UserId = await SessionStorage.GetItemAsync<int>("UserId");
            StateHasChanged();
        }

        private void CalculateTaskHeight()
        {
            SittingPosition = UserHeight * 0.4;
            StandingPosition = UserHeight * 0.6;

            SittingPosition = Math.Clamp(SittingPosition, 68, 132);
            StandingPosition = Math.Clamp(StandingPosition, 68, 132);
        }
        private async Task CreatePreset()
        {
            try
            {
                int SittingHeight = (int)Math.Round(SittingPosition * 10);
                int StandingHeight = (int)Math.Round(StandingPosition * 10);

                var userPresets = await Task.Run(() => presetsModel!.GetAllPresets(UserId));
                if(userPresets.Any(preset => preset.PresetName == "SittingHealthPreset") && 
                userPresets.Any(preset => preset.PresetName == "StandingHealthPreset"))
                {
                    await Task.Run(() => presetsModel?.EditPreset(userPresets.First(preset => preset.PresetName == "SittingHealthPreset").PresetId, "SittingHealthPreset", UserId, SittingHeight, "{}", "fa-medkit"));
                    await Task.Run(() => presetsModel?.EditPreset(userPresets.First(preset => preset.PresetName == "StandingHealthPreset").PresetId, "StandingHealthPreset", UserId, StandingHeight, "{}", "fa-medkit"));
                    Snackbar.Add("Presets updated successfully", Severity.Success);
                }
                else if (userPresets.Any(preset => preset.PresetName == "SittingHealthPreset") && !userPresets.Any(preset => preset.PresetName == "StandingHealthPreset"))
                {
                    await Task.Run(() => presetsModel?.EditPreset(userPresets.First(preset => preset.PresetName == "SittingHealthPreset").PresetId, "SittingHealthPreset", UserId, SittingHeight, "{}", "fa-medkit"));
                    await Task.Run(() => presetsModel?.AddPreset("StandingHealthPreset", UserId, StandingHeight, "{}", "fa-medkit"));
                    Snackbar.Add("Presets updated successfully", Severity.Success);
                }
                else if (!userPresets.Any(preset => preset.PresetName == "SittingHealthPreset") && userPresets.Any(preset => preset.PresetName == "StandingHealthPreset"))
                {
                    await Task.Run(() => presetsModel?.EditPreset(userPresets.First(preset => preset.PresetName == "StandingHealthPreset").PresetId, "StandingHealthPreset", UserId, StandingHeight, "{}", "fa-medkit"));
                    await Task.Run(() => presetsModel?.AddPreset("SittingHealthPreset", UserId, SittingHeight, "{}", "fa-medkit"));
                    Snackbar.Add("Presets updated successfully", Severity.Success);
                }
                else
                {
                    await Task.Run(() => presetsModel?.AddPreset("SittingHealthPreset", UserId, SittingHeight, "{}", "fa-medkit"));
                    await Task.Run(() => presetsModel?.AddPreset("StandingHealthPreset", UserId, StandingHeight, "{}", "fa-medkit"));
                    Snackbar.Add("Presets created successfully", Severity.Success);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                ErrorMessage = "An error occurred while creating the presets.";
                Snackbar.Add("An error occurred while creating the presets", Severity.Error);
                return;
            }
        }
    }
}