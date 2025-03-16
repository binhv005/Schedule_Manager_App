using System;
using System.Collections.Generic;

public class TaskManager : IManager<Task>
{
    private static TaskManager instance;
    private Dictionary<int, Task> tasks;
    private int nextId;

    private TaskManager()
    {
        tasks = new Dictionary<int, Task>();
        nextId = 1;
    }

    public static TaskManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TaskManager();
            }
            return instance;
        }
    }

    public void Create(Task task)
    {
        tasks[nextId] = task;
        nextId++;
    }

    public Task Read(int id)
    {
        if (tasks.ContainsKey(id))
        {
            return tasks[id];
        }
        return null;
    }

    public void Update(int id, Task updatedTask)
    {
        if (tasks.ContainsKey(id))
        {
            tasks[id] = updatedTask;
        }
    }

    public void Delete(int id)
    {
        if (tasks.ContainsKey(id))
        {
            tasks.Remove(id);
        }
    }
}
