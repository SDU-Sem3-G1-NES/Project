using Famicom.Models;
using MudBlazor;
using SharedModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using TableController;
using Models.Services;

namespace Famicom.Components.Pages
{
    public partial class TableComponent : ComponentBase
    {
        private TableModel? tableModel { get; set; }
        private int tableHeight { get; set; } = 1000; // mock
        private int tempHeight { get; set; } = 1000; // mock
        private string? ErrorMessage { get; set; }

        private Timer _timer = null!;
        
        [Parameter]
        public required ITable Table { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; } = default!;

        [Inject]
        IHttpClientFactory ClientFactory { get; set; } = default!;
        [Inject]
        TableControllerService TableControllerService { get; set; } = default!;


        private bool firstAccess = true;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                tableModel = new TableModel(ClientFactory, TableControllerService);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            _timer = new Timer(callback: _ => InvokeAsync(CheckForChangedHeight), null, 500, 500);
            await base.OnInitializedAsync();
        }

        private void AdjustTableHeight(int height)
        {
            tempHeight += height;
            StateHasChanged();
        }

        private async Task CheckForChangedHeight() {
            await InvokeAsync(async () =>
            {
                try
                {
                    var newHeight = await tableModel!.GetTableHeight(Table.GUID);
                    if (newHeight != tableHeight)
                    {
                        tableHeight = newHeight;
                        if(firstAccess)
                        {
                            tempHeight = tableHeight;
                            firstAccess = false;
                        }
                        StateHasChanged();
                    }
                }
                catch (Exception e)
                {
                    tableModel = new TableModel(ClientFactory, TableControllerService);
                    Debug.WriteLine(e.Message);
                    //Snackbar.Add(e.Message, Severity.Error);
                    return;
                }
            });
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
                Snackbar.Add($"Setting height to {(decimal)tempHeight/10} cm...", Severity.Info);
                
                await tableModel!.SetTableHeight(tempHeight, Table.GUID, progress);
                tableHeight = await tableModel.GetTableHeight(Table.GUID);
            }
            catch (Exception e)
            {
                tableModel = new TableModel(ClientFactory, TableControllerService);
                Console.WriteLine(e.Message);
                //Snackbar.Add("An error occurred while setting the height", Severity.Error);
            }
        }
    }
}
