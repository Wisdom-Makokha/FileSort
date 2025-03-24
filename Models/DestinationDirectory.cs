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
        private List<string> Categories { get; set; }

        public DestinationDirectory(List<string> categories, string destinationFolder)
            : base(destinationFolder)
        {
            SpecialPrinting.PrintColored(
                $"Destination directory - {destinationFolder}",
                ConsoleColor.Yellow,
                destinationFolder);

            Categories = categories;
            CheckDestinationSubDirectories();
        }

        private void CheckDestinationSubDirectories()
        {
            SpecialPrinting.PrintColored(
                "Checking destination directory subdirectories... ",
                ConsoleColor.Yellow);

            foreach (var category in Categories)
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
