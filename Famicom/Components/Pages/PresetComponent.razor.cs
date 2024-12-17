using Famicom.Models;
using SharedModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Diagnostics;
using TableController;
using Blazored.SessionStorage;
using Models.Services;


namespace Famicom.Components.Pages
{
    public partial class PresetComponent : ComponentBase
    {
        public enum OverlayMode{ None, Add, Edit }
        private Dictionary<string, string> iconOptions = new Dictionary<string, string>
        {
            { "fa-user", "user" },
            { "fa-star", "star" },
            { "fa-heart", "heart" },
            { "fa-arrow-up", "arrow up" },
            { "fa-arrow-down", "arrow down" },
            { "fa-medkit", "medkit"}
        };
        private OverlayMode currentOverlayMode = OverlayMode.None;
        private PresetsModel? presetsModel { get; set; }
        private TableModel? tableModel { get; set; }
        public List<Presets>? userPresets { get; set; }
        private int PresetId { get; set; }
        private string? PresetName { get; set; }
        private int PresetHeight { get; set; }
        private string? PresetIcon { get; set; }
        private string? ErrorMessage { get; set; }
        private int UserId { get; set; }

        [Inject] 
        ISessionStorageService SessionStorage { get; set; } = default!;

        [Inject]
        HttpClient HttpClient { get; set; } = default!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = default!;

        [Inject]
        TableControllerService TableControllerService { get; set; } = default!;
        
        [Inject]
        public PresetService PresetService { get; set; } = default!;

        [Inject]
        public TableService TableService {get; set; } = default!;


        protected override async Task OnInitializedAsync()
        {
            try
            {
                tableModel = new TableModel(HttpClient, TableControllerService, TableService);
                presetsModel = new PresetsModel(HttpClient, TableControllerService, PresetService);
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
            userPresets = await Task.Run(() => presetsModel!.GetAllPresets(UserId));
            StateHasChanged();
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
        private async Task SelectPreset(string? presetName, int presetHeight)
        {
            try
            {
                Snackbar.Add($"Applying preset {presetName}...", Severity.Info);
                var table = tableModel!.GetTable(UserId);
                if (table == null)
                {
                    Snackbar.Add("Table not found for the user", Severity.Error);
                    return;
                }
                var task = await presetsModel!.SetPresetHeight(presetHeight, table.GUID);
                var severity = task.Status == TableStatus.Success ? Severity.Success : Severity.Error;
                Snackbar.Add($"Status: {task.Status}, Message: {task.Message}", severity);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Snackbar.Add("An error occurred while selecting the preset", Severity.Error);
                return;
            }
        }
        private async Task AddPreset()
        {
            if (string.IsNullOrEmpty(PresetName))
            {
                ErrorMessage = "Preset name is required.";
                return;
            }
            if(PresetName.Length > 20)
            {
                ErrorMessage = "The preset name must be no more than 20 characters long.";
                return;
            }
            if (PresetHeight < 680)
            {
                ErrorMessage = "Preset height must be more than or equal to 680mm.";
                return;
            }
            if (PresetHeight > 1320)
            {
                ErrorMessage = "Preset height must be less than or equal to 1320mm.";
                return;
            }
            if (string.IsNullOrEmpty(PresetIcon))
            {
                ErrorMessage = "Preset icon is required.";
                return;
            }

            var sittingPresetExists = userPresets!.Any(preset => preset.PresetName == "SittingHealthPreset");
            var standingPresetExists = userPresets!.Any(preset => preset.PresetName == "StandingHealthPreset");

            if ((PresetName == "SittingHealthPreset" && sittingPresetExists) || (PresetName == "StandingHealthPreset" && standingPresetExists))
            {
                ErrorMessage = $"Only one preset with the name {PresetName} is allowed.";
                return;
            }

            try
            {
                await Task.Run(() => presetsModel?.AddPreset(PresetName, UserId, PresetHeight, "{}", PresetIcon));
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
            if(PresetName.Length > 20)
            {
                ErrorMessage = "The preset name must be no more than 20 characters long.";
                return;
            }
            if (PresetHeight < 680)
            {
                ErrorMessage = "Preset height must be more than or equal to 680 mm.";
                return;
            }
            if (PresetHeight > 1320)
            {
                ErrorMessage = "Preset height must be less than or equal to 1320 mm.";
                return;
            }
            if (string.IsNullOrEmpty(PresetIcon))
            {
                ErrorMessage = "Preset icon is required.";
                return;
            }

            var sittingPresetExists = userPresets!.Any(preset => preset.PresetName == "SittingHealthPreset");
            var standingPresetExists = userPresets!.Any(preset => preset.PresetName == "StandingHealthPreset");

            if ((PresetName == "SittingHealthPreset" && sittingPresetExists) || (PresetName == "StandingHealthPreset" && standingPresetExists))
            {
                ErrorMessage = $"Only one preset with the name {PresetName} is allowed.";
                return;
            }

            try
            {
                await Task.Run(() => presetsModel?.EditPreset(PresetId, PresetName, UserId, PresetHeight, "{}", PresetIcon));
                ErrorMessage = null;
                Snackbar.Add("Preset edited successfully", Severity.Success);
                userPresets = await Task.Run(() => presetsModel?.GetAllPresets(UserId));
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