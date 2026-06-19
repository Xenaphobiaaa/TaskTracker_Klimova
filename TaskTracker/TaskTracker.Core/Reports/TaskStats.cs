using System;
namespace TaskTracker.Core.Reports;

public class TaskStats
{
    public int Total { get; set; }
    public int NewCount { get; set; }
    public int InProgressCount { get; set; }
    public int DoneCount { get; set; }
}
