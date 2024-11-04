using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using SharedModels;
using Famicom.Models;
using System.Diagnostics;

namespace Famicom.Components.Pages
{
    public partial class TableBase : ComponentBase
    {
        private TableModel tableModel {get; set; } = new TableModel();
        public ITable Table { get; set; }

        protected override void OnInitialized()
        {
            tableModel = new TableModel();
            Table = tableModel.GetTable();
        }
    }
}
