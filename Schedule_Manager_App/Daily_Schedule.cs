using System;
using System.Collections.Generic;

public class DailySchedule
{
    public DateTime Date { get; private set; }
    private List<Task> Tasks;

    public DailySchedule(DateTime date)
    {
        Date = date;
        Tasks = new List<Task>();
    }

    public void AddTask(Task task)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task), "Task cannot be null.");
        }
        Tasks.Add(task);
    }

    public void RemoveTask(string taskName)
    {
        for (int i = 0; i < Tasks.Count; i++)
        {
            if (!string.IsNullOrEmpty(Tasks[i].Name) && Tasks[i].Name.Equals(taskName, StringComparison.OrdinalIgnoreCase))
            {
                Tasks.RemoveAt(i);
                Console.WriteLine($"Công việc '{taskName}' đã được xóa.");
                return;
            }
        }
        Console.WriteLine($"Không tìm thấy công việc '{taskName}' để xóa.");
    }

    public void DisplayDay()
    {
        Console.WriteLine("=== Lịch ngày: " + Date.ToString("dd/MM/yyyy") + " ===");

        if (Tasks.Count == 0)
        {
            Console.WriteLine("Không có công việc nào.");
            return;
        }

        for (int i = 0; i < Tasks.Count; i++)
        {
            Task task = Tasks[i];

            // Chỉ hiển thị tên và trạng thái của Task
            if (task != null)
            {
                Console.WriteLine($"- {task.Name} ({task.GetTaskType()}), Trạng thái: {(task.IsCompleted ? "Hoàn thành" : "Chưa hoàn thành")}");
            }
        }
    }
}
