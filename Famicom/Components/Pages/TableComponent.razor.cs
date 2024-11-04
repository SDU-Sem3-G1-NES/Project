﻿using Famicom.Models;
using SharedModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Components;

namespace Famicom.Components.Pages
{
    public partial class TableComponent : ComponentBase
    {
        // Title and Image URL
        public string TableTitle { get; set; } = "Manage Your Table";
        public string TableImageUrl { get; set; } = ""; //image URL

        private TableModel ?tableModel { get; set; }

        [Parameter]
        public ITable ?Table { get; set; }


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