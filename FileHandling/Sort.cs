using FileSort.DataModels;
using FileSort.AppDirectory;
using FileSort.Display;
using FileSort.Startup;
using FileSort.Repositories;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.FileHandling
{
    internal class Sort : BaseFileHandling
    {
        public Sort(SourceDirectory sourceDirectory, DestinationDirectory destinationDirectory, Startup.Startup startup)
            : base(startup.FailedMovesRepository)
        {
            SourceDirectory = sourceDirectory ?? throw new ArgumentNullException(nameof(sourceDirectory), $"{nameof(sourceDirectory)} cannot be null in {nameof(Sort)} initialization");
            DestinationDirectory = destinationDirectory ?? throw new ArgumentNullException(nameof(destinationDirectory), $"{nameof(destinationDirectory)} cannot be null in {nameof(Sort)} initialization");
            Startup = startup ?? throw new ArgumentNullException(nameof(startup), $"{nameof(startup)} cannot be null in {nameof(Sort)} initialization");
        }

        private SourceDirectory SourceDirectory { get; set; }
        private DestinationDirectory DestinationDirectory { get; set; }
        private Startup.Startup Startup { get; set; }

        // sort the files into their categories
        public void SortFiles()
        {
            AnsiConsole.MarkupLine("[yellow]Sorting files... [/]");

            string subDirectory;

            if (SourceDirectory.SourceFiles.Count > 0)
            {
                foreach (var file in SourceDirectory.SourceFiles)
                {
                    FileInfo fileInfo = new FileInfo(file);

                    var extension = fileInfo.Extension;

                    if (Startup.ExcludedExtensions.Contains(extension))
                    {
                        AnsiConsole.MarkupLine($"[yellow]Skipped file: [/][cyan]{fileInfo.Name}[/][yellow] with excluded extension: [/][cyan]{extension}[/][yellow]\nFile not moved[/]");

                        continue;
                    }
                    else if (Startup.ExtensionCategories!.TryGetValue(extension, out string? value)) { subDirectory = value; }
                    else
                    {
                        subDirectory = "other";
                        Extension newExtension = new Extension()
                        {
                            ExtensionName = extension,
                            CategoryId = Startup.Categories.Single(c => c.CategoryName == subDirectory).Id
                        };

                        Startup.ExtensionRepository.AddEntity(newExtension);
                        Startup.ExtensionRepository.SaveChanges();
                    }

                    FileDataModel fileDataModel = new FileDataModel()
                    {
                        FileName = Path.GetFileNameWithoutExtension(fileInfo.FullName),
                        ExtensionId = Startup.Extensions.Single(e => e.ExtensionName == extension).Id,
                        CategoryId = Startup.Categories.Single(c => c.CategoryName == subDirectory).Id,
                        SourceFolderPath = SourceDirectory.DirectoryPath,
                        DestinationFolderPath = DestinationDirectory.DirectoryPath,
                        ApplicationInstanceId = Startup.ApplicationInstance.ApplicationId,
                        IsSorted = true,
                    };

                    Startup.FileDataModelRepository.AddEntity(fileDataModel);
                    Startup.FileDataModelRepository.SaveChanges();

                    string destination = Path.Combine(Path.Combine(DestinationDirectory.DirectoryPath, subDirectory), Path.GetFileName(file));
                    MoveFile(file, destination);
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[olive]No files to sort[/]");
                //SpecialPrinting.PrintColored("No files to sort", ConsoleColor.DarkYellow);
            }

            AnsiConsole.WriteLine("\n");
        }

    }
}
