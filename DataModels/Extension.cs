using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.DataModels
{
    internal class Extension
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public required string ExtensionName { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public List<FileDataModel> Files { get; set; } = new List<FileDataModel>();
    }
}
