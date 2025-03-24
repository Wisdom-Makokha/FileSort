using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.DataModels
{
    internal class FileDataModel
    {
        public required string FileName { get; set; }
        public Extension FileExtension { get; set; }
        public int ExtensionId { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public required string SourceFolderPath { get; set; }
        public required string DestinationFolderPath { get; set; }
        public required bool IsSorted { get; set; }
        public DateTime SortDate { get; set; }
        public Guid ApplicationInstanceId { get; set; }
        public ApplicationInstance ApplicationInstance { get; set; }
    }
}
