using System;
using System.Collections.Generic;

public class WeeklySchedule
{
    private Dictionary<DateTime, DailySchedule> dailySchedules;

    public WeeklySchedule()
    {
        dailySchedules = new Dictionary<DateTime, DailySchedule>();
    }

    public void AddTask(DateTime date, Task task)
    {
        if (task == null)
        {
            throw new ArgumentNullException("task", "Task cannot be null.");
        }

        if (!dailySchedules.ContainsKey(date))
        {
            dailySchedules[date] = new DailySchedule(date);
        }

        dailySchedules[date].AddTask(task);
    }

    public void DisplayWeek(DateTime startDate)
    {
        DateTime endDate = startDate.AddDays(6);
        Console.WriteLine("=== Lịch tuần từ " + startDate.ToString("dd/MM/yyyy") + " đến " + endDate.ToString("dd/MM/yyyy") + " ===");

        for (int i = 0; i < 7; i++)
        {
            DateTime currentDate = startDate.AddDays(i);
            if (dailySchedules.ContainsKey(currentDate))
            {
                dailySchedules[currentDate].DisplayDay();
            }
            else
            {
                Console.WriteLine(currentDate.ToString("dd/MM/yyyy") + ": Không có công việc nào.");
            }
        }
    }

    public void DisplayDay(DateTime date)
    {
        if (dailySchedules.ContainsKey(date))
        {
            dailySchedules[date].DisplayDay();
        }
        else
        {
            Console.WriteLine("Không có công việc nào cho ngày " + date.ToString("dd/MM/yyyy") + ".");
        }
    }

    public void RemoveTask(DateTime date, string taskName)
    {
        if (string.IsNullOrWhiteSpace(taskName))
        {
            Console.WriteLine("Tên công việc không hợp lệ.");
            return;
        }

        if (dailySchedules.ContainsKey(date))
        {
            dailySchedules[date].RemoveTask(taskName);
        }
        else
        {
            Console.WriteLine("Không có công việc nào vào ngày " + date.ToString("dd/MM/yyyy") + ".");
        }
    }
}
