using FileSort.Data;
using FileSort.DataModels;
using FileSort.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Repositories
{
    internal class ApplicationInstanceRepository : BaseRepository<ApplicationInstance>, IApplicationInstanceRepository
    {
        public ApplicationInstanceRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
        }

        public void SetClosingTime(ApplicationInstance applicationInstance)
        {
            applicationInstance.ClosingTime = DateTime.Now;
            UpdateEntity(applicationInstance);
        }

        public ApplicationInstance? GetInstanceById(Guid id)
        {
            return _dbContext.ApplicationInstances.Find(id);
        }

        public ApplicationInstance? GetInstanceByTime(DateTime dateTime)
        {
            return _dbContext.ApplicationInstances.FirstOrDefault(i => i.InitiationTime == dateTime);
        }

        public void DeleteInstance(Guid id)
        {
            var applicationInstance = GetInstanceById(id);

            if (applicationInstance != null)
            {
                _dbContext.ApplicationInstances.Remove(applicationInstance);
            }
        }

        public bool InstanceExists(Guid id)
        {
            return GetInstanceById(id) != null;
        }
    }
}
