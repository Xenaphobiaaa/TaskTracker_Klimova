using TaskTracker.Core.Models;
using TaskTracker.Core.Services;

var service = new TaskService();

static bool TryReadInt(string prompt, out int value)
{
    Console.Write(prompt);
    var text = Console.ReadLine();
    return int.TryParse(text, out value);
}


while (true)
{
    Console.WriteLine();
    Console.WriteLine("TaskTracker v0.2");
    Console.WriteLine("----------------");
    Console.WriteLine("1) Добавить задачу");
    Console.WriteLine("2) Показать список задач");
    Console.WriteLine("3) Изменить статус задачи");
    Console.WriteLine("4) Удалить задачу");
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

    if (input == "3")
    {
        var tasks = service.GetAll();
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст. Нечего менять.");
            continue;
        }

        Console.WriteLine("Список задач:");
        foreach (var t in tasks)
            Console.WriteLine($"{t.Id}. {t.Title} [{t.Status}]");

        if (!TryReadInt("Введите Id задачи: ", out var id))
        {
            Console.WriteLine("Ошибка: Id должно быть числом.");
            continue;
        }

        Console.WriteLine("Выберите новый статус:");
        Console.WriteLine("0 - New (Новая)");
        Console.WriteLine("1 - InProgress (В работе)");
        Console.WriteLine("2 - Done (Готово)");

        if (!TryReadInt("Введите статус (0/1/2): ", out var statusNumber))
        {
            Console.WriteLine("Ошибка: статус должен быть числом 0/1/2.");
            continue;
        }

        if (statusNumber < 0 || statusNumber > 2)
        {
            Console.WriteLine("Ошибка: статус должен быть 0, 1 или 2.");
            continue;
        }

        var newStatus = (TaskTracker.Core.Models.TaskStatus)statusNumber;

        try
        {
            var updated = service.ChangeStatus(id, newStatus);
            Console.WriteLine($"Статус изменён: #{updated.Id} {updated.Title} [{updated.Status}]");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }

        continue;
    }

    if (input == "4")
    {
        var tasks = service.GetAll();
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст. Нечего удалять.");
            continue;
        }

        Console.WriteLine("Список задач:");
        foreach (var t in tasks)
            Console.WriteLine($"{t.Id}. {t.Title} [{t.Status}]");

        if (!TryReadInt("Введите Id задачи для удаления: ", out var id))
        {
            Console.WriteLine("Ошибка: Id должно быть числом.");
            continue;
        }

        Console.Write("Точно удалить? (y/n): ");
        var answer = (Console.ReadLine() ?? "").Trim().ToLower();

        if (answer != "y")
        {
            Console.WriteLine("Удаление отменено.");
            continue;
        }

        try
        {
            service.Delete(id);
            Console.WriteLine($"Задача с Id={id} удалена.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }

        continue;
    }


    Console.WriteLine("Неизвестная команда. Введите 1, 2 или 0.");
}
