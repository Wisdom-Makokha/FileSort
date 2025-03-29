using FileSort.DataModels;
using FileSort.Repositories;
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
        public BaseFileHandling(FailedMovesRepository failedMovesRepository)
        {
            FailedMovesRepository = failedMovesRepository;
        }

        private FailedMovesRepository FailedMovesRepository { get; set; }

        // method dedicated to moving a file
        protected void MoveFile(string file, string destination)
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
                FailedMovesRepository.AddEntity(failedMove);

                Console.WriteLine();
                AnsiConsole.MarkupLine($"[red]\tError moving file: [/][cyan]{file}[/][red]\n\tto [/][cyan]{destination}[/]");
                //SpecialPrinting.PrintColored($"\tError moving file: {file}\n\tto {destination}", ConsoleColor.Red, file, destination);
                Console.WriteLine();
                //AnsiConsole.WriteException(ex);
                //SpecialPrinting.PrintColored(ex.Message, ConsoleColor.Red);
            }
        }
    }
}
