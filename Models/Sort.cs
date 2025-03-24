using FileSort.DataModels;
using FileSort.Display;
using FileSort.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Models
{
    internal class Sort
    {
        public Sort(SourceDirectory sourceDirectory, DestinationDirectory destinationDirectory, Startup startup, ApplicationInstance instance)
        {
            SourceDirectory = sourceDirectory ?? throw new ArgumentNullException(nameof(sourceDirectory), $"{nameof(sourceDirectory)} cannot be null in {nameof(Sort)} initialization");
            DestinationDirectory = destinationDirectory ?? throw new ArgumentNullException(nameof(destinationDirectory), $"{nameof(destinationDirectory)} cannot be null in {nameof(Sort)} initialization");
            Startup = startup ?? throw new ArgumentNullException(nameof(startup), $"{nameof(startup)} cannot be null in {nameof(Sort)} initialization");
            Instance = instance ?? throw new ArgumentNullException(nameof(instance), $"{nameof(instance)} cannot be null in {nameof(Sort)} initialization");
        }

        private SourceDirectory SourceDirectory { get; set; }
        private DestinationDirectory DestinationDirectory { get; set; }
        private Startup Startup { get; set; }
        private ApplicationInstance Instance { get; set; }

        // this dictionary keeps track of the location of a file
        // true if at destination and sorted
        // false if still at source
        private Dictionary<string, string> MovedFiles = new Dictionary<string, string>();

        private bool SortedFlag = false;

        // method dedicated to moving a file
        private void MoveFile(string file, string destination)
        {
            try
            {
                File.Move(file, destination);

                SpecialPrinting.PrintColored(
                    $"\tMoved\n\t - {file} to {destination}",
                    ConsoleColor.Green,
                    file, destination);
            }
            catch (Exception ex)
            {
                SpecialPrinting.PrintColored(
                    $"\tError moving file: {file}\n\tto {destination}",
                    ConsoleColor.Red,
                    file, destination);
                SpecialPrinting.PrintColored(ex.Message, ConsoleColor.Red);
            }
        }

        // sort the files into their categories
        public void SortFiles()
        {
            SpecialPrinting.PrintColored(
                "Sorting files... ",
                ConsoleColor.Yellow);

            string subDirectory;

            if (SourceDirectory.SourceFiles.Count > 0)
            {
                foreach (var file in SourceDirectory.SourceFiles)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    
                    var extension = fileInfo.Extension;

                    if (Startup.ExcludedExtensions.Contains(extension))
                    {
                        SpecialPrinting.PrintColored($"Skipped file: {fileInfo.Name} with excluded extension: {extension}\nFile not moved", ConsoleColor.Yellow, fileInfo.Name, extension);

                        continue;
                    }
                    else if (Startup.ExtensionCategories!.TryGetValue(extension, out string? value)) { subDirectory = value; }
                    else
                    {
                        subDirectory = "other";
                    }

                    FileDataModel fileDataModel = new FileDataModel()
                    {
                        FileName = Path.GetFileNameWithoutExtension(fileInfo.FullName),
                        ExtensionId = Startup.Extensions.Single(e => e.ExtensionName == extension).Id,
                        CategoryId = Startup.Categories.Single(c => c.CategoryName == subDirectory).Id,
                        SourceFolderPath = SourceDirectory.DirectoryPath,
                        DestinationFolderPath = DestinationDirectory.DirectoryPath,
                        ApplicationInstanceId = Instance.ApplicationId,
                        IsSorted = true,
                    };

                    Startup.FileDataModelRepository.AddEntity(fileDataModel);
                    Startup.FileDataModelRepository.SaveChanges();

                    string destination = Path.Combine(Path.Combine(DestinationDirectory.DirectoryPath, subDirectory), Path.GetFileName(file));
                    MoveFile(file, destination);
                    MovedFiles.Add(fileInfo.FullName, destination);
                }

                SortedFlag = true;
            }
            else
            {
                SpecialPrinting.PrintColored(
                    "No files to sort",
                    ConsoleColor.DarkYellow);
            }

            Console.WriteLine("\n");
        }

        // reverse the move for the files
        public void ReverseSort()
        {
            if (SortedFlag)
            {
                SortedFlag = false;

                SpecialPrinting.PrintColored(
                    "Reversing files sort... ",
                    ConsoleColor.Yellow);

                foreach (var destination in MovedFiles.Keys)
                {
                    FileInfo fileInfo = new FileInfo(destination);
                    FileDataModel fileDataModel = Startup.FileDataModelRepository
                        .GetByFileNameAndExtension(Path.GetFileNameWithoutExtension(destination), fileInfo.Extension)!;

                    fileDataModel.IsSorted = false;
                    Startup.FileDataModelRepository.UpdateEntity(fileDataModel);
                    Startup.FileDataModelRepository .SaveChanges();

                    MoveFile(MovedFiles[destination], destination);
                    MovedFiles.Remove(destination);
                }

                Console.WriteLine("\n");
            }
            else
            {
                SpecialPrinting.PrintColored(
                    "Files not sorted yet!",
                    ConsoleColor.DarkYellow);
            }
        }
    }
}
