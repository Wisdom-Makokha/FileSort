using FileSort.DataModels;
using FileSort.AppDirectory;
using FileSort.FileHandling;
using FileSort.Startup;
using FileSort.Settings;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Display
{
    internal class MainInterface
    {
        public MainInterface(Startup.Startup startup)
        {
            Startup = startup ?? throw new ArgumentNullException(nameof(startup), $"{nameof(startup)} cannot be null in {nameof(MainInterface)} initialization");
        }

        public Startup.Startup Startup { get; set; }

        // user capabilities
        /*
         * sort files
         * check sorting statistics
         * check files that were not moved for one reason or another
         * set the destination directory
         * set the source directory
         * set the categories and extensions currently present
         * check all the above details
         * exit the interface and program
         */
        private static string ExitMessage
        {
            get
            { return "EXIT"; }
        }
        private static string BackMessage
        {
            get
            { return "BACK"; }
        }

        private bool RunOptions(Dictionary<string, Action> options, string interfaceName)
        {
            if (!options.ContainsKey(ExitMessage) && !options.ContainsKey(BackMessage))
                throw new ArgumentException($"Value of {nameof(ExitMessage)} or {nameof(BackMessage)} is invalid in {nameof(RunOptions)} method");

            AnsiConsole.MarkupLine($"[underline silver]{interfaceName}[/]");

            string userChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[magenta]Pick one of these options: [/]")
                    .AddChoices(options.Keys));

            if (userChoice == ExitMessage || userChoice == BackMessage)
                return false;
            else
            {
                options[userChoice]();

                AnsiConsole.MarkupLine("[yellow]Press <Enter> to continue[/]");
                Console.ReadLine();

                return true;
            }
        }

        public void Home()
        {
            Dictionary<string, Action> InterfaceOptions = new Dictionary<string, Action>()
            {
                {"sort", SortFiles },
                //{"reverse recent sort", ReverseSort },
                {"sort history", CheckSortHistory },
                {"source folder", CheckSourceFolder },
                {"destination folder", CheckDestinationFolder },
                {"categories", CheckCategories },
                {"extensions", CheckExtensions },
                {"issue", CheckIssues },
                {ExitMessage, ()=> { } }
            };

            bool keepGoing = true;

            while (keepGoing)
            {
                AnsiConsole.Clear();
                keepGoing = RunOptions(InterfaceOptions, "HOME");
            }
        }

        private void SortFiles()
        {
            SourceDirectory sourceDirectory = new SourceDirectory(Startup.ExcludedExtensions, Startup.AppSettings.SourceFolder);
            DestinationDirectory destinationDirectory = new DestinationDirectory(Startup.CategoryNames, Startup.AppSettings.DestinationFolder);
            Sort sort = new Sort(sourceDirectory, destinationDirectory, Startup);

            sort.SortFiles();
        }

        private void ReverseSort()
        {
            Reverse reverse = new Reverse(Startup.FailedMovesRepository, Startup.ApplicationInstance, Startup.FileDataModelRepository);

            reverse.ReverseSort();
        }

        private void CheckSortHistory()
        {

        }

        public void CheckSourceFolder()
        {
            Dictionary<string, Action> miniFunctions = new Dictionary<string, Action>
            {
                {"edit", EditSourceFolder },
                {BackMessage, () => { } }
            };

            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"[green]Source folder path - [/][cyan]{Startup.AppSettings.SourceFolder}[/]\n\n");

            bool KeepGoing = true;
            while (KeepGoing)
            {
                KeepGoing = RunOptions(miniFunctions, "SOURCE FOLDER");
            }
        }

        private void EditSourceFolder()
        {
            AnsiConsole.Clear();

            Console.WriteLine();
            var newFolderPath = AnsiConsole.Prompt(
                new TextPrompt<string>("[magenta]Enter the new source folder full path: [/]")
                    .DefaultValue(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)));

            bool confirm = AnsiConsole.Prompt(
               new TextPrompt<bool>($"[magenta]Update source folder path from: [/][cyan]{Startup.AppSettings.SourceFolder}[/][magenta] to [/][cyan]{newFolderPath}[/]?")
                   .AddChoice(true)
                   .AddChoice(false)
                   .DefaultValue(false)
                   .WithConverter(choice => choice ? "y" : "n"));

            if (confirm)
            {
                if (Directory.Exists(newFolderPath))
                {
                    SettingsConfigurationHelper.UpdateFolderPath(newFolderPath, SettingsConfigurationHelper.FolderType.Source);
                    Startup.GetAppSettings();
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Invalid folder path given[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Edit canceled[/]");
            }
        }

        public void CheckDestinationFolder()
        {
            Dictionary<string, Action> miniFunctions = new Dictionary<string, Action>
            {
                {"edit", EditDestinationFolder },
                {BackMessage, () => { } }
            };

            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"[green]Destination folder path - [/][cyan]{Startup.AppSettings.DestinationFolder}[/]\n\n");

            bool KeepGoing = true;
            while (KeepGoing)
            {
                KeepGoing = RunOptions(miniFunctions, "DESTINATION FOLDER");
            }
        }

        private void EditDestinationFolder()
        {
            AnsiConsole.Clear();

            Console.WriteLine();
            var newFolderPath = AnsiConsole.Prompt(
                new TextPrompt<string>("[magenta]Enter the new destination folder full path: [/]"));

            bool confirm = AnsiConsole.Prompt(
               new TextPrompt<bool>($"[magenta]Update destination folder path from: [/][cyan]{Startup.AppSettings.DestinationFolder}[/][magenta] to [/][cyan]{newFolderPath}[/]?")
                   .AddChoice(true)
                   .AddChoice(false)
                   .DefaultValue(false)
                   .WithConverter(choice => choice ? "y" : "n"));

            if (confirm)
            {
                if (Directory.Exists(newFolderPath))
                {
                    SettingsConfigurationHelper.UpdateFolderPath(newFolderPath, SettingsConfigurationHelper.FolderType.Destination);
                    Startup.GetAppSettings();
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Invalid folder path given[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Edit canceled[/]");
            }
        }

        public void CheckCategories()
        {
            Dictionary<string, Action> miniOptions = new Dictionary<string, Action>()
            {
                {"show categories", ShowCategories },
                {"add category", AddCategory },
                {BackMessage, () => { } }
            };

            bool keepGoing = true;
            while (keepGoing)
            {
                AnsiConsole.Clear();
                keepGoing = RunOptions(miniOptions, "CATEGORIES");
            }
        }

        private void ShowCategories()
        {
            bool tryAgain = true;

            List<string> categories = new List<string>();
            foreach (var name in Startup.CategoryNames)
            {
                categories.Add(name);
            }
            categories.Add(BackMessage);

            while (tryAgain)
            {
                AnsiConsole.Clear();
                var pickedCategory = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Pick a category for further options: ")
                    .AddChoices(categories));


                if (pickedCategory != BackMessage)
                {
                    var category = Startup.Categories.FirstOrDefault(c => c.CategoryName == pickedCategory);

                    ShowCategory(category!);
                }
                else
                    tryAgain = false;
            }
        }

        private void AddCategory()
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
               new TextPrompt<bool>($"[magenta]Add category with the name: [/][cyan]{newCategory}[/][magenta] and description: [/][cyan]{newCategoryDescription}[/]?")
                   .AddChoice(true)
                   .AddChoice(false)
                   .DefaultValue(false)
                   .WithConverter(choice => choice ? "y" : "n"));

            if (confirm)
            {
                try
                {
                    Startup.CategoryRepository.AddEntity(categoryEntity);
                    Startup.CategoryRepository.SaveChanges();

                    Startup.Categories = (List<Category>)Startup.CategoryRepository.GetAll();
                    Startup.CategoryNames = Startup.GetCategoryNames();
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

        public void ShowCategory(Category category)
        {
            bool tryAgain = true;

            while (tryAgain)
            {
                var moreOnCategory = new Dictionary<string, Func<Category, bool>>
                {
                    {"edit category name", EditCategoryName },
                    {"edit category description", EditCategoryDescription },
                    {"remove category", RemoveCategory},
                };

                AnsiConsole.Clear();

                AnsiConsole.MarkupLine("[underline magenta]Category[/]");
                AnsiConsole.MarkupLine($"[magenta]Name: - [/][cyan]{category.CategoryName}[/]");
                AnsiConsole.MarkupLine($"[magenta]Description: - [/][cyan]{category.CategoryDescription}[/]");
                AnsiConsole.MarkupLine($"[magenta]Extensions: [/]");
                foreach (var item in category.Extensions)
                {
                    AnsiConsole.MarkupLine($"\t[magenta]- [/][cyan]{item.ExtensionName}[/]");
                }

                Console.WriteLine();
                Console.WriteLine();

                HashSet<string> moreOptions = new HashSet<string>();

                foreach (var option in moreOnCategory.Keys)
                {
                    moreOptions.Add(option);
                }
                moreOptions.Add(BackMessage);

                var userChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .AddChoices(moreOptions));

                if (userChoice == BackMessage)
                    tryAgain = false;
                else
                {
                    moreOnCategory[userChoice](category);
                }
            }
        }

        private bool EditCategoryName(Category categoryEdit)
        {
            var updateCategoryName = AnsiConsole.Prompt(new TextPrompt<string>("[magenta]Enter a new category name[/]"));

            bool confirmName = AnsiConsole.Prompt(
               new TextPrompt<bool>($"[magenta]Update category name from: [/][cyan]{categoryEdit.CategoryName}[/][magenta] to [/][cyan]{updateCategoryName}[/]?")
                   .AddChoice(true)
                   .AddChoice(false)
                   .DefaultValue(false)
                   .WithConverter(choice => choice ? "y" : "n"));

            if (confirmName)
            {
                try
                {
                    categoryEdit.CategoryName = updateCategoryName;

                    Startup.CategoryRepository.UpdateEntity(categoryEdit);
                    Startup.CategoryRepository.SaveChanges();

                    AnsiConsole.MarkupLine($"[green]Category name successfully updated to [/][cyan]{categoryEdit.CategoryName}[/]");

                    //Startup.Categories = (List<Category>)Startup.CategoryRepository.GetAll();
                    //Startup.CategoryNames = Startup.GetCategoryNames();
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

        private bool EditCategoryDescription(Category categoryEdit)
        {
            var updateCategoryDescription = AnsiConsole.Prompt(new TextPrompt<string?>("[magenta]Enter a new category description[/]"));
            bool confirmDescription = AnsiConsole.Prompt(
               new TextPrompt<bool>($"[magenta]Update category description from: [/][cyan]{categoryEdit.CategoryDescription}[/][magenta] to [/][cyan]{updateCategoryDescription}[/]?")
                   .AddChoice(true)
                   .AddChoice(false)
                   .DefaultValue(false)
                   .WithConverter(choice => choice ? "y" : "n"));

            if (confirmDescription)
            {
                try
                {
                    categoryEdit.CategoryDescription = updateCategoryDescription;

                    Startup.CategoryRepository.UpdateEntity(categoryEdit);
                    Startup.CategoryRepository.SaveChanges();

                    AnsiConsole.MarkupLine($"[green]Category description successfully updated to [/][cyan]{categoryEdit.CategoryDescription}[/]");

                    Startup.Categories = (List<Category>)Startup.CategoryRepository.GetAll();
                    Startup.CategoryNames = Startup.GetCategoryNames();
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

        private bool RemoveCategory(Category categoryDelete)
        {
            bool confirm = AnsiConsole.Prompt(
                new TextPrompt<bool>($"Remove category: {categoryDelete.CategoryName}?")
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(true)
                    .WithConverter(choice => choice ? "y" : "n"));

            if (confirm)
            {
                try
                {
                    Startup.CategoryRepository.DeleteEntity(categoryDelete.Id);
                    Startup.CategoryRepository.SaveChanges();

                    AnsiConsole.MarkupLine($"[red]Removed category [/][cyan]{categoryDelete.CategoryName}[/]");

                    //Startup.Categories = (List<Category>)Startup.CategoryRepository.GetAll();
                    //Startup.CategoryNames = Startup.GetCategoryNames();
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

        public void CheckExtensions()
        {
            var miniFunctions = new Dictionary<string, Action>()
            {
                {"show extensions", ShowExtensions },
                {"add extension", AddExtension },
                {BackMessage, () => { } }
            };

            bool keepGoing = true;

            while (keepGoing)
            {
                keepGoing = RunOptions(miniFunctions, "EXTENSIONS");
            }
        }

        private void ShowExtensions()
        {
            bool tryAgain = true;

            List<string> extensions = new List<string>();
            foreach (var extension in Startup.Extensions)
                extensions.Add(extension.ExtensionName);

            extensions.Add(BackMessage);

            while (tryAgain)
            {
                AnsiConsole.Clear();
                var pickedExtension = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Pick an extension for further options: ")
                    .AddChoices(extensions));


                if (pickedExtension != BackMessage)
                {
                    var extension = Startup.Extensions.FirstOrDefault(c => c.ExtensionName == pickedExtension);

                    ShowExtension(extension!);

                    AnsiConsole.MarkupLine("[yellow]Press <Enter> to continue.... [/]");
                    Console.ReadLine();
                }
                else
                    tryAgain = false;
            }
        }

        private void AddExtension()
        {
            var newExtension = AnsiConsole.Prompt(new TextPrompt<string>("[magenta]Enter the name for a new extension: [/]"));

            List<string> categories = new List<string>();
            foreach (var name in Startup.CategoryNames)
            {
                categories.Add(name);
            }
            categories.Add(BackMessage);

            var categoryPick = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[magenta]Pick the category for the extension: [/]")
                .AddChoices(Startup.CategoryNames));

            bool confirm = AnsiConsole.Prompt(
               new TextPrompt<bool>($"[magenta]Add extension with the name: [/][cyan]{newExtension}[/][magenta] and in category: [/][cyan]{categoryPick}[/]?")
                   .AddChoice(true)
                   .AddChoice(false)
                   .DefaultValue(false)
                   .WithConverter(choice => choice ? "y" : "n"));

            if (confirm)
            {
                var extension = new Extension()
                {
                    ExtensionName = newExtension
                };
                var category = Startup.Categories.FirstOrDefault(c => c.CategoryName == categoryPick);

                try
                {
                    Startup.ExtensionRepository.AddEntity(extension);
                    Startup.ExtensionRepository.SaveChanges();
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: [/][cyan]{ex.Message}[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]New extension not added[/]");
            }
        }

        private void ShowExtension(Extension extension)
        {
            bool tryAgain = true;

            while (tryAgain)
            {
                var moreOnExtension = new Dictionary<string, Func<Extension, bool>>
                {
                    {"edit extension name", EditExtensionName },
                    {"change category", ChangeExtensionCategory },
                    {"remove extension", RemoveExtension},
                };

                AnsiConsole.Clear();

                AnsiConsole.MarkupLine("[underline magenta]Extension[/]");
                AnsiConsole.MarkupLine($"[magenta]Name: - [/][cyan]{extension.ExtensionName}[/]");
                AnsiConsole.MarkupLine($"[magenta]Category: - [/][cyan]{extension.Category.CategoryName}[/]");

                Console.WriteLine();
                Console.WriteLine();

                HashSet<string> moreOptions = new HashSet<string>();

                foreach (var option in moreOnExtension.Keys)
                {
                    moreOptions.Add(option);
                }
                moreOptions.Add(BackMessage);

                var userChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .AddChoices(moreOptions));

                if (userChoice == BackMessage)
                    tryAgain = false;
                else
                {
                    moreOnExtension[userChoice](extension);
                }
            }
        }

        public bool EditExtensionName(Extension extension)
        {
            var updateExtensionName = AnsiConsole.Prompt(new TextPrompt<string>("[magenta]Enter a new extension name[/]"));

            bool confirmName = AnsiConsole.Prompt(
               new TextPrompt<bool>($"[magenta]Update extension name from: [/][cyan]{extension.ExtensionName}[/][magenta] to [/][cyan]{updateExtensionName}[/]?")
                   .AddChoice(true)
                   .AddChoice(false)
                   .DefaultValue(false)
                   .WithConverter(choice => choice ? "y" : "n"));

            if (confirmName)
            {
                try
                {
                    extension.ExtensionName = updateExtensionName;

                    Startup.ExtensionRepository.UpdateEntity(extension);
                    Startup.ExtensionRepository.SaveChanges();

                    AnsiConsole.MarkupLine($"[green]Extension name successfully updated to [/][cyan]{extension.ExtensionName}[/]");

                    //Startup.Extensions = (List<Extension>)Startup.ExtensionRepository.GetAll();
                    //Startup.ExcludedExtensions = Startup.GetExcludedExtensions();
                    //Startup.ExtensionCategories = Startup.GetExtensionCategory();
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

        public bool ChangeExtensionCategory(Extension extension)
        {
            List<string> categories = new List<string>();
            foreach (var name in Startup.CategoryNames)
            {
                categories.Add(name);
            }
            categories.Add(BackMessage);
            categories.Remove(extension.Category.CategoryName);

            var categoryChange = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[magenta]Pick the category to change to: [/]")
                .AddChoices(categories));

            var confirm = AnsiConsole.Prompt(
               new TextPrompt<bool>($"[magenta]Update extension category from: [/][cyan]{extension.Category.CategoryName}[/][magenta] to [/][cyan]{categoryChange}[/]?")
                   .AddChoice(true)
                   .AddChoice(false)
                   .DefaultValue(false)
                   .WithConverter(choice => choice ? "y" : "n"));

            if (confirm)
            {
                var category = Startup.Categories.FirstOrDefault(c => c.CategoryName == categoryChange);
                try
                {
                    extension.CategoryId = category!.Id;
                    extension.Category = category!;

                    Startup.ExtensionRepository.UpdateEntity(extension);
                    Startup.ExtensionRepository.SaveChanges();

                    AnsiConsole.MarkupLine($"[green]Extension category successfully updated to [/][cyan]{extension.Category.CategoryName}[/]");
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

        public bool RemoveExtension(Extension extension)
        {
            bool confirm = AnsiConsole.Prompt(
                new TextPrompt<bool>($"Remove category: {extension.ExtensionName}?")
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(true)
                    .WithConverter(choice => choice ? "y" : "n"));

            if (confirm)
            {
                try
                {
                    Startup.ExtensionRepository.DeleteEntity(extension.Id);
                    Startup.ExtensionRepository.SaveChanges();

                    AnsiConsole.MarkupLine($"[red]Removed extension [/][cyan]{extension.ExtensionName}[/]");

                    //Startup.Categories = (List<Category>)Startup.CategoryRepository.GetAll();
                    //Startup.CategoryNames = Startup.GetCategoryNames();
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: [/][cyan]{ex.Message}[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]Delete canceled[/]");
            }

            return true;
        }

        public void CheckIssues()
        {
            // deal with failed moves for files
            // unrecognised file extensions 
        }
    }
}
