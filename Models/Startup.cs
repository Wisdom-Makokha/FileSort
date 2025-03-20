using FileSort.Data;
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

            AppSettings = config.GetSection("AppSettings").Get<AppSettings>() ?? throw new ArgumentNullException($"{nameof(AppSettings)} cannot be null in program initialization");
            ApplicationDBContext = new ApplicationDBContext();
        }

        public AppSettings AppSettings { get; set; }
        public ApplicationDBContext ApplicationDBContext { get; set; }
    }
}
