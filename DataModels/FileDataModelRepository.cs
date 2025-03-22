using FileSort.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.DataModels
{
    internal class FileDataModelRepository : BaseRepository<FileDataModel>, IFileDataModelRepository
    {
        public FileDataModelRepository(ApplicationDBContext dbContext) : base(dbContext) { }

    }
}
