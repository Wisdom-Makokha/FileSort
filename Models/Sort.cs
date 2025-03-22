using FileSort.DataModels;
using FileSort.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Models
{
    internal class Sort
    {
        private readonly FileDataModelRepository _repository;

        public Sort(SourceDirectory sourceDirectory, DestinationDirectory destinationDirectory, AppSettings appSettings, FileDataModelRepository fileDataModelRepository)
        {
            SourceDirectory = sourceDirectory ?? throw new ArgumentNullException(nameof(sourceDirectory), $"{nameof(sourceDirectory)} cannot be null in {nameof(Sort)} initialization");
            DestinationDirectory = destinationDirectory ?? throw new ArgumentNullException(nameof(destinationDirectory), $"{nameof(destinationDirectory)} cannot be null in {nameof(Sort)} initialization");
            AppSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings), $"{nameof(appSettings)} cannot be null in {nameof(Sort)} initialization");
            _repository = fileDataModelRepository ?? throw new ArgumentNullException(nameof(_repository), $"{nameof(_repository)} cannot be null in {nameof(Sort)} initialization");
        }

        public SourceDirectory SourceDirectory { get; set; }
        public DestinationDirectory DestinationDirectory { get; set; }
        public AppSettings AppSettings { get; set; }

        // this dictionary keeps track of the location of a file
        // true if at destination and sorted
        // false if still at source
        private Dictionary<string, string> MovedFiles = new Dictionary<string, string>();

        private bool SortedFlag = false;
        private bool ReversedFlag = false;

        // method dedicated to moving a file
        private void MoveFile(string file, string destination)
        {
            try
            {
                //File.Move(file, destination);

                SpecialPrinting.PrintColored(
                    $"Moved\n - {file} to {destination}",
                    ConsoleColor.Green,
                    file, destination);
            }
            catch (Exception ex)
            {
                SpecialPrinting.PrintColored(
                    $"Error moving file: {file}\nto {destination}",
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
            string destination;

            if (SourceDirectory.SourceFiles.Count > 0)
            {
                foreach (var file in SourceDirectory.SourceFiles)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    var extension = fileInfo.Extension;

                    if (AppSettings.ExcludedExtensions.Contains(extension))
                    {
                        SpecialPrinting.PrintColored($"Skipped file: {fileInfo.Name} with excluded extension: {extension}\nFile not moved", ConsoleColor.Yellow, fileInfo.Name, extension);

                        continue;
                    }
                    else if (AppSettings.ExtensionCategories!.TryGetValue(extension, out string? value)) { subDirectory = value; }
                    else
                    {
                        subDirectory = "Other";
                    }

                    destination = Path.Combine(Path.Combine(DestinationDirectory.DirectoryPath, subDirectory), Path.GetFileName(file));
                    MoveFile(file, destination);
                    MovedFiles.Add(fileInfo.FullName, destination);
                }

                SortedFlag = true;
            }
            else
            {
                SpecialPrinting.PrintColored(
                    "No files to sort",
                    ConsoleColor.Green);
            }

            Console.WriteLine("\n");
        }

        // reverse the move for the files
        public void ReverseSort()
        {
            SpecialPrinting.PrintColored(
                "Reversing files sort... ",
                ConsoleColor.Yellow);

            foreach (var destination in MovedFiles.Keys)
            {
                MoveFile(MovedFiles[destination], destination);
                MovedFiles.Remove(destination);
            }
        }
    }
}
