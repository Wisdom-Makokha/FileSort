using FileSort.Data.Interfaces;
using FileSort.DataModels;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.FileHandling
{
    internal class BaseFileHandling
    {
        private IFailedMovesRepository FailedMovesRepository { get; }

        public BaseFileHandling(IFailedMovesRepository failedMovesRepository)
        {
            FailedMovesRepository = failedMovesRepository;
        }


        // method dedicated to moving a file
        protected void MoveFile(string file, string destination)
        {
            try
            {
                File.Move(file, destination);
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
                FailedMovesRepository.AddEntity(failedMove);

                Console.WriteLine();
                AnsiConsole.MarkupLine($"[red]\tError moving file: [/][cyan]{file}[/][red]\n\tto [/][cyan]{destination}[/]");
                Console.WriteLine();
            }
        }
    }
}
