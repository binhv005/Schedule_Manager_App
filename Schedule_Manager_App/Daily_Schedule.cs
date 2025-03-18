using System;
using System.Collections.Generic;
using System.Globalization;

class DailySchedule
{
    public string Day { get; set; }
    public string Week { get; set; }
    public List<BaseTask> DailyTasks { get; set; }

    public DailySchedule(string day, string week)
    {
        this.Day = day;
        this.Week = week;
        this.DailyTasks = new List<BaseTask>();
    }

    public void AddTask(BaseTask task)
    {
        this.DailyTasks.Add(task);
    }

    public void DisplayTasks()
    {
        Console.WriteLine("📅 Ngày: " + this.Day);

        DateTime currentDay;
        bool isValidDate = DateTime.TryParseExact(this.Day, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out currentDay);

        if (!isValidDate)
        {
            Console.WriteLine("❌ Lỗi: Ngày không hợp lệ.");
            return;
        }

        bool hasTask = false;

        for (int i = 0; i < this.DailyTasks.Count; i++)
        {
            BaseTask task = this.DailyTasks[i];

            if (task.StartTime.Date == currentDay.Date)
            {
                task.Display();
                hasTask = true;
            }
        }

        if (!hasTask)
        {
            Console.WriteLine("❌ Không có nhiệm vụ nào trong ngày này.");
        }
    }
}
