﻿using FileSort.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Models
{
    internal class SourceDirectory : BaseDirectory
    {
        public List<string> SourceFiles { get; set; }
        AppSettings AppSettings { get; set; }

        public SourceDirectory(AppSettings appSettings)
            : base(appSettings.SourceFolder)
        {
            SpecialPrinting.PrintColored(
                $"Source directory - {appSettings.SourceFolder}...",
                ConsoleColor.Yellow,
                appSettings.SourceFolder
                );

            AppSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings), $"{nameof(appSettings)} cannot be null in {nameof(SourceDirectory)} initialization");

            //get the source files to sort
            SourceFiles = GetSourceFiles();
        }

        private List<string> GetSourceFiles()
        {
            SpecialPrinting.PrintColored(
                "Retrieving source files... ",
                ConsoleColor.Yellow);

            List<string> sourceFiles = new List<string>();
            IEnumerable<string> files = Directory.EnumerateFiles(DirectoryPath);

            foreach (string file in files)
            {
                //FileInfo fileInfo = new FileInfo(file);
                //SpecialPrinting.PrintColored(fileInfo.FullName, ConsoleColor.Yellow);
                
                var extension = Path.GetExtension(file);

                if (!AppSettings.ExcludedExtensions.Contains(extension))
                    sourceFiles.Add(file);
            }

            SpecialPrinting.PrintColored(
                $"Retrieved {sourceFiles.Count} source files",
                ConsoleColor.Green,
                sourceFiles.Count);

            return sourceFiles;
        }
    }
}
