using FileSort.Data.Interfaces;
using FileSort.DataModels;
using FileSort.Display.Interfaces;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Display.Managers
{
    internal class CategoryManager : ICategoryManager
    {
        private readonly ICategoryRepository _repository;
        private readonly List<Category> _categories;
        private readonly List<string> _categoryNames;

        public CategoryManager(ICategoryRepository repository)
        {
            _repository = repository;
            _categories = (List<Category>)_repository.GetAll();
            _categoryNames = _categories.Select(c => c.CategoryName).ToList();
        }

        public void CheckCategories()
        {
            var miniFunctions = new Dictionary<string, Action>()
            {
                {"show categories", ShowCategories },
                {"add category", AddCategory },
                {MainInterface.BackMessage, () => { } }
            };

            bool keepGoing = true;

            while (keepGoing)
            {
                keepGoing = MainInterface.RunOptions(miniFunctions, "CATEGORIES");
            }
        }

        public void ShowCategories()
        {
            var categories = new List<string>(_categoryNames)
            { MainInterface.BackMessage };

            bool running = true;

            while (running)
            {
                AnsiConsole.Clear();
                var pickedCategory = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Pick a category for further options: ")
                    .PageSize(15)
                    .AddChoices(categories));


                if (pickedCategory != MainInterface.BackMessage)
                {
                    var category = _repository.GetCategoryByName(pickedCategory);

                    ShowCategory(category!);
                }
                else
                    running = false;
            }
        }

        public void ViewCategoryDetails(Category category)
        {
            AnsiConsole.MarkupLine("[underline silver]CATEGORY[/]");

            AnsiConsole.MarkupLine($"[magenta]Name: - [/][cyan]{category.CategoryName}[/]");
            AnsiConsole.MarkupLine($"[magenta]Description: - [/][cyan]{category.CategoryDescription}[/]");
            AnsiConsole.MarkupLine($"[magenta]Extensions: [/]");
            foreach (var item in category.Extensions)
            {
                AnsiConsole.MarkupLine($"\t[magenta]- [/][cyan]{item.ExtensionName}[/]");
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        public void ShowCategory(Category category)
        {
            ViewCategoryDetails(category);

            var options = new List<string>()
            { "more options", MainInterface.BackMessage };

            var moreOptions = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[magenta]More category options: -[/]")
                .AddChoices(options));

            switch (moreOptions)
            {
                case "more options":
                    CategoryOptions(category);
                    break;
                default:
                    break;
            }
        }

        public void CategoryOptions(Category category)
        {
            bool tryAgain = true;

            var moreOnCategory = new Dictionary<string, Func<Category, bool>>
            {
                {"edit category name", EditCategoryName },
                {"edit category description", EditCategoryDescription },
                {"remove category", RemoveCategory},
            };

            HashSet<string> moreOptions = new HashSet<string>(moreOnCategory.Keys)
                { MainInterface.BackMessage};

            while (tryAgain)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[underline silver]CATEGORY[/]");

                var userChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .AddChoices(moreOptions));

                if (userChoice == MainInterface.BackMessage)
                    tryAgain = false;
                else
                {
                    moreOnCategory[userChoice](category);
                }
            }
        }

        public void AddCategory()
        {
            var newCategory = AnsiConsole.Prompt(new TextPrompt<string>("[magenta]Enter a name for a new category: [/]"));
            var newCategoryDescription = AnsiConsole.Prompt(
                new TextPrompt<string?>("[magenta]Enter a description for the new category: [/]")
                .DefaultValue("No description added"));

            Category categoryEntity = new Category()
            {
                CategoryName = newCategory,
                CategoryDescription = newCategoryDescription
            };

            bool confirm = AnsiConsole.Prompt(
               new TextPrompt<bool>($"[magenta0]Are your sure you want to add a category with the name: [/][cyan]{newCategory}[/][magenta] and description: [/][cyan]{newCategoryDescription}[/]?")
                   .AddChoice(true)
                   .AddChoice(false)
                   .DefaultValue(false)
                   .WithConverter(choice => choice ? "y" : "n"));

            if (confirm)
            {
                try
                {
                    _repository.AddEntity(categoryEntity);
                    _repository.SaveChanges();
                    _categories.Add(categoryEntity);
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: [/][cyan]{ex.Message}[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]New category not added[/]");
            }
        }

        public bool RemoveCategory(Category category)
        {
            bool confirm = AnsiConsole.Prompt(
                new TextPrompt<bool>($"Are your sure you want to remove category: {category.CategoryName}?")
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(true)
                    .WithConverter(choice => choice ? "y" : "n"));

            if (confirm)
            {
                try
                {
                    _repository.DeleteEntity(category.Id);
                    _repository.SaveChanges();
                    _categories.Remove(category);

                    AnsiConsole.MarkupLine($"[red]Removed category [/][cyan]{category.CategoryName}[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: [/][cyan]{ex.Message}[/]");
                }

                return true;
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]Delete canceled[/]");
                return false;
            }
        }

        public bool EditCategoryName(Category category)
        {
            var updateCategoryName = AnsiConsole.Prompt(new TextPrompt<string>("[magenta]Enter a new category name: [/]"));

            bool confirmName = AnsiConsole.Prompt(
               new TextPrompt<bool>($"[magenta]Are your sure you want to update category name from: [/][cyan]{category.CategoryName}[/][magenta] to [/][cyan]{updateCategoryName}[/]?")
                   .AddChoice(true)
                   .AddChoice(false)
                   .DefaultValue(false)
                   .WithConverter(choice => choice ? "y" : "n"));

            if (confirmName)
            {
                try
                {
                    category.CategoryName = updateCategoryName;

                    _repository.UpdateEntity(category);
                    _repository.SaveChanges();
                    _categories.FirstOrDefault(category => category.Id == category.Id)!.CategoryName = category.CategoryName;

                    AnsiConsole.MarkupLine($"[green]Category name successfully updated to [/][cyan]{category.CategoryName}[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: [/][cyan]{ex.Message}[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Update canceled[/]");
            }

            return true;
        }
        public bool EditCategoryDescription(Category category)
        {
            var updateCategoryDescription = AnsiConsole.Prompt(new TextPrompt<string?>("[magenta]Enter a new category description: [/]"));
            bool confirmDescription = AnsiConsole.Prompt(
               new TextPrompt<bool>($"[magenta]Are your sure you want to update category description from: [/][cyan]{category.CategoryDescription}[/][magenta] to [/][cyan]{updateCategoryDescription}[/]?")
                   .AddChoice(true)
                   .AddChoice(false)
                   .DefaultValue(false)
                   .WithConverter(choice => choice ? "y" : "n"));

            if (confirmDescription)
            {
                try
                {
                    category.CategoryDescription = updateCategoryDescription;

                    _repository.UpdateEntity(category);
                    _repository.SaveChanges();
                    _categories.FirstOrDefault(category => category.Id == category.Id)!.CategoryDescription = category.CategoryDescription;

                    AnsiConsole.MarkupLine($"[green]Category description successfully updated to [/][cyan]{category.CategoryDescription}[/]");
                }

                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: [/][cyan]{ex.Message}[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Update canceled[/]");
            }

            return true;
        }
    }
}
