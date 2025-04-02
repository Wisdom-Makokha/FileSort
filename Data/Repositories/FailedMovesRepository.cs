using FileSort.Data;
using FileSort.Data.Interfaces;
using FileSort.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Data.Repositories
{
    internal class FailedMovesRepository : BaseRepository<FailedMoves>, IFailedMovesRepository
    {
        public FailedMovesRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
        }
    }
}
