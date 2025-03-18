

namespace Schedule_Manager_App.Excel_File
{
    class Study_TaskFilter : ISpecification<StudyTask>
    {
        Study_TaskRepo study_TaskRepo;
        public Study_TaskFilter()
        {
            study_TaskRepo = new Study_TaskRepo();
        }
        public StudyTask GetByUserName(string username)
        {
            return study_TaskRepo.FindTaskByTaskname(username);
        }

        public bool IsSatisfiedByDeadline(int id)
        {
            
            if (IsSatisfiedById(id))
            {
                StudyTask task = study_TaskRepo.FindTaskById(id);
                if (task.Deadline <= DateTime.Now)
                    return false;
            }
            return true;
        }

        public bool IsSatisfiedById(int id)
        {
            if (study_TaskRepo.FindTaskById(id) == null)
                return false;
            else
                return true;
        }

        public bool IsSatisfiedByUsename(string username)
        {
            if (study_TaskRepo.FindTaskByTaskname(username) == null)
                return false;
            else
                return true;
        }

       

        public StudyTask? GetById(int id)
        {
            return study_TaskRepo.FindTaskById(id);
        }

        public List<StudyTask> sortbyEndtime()
        {
            List<StudyTask> original = study_TaskRepo.GetAllTasks();
            StudyTask temp;
            int len = original.Count;
            for (int i = 0; i < len - 1; i++)
            {
                for (int j = i+1; j <len; j++)
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

        public List<StudyTask> sortbyStartTime()
        {
            List<StudyTask> original = study_TaskRepo.GetAllTasks();
            StudyTask temp;
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
