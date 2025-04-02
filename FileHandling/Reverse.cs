using FileSort.DataModels;
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
    internal class Reverse : BaseFileHandling
    {
        public Reverse(IFailedMovesRepository failedMovesRepository, Guid applicationInstanceId, IFileDataModelRepository fileDataModelRepository)
            : base(failedMovesRepository)
        {
            ApplicationInstanceId = applicationInstanceId;
            FileRepository = fileDataModelRepository;
        }

        private Guid ApplicationInstanceId { get; }
        private IFileDataModelRepository FileRepository { get; }

        // reverse the move for the files
        public void ReverseSort()
        {
            var movedFiles = FileRepository.GetInstanceMovedFiles(ApplicationInstanceId);

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
