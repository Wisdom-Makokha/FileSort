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
        public required string FileName { get; set; }
        public required string FilePath { get; set; }
        public required string Extension { get; set; }
        public required string Category { get; set; }
        public required string SourceFolderPath { get; set; }
        public required string DestinationFolderPath { get; set; }
        public required bool IsSorted { get; set; }
        public DateTime SortDate { get; set; }
    }
}
