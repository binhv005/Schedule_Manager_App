class PersonalTask : BaseTask
{
    public string Frequency { get; set; }

    public PersonalTask(string taskName, DateTime startTime, DateTime endTime, string frequency)
        : base(taskName, startTime, endTime)
    {
        this.Frequency = frequency;
    }

    public override void Display()
    {
        Console.WriteLine("[Personal Task] " + TaskName + " - Time " + StartTime.TimeOfDay);
    }
}
