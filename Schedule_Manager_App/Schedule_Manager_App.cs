using System;

public class ScheduleManagerApp
{
    private static ScheduleManagerApp instance;
    private WeeklySchedule weeklySchedule;

    private ScheduleManagerApp()
    {
        weeklySchedule = new WeeklySchedule();
    }

    public static ScheduleManagerApp Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ScheduleManagerApp();
            }
            return instance;
        }
    }

    public void AddTask(DateTime date, Task task)
    {
        weeklySchedule.AddTask(date, task);
    }

    public void DisplayDailySchedule(DateTime date)
    {
        Console.WriteLine("\n===== Lịch ngày =====");
        // Gọi phương thức DisplayDay từ weeklySchedule
        weeklySchedule.DisplayDay(date);
    }

    public void DisplayWeeklySchedule(DateTime startDate)
    {
        Console.WriteLine("\n===== Lịch tuần =====");
        weeklySchedule.DisplayWeek(startDate);
    }

    // Phương thức xóa công việc
    public void RemoveTask(DateTime date, string taskName)
    {
        weeklySchedule.RemoveTask(date, taskName);
    }
}
