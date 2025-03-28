using FileSort.Data;
using FileSort.DataModels;
using FileSort.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Repositories
{
    internal class FileDataModelRepository : BaseRepository<FileDataModel>, IFileDataModelRepository
    {
        public FileDataModelRepository(ApplicationDBContext dbContext) : base(dbContext) { }

        public FileDataModel? GetByFileNameAndExtension(string fileName, string extension)
        {
            return _dbContext.Files.FirstOrDefault(file => file.FileName == fileName && file.FileExtension.ExtensionName == extension);
        }

        public Dictionary<string, string> GetInstanceMovedFiles(Guid applicationInstance)
        {
            var movedFiles = from file in _dbContext.Files
                             where file.ApplicationInstanceId == applicationInstance
                             select new
                             {
                                 name = file.FileName,
                                 destination = file.DestinationFolderPath
                             };

            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var file in movedFiles)
            {
                result.Add(file.name, file.destination);
            }

            return result;
        }
    }
}
