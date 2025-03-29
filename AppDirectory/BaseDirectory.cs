using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.AppDirectory
{
    internal class BaseDirectory
    {
        public string DirectoryPath { get; private set; }
        public BaseDirectory(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
                directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            if (Directory.Exists(directoryPath))
                DirectoryPath = directoryPath;
            else
                throw new ArgumentException($"Provided directory path: {directoryPath} is invalid");
        }
    }
}
