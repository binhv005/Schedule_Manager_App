using System;
using System.Collections.Generic;

class WeeklySchedule
{
    public string Week { get; set; }
    public List<DailySchedule> DailySchedules { get; set; }

    public WeeklySchedule(string week)
    {
        this.Week = week;
        this.DailySchedules = new List<DailySchedule>();
    }

    public void AddDailySchedule(DailySchedule dailySchedule)
    {
        if (dailySchedule == null)
        {
            Console.WriteLine("❌ Lỗi: Lịch ngày không hợp lệ.");
            return;
        }
        this.DailySchedules.Add(dailySchedule);
    }

    public void DisplayWeeklySchedule()
    {
        Console.WriteLine("📆 Lịch tuần: " + this.Week);

        if (this.DailySchedules.Count == 0)
        {
            Console.WriteLine("❌ Không có lịch ngày nào trong tuần này.");
            return;
        }

        for (int i = 0; i < this.DailySchedules.Count; i++)
        {
            this.DailySchedules[i].DisplayTasks();
        }
    }
}
