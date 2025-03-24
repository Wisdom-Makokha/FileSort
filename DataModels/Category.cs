using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.DataModels
{
    internal class Category
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public required string CategoryName { get; set; }
        public string? CategoryDescription { get; set; } = string.Empty;
        public List<Extension> Extensions { get; set; } = new List<Extension>();
        public List<FileDataModel> Files { get; set; } = new List<FileDataModel>();
    }
}
