using System;

class EventTask : BaseTask
{
    public string Location { get; set; }

    // Constructor rỗng
    public EventTask() { }

    public EventTask(string taskName, DateTime startTime, DateTime endTime, string location)
        : base(taskName, startTime, endTime)
    {
        this.Location = location;
    }

    public override void Display()
    {
        Console.WriteLine("[Event Task] " + TaskName + " - Địa điểm: " + Location + " - Thời gian: " + StartTime.TimeOfDay + " - " + EndTime.TimeOfDay);
    }
}
