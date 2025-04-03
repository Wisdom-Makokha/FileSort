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
    internal class IssuesManager : IIssuesManager
    {
        private readonly IFailedMovesRepository _failedMovesRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryManager _categoryManager;

        public IssuesManager(IFailedMovesRepository failedMovesRepository, ICategoryRepository categoryRepository, ICategoryManager categoryManager)
        {
            _failedMovesRepository = failedMovesRepository;
            _categoryRepository = categoryRepository;
            _categoryManager = categoryManager;
        }

        public void CheckIssues()
        {
            // AnsiConsole.MarkupLine("[olive]Interface not yet implemented[/]");
            // unrecognized extensions
            // failed move files
            var miniFunctions = new Dictionary<string, Action>
            {
                {"show unrecognized extensions", ShowUnrecognizedExtensions },
                {"show failed moves", ShowFailedMoves },
                {MainInterface.BackMessage, () => { } }
            };

            bool keepGoing = true;

            while (keepGoing)
            {
                keepGoing = MainInterface.RunOptions(miniFunctions, "ISSUES");
            }
        }

        private void ShowUnrecognizedExtensions()
        {
            var unrecognizedCategory = _categoryRepository.GetCategoryByName("other");

            if (unrecognizedCategory != null)
            {
                if (unrecognizedCategory.Extensions.Any())
                {
                    _categoryManager.ViewCategoryDetails(unrecognizedCategory);
                }
                else
                {
                    AnsiConsole.MarkupLine("[olive]No unrecognized extensions found\n\n[/]");

                    AnsiConsole.MarkupLine("[yellow]Press <Enter> to continue[/]");
                    Console.ReadLine();
                    AnsiConsole.Clear();
                }
            }
        }

        private void ShowFailedMoves()
        {
            var failedMoves = _failedMovesRepository.GetAll().ToList();
            var fileNames = failedMoves.Select(x => x.FileName).ToList();

            for (int i = 0; i < fileNames.Count; i++)
            {
                fileNames[i] = Path.GetFileName(fileNames[i]);
            }
            fileNames.Add(MainInterface.BackMessage);

            bool keepGoing = true;
            while (keepGoing)
            {
                AnsiConsole.Clear();
                var pickedFile = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Pick one of these files to view the reason it failed to move: ")
                        .PageSize(20)
                        .AddChoices(fileNames));

                if (pickedFile != MainInterface.BackMessage)
                {
                    var failedMove = failedMoves.FirstOrDefault(f => f.FileName == pickedFile);

                    ShowFailedMove(failedMove!);
                }
                else
                    keepGoing = false;
            }
        }

        private void ShowFailedMove(FailedMoves failedMoves)
        {

        }
    }
}
