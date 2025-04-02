using FileSort.DataModels;
using FileSort.AppDirectory;
using FileSort.Display;
using FileSort.Startup;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSort.Data.Interfaces;

namespace FileSort.FileHandling
{
    internal class Sort : BaseFileHandling
    {
        public Sort(
            SourceDirectory sourceDirectory,
            string destinationDirectoryPath,
            Guid applicationInstanceId,
            IFileDataModelRepository fileDataModelRepository,
            IExtensionRepository extensionRepository,
            IFailedMovesRepository failedMovesRepository,
            List<Extension> extensions,
            List<Category> categories)
            : base(failedMovesRepository)
        {
            SourceDirectory = sourceDirectory;
            DestinationDirectoryPath = destinationDirectoryPath;
            ApplicationInstanceId = applicationInstanceId;
            _fileDataModelRepository = fileDataModelRepository;
            _extensionRepository = extensionRepository;
            Extensions = extensions;
            Categories = categories;
        }

        private SourceDirectory SourceDirectory { get; }
        private string DestinationDirectoryPath { get; }
        private Guid ApplicationInstanceId { get; }
        private IFileDataModelRepository _fileDataModelRepository { get; }
        private IExtensionRepository _extensionRepository { get; }
        List<Extension> Extensions { get; }
        List<Category> Categories { get; }

        // sort the files into their categories
        public void SortFiles()
        {
            //AnsiConsole.MarkupLine("[yellow]Sorting files... [/]");

            string subDirectory;

            if (SourceDirectory.SourceFiles.Count > 0)
            {
                int fileCount = 0;
                foreach (var file in SourceDirectory.SourceFiles)
                {
                    FileInfo fileInfo = new FileInfo(file);

                    var extension = fileInfo.Extension;

                    var extensionObj = Extensions.FirstOrDefault(x => x.ExtensionName == extension);
                    var category = extensionObj == null ? null : extensionObj.Category;
                    subDirectory = category == null ? "other" : category.CategoryName;

                    subDirectory = "other";
                    Extension newExtension = new Extension()
                    {
                        ExtensionName = extension,
                        CategoryId = Categories.Single(c => c.CategoryName == subDirectory).Id
                    };

                    try
                    {
                        _extensionRepository.AddEntity(newExtension);
                        _extensionRepository.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"Error: {ex.Message}");
                    }

                    FileDataModel fileDataModel = new FileDataModel()
                    {
                        FileName = Path.GetFileNameWithoutExtension(fileInfo.FullName),
                        ExtensionId = Extensions.Single(e => e.ExtensionName == extension).Id,
                        CategoryId = Categories.Single(c => c.CategoryName == subDirectory).Id,
                        SourceFolderPath = SourceDirectory.DirectoryPath,
                        DestinationFolderPath = DestinationDirectoryPath,
                        ApplicationInstanceId = ApplicationInstanceId,
                        IsSorted = true,
                    };

                    try
                    {
                        string destination = Path.Combine(Path.Combine(DestinationDirectoryPath, subDirectory), Path.GetFileName(file));
                        MoveFile(file, destination);

                        _fileDataModelRepository.AddEntity(fileDataModel);
                        _fileDataModelRepository.SaveChanges();

                        AnsiConsole.MarkupLine($"[green]{fileCount,4}.Filename[/][cyan]{fileDataModel.FileName}[/]");
                        AnsiConsole.MarkupLine($"[green]{" ",4}.Source - [/][cyan]{fileDataModel.SourceFolderPath}[/]");
                        AnsiConsole.MarkupLine($"[green]{" ",4}.Destination - [/][cyan]{fileDataModel.DestinationFolderPath}[/]");
                        Console.WriteLine();
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"Error: {ex.Message}");
                    }

                    fileCount++;
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[olive]No files to sort[/]");
            }

            AnsiConsole.WriteLine("\n");
        }

    }
}
