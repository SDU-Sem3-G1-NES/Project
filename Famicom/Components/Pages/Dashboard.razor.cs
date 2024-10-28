using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;

namespace Famicom.Components.Pages
{
    public class DashboardBase : ComponentBase
    {
        public string NotificationsTitle { get; set; } = "Notifications";
        public string TodayUsageGraphTitle { get; set; } = "Today's Usage Graph";
        public string WeeklyUsageGraphTitle { get; set; } = "Weekly Usage Graph";
        public string TodayUsageGraphLabel { get; set; } = "Today's Usage";
        public string WeeklyUsageGraphLabel { get; set; } = "Weekly Usage";

        public TableInfo Table { get; set; } = new TableInfo
        {
            TableName = "Table #1",
            Room = "Room 123",
            LastPositionChange = "20m ago"
        };

        public List<int> WeeklyUsageData { get; set; } = new List<int> { 5, 10, 17, 10, 20 };
        public List<int> TodayUsageData { get; set; } = new List<int> { 30, 15, 25 };

        public List<Notification> Notifications { get; set; } = new List<Notification>
        {
            new Notification { Title = "Health Notification", Message = "You've been sitting too long!", Action = "Raise desk" },
            new Notification { Title = "Health Panel", Message = "Check new suggestions", Action = "Go to Health Panel" },
            new Notification { Title = "Collision Detected", Message = "Could not raise the table", Action = "Go to Table tab" }
        };

        public void HandleNotification(Notification notification)
        {
            if (notification.Action == "Raise desk")
            {
                MoveTableUp();
            }
            else if (notification.Action == "Go to Health Panel")
            {
                // Logic
            }
        }

        public void MoveTableUp()
        {
            //Logic to move the table up
            Table.LastPositionChange = "Just now";
        }

        public void MoveTableDown()
        {
            // Logic to move the table down
            Table.LastPositionChange = "Just now";
        }

        public class TableInfo
        {
            public string TableName { get; set; } = string.Empty;
            public string Room { get; set; } = string.Empty;
            public string LastPositionChange { get; set; } = string.Empty;
        }

        public class Notification
        {
            public string Title { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
            public string Action { get; set; } = string.Empty;
        }
    }
}
