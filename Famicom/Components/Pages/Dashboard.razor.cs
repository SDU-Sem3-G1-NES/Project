using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using SharedModels;
using Famicom.Models;
using System.Diagnostics;

namespace Famicom.Components.Pages
{
    public partial class DashboardBase : ComponentBase
    {
        private DashboardModel DashboardModel { get; set; } = new DashboardModel();

        public string NotificationsTitle => DashboardModel.NotificationsTitle;
        public string TodayUsageGraphTitle => DashboardModel.TodayUsageGraphTitle;
        public string WeeklyUsageGraphTitle => DashboardModel.WeeklyUsageGraphTitle;
        public string TodayUsageGraphLabel => DashboardModel.TodayUsageGraphLabel;
        public string WeeklyUsageGraphLabel => DashboardModel.WeeklyUsageGraphLabel;

        public ITable Table => DashboardModel.Table;
        public List<int> WeeklyUsageData => DashboardModel.WeeklyUsageData;
        public List<int> TodayUsageData => DashboardModel.TodayUsageData;
        public List<DashboardModel.Notification> Notifications => DashboardModel.Notifications;

        public void StoreDashboardData()
        {
            DashboardModel.StoreDashboardData();
        }

        public void HandleNotification(DashboardModel.Notification notification)
        {
            if (notification.Action == "Raise desk")
            {
                MoveTableUp();
            }
            else if (notification.Action == "Go to Health Panel")
            {
                //logic to navigate to health view
            }
        }

        public void MoveTableUp()
        {
            Debug.WriteLine("Table moved up");
            DashboardModel.Table.LastPositionChange = "Just now";
        }

        public void MoveTableDown()
        {
            Debug.WriteLine("Table moved Down");
            DashboardModel.Table.LastPositionChange = "Just now";
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            StoreDashboardData();
        }
    }
}
