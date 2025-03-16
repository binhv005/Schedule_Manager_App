using System;
using System.Xml.Linq;

public class StudyTask : Task
{
    public string Subject { get; private set; }
    public DateTime Deadline { get; private set; }

    public StudyTask(string name, string notes, DateTime startTime, DateTime endTime, string subject, DateTime deadline)
        : base(name, notes, startTime, endTime)
    {
        Subject = subject;
        Deadline = deadline;
    }

    public override string GetTaskType()
    {
        return "Study Task";
    }

    public override void Display()
    {
        Console.WriteLine("=== Study Task ===");
        Console.WriteLine("Subject: " + Subject);
        Console.WriteLine("Task: " + Name);
        Console.WriteLine("Deadline: " + Deadline.ToString("yyyy-MM-dd HH:mm"));
        Console.WriteLine("Status: " + (IsCompleted ? "Completed" : "Not Completed"));
    }
}
