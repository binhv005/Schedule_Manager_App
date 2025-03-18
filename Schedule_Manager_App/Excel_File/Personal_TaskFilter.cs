

namespace Schedule_Manager_App.Excel_File
{
    class Personal_TaskFilter : ISpecification<PersonalTask>
    {
        public Personal_TaskRepo personalTask;
        public Personal_TaskFilter()
        {
            personalTask = new Personal_TaskRepo();
        }
        public PersonalTask GetById(int id)
        {
            if(IsSatisfiedById(id))
            {
                return personalTask.FindTaskById(id);
            }
            return null;
        }

        public PersonalTask GetByUserName(string username)
        {
            if (IsSatisfiedByUsename(username))
            {
                return personalTask.FindTaskByTaskname(username);
            }
            return null;
        }

        public bool IsSatisfiedByDeadline(int id)
        {
            if (IsSatisfiedById(id))
            {
                PersonalTask task = personalTask.FindTaskById(id);
                if (task.EndTime <= DateTime.Now)
                    return false;
            }
            return true;
        }

        public bool IsSatisfiedById(int id)
        {
            if(personalTask.FindTaskById(id) != null)
                return true;
            return false;
        }

        public bool IsSatisfiedByUsename(string username)
        {
            if(personalTask.FindTaskByTaskname(username) != null)
                return true;
            else
                return false;
        }

        public List<PersonalTask> sortbyEndtime()
        {
            List<PersonalTask> original = personalTask.GetAllTasks();
            PersonalTask temp;
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

        public List<PersonalTask> sortbyStartTime()
        {
            List<PersonalTask> original = personalTask.GetAllTasks();
            PersonalTask temp;
            int len = original.Count;
            for (int i = 0; i < len - 1; i++)
            {
                for (int j = i + 1; j < len; j++)
                {
                    if (original[i].StartTime > original[j].EndTime)
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
