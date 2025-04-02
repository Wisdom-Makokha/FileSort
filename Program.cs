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

            var serviceProvider = services.BuildServiceProvider();

            try
            {
                //Console.WriteLine(startup.AppSettings.ToString());

                //var mainInterface = new MainInterface(startup);
                //mainInterface.Home();

                var startup = serviceProvider.GetRequiredService<Startup.Startup>();
                startup.Initialize();
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenMethods);
            }
            finally
            {
                //startup.ApplicationInstanceRepository.SetClosingTime(startup.ApplicationInstance);
                //startup.ApplicationInstanceRepository.SaveChanges();

                Console.WriteLine("Press Enter to exit");
                Console.ReadLine();
            }
        }
    }
}
