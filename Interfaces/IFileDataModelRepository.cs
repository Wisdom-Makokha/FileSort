using FileSort.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Interfaces
{
    internal interface IFileDataModelRepository : IBaseRepository<FileDataModel>
    {
        FileDataModel? GetByFileNameAndExtension(string fileName, string extension);
        List<FileDataModel> GetInstanceMovedFiles(Guid applicationInstance);
    }
}
