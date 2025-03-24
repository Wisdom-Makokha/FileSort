using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Interfaces
{
    internal interface IBaseRepository<T> where T : class
    {
        T? GetById(int id);
        IEnumerable<T> GetAll();
        void AddEntity(T entity);
        void UpdateEntity(T entity);
        void DeleteEntity(int id);
        bool Exists(int id);
        void SaveChanges();
    }
}
