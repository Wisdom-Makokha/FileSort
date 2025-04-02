using FileSort.Data;
using FileSort.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Services
{
    internal interface IDataService
    {
        List<Category> Categories { get; }
        List<Extension> Extensions { get; }

        ApplicationInstance CreateApplicationInstance(DateTime dateTime);
        void EnsureDatabaseSeeded();
        void LoadInitialData();
    }
}
