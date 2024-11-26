using Famicom.Models;
using SharedModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Blazored.SessionStorage;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Famicom.Components.Pages
{
    public partial class PresetComponent : ComponentBase
    {
        public enum OverlayMode{ None, Add, Edit }
        private List<string> iconOptions = new List<string>
        {
            "fa-user",
            "fa-star",
            "fa-heart"
        };
        private OverlayMode currentOverlayMode = OverlayMode.None;
        private PresetsModel? presetsModel { get; set; }
        public List<Presets>? userPresets { get; set; }

        [CascadingParameter]
        public string? Email { get; set; }

        [CascadingParameter]
        public int? UserId { get; set; }
        private int PresetId { get; set; }
        private string? PresetName { get; set; }
        private int PresetHeight { get; set; }
        private string? PresetIcon { get; set; }
        private string? ErrorMessage { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; } = default!;
        [Inject]
        private ISessionStorageService? SessionStorage { get; set; }

        private bool _isInitialized;

        protected override async void OnInitialized()
        {
            try
            {
                presetsModel = new PresetsModel();
                userPresets = await Task.Run(() => presetsModel.GetAllPresets((int)UserId!));
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
        public void OpenOverlay(OverlayMode mode, int presetId = 0)
        {
            currentOverlayMode = mode;
            if (mode == OverlayMode.Edit)
            {
                var preset = userPresets?.FirstOrDefault(p => p.PresetId == presetId);
                if (preset != null)
                {
                    PresetId = preset.PresetId;
                    PresetName = preset.PresetName;
                    PresetHeight = preset.Height;
                    PresetIcon = preset.Icon;
                }
            }
        }
        private void CancelOverlay()
        {
            currentOverlayMode = OverlayMode.None;
            PresetName = string.Empty;
            PresetHeight = 0;
            PresetIcon = string.Empty;
            ErrorMessage = null;
        }
        private async Task SelectPreset(int presetId)
        {
            // This is where the selected preset would be applied to the table
            // Does it go to the tablecontroller or straight to the database?
            await Task.CompletedTask;
        }
        private async Task AddPreset()
        {
            if (string.IsNullOrEmpty(PresetName))
            {
                ErrorMessage = "Preset name is required.";
                return;
            }

            if (PresetHeight <= 0)
            {
                ErrorMessage = "Preset height must be greater than zero.";
                return;
            }

            if (string.IsNullOrEmpty(PresetIcon))
            {
                ErrorMessage = "Preset icon is required.";
                return;
            }

            try
            {
                await Task.Run(() => presetsModel?.AddPreset(PresetName, (int)UserId!, PresetHeight, "{}", PresetIcon));
                ErrorMessage = null;
                Snackbar.Add("Preset added successfully", Severity.Success);
                userPresets = await Task.Run(() => presetsModel?.GetAllPresets((int)UserId!));
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
            if (string.IsNullOrEmpty(PresetName))
            {
                ErrorMessage = "Preset name is required.";
                return;
            }

            if (PresetHeight <= 0)
            {
                ErrorMessage = "Preset height must be greater than zero.";
                return;
            }

            if (string.IsNullOrEmpty(PresetIcon))
            {
                ErrorMessage = "Preset icon is required.";
                return;
            }

            try
            {
                await Task.Run(() => presetsModel?.EditPreset(PresetId, PresetName, (int)UserId!, PresetHeight, "{}", PresetIcon));
                ErrorMessage = null;
                Snackbar.Add("Preset edited successfully", Severity.Success);
                userPresets = await Task.Run(() => presetsModel?.GetAllPresets((int)UserId!));
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