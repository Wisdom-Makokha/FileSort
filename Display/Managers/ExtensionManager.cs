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
    internal class ExtensionManager : IExtensionManager
    {
        private readonly IExtensionRepository _extensionRepository;
        private readonly List<Extension> _extensions;
        private readonly List<string> _extensionNames;
        private readonly List<Category> _categories;

        public ExtensionManager(IExtensionRepository extensionRepository, List<Category> categories)
        {
            _extensionRepository = extensionRepository;
            _extensions = (List<Extension>)_extensionRepository.GetAll();
            _extensionNames = _extensions.Select(e => e.ExtensionName).ToList();
            _categories = categories;
        }

        public void ShowExtensions()
        {
            bool tryAgain = true;
            var extensions = new List<string>(_extensionNames) { MainInterface.BackMessage };

            while (tryAgain)
            {
                AnsiConsole.Clear();
                var pickedExtension = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Pick an extension for further options: ")
                    .PageSize(20)
                    .AddChoices(extensions));


                if (pickedExtension != MainInterface.BackMessage)
                {
                    var extension = _extensions.FirstOrDefault(c => c.ExtensionName == pickedExtension);

                    ShowExtension(extension!);

                    AnsiConsole.MarkupLine("[yellow]Press <Enter> to continue.... [/]");
                    Console.ReadLine();
                }
                else
                    tryAgain = false;
            }
        }

        public void AddExtension()
        {
            var newExtension = AnsiConsole.Prompt(new TextPrompt<string>("[magenta]Enter the name for a new extension: [/]"));

            var categoryNames = _categories.Select(c => c.CategoryName).ToList();
            categoryNames.Add(MainInterface.BackMessage);

            var categoryPick = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[magenta]Pick the category for the extension: [/]")
                .AddChoices(categoryNames));

            bool confirm = AnsiConsole.Prompt(
               new TextPrompt<bool>($"[magenta]Add extension with the name: [/][cyan]{newExtension}[/][magenta] and in category: [/][cyan]{categoryPick}[/]?")
                   .AddChoice(true)
                   .AddChoice(false)
                   .DefaultValue(false)
                   .WithConverter(choice => choice ? "y" : "n"));

            if (confirm)
            {
                if (!newExtension.StartsWith('.'))
                { newExtension = "." + newExtension; }

                var category = _categories.FirstOrDefault(c => c.CategoryName == categoryPick);
                var extension = new Extension()
                {
                    ExtensionName = newExtension,
                    CategoryId = category!.Id,
                    Category = category!,
                };

                try
                {
                    _extensionRepository.AddEntity(extension);
                    _extensionRepository.SaveChanges();
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

        public void ShowExtension(Extension extension)
        {
            bool tryAgain = true;
            var moreOnExtension = new Dictionary<string, Func<Extension, bool>>
            {
                {"edit extension name", EditExtensionName },
                {"change category", ChangeExtensionCategory },
                {"remove extension", RemoveExtension},
            };

            HashSet<string> moreOptions = new HashSet<string>(moreOnExtension.Keys) { MainInterface.BackMessage };

            while (tryAgain)
            {
                AnsiConsole.Clear();

                AnsiConsole.MarkupLine("[underline magenta]Extension[/]");
                AnsiConsole.MarkupLine($"[magenta]Name: - [/][cyan]{extension.ExtensionName}[/]");
                AnsiConsole.MarkupLine($"[magenta]Category: - [/][cyan]{extension.Category.CategoryName}[/]");

                Console.WriteLine();
                Console.WriteLine();


                var userChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .AddChoices(moreOptions));

                if (userChoice == MainInterface.BackMessage)
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

                    _extensionRepository.UpdateEntity(extension);
                    _extensionRepository.SaveChanges();
                    _extensions.FirstOrDefault(extension => extension.Id == extension.Id)!.ExtensionName = updateExtensionName;

                    AnsiConsole.MarkupLine($"[green]Extension name successfully updated to [/][cyan]{extension.ExtensionName}[/]");
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
            var categoryNames = _categories.Select(c => c.CategoryName).ToList();
            categoryNames.Add(MainInterface.BackMessage);

            categoryNames.Remove(extension.Category.CategoryName);

            var categoryChange = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[magenta]Pick the category to change to: [/]")
                .AddChoices(categoryNames)
                .PageSize(15));

            var confirm = AnsiConsole.Prompt(
               new TextPrompt<bool>($"[magenta]Update extension category from: [/][cyan]{extension.Category.CategoryName}[/][magenta] to [/][cyan]{categoryChange}[/]?")
                   .AddChoice(true)
                   .AddChoice(false)
                   .DefaultValue(false)
                   .WithConverter(choice => choice ? "y" : "n"));

            if (confirm)
            {
                var category = _categories.FirstOrDefault(c => c.CategoryName == categoryChange);
                try
                {
                    extension.CategoryId = category!.Id;
                    extension.Category = category!;
                    
                    _extensionRepository.UpdateEntity(extension);
                    _extensionRepository.SaveChanges();
                    _extensions.FirstOrDefault(e => e.Id == extension.Id)!.CategoryId = category.Id;
                    _extensions.FirstOrDefault(e => e.Id == extension.Id)!.Category = category!;

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
                new TextPrompt<bool>($"Are your sure you want to remove extension: {extension.ExtensionName}?")
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(true)
                    .WithConverter(choice => choice ? "y" : "n"));

            if (confirm)
            {
                try
                {
                    _extensionRepository.DeleteEntity(extension.Id);
                    _extensionRepository.SaveChanges();
                    _extensions.Remove(extension);

                    AnsiConsole.MarkupLine($"[red]Removed extension [/][cyan]{extension.ExtensionName}[/]");
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
    }
}
