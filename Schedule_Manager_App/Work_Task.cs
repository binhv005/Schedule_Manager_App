using System;

public class WorkTask : Task
{
    public string Project { get; private set; }

    public WorkTask(string name, string notes, DateTime startTime, DateTime endTime, string project)
        : base(name, notes, startTime, endTime)
    {
        Project = project;
    }

    public override string GetTaskType()
    {
        return "Work Task";
    }

    public override void Display()
    {
        Console.WriteLine("=== Work Task ===");
        Console.WriteLine("Project: " + Project);
        Console.WriteLine("Task: " + Name);
        Console.WriteLine("Status: " + (IsCompleted ? "Completed" : "Not Completed"));
    }
}
