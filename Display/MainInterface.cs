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
        public static string ExitMessage
        {
            get
            { return "EXIT"; }
        }
        public static string BackMessage
        {
            get
            { return "BACK"; }
        }

        public static bool RunOptions(Dictionary<string, Action> options, string interfaceName)
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
                {"source folder", CheckSourceFolder },
                {"destination folder", CheckDestinationFolder },
                {"categories", CheckCategories },
                {"extensions", CheckExtensions },
                {"sort history", CheckSortHistory },
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

        private void ReverseSort()
        {
            Reverse reverse = new Reverse(Startup.FailedMovesRepository, Startup.ApplicationInstance, Startup.FileDataModelRepository);

            reverse.ReverseSort();
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

        public void CheckSortHistory()
        {
            // show most recent stats
            // pick stats for any particular application instance to view
            // show most used categories

            Dictionary<string, Action> miniFunctions = new Dictionary<string, Action>()
            {
                {"show most recent stats", ShowRecentStats },
                {"select application instances", SelectApplicationInstance},
                {"show combined stats", ShowCombinedStats },
                {BackMessage, () => { } }
            };

            bool keepGoing = true;

            while (keepGoing)
            {
                AnsiConsole.Clear();
                keepGoing = RunOptions(miniFunctions, "SORT HISTORY");
            }
        }

        private void ShowApplicationStats(Guid? applicationInstanceId)
        {
            var borderColor = Color.Yellow;

            var categoriesTable = new Table();
            categoriesTable.Title("Stats for recent application run");
            categoriesTable.Border = TableBorder.AsciiDoubleHead;
            categoriesTable.Width = 50;
            categoriesTable.BorderColor(borderColor);
            //categoriesTable.ShowRowSeparators = true;

            categoriesTable.AddColumn("[olive]Category[/]");
            categoriesTable.AddColumn("[olive]Extension[/]");
            categoriesTable.AddColumn("[olive]File Count[/]");

            var startingStyle = new Style(Color.Cyan1, Color.Black);
            var proceedingStyles = new Style(Color.Grey70, Color.Black);

            foreach (var category in Startup.Categories)
            {
                string categoryName = category.CategoryName;

                foreach (var extension in category.Extensions)
                {
                    int fileCount = (applicationInstanceId == null) ? extension.Files.Count()
                        : extension.Files.Where(f => f.ApplicationInstanceId == applicationInstanceId).Count();
                    var newRow = new List<Text>
                    {
                        new Text(categoryName, categoryName == string.Empty ? proceedingStyles : startingStyle),
                        new Text(extension.ExtensionName, categoryName == string.Empty ? proceedingStyles : startingStyle),
                        new Text(fileCount.ToString(), categoryName == string.Empty ? proceedingStyles : startingStyle)
                    };

                    if (fileCount > 0)
                    {
                        categoriesTable.AddRow(newRow);

                        categoryName = string.Empty;
                    }
                }
            }

            if (categoriesTable.Rows.Count() > 0)
                AnsiConsole.Write(categoriesTable);
            else
                AnsiConsole.MarkupLine("[olive]No records to show[/]");
        }

        private void ShowRecentStats()
        {
            ShowApplicationStats(Startup.ApplicationInstance.ApplicationId);
        }

        private void SelectApplicationInstance()
        {
            List<string> applicationInstances = new List<string>();
            foreach (var instance in Startup.ApplicationInstanceRepository.GetAll().Where(i => i.Files.Count > 0))
            { applicationInstances.Add(instance.ApplicationId.ToString()); }
            applicationInstances.Add(BackMessage);

            bool tryAgain = true;

            while (tryAgain)
            {
                AnsiConsole.Clear();
                var userPick = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select an application instance to view the statistics: ")
                .AddChoices(applicationInstances));

                if (userPick != BackMessage)
                {
                    ShowApplicationStats(Guid.Parse(userPick));

                    AnsiConsole.MarkupLine("[yellow]Press <Enter> to continue.... [/]");
                    Console.ReadLine();
                }
                else
                    tryAgain = false;
            }
        }

        public void ShowCombinedStats()
        {
            ShowApplicationStats(null);
        }

        public void CheckIssues()
        {
            // deal with failed moves for files
            // unrecognised file extensions 
        }
    }
}
