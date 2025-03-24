using FileSort.Data;
using FileSort.DataModels;
using FileSort.Models;
using FileSort.Repositories;

namespace FileSort
{
    internal class Program
    {
        static void Main()
        {
            var startup = new Startup();
            DateTime dateTime = DateTime.Now;
            ApplicationInstance applicationInstance = new ApplicationInstance()
            {
                InitiationTime = dateTime,
            };

            startup.ApplicationInstanceRepository.AddEntity(applicationInstance);
            startup.ApplicationInstanceRepository.SaveChanges();

            applicationInstance = startup.ApplicationInstanceRepository.GetInstanceByTime(dateTime)!;

            SourceDirectory sourceDirectory = new SourceDirectory(startup.ExcludedExtensions, startup.AppSettings.SourceFolder);
            DestinationDirectory destinationDirectory = new DestinationDirectory(startup.CategoryNames, startup.AppSettings.DestinationFolder);
            Sort sort = new Sort(sourceDirectory, destinationDirectory, startup, applicationInstance);

            sort.SortFiles();

            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();

            startup.ApplicationInstanceRepository.SetClosingTime(applicationInstance);
            startup.ApplicationInstanceRepository.SaveChanges();
        }
    }
}
