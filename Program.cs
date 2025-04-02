using FileSort.Data;
using FileSort.DataModels;
using FileSort.AppDirectory;
using FileSort.Display;
using FileSort.Startup;
using Spectre.Console;
using Microsoft.Extensions.DependencyInjection;
using FileSort.Services;
using FileSort.Data.Repositories;
using FileSort.Data.Interfaces;
using FileSort.Display.Interfaces;
using FileSort.Display.Managers;

namespace FileSort
{
    internal class Program
    {
        static void Main()
        {
            // build the service collection 
            var services = new ServiceCollection();

            // add configuration 
            services.AddSingleton<IConfigurationService, ConfigurationService>();

            // database context
            services.AddDbContext<ApplicationDBContext>();

            // register repositories
            services.AddScoped<IApplicationInstanceRepository, ApplicationInstanceRepository>();
            services.AddScoped<IFileDataModelRepository, FileDataModelRepository>();
            services.AddScoped<IExtensionRepository, ExtensionRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IFailedMovesRepository, FailedMovesRepository>();

            services.AddSingleton<IDataService, DataService>();
            services.AddSingleton<Startup.Startup>();

            services.AddSingleton<ICategoryManager, CategoryManager>();
            services.AddSingleton<IExtensionManager, ExtensionManager>();
            services.AddSingleton<IFolderManager, FolderManager>();
            services.AddSingleton<ISortManager, SortManager>();
            services.AddSingleton<IStatManager, StatManager>();
            services.AddSingleton<IIssuesManager, IssuesManager>();

            services.AddSingleton<MainInterface>();

            var serviceProvider = services.BuildServiceProvider();

            try
            {
                //Console.WriteLine(startup.AppSettings.ToString());

                //var mainInterface = new MainInterface(startup);
                //mainInterface.Home();

                var startup = serviceProvider.GetRequiredService<Startup.Startup>();
                startup.Initialize();

                var mainInterface = serviceProvider.GetRequiredService<MainInterface>();
                mainInterface.Home();
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenMethods);
            }
            finally
            {
                //startup.ApplicationInstanceRepository.SetClosingTime(startup.ApplicationInstance);
                //startup.ApplicationInstanceRepository.SaveChanges();

                // ensure db context is disposed
                if(serviceProvider is  IDisposable disposable)
                {
                    disposable.Dispose();
                }

                Console.WriteLine("Press Enter to exit");
                Console.ReadLine();
            }
        }
    }
}
