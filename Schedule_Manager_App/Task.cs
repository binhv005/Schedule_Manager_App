using System;

public abstract class Task
{
    public string Name { get; protected set; }
    public string Notes { get; protected set; }
    public bool IsCompleted { get; protected set; }
    public DateTime StartTime { get; protected set; }
    public DateTime EndTime { get; protected set; }

    public Task(string name, string notes, DateTime startTime, DateTime endTime)
    {
        Name = name;
        Notes = notes;
        StartTime = startTime;
        EndTime = endTime;
        IsCompleted = false;
    }

    public void MarkAsCompleted()
    {
        if (DateTime.Now >= EndTime)
        {
            IsCompleted = true;
        }
    }

    public bool IsTimeToRemind()
    {
        return (DateTime.Now >= StartTime.AddMinutes(-10) && DateTime.Now < StartTime);
    }

    public abstract string GetTaskType();
    public abstract void Display();
}
