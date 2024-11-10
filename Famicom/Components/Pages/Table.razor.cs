using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using SharedModels;
using Famicom.Models;

namespace Famicom.Components.Pages
{
    public partial class TableBase : ComponentBase
    {
        public string? PanelTitle { get; set; }
        private TableModel? tableModel {get; set; }
        private UserModel userModel { get; set; } = new UserModel();
        public required List<ITable> Table { get; set; }
        public string? orderValue { get; set; }
        public string? selectedRoom { get; set; }

        public List<string> roomNames = new List<string>() {"None","Room 1", "Room 2", "Room 3", "Room 4", "Room 5" };

        protected override void OnInitialized()
        {
            tableModel = new TableModel();
            Table = tableModel.GetTable();
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


    }
}
