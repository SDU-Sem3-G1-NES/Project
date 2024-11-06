using Famicom.Models;
using SharedModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Components;

namespace Famicom.Components.Pages
{
    public partial class TableComponent : ComponentBase
    {
        
        public string TableImageUrl { get; set; } = "";

        private TableModel ?tableModel { get; set; }

        [Parameter]
        public ITable ?Table { get; set; }


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
