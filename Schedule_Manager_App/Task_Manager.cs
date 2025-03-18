using System;
using System.Collections.Generic;
using System.Globalization;

class TaskManager
{
    private static TaskManager instance;
    private List<WeeklySchedule> weeklySchedules;

    private TaskManager()
    {
        this.weeklySchedules = new List<WeeklySchedule>();
    }

    public static TaskManager GetInstance()
    {
        if (instance == null)
        {
            instance = new TaskManager();
        }
        return instance;
    }

    public void CreateTask(string taskName, DateTime startTime, DateTime endTime, string taskType, string note)
    {
        BaseTask newTask;
        if (taskType == "Work")
        {
            newTask = new WorkTask(taskName, startTime, endTime, note);
        }
        else if (taskType == "Personal")
        {
            newTask = new PersonalTask(taskName, startTime, endTime, note);
        }
        else if (taskType == "Event")
        {
            newTask = new EventTask(taskName, startTime, endTime, note);
        }
        else if (taskType == "Study")
        {
            newTask = new StudyTask(taskName, startTime, endTime, note, startTime.AddDays(7));
        }
        else
        {
            throw new ArgumentException("Invalid Task Type");
        }

        AddTaskToSchedule(newTask);
    }

    private void AddTaskToSchedule(BaseTask task)
    {
        string week = GetWeekOfYear(task.StartTime);
        string day = task.StartTime.ToShortDateString();

        WeeklySchedule weeklySchedule = GetOrCreateWeeklySchedule(week);
        DailySchedule dailySchedule = GetOrCreateDailySchedule(weeklySchedule, day);

        dailySchedule.AddTask(task);

        // 🆕 Kiểm tra nếu chưa có DailySchedule trong WeeklySchedule thì thêm vào
        bool dailyExists = false;
        for (int i = 0; i < weeklySchedule.DailySchedules.Count; i++)
        {
            if (weeklySchedule.DailySchedules[i].Day == day)
            {
                dailyExists = true;
                break;
            }
        }

        if (!dailyExists)
        {
            weeklySchedule.AddDailySchedule(dailySchedule);
        }
    }

    private WeeklySchedule GetOrCreateWeeklySchedule(string week)
    {
        for (int i = 0; i < weeklySchedules.Count; i++)
        {
            if (weeklySchedules[i].Week == week)
            {
                return weeklySchedules[i];
            }
        }
        WeeklySchedule newWeeklySchedule = new WeeklySchedule(week);
        weeklySchedules.Add(newWeeklySchedule);
        return newWeeklySchedule;
    }

    private DailySchedule GetOrCreateDailySchedule(WeeklySchedule weeklySchedule, string day)
    {
        DateTime searchDate = DateTime.ParseExact(day, "dd/MM/yyyy", null); // Chỉ lấy ngày

        for (int i = 0; i < weeklySchedule.DailySchedules.Count; i++)
        {
            DateTime existingDate = DateTime.ParseExact(weeklySchedule.DailySchedules[i].Day, "dd/MM/yyyy", null);
            if (existingDate == searchDate)
            {
                return weeklySchedule.DailySchedules[i];
            }
        }

        DailySchedule newDailySchedule = new DailySchedule(day, weeklySchedule.Week);
        weeklySchedule.AddDailySchedule(newDailySchedule);
        return newDailySchedule;
    }


    private string GetWeekOfYear(DateTime date)
    {
        int weekNum = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
            date,
            CalendarWeekRule.FirstFourDayWeek,
            DayOfWeek.Monday
        );
        return date.Year + "-W" + weekNum;
    }

    // ✅ Thêm phương thức xóa nhiệm vụ theo ID
    public bool DeleteTask(int taskId)
    {
        for (int i = 0; i < weeklySchedules.Count; i++)
        {
            WeeklySchedule weeklySchedule = weeklySchedules[i];

            for (int j = 0; j < weeklySchedule.DailySchedules.Count; j++)
            {
                DailySchedule dailySchedule = weeklySchedule.DailySchedules[j];

                for (int k = 0; k < dailySchedule.DailyTasks.Count; k++)
                {
                    if (dailySchedule.DailyTasks[k].TaskId == taskId)
                    {
                        dailySchedule.DailyTasks.RemoveAt(k);
                        Console.WriteLine($"✅ Nhiệm vụ có ID {taskId} đã bị xóa.");
                        return true;
                    }
                }
            }
        }
        Console.WriteLine($"❌ Không tìm thấy nhiệm vụ có ID {taskId}.");
        return false;
    }
}
