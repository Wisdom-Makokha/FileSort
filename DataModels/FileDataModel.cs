using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.DataModels
{
    internal class FileDataModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Extension { get; set; }
        public string Category { get; set; }
        public string SourceFolderPath { get; set; }
        public string DestinationFolderPath { get; set; }
        public DateTime SortDate { get; set; }
    }
}
