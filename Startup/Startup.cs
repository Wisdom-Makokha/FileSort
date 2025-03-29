using FileSort.Data;
using FileSort.DataModels;
using FileSort.Display;
using FileSort.Migrations;
using FileSort.Repositories;
using FileSort.Settings;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Startup
{
    internal class Startup
    {
        public Startup()
        {
            var config = SettingsConfigurationHelper.BuildConfiguration();

            AppSettings = config.GetSection("AppSettings").Get<AppSettings>()
                ?? throw new ArgumentNullException(nameof(AppSettings), $"{nameof(AppSettings)} cannot be null in {nameof(Startup)} initialization");
            ApplicationDBContext = new ApplicationDBContext();

            DateTime dateTime = DateTime.Now;
            ApplicationInstance = new ApplicationInstance()
            {
                InitiationTime = dateTime,
            };

            ApplicationInstanceRepository = new ApplicationInstanceRepository(ApplicationDBContext);
            FileDataModelRepository = new FileDataModelRepository(ApplicationDBContext);
            ExtensionRepository = new ExtensionRepository(ApplicationDBContext);
            CategoryRepository = new CategoryRepository(ApplicationDBContext);
            FailedMovesRepository = new FailedMovesRepository(ApplicationDBContext);

            ApplicationInstanceRepository.AddEntity(ApplicationInstance);
            ApplicationInstanceRepository.SaveChanges();

            ApplicationInstance = ApplicationInstanceRepository.GetInstanceByTime(dateTime)!;
            DbInitializer.SeedDatabase(applicationDBContext: ApplicationDBContext);

            Categories = (List<Category>)CategoryRepository.GetAll();
            Extensions = (List<Extension>)ExtensionRepository.GetAll();
            ExcludedExtensions = GetExcludedExtensions();
            ExtensionCategories = GetExtensionCategory();
            CategoryNames = GetCategoryNames();
        }

        // properties
        public ApplicationInstance ApplicationInstance { get; set; }
        public AppSettings AppSettings { get; set; }
        public ApplicationDBContext ApplicationDBContext { get; set; }
        public ApplicationInstanceRepository ApplicationInstanceRepository { get; set; }
        public FileDataModelRepository FileDataModelRepository { get; set; }
        public ExtensionRepository ExtensionRepository { get; set; }
        public CategoryRepository CategoryRepository { get; set; }
        public FailedMovesRepository FailedMovesRepository { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<string> CategoryNames { get; set; } = new List<string>();
        public List<Extension> Extensions { get; set; } = new List<Extension>();
        public List<string> ExcludedExtensions { get; set; } = new List<string>();
        public Dictionary<string, string> ExtensionCategories { get; set; } = new Dictionary<string, string>();

        // methods
        public List<string> GetExcludedExtensions()
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

                //AnsiConsole.MarkupLine($"[magenta]Excluded Extension: [/][cyan]{extension}[/]");
            }

            return result;
        }

        public Dictionary<string, string> GetExtensionCategory()
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
            //    AnsiConsole.MarkupLine($"[magenta]Extension: [/][cyan]{extension,-15}[/][magenta] - Category: [/][cyan]{result[extension]}[/]");
            //}

            return result;
        }

        public List<string> GetCategoryNames()
        {
            List<string> result = new List<string>();

            foreach (var category in Categories)
            {
                result.Add(category.CategoryName);
            }

            return result;
        }

        public void GetAppSettings()
        {
            var config = SettingsConfigurationHelper.BuildConfiguration();

            AppSettings = config.GetSection("AppSettings").Get<AppSettings>()
                ?? throw new ArgumentNullException(nameof(AppSettings), $"{nameof(AppSettings)} cannot be null in {nameof(Startup)} initialization");
        }
    }
}
