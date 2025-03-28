using FileSort.Data;
using FileSort.DataModels;
using FileSort.Display;
using FileSort.Models;
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
                var startup = new Startup();

                //Console.WriteLine(startup.AppSettings.ToString());
                SourceDirectory sourceDirectory = new SourceDirectory(startup.ExcludedExtensions, startup.AppSettings.SourceFolder);

                //BaseInterface mainInterface = new BaseInterface(startup);

                //mainInterface.HomeInterface();

                AnsiConsole.MarkupLine("[yellow]Press Enter to Exit[/]");
                //Console.WriteLine("Press Enter to exit");
                Console.ReadLine();

                startup.ApplicationInstanceRepository.SetClosingTime(startup.ApplicationInstance);
                startup.ApplicationInstanceRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenMethods);
                
                AnsiConsole.MarkupLine($"[red]Exception caught\nLocated[/][cyan]{ex.StackTrace}[/][red]\nException Type: [/][cyan]{ex.GetType().Name}[/][red]\nMessage: [/][cyan]{ex.Message}[/]");

                //SpecialPrinting.PrintColored(
                //    $"Exception caught\nLocated{ex.StackTrace}\nException Type: {ex.GetType().Name}\nMessage: {ex.Message}",
                //    ConsoleColor.Red,
                //    ex.StackTrace!, ex.GetType().Name, ex.Message);


                Console.WriteLine("Press Enter to exit");
                Console.ReadLine();
            }
        }
    }
}
