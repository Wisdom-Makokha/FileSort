using FileSort.Display.Interfaces;
using FileSort.Services;
using FileSort.Settings;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Display.Managers
{
    internal class FolderManager : IFolderManager
    {
        private readonly AppSettings _settings;
        private readonly IConfigurationService _configurationService;
        private string _sourceFolderStr = "Source folder";
        private string _destinationFolderStr = "Destination folder";

        public FolderManager(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
            _settings = _configurationService.AppSettings;
        }

        public void CheckSourceFolder()
        {
            CheckFolder(IConfigurationService.FolderType.Source);
        }

        public void CheckDestinationFolder()
        {
            CheckFolder(IConfigurationService.FolderType.Destination);
        }
        
        private void CheckFolder(IConfigurationService.FolderType folderType)
        {
            string interfaceName;
            string folderName;
            if (folderType == IConfigurationService.FolderType.Destination)
            {
                interfaceName = _destinationFolderStr;
                folderName = _settings.SourceFolder;
            }
            else
            {
                interfaceName = _sourceFolderStr;
                folderName = _settings.DestinationFolder;
            }

            var options = new List<string> { "edit", MainInterface.BackMessage };
            bool KeepGoing = true;

            while (KeepGoing)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"[underline silver]{interfaceName.ToUpper()}[/]");
                AnsiConsole.MarkupLine($"[green]{interfaceName} path - [/][cyan]{folderName}[/]\n\n");

                string userChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[magenta]Pick one of these options: [/]")
                        .AddChoices(options));

                if (userChoice != MainInterface.BackMessage)
                {
                    EditFolder(folderType, interfaceName, folderName);
                }
                else
                {
                    KeepGoing = false;
                }
            }
        }

        private bool EditFolder(IConfigurationService.FolderType folderType, string interfaceName, string folderName)
        {
            AnsiConsole.Clear();

            Console.WriteLine();
            var newFolderPath = AnsiConsole.Prompt(
                new TextPrompt<string>($"[magenta]Enter the new {interfaceName.ToLower()} full path: [/]")
                    .DefaultValue(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)));

            bool confirm = AnsiConsole.Prompt(
               new TextPrompt<bool>($"[magenta]Update {interfaceName.ToLower()} path from: [/][cyan]{folderName}[/][magenta] to [/][cyan]{newFolderPath}[/]?")
                   .AddChoice(true)
                   .AddChoice(false)
                   .DefaultValue(false)
                   .WithConverter(choice => choice ? "y" : "n"));

            if (confirm)
            {
                if (Directory.Exists(newFolderPath))
                {
                    _configurationService.UpdateFolderPath(newFolderPath, folderType);
                    _configurationService.ReloadAppSettings();
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

            return true;
        }
    }
}
