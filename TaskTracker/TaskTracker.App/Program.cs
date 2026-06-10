using TaskTracker.Core.Models;
using TaskTracker.Core.Services;

var service = new TaskService();


while (true)
{
    Console.WriteLine();
    Console.WriteLine("TaskTracker v0.2");
    Console.WriteLine("----------------");
    Console.WriteLine("1) Добавить задачу");
    Console.WriteLine("2) Показать список задач");
    Console.WriteLine("0) Выход");
    Console.WriteLine("----------------");
    Console.Write("Выберите пункт меню: ");

    var input = Console.ReadLine();

    if (input == "0")
    {
        Console.WriteLine("Выход...");
        break;
    }

    if (input == "1")
    {
        Console.Write("Введите название задачи: ");
        var title = Console.ReadLine() ?? "";

        // Валидация: нельзя пустое
        try
        {
            var task = service.Add(title);
            Console.WriteLine($"Задача добавлена: #{task.Id} {task.Title} [{task.Status}]");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }

    }

    if (input == "2")
    {
        var tasks = service.GetAll();

        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст.");
            continue;
        }

        Console.WriteLine("Список задач:");
        foreach (var t in tasks)
        {
            Console.WriteLine($"{t.Id}. {t.Title} [{t.Status}]");
        }
        continue;
    }

    Console.WriteLine("Неизвестная команда. Введите 1, 2 или 0.");
}
