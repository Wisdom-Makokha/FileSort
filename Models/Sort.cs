using FileSort.DataModels;
using FileSort.Display;
using FileSort.Repositories;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Models
{
    internal class Sort
    {
        public Sort(SourceDirectory sourceDirectory, DestinationDirectory destinationDirectory, Startup startup)
        {
            SourceDirectory = sourceDirectory ?? throw new ArgumentNullException(nameof(sourceDirectory), $"{nameof(sourceDirectory)} cannot be null in {nameof(Sort)} initialization");
            DestinationDirectory = destinationDirectory ?? throw new ArgumentNullException(nameof(destinationDirectory), $"{nameof(destinationDirectory)} cannot be null in {nameof(Sort)} initialization");
            Startup = startup ?? throw new ArgumentNullException(nameof(startup), $"{nameof(startup)} cannot be null in {nameof(Sort)} initialization");
        }

        private SourceDirectory SourceDirectory { get; set; }
        private DestinationDirectory DestinationDirectory { get; set; }
        private Startup Startup { get; set; }

        // this dictionary keeps track of the location of a file
        // true if at destination and sorted
        // false if still at source

        // method dedicated to moving a file
        private void MoveFile(string file, string destination)
        {
            try
            {
                File.Move(file, destination);

                AnsiConsole.MarkupLine($"[green]\tMoved\n\t - [/][cyan]{file}[/][green] to [/][green]{destination}[/]");
                //SpecialPrinting.PrintColored($"\tMoved\n\t - {file} to {destination}", ConsoleColor.Green, file, destination);
            }
            catch (Exception ex)
            {
                FailedMoves failedMove = new FailedMoves()
                {
                    FileName = file,
                    SourceFolderPath = Path.GetDirectoryName(file)!,
                    DestinationFolderPath = destination,
                    FailureMessage = ex.Message,
                    IsResolved = false,

                };
                Startup.FailedMovesRepository.AddEntity(failedMove);

                Console.WriteLine();
                AnsiConsole.MarkupLine($"[red]\tError moving file: [/][cyan]{file}[/][red]\n\tto [/][cyan]{destination}[/]");
                //SpecialPrinting.PrintColored($"\tError moving file: {file}\n\tto {destination}", ConsoleColor.Red, file, destination);
                Console.WriteLine();
                //AnsiConsole.WriteException(ex);
                //SpecialPrinting.PrintColored(ex.Message, ConsoleColor.Red);
            }
        }

        // sort the files into their categories
        public void SortFiles()
        {
            AnsiConsole.MarkupLine("[yellow]Sorting files... [/]");
            //SpecialPrinting.PrintColored("Sorting files... ", ConsoleColor.Yellow);

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
                        //SpecialPrinting.PrintColored($"Skipped file: {fileInfo.Name} with excluded extension: {extension}\nFile not moved", ConsoleColor.Yellow, fileInfo.Name, extension);

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
                AnsiConsole.MarkupLine("[darkyellow]No files to sort[/]");
                //SpecialPrinting.PrintColored("No files to sort", ConsoleColor.DarkYellow);
            }

            AnsiConsole.WriteLine("\n");
        }

        // reverse the move for the files
        public void ReverseSort()
        {
            Dictionary<string, string> movedFiles = new Dictionary<string, string>();
            movedFiles = Startup.FileDataModelRepository.GetInstanceMovedFiles(Startup.ApplicationInstance.ApplicationId);

            if (movedFiles.Count > 0)
            {
                AnsiConsole.MarkupLine("[yellow]Reversing files sort... [/]");
                //SpecialPrinting.PrintColored("Reversing files sort... ", ConsoleColor.Yellow);

                foreach (var destination in movedFiles.Keys)
                {
                    FileInfo fileInfo = new FileInfo(destination);
                    FileDataModel fileDataModel = Startup.FileDataModelRepository
                        .GetByFileNameAndExtension(Path.GetFileNameWithoutExtension(destination), fileInfo.Extension)!;

                    fileDataModel.IsSorted = false;
                    Startup.FileDataModelRepository.UpdateEntity(fileDataModel);
                    Startup.FileDataModelRepository.SaveChanges();

                    MoveFile(movedFiles[destination], destination);
                    movedFiles.Remove(destination);
                }

                AnsiConsole.WriteLine("\n");
            }
            else
            {
                AnsiConsole.MarkupLine("[darkyellow]Files not sorted yet![/]");
            }
        }
    }
}
