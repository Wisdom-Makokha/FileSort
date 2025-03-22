using FileSort.Data;
using FileSort.DataModels;
using FileSort.Models;

namespace FileSort
{
    internal class Program
    {
        static void Main()
        {
            var startup = new Startup();

            //Console.WriteLine(startup.AppSettings.ToString());
            var sourceDirectory = new SourceDirectory(startup.AppSettings);
            var destinationDirectory = new DestinationDirectory(startup.AppSettings);
            var repository = new FileDataModelRepository(startup.ApplicationDBContext);

            var sort = new Sort(sourceDirectory, destinationDirectory, startup.AppSettings, repository);
            sort.SortFiles();
            sort.ReverseSort();

            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }
    }
}
