class WorkTask : BaseTask
{
    public string Department { get; set; }
    public string Note { get; set; }
    public WorkTask () { }
    public WorkTask(string taskName, DateTime startTime, DateTime endTime, string department, string note = "None")
        : base(taskName, startTime, endTime)
    {
        this.Department = department;
        this.Note = note;
    }

    public override void Display()
    {
        Console.WriteLine("[Work Task] " + TaskName + " - Day: " + StartTime.Day + " - Time: " + StartTime.TimeOfDay);
    }
}
