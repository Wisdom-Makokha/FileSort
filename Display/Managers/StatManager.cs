using FileSort.Data.Interfaces;
using FileSort.Data.Repositories;
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
    internal class StatManager : IStatManager
    {
        private List<Category> _categories;
        private Guid _applicationInstanceId;
        private IApplicationInstanceRepository _applicationInstanceRepository;

        public StatManager(ICategoryRepository categoryRepository, IApplicationInstanceRepository applicationInstanceRepository)
        {
            _categories = categoryRepository.GetAll().ToList();
            _applicationInstanceId = applicationInstanceRepository
                                        .GetAll().ToList()
                                        .OrderByDescending(i => i.InitiationTime)
                                        .FirstOrDefault()!.ApplicationId;
            _applicationInstanceRepository = applicationInstanceRepository;
        }

        public void CheckSortHistory()
        {
            Dictionary<string, Action> miniFunctions = new Dictionary<string, Action>()
            {
                {"show most recent stats", ShowRecentStats },
                {"select application instances", SelectApplicationInstanceStats},
                {"show combined stats", ShowCombinedStats },
                {MainInterface.BackMessage, () => { } }
            };

            bool keepGoing = true;

            while (keepGoing)
            {
                AnsiConsole.Clear();
                keepGoing = MainInterface.RunOptions(miniFunctions, "SORT HISTORY");
            }
        }

        public void ShowRecentStats()
        {
            ShowApplicationStats(_applicationInstanceId);
        }

        public void SelectApplicationInstanceStats()
        {
            List<string> applicationInstances = new List<string>();
            foreach (var instance in _applicationInstanceRepository.GetAll().Where(i => i.Files.Count > 0))
            { applicationInstances.Add(instance.ApplicationId.ToString()); }

            applicationInstances.Add(MainInterface.BackMessage);

            bool tryAgain = true;

            while (tryAgain)
            {
                AnsiConsole.Clear();
                var userPick = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select an application instance to view the statistics: ")
                .AddChoices(applicationInstances));

                if (userPick != MainInterface.BackMessage)
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

        public void ShowApplicationStats(Guid? applicationInstanceId)
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

            foreach (var category in _categories)
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
    }
}
