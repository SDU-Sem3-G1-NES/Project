using Famicom.Models;
using SharedModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Diagnostics;

namespace Famicom.Components.Pages
{
    public partial class PresetComponent : ComponentBase
    {
        public enum OverlayMode{ None, Add, Edit }
        private OverlayMode currentOverlayMode = OverlayMode.None;
        private PresetsModel? presetsModel { get; set; }
        private TableModel? tableModel { get; set; }
        public List<Presets>? userPresets { get; set; }
        private int PresetId { get; set; }
        private string? PresetName { get; set; }
        private int PresetHeight { get; set; }
        private string? ErrorMessage { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            presetsModel = new PresetsModel();
            userPresets = await Task.Run(() => presetsModel.GetAllPresets(1));
        }
        public void OpenOverlay(OverlayMode mode, int presetId = 0)
        {
            currentOverlayMode = mode;
            if (mode == OverlayMode.Edit)
            {
                var preset = userPresets?.FirstOrDefault(p => p.PresetId == presetId);
                if (preset != null)
                {
                    PresetName = preset.PresetName;
                    PresetHeight = preset.Height;
                }
            }
        }
        private void CancelOverlay()
        {
            currentOverlayMode = OverlayMode.None;
            PresetName = string.Empty;
            PresetHeight = 0;
            ErrorMessage = null;
        }
        private async Task SelectPreset(int presetId)
        {
            // This is where the selected preset would be applied to the table
            // Does it go to the tablecontroller or straight to the database?
            Console.WriteLine($"Selected preset {presetId}");
            await Task.CompletedTask;
        }
        private async Task AddPreset()
        {
            if (string.IsNullOrEmpty(PresetName) || PresetHeight <= 0)
            {
                ErrorMessage = "Please fill in all fields.";
                return;
            }

            try
            {
                await Task.Run(() => presetsModel?.AddPreset(PresetName, 1, PresetHeight, "{}"));
                ErrorMessage = null;
                Snackbar.Add("Preset added successfully", Severity.Success);
                userPresets = await Task.Run(() => presetsModel?.GetAllPresets(1));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                ErrorMessage = "An error occurred while adding the preset.";
                Snackbar.Add("An error occurred while adding the preset", Severity.Error);
                return;
            }
            CancelOverlay();
        }
        private async Task EditPreset()
        {
            if (string.IsNullOrEmpty(PresetName) || PresetHeight <= 0)
            {
                ErrorMessage = "Please fill in all fields.";
                return;
            }

            try
            {
                await Task.Run(() => presetsModel?.EditPresetName(PresetId, PresetName));
                await Task.Run(() => presetsModel?.EditPresetHeight(PresetId, PresetHeight));
                ErrorMessage = null;
                Snackbar.Add("Preset edited successfully", Severity.Success);
                userPresets = await Task.Run(() => presetsModel?.GetAllPresets(1));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                ErrorMessage = "An error occurred while editing the preset.";
                Snackbar.Add("An error occurred while editing the preset", Severity.Error);
                return;
            }
            CancelOverlay();
        }
        private async Task DeletePreset(int PresetId)
        {
            try
            {
                await Task.Run(() => presetsModel?.RemovePreset(PresetId));
                Snackbar.Add("Preset removed successfully", Severity.Success);
                userPresets = await Task.Run(() => presetsModel?.GetAllPresets(1));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Snackbar.Add("An error occurred while removing the preset", Severity.Error);
                return;
            }
        }
    }
}