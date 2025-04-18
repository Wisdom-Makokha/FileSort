﻿using FileSort.Data;
using FileSort.Data.Interfaces;
using FileSort.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Data.Repositories
{
    internal class FileDataModelRepository : BaseRepository<FileDataModel>, IFileDataModelRepository
    {
        public FileDataModelRepository(ApplicationDBContext dbContext) : base(dbContext) { }

        public FileDataModel? GetByFileNameAndExtension(string fileName, string extension)
        {
            return _dbContext.Files.FirstOrDefault(file => file.FileName == fileName && file.FileExtension.ExtensionName == extension);
        }

        public List<FileDataModel> GetInstanceMovedFiles(Guid applicationInstance)
        {
            return _dbContext.Files.Where(file => file.ApplicationInstanceId == applicationInstance).ToList();
        }
    }
}
