using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Models
{
    public static class SettingsConfigurationHelper
    {
        private static readonly string AppSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        public static IConfigurationRoot ConfigurationRoot;

        public static IConfigurationRoot BuildConfiguration()
        {
            ConfigurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            return ConfigurationRoot;
        }

        public enum FolderType
        { 
            Destination,
            Source
        }

        public static void UpdateFolderPath(string newPath, FolderType folderType)
        {
            string folderVariable = string.Empty;

            if (folderType == FolderType.Destination)
            {
                folderVariable = "DestinationFolder";
            }
            else
            {
                folderVariable = "SourceFolder";
            }

            var json = File.ReadAllText(AppSettingsPath);
            dynamic jsonObj = JsonConvert.DeserializeObject(json)!;

            jsonObj["AppSettings"][folderVariable] = newPath;

            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(AppSettingsPath, output);

            BuildConfiguration();
        }
    }
}
