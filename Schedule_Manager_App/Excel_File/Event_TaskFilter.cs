using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule_Manager_App.Excel_File
{
    class Event_TaskFilter : ISpecification<EventTask>
    {
        public Event_TaskRepo event_TaskRepo;
        public Event_TaskFilter()
        {
            event_TaskRepo = new Event_TaskRepo();
        }
        public EventTask GetById(int id)
        {
            if(IsSatisfiedById(id))
            {
                return event_TaskRepo.FindTaskById(id);
            }
            return null;
        }

        public EventTask GetByUserName(string username)
        {
            if (IsSatisfiedByUsename(username))
            {
                return event_TaskRepo.FindTaskByTaskname(username);
            }
            return null;
        }

        public bool IsSatisfiedByDeadline(int id)
        {
            if (IsSatisfiedById(id))
            {
                EventTask task = event_TaskRepo.FindTaskById(id);
                if (task.EndTime <= DateTime.Now)
                    return false;
            }
            return true;
        }

        public bool IsSatisfiedById(int id)
        {
            if (event_TaskRepo.FindTaskById(id) != null)
                return true;
            return false;
        }

        public bool IsSatisfiedByUsename(string username)
        {
            if (event_TaskRepo.FindTaskByTaskname(username) != null)
                return true;
            else
                return false;
        }

        public List<EventTask> sortbyEndtime()
        {
            List<EventTask> original = event_TaskRepo.GetAllTasks();
            EventTask temp;
            int len = original.Count;
            for (int i = 0; i < len - 1; i++)
            {
                for (int j = i + 1; j < len; j++)
                {
                    if (original[i].EndTime > original[j].EndTime)
                    {
                        temp = original[i];
                        temp = original[j];
                        original[j] = temp;
                    }

                }
            }
            return original;
        }

        public List<EventTask> sortbyStartTime()
        {
            List<EventTask> original = event_TaskRepo.GetAllTasks();
            EventTask temp;
            int len = original.Count;
            for (int i = 0; i < len - 1; i++)
            {
                for (int j = i + 1; j < len; j++)
                {
                    if (original[i].StartTime > original[j].StartTime)
                    {
                        temp = original[i];
                        temp = original[j];
                        original[j] = temp;
                    }

                }
            }
            return original;
        }
    }
}
