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
    internal class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
        }

        public Category? GetCategoryByName(string categoryName)
        {
            return _dbContext.Categories.FirstOrDefault(c => c.CategoryName == categoryName);
        }
    }
}
