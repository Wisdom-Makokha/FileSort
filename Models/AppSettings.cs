﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Models
{
    internal class AppSettings
    {
        public required string SourceFolder { get; set; }
        public  required string DestinationFolder { get; set; }
        public required List<string> ExcludedExtensions { get; set; }
        public required Dictionary<string, string> ExtensionCategories { get; set; }
        public HashSet<string> Categories => new HashSet<string>(
            ExtensionCategories!.Values.Select(subCategory => $"{subCategory}")
            .Concat(new[] { "Other" })
            );

        public override string ToString()
        {
            string appSettingsStr = $"{nameof(SourceFolder)}: {SourceFolder}\n";
            appSettingsStr += $"{nameof(DestinationFolder)}: {DestinationFolder}\n";
            
            appSettingsStr += nameof(ExcludedExtensions) + "\n";
            foreach (var subCategory in ExcludedExtensions) { appSettingsStr += $"\t = {subCategory}\n"; }

            appSettingsStr += nameof(ExcludedExtensions) + "\n";
            foreach (var key in ExtensionCategories.Keys) { appSettingsStr += $"\t = {key, -5} - {ExtensionCategories[key]}\n"; }

            appSettingsStr += nameof(Categories) + "\n";
            foreach (var category in Categories) { appSettingsStr += $"\t = {category}\n"; }

            return appSettingsStr;
        }
    }
}
