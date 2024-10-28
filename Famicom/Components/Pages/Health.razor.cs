﻿using Microsoft.AspNetCore.Components;
using System;
using System.Timers; // Keep this if you want to use System.Timers.Timer

public class HealthBase : ComponentBase
{
    public string SittingDuration { get; set; } = "00:00:00";
    private System.Timers.Timer SittingTimer { get; set; } // Specify System.Timers.Timer explicitly
    private DateTime StartTime;
    public bool TimerRunning { get; set; } = false;

    public string PostureCheckResult { get; set; } = "Not checked yet";
    public int BreakInterval { get; set; } = 30;
    public string ReminderMessage { get; set; } = "No reminder set";

    public void StartSittingTimer()
    {
        StartTime = DateTime.Now;
        TimerRunning = true;
        SittingTimer = new System.Timers.Timer(1000); // Use fully qualified name
        SittingTimer.Elapsed += UpdateSittingDuration;
        SittingTimer.Start();
    }

    public void StopSittingTimer()
    {
        TimerRunning = false;
        SittingTimer?.Stop();
        SittingDuration = "00:00:00";
    }

    private void UpdateSittingDuration(object sender, ElapsedEventArgs e)
    {
        SittingDuration = (DateTime.Now - StartTime).ToString(@"hh\:mm\:ss");
        InvokeAsync(StateHasChanged);
    }

    public void PerformPostureCheck()
    {
        var random = new Random();
        PostureCheckResult = random.Next(0, 2) == 0 ? "Good posture!" : "Adjust your posture!";
    }

    public void SetBreakReminder()
    {
        ReminderMessage = $"Break reminder set for every {BreakInterval} minutes.";
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }
}
