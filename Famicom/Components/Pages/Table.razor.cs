using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using SharedModels;
using Famicom.Models;
using Models.Services;
using System.Diagnostics;

namespace Famicom.Components.Pages
{
    public partial class TableBase : ComponentBase
    {
        public string? PanelTitle { get; set; }
        private TableModel? tableModel { get; set; }
        private UserModel userModel { get; set; } = new UserModel();
        public ITable? Table { get; set; }

        protected override void OnInitialized()
        {

            tableModel = new TableModel();
            Table = tableModel.GetTable(1);
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