using TaskTracker.Core.Models;

namespace TaskTracker.Core.Services;

public class TaskService
{
    private readonly List<TaskItem> _tasks = new();
    private int _nextId = 1;

    public TaskItem Add(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Название не может быть пустым.");

        var task = new TaskItem
        {
            Id = _nextId++,
            Title = title.Trim(),
            Status = TaskTracker.Core.Models.TaskStatus.New
        };

        _tasks.Add(task);
        return task;
    }

    public List<TaskItem> GetAll()
    {
        // Возвращаем копию, чтобы внешний код не ломал список
        return _tasks.ToList();
    }
    private TaskItem GetExisting(int id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task is null)
            throw new ArgumentException($"Задача с Id={id} не найдена.");
        return task;
    }

    public TaskItem ChangeStatus(int id, TaskTracker.Core.Models.TaskStatus newStatus)
    {
        var task = GetExisting(id);
        task.Status = newStatus;
        return task;
    }

    public void Delete(int id)
    {
        var task = GetExisting(id);
        _tasks.Remove(task);
    }
    public TaskItem Update(int id, string newTitle, string newDescription)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Название не может быть пустым.");

        var task = GetExisting(id);

        task.Title = newTitle.Trim();
        task.Description = (newDescription ?? "").Trim();

        return task;
    }
    public List<TaskItem> SearchByTitle(string query)
    {
        query ??= "";
        query = query.Trim();

        if (query.Length == 0)
            return GetAll();

        return _tasks
            .Where(t => (t.Title ?? "").Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
    public List<TaskItem> FilterByStatus(Models.TaskStatus? status1)
    {
        if (status1 is null)
            return GetAll(); // null = All

        return _tasks.Where(t => t.Status == status1).ToList();
    }
    public List<TaskItem> SortById(bool ascending = true)
    {
        return ascending
            ? _tasks.OrderBy(t => t.Id).ToList()
            : _tasks.OrderByDescending(t => t.Id).ToList();
    }
    public List<TaskItem> SortByStatusThenId()
    {
        return _tasks
            .OrderBy(t => t.Status)
            .ThenBy(t => t.Id)
            .ToList();
    }
    public void ReplaceAll(List<TaskItem> newTasks)
    {
        newTasks ??= new List<TaskItem>();

        _tasks.Clear();
        _tasks.AddRange(newTasks);

        // Пересчитать следующий Id
        _nextId = _tasks.Count == 0 ? 1 : _tasks.Max(t => t.Id) + 1;
    }


}
