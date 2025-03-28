using FileSort.DataModels;
using FileSort.Models;
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
        public MainInterface(Startup startup) 
        {
            Startup = startup;
        }

        public Startup Startup { get; set; }

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

        protected bool RunOptions(Dictionary<string, Action> options)
        {
            if (!options.ContainsKey(ExitMessage))
                throw new ArgumentException($"{nameof(ExitMessage)} have invalid values in {nameof(RunOptions)} method");

            AnsiConsole.Clear();
            string userChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[magenta]Pick one of these options: [/]")
                    .MoreChoicesText("[grey]Scroll using the arrow keys then click <Enter>[/]")
                    .AddChoices(options.Keys));

            //AnsiConsole.MarkupLine($"[cyan]{userChoice}[/]");
            if (userChoice == ExitMessage)
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
                {"reverse sort", ReverseSort },
                {"sort history", CheckSortHistory },
                {"settings", CheckSettings },
                {"issue", CheckIssues },
                {ExitMessage, ()=> { } }
            };

            bool keepGoing = true;

            while (keepGoing)
            {
                keepGoing = RunOptions(InterfaceOptions);
            }
        }

        public void SortFiles()
        {
            SourceDirectory sourceDirectory = new SourceDirectory(Startup.ExcludedExtensions, Startup.AppSettings.SourceFolder);
            DestinationDirectory destinationDirectory = new DestinationDirectory(Startup.CategoryNames, Startup.AppSettings.DestinationFolder);
            Sort sort = new Sort(sourceDirectory, destinationDirectory, Startup);
        }
    }
}
