namespace TaskTracker.Core.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";   // ← добавили
    public TaskStatus Status { get; set; } = TaskStatus.New;
}
