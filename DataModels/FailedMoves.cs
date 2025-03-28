using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.DataModels
{
    internal class FailedMoves
    {
        public int Id { get; set; }
        public required string FileName { get; set; }
        public required string SourceFolderPath { get; set; }
        public required string DestinationFolderPath {  get; set; }
        public required string FailureMessage { get; set; }
        public DateTime FailedDate { get; set; }
        public bool IsResolved { get; set; }
        public DateTime? ResolvedDate { get; set; }
    }
}
