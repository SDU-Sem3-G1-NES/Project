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
        // Title and Image URL
        public string TableTitle { get; set; } = "Manage Your Table";
        public string TableImageUrl { get; set; } = ""; //image URL

        private TableModel tableModel {get; set; } = new TableModel();
        public ITable ?Table { get; set; }
        protected override void OnInitialized()
        {
            tableModel = new TableModel();
            Table = tableModel.GetTable();
        }

        // Methods for moving the table up and down
        public void MoveTableUp()
        {
            Debug.WriteLine("Table moved up");
        }

        public void MoveTableDown()
        {
            Debug.WriteLine("Table moved down");
        }
    }
}
