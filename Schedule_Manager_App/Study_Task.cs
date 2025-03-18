using System;

class StudyTask : BaseTask
{
    public string Subject { get; set; }
    public DateTime Deadline { get; set; }

    public StudyTask(string taskName, DateTime startTime, DateTime endTime, string subject, DateTime deadline)
        : base(taskName, startTime, endTime)
    {
        this.Subject = subject;
        this.Deadline = deadline;
    }

    public override void Display()
    {
        Console.WriteLine("[Study Task] " + TaskName + " - Môn học: " + Subject + " - Deadline: " + Deadline.ToShortDateString() + " - Thời gian: " + StartTime.TimeOfDay + " - " + EndTime.TimeOfDay);
    }

    public void CheckDeadline()
    {
        if (DateTime.Now > Deadline)
        {
            Console.WriteLine("❌ Nhiệm vụ '" + TaskName + "' đã quá hạn! Deadline là " + Deadline.ToShortDateString());
        }
        else
        {
            Console.WriteLine("⏳ Deadline của '" + TaskName + "' là " + Deadline.ToShortDateString());
        }
    }
}
