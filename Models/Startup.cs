using FileSort.Data;
using FileSort.DataModels;
using FileSort.Display;
using FileSort.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Models
{
    internal class Startup
    {
        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();

            AppSettings = config.GetSection("AppSettings").Get<AppSettings>()
                ?? throw new ArgumentNullException(nameof(AppSettings), $"{nameof(AppSettings)} cannot be null in {nameof(Startup)} initialization");
            ApplicationDBContext = new ApplicationDBContext();

            ApplicationInstanceRepository = new ApplicationInstanceRepository(ApplicationDBContext);
            FileDataModelRepository = new FileDataModelRepository(ApplicationDBContext);
            ExtensionRepository = new ExtensionRepository(ApplicationDBContext);
            CategoryRepository = new CategoryRepository(ApplicationDBContext);

            DbInitializer.SeedDatabase(applicationDBContext: ApplicationDBContext);

            Categories = (List<Category>)CategoryRepository.GetAll();
            Extensions = (List<Extension>)ExtensionRepository.GetAll();
            ExcludedExtensions = GetExcludedExtensions();
            ExtensionCategories = GetExtensionCategory();
            CategoryNames = GetCategoryNames();
        }

        // properties
        public AppSettings AppSettings { get; set; }
        public ApplicationDBContext ApplicationDBContext { get; set; }
        public ApplicationInstanceRepository ApplicationInstanceRepository { get; set; }
        public FileDataModelRepository FileDataModelRepository { get; set; }
        public ExtensionRepository ExtensionRepository { get; set; }
        public CategoryRepository CategoryRepository { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<string> CategoryNames { get; set; } = new List<string>();
        public List<Extension> Extensions { get; set; } = new List<Extension>();
        public List<string> ExcludedExtensions { get; set; } = new List<string>();
        public Dictionary<string, string> ExtensionCategories { get; set; } = new Dictionary<string, string>();

        // methods
        private List<string> GetExcludedExtensions()
        {
            var excludedExtensions = from Extension extension in Extensions
                                     join Category category in Categories
                                     on extension.CategoryId equals category.Id
                                     where category.CategoryName == "excludedextensions"
                                     select extension.ExtensionName;

            List<string> result = new List<string>();
            foreach (var extension in excludedExtensions)
            {
                result.Add(extension);

                //SpecialPrinting.PrintColored($"Excluded Extension: {extension}", ConsoleColor.Magenta, extension);
            }

            return result;
        }

        private Dictionary<string, string> GetExtensionCategory()
        {
            var extensionCategory = from Extension extension in Extensions
                                    join Category category in Categories
                                    on extension.CategoryId equals category.Id
                                    select new
                                    {
                                        newExtension = extension.ExtensionName,
                                        newCategory = category.CategoryName
                                    };

            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (var extension in extensionCategory)
            { result.Add(extension.newExtension, extension.newCategory); }

            //foreach (var extension in result.Keys)
            //{
            //    SpecialPrinting.PrintColored(
            //        $"Extension: {extension,-15} - Category: {result[extension]}",
            //        ConsoleColor.Magenta,
            //        extension, result[extension]);
            //}

            return result;
        }

        private List<string> GetCategoryNames()
        {
            List<string> result = new List<string>();

            foreach (var category in Categories)
            {
                result.Add(category.CategoryName);
            }

            return result;
        }
    }
}
