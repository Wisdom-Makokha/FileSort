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
using FileSort.Display.Interfaces;

namespace FileSort.Display
{
    internal class MainInterface
    {
        private readonly ICategoryManager _categoryManager;
        private readonly IExtensionManager _extensionManager;
        private readonly IFolderManager _folderManager;
        private readonly IStatManager _statManager;
        private readonly ISortManager _sortManager;
        private readonly IIssuesManager _issuesManager;

        public MainInterface(
            ICategoryManager categoryManager,
            IExtensionManager extensionManager,
            IFolderManager folderManager,
            IStatManager statManager,
            ISortManager sortManager,
            IIssuesManager issuesManager)
        {
            _categoryManager = categoryManager;
            _extensionManager = extensionManager;
            _folderManager = folderManager;
            _statManager = statManager;
            _sortManager = sortManager;
            _issuesManager = issuesManager;
        }

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
                {"sort", _sortManager.SortFiles },
                //{"reverse recent sort", ReverseSort },
                {"source folder", _folderManager.CheckSourceFolder },
                {"destination folder", _folderManager.CheckDestinationFolder },
                {"categories", _categoryManager.CheckCategories },
                {"extensions", _extensionManager.CheckExtensions },
                {"sort history", _statManager.CheckSortHistory },
                {"issue", _issuesManager.CheckIssues },
                {ExitMessage, ()=> { } }
            };

            bool keepGoing = true;

            while (keepGoing)
            {
                AnsiConsole.Clear();
                keepGoing = RunOptions(InterfaceOptions, "HOME");
            }
        }
    }
}
