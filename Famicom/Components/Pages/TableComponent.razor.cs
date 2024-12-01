using Famicom.Models;
using MudBlazor;
using SharedModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using TableController;

namespace Famicom.Components.Pages
{
    public partial class TableComponent : ComponentBase
    {
        private TableModel? tableModel { get; set; }
        private int tableHeight { get; set; } = 1000; // mock
        private int tempHeight { get; set; } = 1000; // mock
        private string? ErrorMessage { get; set; }
        
        [Parameter]
        public required ITable Table { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; } = default!;

        [Inject]
        IHttpClientFactory ClientFactory { get; set; } = default!;

        protected override void OnInitialized()
        {
            try
            {
                tableModel = new TableModel(ClientFactory);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private void AdjustTableHeight(int height)
        {
            tempHeight += height;
            StateHasChanged();
        }

        private async Task SetTableHeight()
        {
            try
            {
                Snackbar.Add($"Setting height to {(decimal)tempHeight/10} cm...", Severity.Info);
                var task = await tableModel!.SetTableHeight(tempHeight, Table.GUID);
                var severity = task.Status == TableStatus.Success ? Severity.Success : Severity.Error;
                Snackbar.Add($"Status: {task.Status}, Message: {task.Message}", severity);
                tableHeight = await tableModel.GetTableHeight(Table.GUID);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Snackbar.Add("An error occurred while setting the height", Severity.Error);
                return;
            }

        }
    }
}
