using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;

public class DashboardBase : ComponentBase
{
    // Properties to define titles and labels
    public string NotificationsTitle { get; set; } = "Notifications";
    public string TodayUsageGraphTitle { get; set; } = "Today's Usage Graph";
    public string WeeklyUsageGraphTitle { get; set; } = "Weekly Usage Graph";
    public string TodayUsageGraphLabel { get; set; } = "Today's Usage";
    public string WeeklyUsageGraphLabel { get; set; } = "Weekly Usage";

    // Desk/Table Information
    public TableInfo Table { get; set; } = new TableInfo
    {
        TableName = "Table #1",
        Room = "Room 123",
        LastPositionChange = "20m ago"
    };

    // Data Sources for the Usage Graphs
    public List<int> WeeklyUsageData { get; set; } = new List<int> { 5, 10, 17, 10, 20 };
    public List<int> TodayUsageData { get; set; } = new List<int> { 30, 15, 25 };

    // Notifications
    public List<Notification> Notifications { get; set; } = new List<Notification>
    {
        new Notification { Title = "Health Notification", Message = "You've been sitting too long!", Action = "Raise desk" },
        new Notification { Title = "Health Panel", Message = "Check new suggestions", Action = "Go to Health Panel" },
        new Notification { Title = "Collision Detected", Message = "Could not raise the table", Action = "Go to Table tab" }
    };

    // Logic for Notifications
    public void HandleNotification(Notification notification)
    {
        if (notification.Action == "Raise desk")
        {
            MoveTableUp();
        }
        else if (notification.Action == "Go to Health Panel")
        {
            // Logic to navigate or handle the Health Panel action
        }
    }

    // Logic for Table Movement
    public void MoveTableUp()
    {
        // Implement logic to move the table up
        Table.LastPositionChange = "Just now";
    }

    public void MoveTableDown()
    {
        // Implement logic to move the table down
        Table.LastPositionChange = "Just now";
    }

    // Nested Models
    public class TableInfo
    {
        public string TableName { get; set; }
        public string Room { get; set; }
        public string LastPositionChange { get; set; }
    }

    public class Notification
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Action { get; set; }
    }
}
