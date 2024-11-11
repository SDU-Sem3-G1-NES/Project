using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using SharedModels;
using Famicom.Models;
using static MudBlazor.Colors;
using Models.Services;
using System.Diagnostics;

namespace Famicom.Components.Pages
{
    public partial class TableBase : ComponentBase
    {
        public string? PanelTitle { get; set; }
        private TableModel? tableModel { get; set; }
        private UserModel userModel { get; set; } = new UserModel();
        public required List<ITable> Table { get; set; }

        #region Properties for Search, Filter and Sorting
        public string? orderValue { get; set; }
        public string? selectedRoom { get; set; }
        public string? selectedTable { get; set; }
        public bool coerceValue { get; set; }
        public bool resetValueOnEmptyText { get; set; }
        public bool coerceText { get; set; }

        //Mock data for rooms and tables
        public List<string> roomNames = new List<string>() {"None","Room 1", "Room 2", "Room 3", "Room 4", "Room 5" };

        public List<string> tableNames = new List<string>() { "None", "Table 1", "Table 2", "Table 3", "Table 4", "Table 5", "Julka", "Hulk", "SpiderMan", "America", "Razor" };

        #endregion

        protected override void OnInitialized()
        {

            tableModel = new TableModel();
            //Table = tableModel.GetTable(1);
            //For testing only
            Table = tableModel.GetTableList();
            PanelTitle = GetUserType();
        }

        private string GetUserType()
        {
            string userType = userModel.GetUserType();
            if (userType == "Admin")
            {
                return "Admin Panel";
            }
            return "User Panel";
        }

        public async Task<IEnumerable<string>> Search1(string value, CancellationToken token)
        {
           
            await Task.Delay(5, token);

            if (string.IsNullOrEmpty(value))
                return tableNames;
            return tableNames.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }


    }
}