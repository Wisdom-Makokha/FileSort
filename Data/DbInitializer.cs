using FileSort.Data.Interfaces;
using FileSort.DataModels;
using FileSort.Display;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Data
{
    internal static class DbInitializer
    {
        public static void SeedDatabase(ICategoryRepository categoryRepository, IExtensionRepository extensionRepository)
        {
            try
            {
                if (SeedCategories(categoryRepository, extensionRepository))
                {
                    AnsiConsole.MarkupLine("[green]Database successfully seeded[/]");
                    //SpecialPrinting.PrintColored("Database successfully seeded", ConsoleColor.Green);
                }
                //else
                //{
                //    SpecialPrinting.PrintColored("Database already seeded", ConsoleColor.DarkYellow);
                //}
            }
            catch
            {
                throw;
            }
        }

        public static bool SeedCategories(ICategoryRepository categoryRepository, IExtensionRepository extensionRepository)
        {
            if (!categoryRepository.GetAll().Any())
            {
                var categories = new List<Category>
                {
                    new Category
                    {
                        CategoryName = "audio",
                        CategoryDescription = ""
                    },
                    new Category
                    {
                        CategoryName = "compressed",
                        CategoryDescription = ""
                    },
                    new Category
                    {
                        CategoryName = "videos",
                        CategoryDescription = ""
                    },
                    new Category
                    {
                        CategoryName = "pics",
                        CategoryDescription = ""
                    },
                    new Category
                    {
                        CategoryName = "documents",
                        CategoryDescription = ""
                    },
                    new Category
                    {
                        CategoryName = "scripts",
                        CategoryDescription = ""
                    },
                    new Category
                    {
                        CategoryName = "software",
                        CategoryDescription = ""
                    },
                    new Category
                    {
                        CategoryName = "gifs",
                        CategoryDescription = ""
                    },
                    new Category
                    {
                        CategoryName = "excludedextensions",
                        CategoryDescription = ""
                    },
                    new Category
                    {
                        CategoryName = "Other",
                        CategoryDescription = ""
                    },
                };

                foreach (var category in categories)
                {
                    categoryRepository.AddEntity(category);
                }
                categoryRepository.SaveChanges();

                AnsiConsole.MarkupLine("[green]Categories table seeded successfully[/]");
                //SpecialPrinting.PrintColored("Categories table seeded successfully", ConsoleColor.Green);
            }
            //else
            //{
            //    AnsiConsole.MarkupLine("[olive]Categories table already seeded[/]");
            //    SpecialPrinting.PrintColored("Categories table already seeded", ConsoleColor.DarkYellow);
            //}

            bool result = SeedExtensions(categoryRepository, extensionRepository);

            return result;
        }

        public static bool SeedExtensions(ICategoryRepository categoryRepository, IExtensionRepository extensionRepository)
        {
            bool result = false;
            if (!extensionRepository.GetAll().Any())
            {
                var categories = categoryRepository.GetAll();

                var extensions = new List<Extension>
                {
                    new Extension
                    {
                        ExtensionName = ".tmp",
                        CategoryId = categories.Single(c =>  c.CategoryName == "excludedextensions").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".bak",
                        CategoryId = categories.Single(c =>  c.CategoryName == "excludedextensions").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".log",
                        CategoryId = categories.Single(c =>  c.CategoryName == "excludedextensions").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".crdownload",
                        CategoryId = categories.Single(c =>  c.CategoryName == "excludedextensions").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".flac",
                        CategoryId = categories.Single(c =>  c.CategoryName == "audio").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".doc",
                        CategoryId = categories.Single(c =>  c.CategoryName == "documents").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".7z",
                        CategoryId = categories.Single(c =>  c.CategoryName == "compressed").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".ps1",
                        CategoryId = categories.Single(c =>  c.CategoryName == "scripts").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".xlsx",
                        CategoryId = categories.Single(c =>  c.CategoryName == "documents").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".jfif",
                        CategoryId = categories.Single(c =>  c.CategoryName == "pics").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".docx",
                        CategoryId = categories.Single(c =>  c.CategoryName == "documents").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".bat",
                        CategoryId = categories.Single(c =>  c.CategoryName == "scripts").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".exe",
                        CategoryId = categories.Single(c =>  c.CategoryName == "software").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".jpeg",
                        CategoryId = categories.Single(c =>  c.CategoryName == "pics").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".png",
                        CategoryId = categories.Single(c =>  c.CategoryName == "pics").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".pptx",
                        CategoryId = categories.Single(c =>  c.CategoryName == "documents").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".gif",
                        CategoryId = categories.Single(c =>  c.CategoryName == "gifs").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".rar",
                        CategoryId = categories.Single(c =>  c.CategoryName == "compressed").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".mp4",
                        CategoryId = categories.Single(c =>  c.CategoryName == "videos").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".zip",
                        CategoryId = categories.Single(c =>  c.CategoryName == "compressed").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".avi",
                        CategoryId = categories.Single(c =>  c.CategoryName == "videos").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".gz",
                        CategoryId = categories.Single(c =>  c.CategoryName == "compressed").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".txt",
                        CategoryId = categories.Single(c =>  c.CategoryName == "documents").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".jpg",
                        CategoryId = categories.Single(c =>  c.CategoryName == "pics").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".wav",
                        CategoryId = categories.Single(c =>  c.CategoryName == "audio").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".pdf",
                        CategoryId = categories.Single(c =>  c.CategoryName == "documents").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".mkv",
                        CategoryId = categories.Single(c =>  c.CategoryName == "videos").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".bmp",
                        CategoryId = categories.Single(c =>  c.CategoryName == "pics").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".mp3",
                        CategoryId = categories.Single(c =>  c.CategoryName == "audio").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".msi",
                        CategoryId = categories.Single(c =>  c.CategoryName == "software").Id
                    },
                    new Extension
                    {
                        ExtensionName = ".jar",
                        CategoryId = categories.Single(c =>  c.CategoryName == "compressed").Id
                    }
                };

                foreach(var extension in extensions)
                {
                    extensionRepository.AddEntity(extension);
                }
                extensionRepository.SaveChanges();

                AnsiConsole.MarkupLine("[green]Extensions table seeded successfully[/]");
                //SpecialPrinting.PrintColored("Extensions table seeded successfully", ConsoleColor.Green);

                result = true;
            }
            //else
            //{
            //    AnsiConsole.MarkupLine("[olive]Extensions table already seeded[/]");
            //    SpecialPrinting.PrintColored("Extensions table already seeded", ConsoleColor.DarkYellow);
            //}

            return result;
        }
    }
}
