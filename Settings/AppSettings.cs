using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Settings
{
    internal class AppSettings
    {
        public required string SourceFolder { get; set; }
        public  required string DestinationFolder { get; set; }

        public override string ToString()
        {
            string appSettingsStr = $"{nameof(SourceFolder)}: {SourceFolder}\n";
            appSettingsStr += $"{nameof(DestinationFolder)}: {DestinationFolder}\n";
            
            return appSettingsStr;
        }
    }
}
