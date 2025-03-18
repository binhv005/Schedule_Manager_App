using System;
using System.Collections.Generic;
using System.Globalization;

class ScheduleManagerApp
{
    private UserManager userManager;
    private TaskManager taskManager;
    private List<WeeklySchedule> weeklySchedules;

    public ScheduleManagerApp()
    {
        this.userManager = UserManager.GetInstance();
        this.taskManager = TaskManager.GetInstance();
        this.weeklySchedules = new List<WeeklySchedule>();
    }

    public void AddUser(string userName, string password)
    {
        userManager.Register(userName, password);
    }

    public void CreateTask(string taskName, DateTime startTime, DateTime endTime, string taskType, string note)
    {
        taskManager.CreateTask(taskName, startTime, endTime, taskType, note);
    }

    public void AssignTaskToSchedule(DailySchedule schedule, BaseTask task)
    {
        if (schedule == null || task == null)
        {
            Console.WriteLine("❌ Lịch trình hoặc nhiệm vụ không hợp lệ.");
            return;
        }
        schedule.AddTask(task);
    }

    public bool DeleteTask(int taskId)
    {
        return taskManager.DeleteTask(taskId);
    }

    public void DisplayWeeklySchedule(WeeklySchedule weeklySchedule)
    {
        if (weeklySchedule == null)
        {
            Console.WriteLine("❌ Lịch tuần không hợp lệ.");
            return;
        }

        if (weeklySchedule.DailySchedules.Count == 0)
        {
            Console.WriteLine("❌ Không có lịch trình nào trong tuần này.");
            return;
        }

        Console.WriteLine("📆 Lịch trình tuần: " + weeklySchedule.Week);
        for (int i = 0; i < weeklySchedule.DailySchedules.Count; i++)
        {
            weeklySchedule.DailySchedules[i].DisplayTasks();
        }
    }

    public void DisplayDailySchedule(string day)
    {
        DateTime searchDate;
        if (!DateTime.TryParseExact(day, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out searchDate))
        {
            Console.WriteLine("❌ Định dạng ngày không hợp lệ. Vui lòng nhập theo dạng dd/MM/yyyy.");
            return;
        }

        for (int i = 0; i < weeklySchedules.Count; i++)
        {
            WeeklySchedule weeklySchedule = weeklySchedules[i];
            for (int j = 0; j < weeklySchedule.DailySchedules.Count; j++)
            {
                DailySchedule dailySchedule = weeklySchedule.DailySchedules[j];
                DateTime existingDate;
                if (DateTime.TryParseExact(dailySchedule.Day, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out existingDate) && existingDate == searchDate)
                {
                    dailySchedule.DisplayTasks();
                    return;
                }
            }
        }

        Console.WriteLine("❌ Không có nhiệm vụ nào trong ngày này.");
    }

    public WeeklySchedule GetWeeklySchedule(string week)
    {
        for (int i = 0; i < weeklySchedules.Count; i++)
        {
            if (weeklySchedules[i].Week == week)
            {
                return weeklySchedules[i];
            }
        }

        Console.WriteLine("❌ Không có lịch trình nào trong tuần này.");
        return null; // Trả về null thay vì tạo mới
    }
}
