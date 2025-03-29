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
            Dictionary<string, string> movedFiles = new Dictionary<string, string>();
            movedFiles = FileRepository.GetInstanceMovedFiles(ApplicationInstance.ApplicationId);

            if (movedFiles.Count > 0)
            {
                AnsiConsole.MarkupLine("[yellow]Reversing files sort... [/]");
                //SpecialPrinting.PrintColored("Reversing files sort... ", ConsoleColor.Yellow);

                foreach (var destination in movedFiles.Keys)
                {
                    FileInfo fileInfo = new FileInfo(destination);
                    FileDataModel fileDataModel = FileRepository
                        .GetByFileNameAndExtension(Path.GetFileNameWithoutExtension(destination), fileInfo.Extension)!;

                    fileDataModel.IsSorted = false;
                    FileRepository.UpdateEntity(fileDataModel);
                    FileRepository.SaveChanges();

                    MoveFile(movedFiles[destination], destination);
                    movedFiles.Remove(destination);
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
