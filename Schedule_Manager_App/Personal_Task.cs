using System;
using System.Xml.Linq;

public class PersonalTask : Task
{
    public string Frequency { get; private set; }

    public PersonalTask(string name, string notes, DateTime startTime, DateTime endTime, string frequency)
        : base(name, notes, startTime, endTime)
    {
        Frequency = frequency;
    }

    public override string GetTaskType()
    {
        return "Personal Task";
    }

    public override void Display()
    {
        Console.WriteLine("=== Personal Task ===");
        Console.WriteLine("Task: " + Name);
        Console.WriteLine("Frequency: " + Frequency);
        Console.WriteLine("Status: " + (IsCompleted ? "Completed" : "Not Completed"));
    }
}
