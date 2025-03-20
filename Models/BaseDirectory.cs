using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Models
{
    internal class BaseDirectory
    {
        public string DirectoryPath { get; private set; }
        public BaseDirectory(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
                throw new ArgumentNullException(nameof(directoryPath), "No directory path provided");

            if (Directory.Exists(directoryPath))
                DirectoryPath = directoryPath;
            else
                throw new ArgumentException($"Provided directory path: {directoryPath} is invalid");
        }
    }
}
