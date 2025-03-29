using FileSort.Display;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.AppDirectory
{
    internal class SourceDirectory : BaseDirectory
    {
        public List<string> SourceFiles { get; set; }
        private List<string> ExcludedExtensions { get; set; }

        public SourceDirectory(List<string> excludedExtensions, string sourceFolder)
            : base(sourceFolder)
        {
            AnsiConsole.MarkupLine($"[yellow]Source directory - [/][cyan]{sourceFolder}[/]");
            //SpecialPrinting.PrintColored($"Source directory - {sourceFolder}...", ConsoleColor.Yellow, sourceFolder);

            ExcludedExtensions = excludedExtensions;
            //get the source files to sort
            SourceFiles = GetSourceFiles();
        }

        private List<string> GetSourceFiles()
        {
            AnsiConsole.MarkupLine("[yellow]Retrieving source files... [/]");
            //SpecialPrinting.PrintColored("Retrieving source files... ", ConsoleColor.Yellow);

            List<string> sourceFiles = new List<string>();
            IEnumerable<string> files = Directory.EnumerateFiles(DirectoryPath);

            foreach (string file in files)
            {
                //FileInfo fileInfo = new FileInfo(file);
                //SpecialPrinting.PrintColored(fileInfo.FullName, ConsoleColor.Yellow);

                var extension = Path.GetExtension(file);

                if (!ExcludedExtensions.Contains(extension))
                    sourceFiles.Add(file);
            }

            AnsiConsole.MarkupLine($"[green]Retrieved [/][cyan]{sourceFiles.Count}[/][green] source files [/]");
            //SpecialPrinting.PrintColored($"Retrieved {sourceFiles.Count} source files", ConsoleColor.Green, sourceFiles.Count);

            return sourceFiles;
        }
    }
}
