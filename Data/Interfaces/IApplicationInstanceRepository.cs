using FileSort.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Data.Interfaces
{
    internal interface IApplicationInstanceRepository : IBaseRepository<ApplicationInstance>
    {
        void SetClosingTime(ApplicationInstance applicationInstance);
        ApplicationInstance? GetInstanceById(Guid id);
        ApplicationInstance? GetInstanceByTime(DateTime dateTime);
        void DeleteInstance(Guid id);
        bool InstanceExists(Guid id);
    }
}
