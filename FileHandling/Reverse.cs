using FileSort.DataModels;
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
    internal class Reverse : BaseFileHandling
    {
        public Reverse(FailedMovesRepository failedMovesRepository, ApplicationInstance applicationInstance, FileDataModelRepository fileDataModelRepository)
            : base(failedMovesRepository)
        {
            ApplicationInstance = applicationInstance;
            FileRepository = fileDataModelRepository;
        }

        private ApplicationInstance ApplicationInstance { get; }
        private FileDataModelRepository FileRepository { get; }

        // reverse the move for the files
        public void ReverseSort()
        {
            var movedFiles = FileRepository.GetInstanceMovedFiles(ApplicationInstance.ApplicationId);

            if (movedFiles.Count > 0)
            {
                AnsiConsole.MarkupLine("[yellow]Reversing files sort... [/]");
                foreach (var file in movedFiles)
                {
                    string destination = Path.Combine(file.SourceFolderPath, file.FileName + file.FileExtension.ExtensionName);
                    string source = Path.Combine(file.DestinationFolderPath, file.FileName + file.FileExtension.ExtensionName);

                    file.IsSorted = false;
                    FileRepository.UpdateEntity(file);
                    FileRepository.SaveChanges();

                    MoveFile(source, destination);
                }

                AnsiConsole.WriteLine("\n");
            }
            else
            {
                AnsiConsole.MarkupLine("[olive]Files not sorted yet![/]");
            }
        }
    }
}
