using FileSort.Data;
using FileSort.DataModels;
using FileSort.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Repositories
{
    internal class FailedMovesRepository : BaseRepository<FailedMoves>, IFailedMovesRepository
    {
        public FailedMovesRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
        }
    }
}
