using FileSort.DataModels;
using FileSort.Display;
using Microsoft.Extensions.Primitives;
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
        private List<Extension> ExcludedExtensions { get; }

        public SourceDirectory(List<Extension> excludedExtensions, string sourceFolder)
            : base(sourceFolder)
        {
            //AnsiConsole.MarkupLine($"[yellow]Source directory - [/][cyan]{sourceFolder}[/]");

            ExcludedExtensions = excludedExtensions;

            foreach (var extension in excludedExtensions)
            {
                AnsiConsole.MarkupLine(extension.ExtensionName);
            }
            Console.ReadLine();

            SourceFiles = GetSourceFiles();
        }

        //get the source files to sort
        private List<string> GetSourceFiles()
        {
            //AnsiConsole.MarkupLine("[yellow]Retrieving source files... [/]");

            List<string> sourceFiles = new List<string>();
            IEnumerable<string> files = Directory.EnumerateFiles(DirectoryPath);

            foreach (string file in files)
            {
                var extension = Path.GetExtension(file);

                var extensionObj = ExcludedExtensions.FirstOrDefault(e => e.ExtensionName == extension);

                if (extensionObj != null)
                    sourceFiles.Add(file);
            }

            AnsiConsole.MarkupLine($"[green]Retrieved [/][cyan]{sourceFiles.Count}[/][green] source files [/]");

            return sourceFiles;
        }
    }
}
