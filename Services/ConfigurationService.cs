using FileSort.Settings;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Services
{
    internal class ConfigurationService : IConfigurationService
    {
        private readonly string AppSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        public AppSettings AppSettings { get; private set; }

        public ConfigurationService() 
        {
            AppSettings = LoadAppSettings();
        }

        public void ReloadAppSettings()
        {
            AppSettings = LoadAppSettings();
        }

        private AppSettings LoadAppSettings()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            return config.GetSection("AppSettings").Get<AppSettings>()
                ?? throw new ArgumentNullException(nameof(AppSettings), "AppSettings configuration is missing");
        }

        public void UpdateFolderPath(string newPath, IConfigurationService.FolderType folderType)
        {
            string folderVariable = string.Empty;

            if (folderType == IConfigurationService.FolderType.Destination)
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

            ReloadAppSettings();
        }
    }
}
