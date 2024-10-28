using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;

namespace Famicom.Components.Pages
{
    public partial class TableBase : ComponentBase
    {
        // Title and Image URL
        public string TableTitle { get; set; } = "Manage Your Table";
        public string TableImageUrl { get; set; } = ""; //image URL

        // Table Model instance
        public TableModel Table { get; set; } = new TableModel();

        protected override void OnInitialized()
        {
            // Initialize Table data. Replace with actual data loading logic if needed.
            Table = new TableModel
            {
                TableName = "Table #1",
                Room = "Room 123",
                LastPositionChange = "20m ago"
            };
        }

        // Methods for moving the table up and down
        public void MoveTableUp()
        {
            Table.LastPositionChange = "Just now";
            //Table move up logic
            StateHasChanged(); // Trigger UI update
        }

        public void MoveTableDown()
        {
            Table.LastPositionChange = "Just now";
           //Table move down logic
            StateHasChanged(); // Trigger UI update
        }
    }

    // TableModel class to encapsulate table information
    public class TableModel
    {
        public string TableName { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;
        public string LastPositionChange { get; set; } = string.Empty;
    }
}
