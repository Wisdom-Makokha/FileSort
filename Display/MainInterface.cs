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
        protected static string ExitMessage
        {
            get
            { return "exit"; }
        }
        protected static string BackMessage
        {
            get
            { return "back"; }
        }

        protected bool RunOptions(Dictionary<string, Action> options, string interfaceName)
        {
            if (!options.ContainsKey(ExitMessage) || !options.ContainsKey(BackMessage))
                throw new ArgumentException($"Value of {nameof(ExitMessage)} or {nameof(BackMessage)} is invalid in {nameof(RunOptions)} method");

            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"[underline silver]{interfaceName}[/]");

            string userChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[magenta]Pick one of these options: [/]")
                    .AddChoices(options.Keys));

            //AnsiConsole.MarkupLine($"[cyan]{userChoice}[/]");
            if (userChoice == ExitMessage || userChoice == BackMessage)
                return false;
            else
            {
                options[userChoice]();

                AnsiConsole.MarkupLine("[yellow]Press <Enter> to continue.... [/]");
                //SpecialPrinting.PrintColored("Press <Enter> to continue.... ", ConsoleColor.Yellow);
                Console.ReadLine();

                return true;
            }
        }

        public void Home()
        {
            Dictionary<string, Action> InterfaceOptions = new Dictionary<string, Action>()
            {
                {"sort", SortFiles },
                {"reverse recent sort", ReverseSort },
                {"sort history", CheckSortHistory },
                {"settings", CheckSettings },
                {"issue", CheckIssues },
                {ExitMessage, ()=> { } }
            };

            bool keepGoing = true;

            while (keepGoing)
            {
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

        public void CheckSettings()
        {
            Dictionary<string, Action> InterfaceOptions = new Dictionary<string, Action>()
            {
                {"source folder", CheckSourceFolder },
                {"destination folder", CheckDestinationFolder },
                {"categories", CheckCategories },
                {"extensions", CheckExtensions },
                {BackMessage, () => { } }
            };

            bool keepGoing = true;

            while (keepGoing)
            {
                keepGoing = RunOptions(InterfaceOptions, "SETTINGS");
            }
        }

        public void CheckSourceFolder()
        {
            Dictionary<string, Action> miniFunctions = new Dictionary<string, Action>
            {
                {"show", ShowSourceFolder },
                {"set", SetSourceFolder },
                {BackMessage, () => { } }
            };

            bool KeepGoing = true;
            while (KeepGoing)
            {
                KeepGoing = RunOptions(miniFunctions, "SOURCE FOLDER");
            }
        }

        private void ShowSourceFolder()
        {
            AnsiConsole.MarkupLine($"[green]Source folder - [/][cyan]{Startup.AppSettings.SourceFolder}[/]");
        }

        private void SetSourceFolder()
        {
            bool tryAgain = true;

            while (tryAgain)
            {
                AnsiConsole.Clear();
                ShowSourceFolder();

                Console.WriteLine();
                var newFolderPath = AnsiConsole.Prompt(
                    new TextPrompt<string>("[magenta]Enter the new source folder full path: [/]")
                        .DefaultValue(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)));

                if (Directory.Exists(newFolderPath))
                {
                    SettingsConfigurationHelper.UpdateFolderPath(newFolderPath, SettingsConfigurationHelper.FolderType.Source);
                    Startup.GetAppSettings();

                    ShowSourceFolder();

                    tryAgain = false;
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Invalid folder path given[/]");
                }
            }
        }

        public void CheckDestinationFolder()
        {
            Dictionary<string, Action> miniFunctions = new Dictionary<string, Action>
            {
                {"show", ShowDestinationFolder },
                {"set", SetDestinationFolder },
                {BackMessage, () => { } }
            };

            bool KeepGoing = true;
            while (KeepGoing)
            {
                KeepGoing = RunOptions(miniFunctions, "DESTINATION FOLDER");
            }
        }

        private void ShowDestinationFolder()
        {
            AnsiConsole.MarkupLine($"[green]Destination folder - [/][cyan]{Startup.AppSettings.DestinationFolder}[/]");
        }

        private void SetDestinationFolder()
        {
            bool tryAgain = true;

            while (tryAgain)
            {
                AnsiConsole.Clear();
                ShowDestinationFolder();

                Console.WriteLine();
                var newFolderPath = AnsiConsole.Prompt(
                    new TextPrompt<string>("[magenta]Enter the new destination folder full path: [/]"));

                if (Directory.Exists(newFolderPath))
                {
                    SettingsConfigurationHelper.UpdateFolderPath(newFolderPath, SettingsConfigurationHelper.FolderType.Destination);
                    Startup.GetAppSettings();

                    ShowDestinationFolder();

                    tryAgain = false;
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Invalid folder path given[/]");
                }
            }
        }

        public void CheckCategories()
        {
            Dictionary<string, Action> miniOptions = new Dictionary<string, Action>()
            {
                {"show categories", ShowCategories },
                {"add category", AddCategory },
                //{"edit category", EditCategory },
                //{"remove category", RemoveCategory},
                {BackMessage, () => { } }
            };

            bool keepGoing = true;
            while (!keepGoing)
            {
                keepGoing = RunOptions(miniOptions, "CATEGORIES");
            }
        }

        private void ShowCategories()
        {
            //AnsiConsole.MarkupLine("[magenta]Categories: -[/]");
            //for (int i = 0; i < Startup.CategoryNames.Count; i++)
            //{
            //    AnsiConsole.MarkupLine($"[magenta]{i}{".",3}[/][green]{Startup.CategoryNames[i]}[/]");
            //}


        }

        private void AddCategory()
        {
            bool tryAgain = true;

            while (tryAgain)
            {
                AnsiConsole.Clear();
                ShowCategories();
                Console.WriteLine();

                var newCategory = AnsiConsole.Prompt(new TextPrompt<string>("[magenta]Enter a name for a new category: [/]"));
                var newCategoryDescription = AnsiConsole.Prompt(new TextPrompt<string?>("[magenta]Enter a description for the new category: [/]"));

                try
                {
                    Category categoryEntity = new Category()
                    {
                        CategoryName = newCategory,
                        CategoryDescription = newCategoryDescription
                    };

                    Startup.CategoryRepository.AddEntity(categoryEntity);
                    Startup.CategoryRepository.SaveChanges();

                    Startup.Categories = (List<Category>)Startup.CategoryRepository.GetAll();
                    Startup.CategoryNames = Startup.GetCategoryNames();

                    ShowCategories();

                    tryAgain = false;
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: [/][cyan]{ex.Message}[/]");
                }
            }
        }

        public void EditCategory()
        {
            var pickedCategoryStr = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[magenta]Pick a category to edit: - [/]")
                    .AddChoices(Startup.CategoryNames));

            Category category = Startup.Categories.FirstOrDefault(c => c.CategoryName == pickedCategoryStr)!;

            var updateCategoryName = AnsiConsole.Prompt(new TextPrompt<string>("[magenta]Enter a new category name[/]"));
            var updateCategoryDescription = AnsiConsole.Prompt(new TextPrompt<string>("[magenta]Enter a new category description[/]"));

            bool confirm = AnsiConsole.Prompt(
               new TextPrompt<bool>($"[magenta]Update category name from: [/][cyan]{category.CategoryName}[/][magenta] to [/][cyan]{pickedCategoryStr}[/]?")
                   .AddChoice(true)
                   .AddChoice(false)
                   .DefaultValue(false)
                   .WithConverter(choice => choice ? "y" : "n"));

            if (confirm)
            {
                category.CategoryName = pickedCategoryStr;

                Startup.CategoryRepository.UpdateEntity(category);
                Startup.CategoryRepository.SaveChanges();

                AnsiConsole.MarkupLine($"[green]Category name successfully updated to [/][cyan]{pickedCategoryStr}[/]");

                Startup.Categories = (List<Category>)Startup.CategoryRepository.GetAll();
                Startup.CategoryNames = Startup.GetCategoryNames();
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Update canceled[/]");
            }

            ShowCategories();
        }

        public void RemoveCategory()
        {
            var pickedCategoryStr = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[magenta]Pick a category to remove: - [/]")
                    .AddChoices(Startup.CategoryNames));

            Category category = Startup.Categories.FirstOrDefault(c => c.CategoryName == pickedCategoryStr)!;

            bool confirm = AnsiConsole.Prompt(
                new TextPrompt<bool>($"Remove category: {category.CategoryName}?")
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(true)
                    .WithConverter(choice => choice ? "y" : "n"));

            if (confirm)
            {
                category.CategoryName = pickedCategoryStr;

                Startup.CategoryRepository.DeleteEntity(category.Id);
                Startup.CategoryRepository.SaveChanges();

                AnsiConsole.MarkupLine($"[red]Removed category [/][cyan]{pickedCategoryStr}[/]");

                Startup.Categories = (List<Category>)Startup.CategoryRepository.GetAll();
                Startup.CategoryNames = Startup.GetCategoryNames();
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]Delete canceled[/]");
            }

            ShowCategories();
        }

        public void CheckExtensions()
        {

        }

        public void CheckIssues()
        {

        }
    }
}
