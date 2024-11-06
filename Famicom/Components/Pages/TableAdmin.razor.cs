using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedModels;
using Famicom.Models;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Famicom.Components.Pages
{
    public partial class TableAdminBase : ComponentBase
    {
        private AdminModel adminModel { get; set; } = new AdminModel();
        public List<ITable> AllTables { get; set; } = new List<ITable>();

        public HashSet<ITable> SelectedTables { get; set; } = new HashSet<ITable>();

        public List<ITable> FilteredTables =>
            string.IsNullOrEmpty(SearchTerm) ? AllTables : AllTables.Where(t => t.Name.Contains(SearchTerm, System.StringComparison.OrdinalIgnoreCase)).ToList();

        public string SearchTerm { get; set; } = "";

        protected override void OnInitialized()
        {
            AllTables = adminModel.GetAllTables();
        }

        public void SelectAllTables()
        {
            SelectedTables = new HashSet<ITable>(FilteredTables);
        }
        public void DeselectAllTables()
        {
            SelectedTables.Clear();
        }

        public void MoveSelectedTablesUp()
        {
            foreach (var table in SelectedTables)
            {
                Debug.WriteLine($"Moving table {table.Name} up");
                // logic to move the table up
            }
        }

        public void MoveSelectedTablesDown()
        {
            foreach (var table in SelectedTables)
            {
                Debug.WriteLine($"Moving table {table.Name} down");
                //logic to move the table down
            }
        }
    }
}
