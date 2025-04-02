using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSort.Settings;

namespace FileSort.Services
{
    internal interface IConfigurationService
    {
        AppSettings AppSettings { get; }
        enum FolderType
        {
            Destination,
            Source
        }
        void ReloadAppSettings();
        void UpdateFolderPath(string newPath, FolderType folderType);
    }
}
