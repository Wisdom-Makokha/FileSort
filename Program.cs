using FileSort.Data;
using FileSort.DataModels;
using FileSort.AppDirectory;
using FileSort.Display;
using FileSort.Startup;
using FileSort.Repositories;
using Spectre.Console;

namespace FileSort
{
    internal class Program
    {
        static void Main()
        {
            try
            {
                var startup = new Startup.Startup();

                //Console.WriteLine(startup.AppSettings.ToString());

                var mainInterface = new MainInterface(startup);
                mainInterface.Home();

                AnsiConsole.MarkupLine("[yellow]Press Enter to Exit[/]");
                Console.ReadLine();

                startup.ApplicationInstanceRepository.SetClosingTime(startup.ApplicationInstance);
                startup.ApplicationInstanceRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenMethods);

                Console.WriteLine("Press Enter to exit");
                Console.ReadLine();
            }
        }
    }
}
