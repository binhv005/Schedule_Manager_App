using System;

abstract class BaseTask
{
    private static int _taskCounter = 100;

    public string TaskName { get; set; }
    public int TaskId { get; private set; }
    public string TaskStatus { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public BaseTask(string taskName, DateTime startTime, DateTime endTime)
    {
        this.TaskName = taskName;
        this.TaskId = _taskCounter++;
        this.TaskStatus = "0%";
        this.StartTime = startTime;
        this.EndTime = endTime;
    }
    public BaseTask() { }
    public abstract void Display();
}
