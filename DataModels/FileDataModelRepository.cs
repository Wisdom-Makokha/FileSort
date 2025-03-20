using FileSort.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.DataModels
{
    internal class FileDataModelRepository : IFileDataModelRepository
    {
        protected readonly ApplicationDBContext _dbContext;

        public FileDataModelRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public FileDataModel? GetFileDataModel(int id)
        {
            var file = _dbContext.Files.FirstOrDefault(f => f.Id == id);

            if (file != null)
            {
                return file;
            }

            return null;
        }

        public IEnumerable<FileDataModel> GetAll()
        {
            return _dbContext.Files.ToList();
        }

        public void AddDataModel(FileDataModel dataModel)
        {
            _dbContext.Files.Add(dataModel);
        }

        public void UpdateDataModel(FileDataModel dataModel)
        {
            var file = GetFileDataModel(dataModel.Id);

            if (file != null)
            {
                _dbContext.Files.Update(dataModel);
            }
            else
            {
                throw new ArgumentNullException($"{nameof(dataModel)} cannot be null in update request");
            }
        }

        public void DeleteDataModel(int id)
        {
            var file = GetFileDataModel(id);

            if (file != null)
            {
                _dbContext.Files.Remove(file);
            }
            else
            {
                throw new ArgumentNullException($"{nameof(file)} cannot be null in dellete request");
            }
        }

        public bool DataModelExists(FileDataModel dataModel)
        {
            return GetFileDataModel(dataModel.Id) != null;
        }

        public void SaveChanges()
        { _dbContext.SaveChanges(); }
    }
}
