
namespace Schedule_Manager_App.Excel_File
{
    class Work_TaskFilter : ISpecification<WorkTask>
    {
        public Work_TaskRepo work_TaskRepo;
        public Work_TaskFilter()
        {
            work_TaskRepo = new Work_TaskRepo();
        }
        public WorkTask GetById(int id)
        {
            return work_TaskRepo.FindTaskById(id);
        }

        public WorkTask GetByUserName(string username)
        {
            if (IsSatisfiedByUsename(username))
            {
                return work_TaskRepo.FindTaskByTaskname(username);
            }
            return null;
        }

        public bool IsSatisfiedByDeadline(int id)
        {
            if (IsSatisfiedById(id))
            {
                WorkTask task = work_TaskRepo.FindTaskById(id);
                if (task.EndTime <= DateTime.Now)
                    return false;
            }
            return true;
        }

        public bool IsSatisfiedById(int id)
        {
            if (work_TaskRepo.FindTaskById(id) != null)
                return true;
            return false;
        }

        public bool IsSatisfiedByUsename(string username)
        {
            if (work_TaskRepo.FindTaskByTaskname(username) != null)
                return true;
            return false;
        }

        public List<WorkTask> sortbyStarttime()
        {
            List<WorkTask> original = work_TaskRepo.GetAllTasks();
            WorkTask temp;
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

        public List<WorkTask> sortbyEndtime()
        {
            List<WorkTask> original = work_TaskRepo.GetAllTasks();
            WorkTask temp;
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

        public List<WorkTask> sortbyStartTime()
        {
            List<WorkTask> original = work_TaskRepo.GetAllTasks();
            WorkTask temp;
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
    }
}
