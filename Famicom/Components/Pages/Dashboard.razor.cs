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

        public List<ITable> Table => DashboardModel.Table;
        public List<int> WeeklyUsageData => DashboardModel.WeeklyUsageData;
        public List<int> TodayUsageData => DashboardModel.TodayUsageData;
        public List<DashboardModel.Notification> Notifications => DashboardModel.Notifications;


        //Series for Weekly Usage Graph used to bind chart data
        //It's a quick fix
        //But data should be binded in this way
        public List<ChartSeries> WeeklyUsageSeries = new()
        {
            new ChartSeries() { Name = "Weekly Data", Data = new double[]{40,20,50,50 } }
        };

        //Series for Weekly Usage Graph used to bind chart data
        //It's a quick fix
        //But data should be binded in this way
        public List<ChartSeries> TodayUsageSeries = new()
        {
            new ChartSeries() { Name = " Todays Data", Data = new double[]{420,250,520,504 } }
        };

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
        }

        public void MoveTableDown()
        {
            Debug.WriteLine("Table moved Down");
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            StoreDashboardData();
        }
    }
}
