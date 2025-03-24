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
    }
}
