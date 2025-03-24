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
    internal class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
        }

        public int GetCategoryId(string categoryName)
        {
            return _dbContext.Categories.Single(c =>  c.CategoryName == categoryName).Id;
        }
    }
}
