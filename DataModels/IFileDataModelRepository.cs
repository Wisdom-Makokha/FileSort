using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.DataModels
{
    internal interface IFileDataModelRepository
    {
        FileDataModel? GetFileDataModel(int id);
        IEnumerable<FileDataModel> GetAll();
        void AddDataModel(FileDataModel dataModel);
        void UpdateDataModel (FileDataModel dataModel);
        void DeleteDataModel (int id);
        bool DataModelExists (FileDataModel dataModel);
        void SaveChanges();
    }
}
