using System;

public class EventTask : Task
{
    public string Location { get; private set; }

    public EventTask(string name, string notes, DateTime startTime, DateTime endTime, string location)
        : base(name, notes, startTime, endTime)
    {
        Location = location;
    }

    public override string GetTaskType()
    {
        return "Event Task";
    }

    public override void Display()
    {
        Console.WriteLine("=== Event Task ===");
        Console.WriteLine("Event: " + Name);
        Console.WriteLine("Location: " + Location);
        Console.WriteLine("Status: " + (IsCompleted ? "Completed" : "Not Completed"));
    }
}
