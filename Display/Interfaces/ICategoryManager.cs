using FileSort.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Display.Interfaces
{
    internal interface ICategoryManager
    {
        void CheckCategories();
        void ShowCategories();
        void AddCategory();
        void ShowCategory(Category category);
        bool RemoveCategory(Category category);
        bool EditCategoryName(Category category);
        bool EditCategoryDescription(Category category);
    }
}
