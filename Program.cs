using FileSort.Data;
using FileSort.Models;

namespace FileSort
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var startup = new Startup();
            var sourceFolder = new SourceDirectory(startup.AppSettings);

            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }
    }
}
