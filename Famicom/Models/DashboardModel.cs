using System.Collections.Generic;
using SharedModels;
using Models.Services;

namespace Famicom.Models
{
    public class DashboardModel
    {
        private readonly TableModel tableModel;

        public DashboardModel()
        {
            tableModel = new TableModel();
        }

        public string NotificationsTitle { get; set; } = "Notifications";
        public string TodayUsageGraphTitle { get; set; } = "Today's Usage Graph";
        public string WeeklyUsageGraphTitle { get; set; } = "Weekly Usage Graph";
        public string TodayUsageGraphLabel { get; set; } = "Today's Usage";
        public string WeeklyUsageGraphLabel { get; set; } = "Weekly Usage";

        public List<ITable> Table => tableModel.GetTable();

        public List<int> WeeklyUsageData { get; set; } = new List<int> { 5, 10, 17, 10, 20 };
        public List<int> TodayUsageData { get; set; } = new List<int> { 30, 15, 25 };

        public List<Notification> Notifications { get; set; } = new List<Notification>
        {
            new Notification { Title = "Health Notification", Message = "You've been sitting too long!", Action = "Raise desk" },
            new Notification { Title = "Health Panel", Message = "Check new suggestions", Action = "Go to Health Panel" },
            new Notification { Title = "Collision Detected", Message = "Could not raise the table", Action = "Go to Table tab" }
        };

        public void StoreDashboardData()
        {
            // Dashboard data storage logic
        }

        public class Notification
        {
            public string Title { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
            public string Action { get; set; } = string.Empty;
        }
    }
}