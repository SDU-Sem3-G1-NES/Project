using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedModels;
using Famicom.Models;
using System.Diagnostics;
using TableController;
using Models.Services;
using Blazored.SessionStorage;

namespace Famicom.Components.Pages
{
    public partial class HealthNotificationComponent : ComponentBase
    {
        private TableModel? tableModel { get; set; }
        private PresetsModel? presetsModel { get; set; }
        private int userId { get; set; }
        private int tableHeight { get; set; }
        private string? errorMessage { get; set; }
        public List<Presets>? userPresets { get; set; }
        private int userSittingPresetHeight { get; set; }
        private int userStandingPresetHeight { get; set; }
        private Stopwatch positionChangeStopwatch = new Stopwatch();
        private bool isUserStanding;
        private Timer _timer = null!;
        
        [Parameter]
        public required ITable Table { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; } = default!;

        [Inject]
        IHttpClientFactory ClientFactory { get; set; } = default!;

        [Inject]
        TableControllerService TableControllerService { get; set; } = default!;

        [Inject]
        ISessionStorageService SessionStorage { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                tableModel = new TableModel(ClientFactory, TableControllerService);
                presetsModel = new PresetsModel(ClientFactory, TableControllerService);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            positionChangeStopwatch.Start();
            _timer = new Timer(callback: _ => InvokeAsync(CheckForUserHealthPresets), null, 500, 5000);

            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            userId = await SessionStorage.GetItemAsync<int>("UserId");
            StateHasChanged();
        }

        private async Task CheckForUserHealthPresets()
        {
            await InvokeAsync(async () =>
            {
                try
                {
                    var userPresets = await Task.Run(() => presetsModel!.GetAllPresets(userId));
                    var sittingPresetExists = userPresets.Any(preset => preset.PresetName == "SittingHealthPreset");
                    var standingPresetExists = userPresets.Any(preset => preset.PresetName == "StandingHealthPreset");

                    if (sittingPresetExists && standingPresetExists)
                    {
                        userSittingPresetHeight = userPresets.First(preset => preset.PresetName == "SittingHealthPreset").Height;
                        userStandingPresetHeight = userPresets.First(preset => preset.PresetName == "StandingHealthPreset").Height;
                        await CheckForChangedHeight();
                    }
                    else
                    {
                        Snackbar.Add("Sitting and standing health presets not found. Please make them in the health page.", Severity.Info);
                    }

                }
                catch (Exception e)
                {
                    tableModel = new TableModel(ClientFactory, TableControllerService);
                    Debug.WriteLine(e.Message);
                    return;
                }
            });
        }

        private async Task CheckForChangedHeight()
        {
            try
            {
                var newHeight = await tableModel!.GetTableHeight(Table.GUID);
                if(tableHeight  < 1000 && newHeight > 1000)
                {
                    isUserStanding = true;
                    positionChangeStopwatch.Restart();
                    tableHeight = newHeight;
                }
                if(tableHeight  > 1000 && newHeight < 1000)
                {
                    isUserStanding = false;
                    positionChangeStopwatch.Restart();
                    tableHeight = newHeight;
                }
                // Logic for transitioning from sitting to standing
                if (tableHeight < 1000 && newHeight < 1000)
                {
                    if(positionChangeStopwatch.Elapsed >= TimeSpan.FromMinutes(0.3))
                    {
                        string message = "You have been sitting for too long! You should stand up for a while! Press to change the table to a standing position.";
                        Snackbar.Add(message, Severity.Info, config =>
                        {
                            config.RequireInteraction = true;
                            config.ShowCloseIcon = true;
                            config.Onclick = async snackbar =>
                            {
                                isUserStanding = true;
                                positionChangeStopwatch.Restart();
                                await SetTableHeight();
                            };
                        });
                    }
                }
                // Logic for transitioning from standing to sitting
                if (tableHeight > 1000 && newHeight > 1000)
                {
                    if(positionChangeStopwatch.Elapsed >= TimeSpan.FromMinutes(0.2))
                    {
                        string message = "You have been standing for too long! You should sit down for a while! Press to change table to a seated position.";
                        Snackbar.Add(message, Severity.Info, config =>
                        {
                            config.RequireInteraction = true;
                            config.Onclick = async snackbar =>
                            {
                                isUserStanding = false;
                                positionChangeStopwatch.Restart();
                                await SetTableHeight();
                            };
                        });
                    }
                }
            }
            catch (Exception e)
            {
                tableModel = new TableModel(ClientFactory, TableControllerService);
                Debug.WriteLine(e.Message);
                return;
            }
        }

        private async Task SetTableHeight()
        {
            try
            {
                var progress = new Progress<ITableStatusReport>(message =>
                {
                    Debug.WriteLine(message);
                    switch (message.Status)
                    {
                        case TableStatus.Success:
                            Snackbar.Add("Height set successfully", Severity.Success);
                            break;
                        case TableStatus.Collision:
                            Snackbar.Add("Collision detected", Severity.Error);
                            break;
                        case TableStatus.Overheat:
                            Snackbar.Add("Overheat detected", Severity.Error);
                            break;
                        case TableStatus.Lost:
                            Snackbar.Add("Table lost", Severity.Error);
                            break;
                        case TableStatus.Overload:
                            Snackbar.Add("Overload detected", Severity.Error);
                            break;
                        case TableStatus.OtherError:
                            Snackbar.Add("An error occurred", Severity.Error);
                            break;
                    }
                });
                int newHeight = isUserStanding ? userStandingPresetHeight : userSittingPresetHeight;
                Snackbar.Add($"Setting height to {(decimal)newHeight / 10} cm...", Severity.Info);
                await tableModel!.SetTableHeight(newHeight, Table.GUID, progress);
                tableHeight = await tableModel.GetTableHeight(Table.GUID);
            }
            catch (Exception e)
            {
                tableModel = new TableModel(ClientFactory, TableControllerService);
                Debug.WriteLine(e.Message);
                Snackbar.Add("An error occurred while setting the height", Severity.Error);
            }
        }
    }
}