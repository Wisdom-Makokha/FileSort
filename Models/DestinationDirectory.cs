using FileSort.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Models
{
    internal class DestinationDirectory : BaseDirectory
    {
        public AppSettings AppSettings { get; set; }

        public DestinationDirectory(AppSettings appSettings)
            : base(appSettings.DestinationFolder)
        {
            SpecialPrinting.PrintColored(
                $"Destination directory - {appSettings.DestinationFolder}",
                ConsoleColor.Yellow,
                appSettings.DestinationFolder);

            AppSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings), $"{nameof(appSettings)} cannot be null in {nameof(DestinationDirectory)} initialization");
            CheckDestinationSubDirectories();
        }

        private void CheckDestinationSubDirectories()
        {
            SpecialPrinting.PrintColored(
                "Checking destination directory subdirectories... ",
                ConsoleColor.Yellow);

            foreach (var category in AppSettings.Categories)
            {
                string destination = Path.Combine(DirectoryPath, category);

                if (!Directory.Exists(destination))
                {
                    try
                    {
                        Directory.CreateDirectory(destination);
                        SpecialPrinting.PrintColored($"Created: {category}", ConsoleColor.Green, category);
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }
    }
}
