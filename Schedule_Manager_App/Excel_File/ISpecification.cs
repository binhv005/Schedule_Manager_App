using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule_Manager_App.Excel_File
{
    interface ISpecification<T>
    {
        bool IsSatisfiedById(int id);
        bool IsSatisfiedByDeadline(int id);
        public bool IsSatisfiedByUsename(string username);
        public List<T> sortbyEndtime();
        public List<T> sortbyStartTime();
        public T GetByUserName(string username);
        public T GetById(int id);
    }
}
