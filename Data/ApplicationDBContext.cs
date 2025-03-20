using FileSort.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Data
{
    internal class ApplicationDBContext : DbContext
    {
        public DbSet<FileDataModel> Files { get; set; }

        public ApplicationDBContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var configSection = configBuilder.GetSection("ConnectionStrings");
            var connectionString = configSection["SQLServerConnection"] ?? throw new ArgumentNullException("Database connection string cannot be null");

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FileDataModel>(f =>
            {
                f.HasKey(file => file.Id);

                f.HasIndex(file => new { file.FileName, file.Category, file.Extension})
                    .IsUnique();

                f.Property(file => file.SortDate)
                    .HasDefaultValueSql("GETDATE()");
            });
        }
    }
}
