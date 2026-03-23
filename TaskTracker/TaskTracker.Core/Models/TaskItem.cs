using System;
namespace TaskTracker.Core.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public TaskStatus Status { get; set; } = TaskStatus.New;
}
