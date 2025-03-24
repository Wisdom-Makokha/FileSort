using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.DataModels
{
    internal class ApplicationInstance
    {
        public Guid ApplicationId { get; set; }
        public DateTime InitiationTime { get; set; }
        public DateTime? ClosingTime { get; set; }
        public List<FileDataModel> Files { get; set; } = new List<FileDataModel>();
    }
}
