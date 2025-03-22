using FileSort.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.DataModels
{
    internal class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDBContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<T>();
        }

        public T? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public void AddEntity(T entity)
        {
            _dbSet.Add(entity);
        }

        public void DeleteEntity(int id)
        {
            var entity = GetById(id);

            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public void UpdateEntity(T entity)
        {
            _dbSet.Update(entity);
        }

        public bool Exists(int id)
        {
            var T = GetById(id);

            return T != null;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
