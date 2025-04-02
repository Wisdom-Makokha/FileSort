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
        public DbSet<Extension> Extensions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ApplicationInstance> ApplicationInstances { get; set; }
        public DbSet<FailedMoves> FailedMoves { get; set; }

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
                f.HasKey(file => new { file.FileName, file.ExtensionId, file.SourceFolderPath });

                f.Property(file => file.SortDate)
                    .HasDefaultValueSql("GETDATE()");

                f.Property(file => file.IsSorted)
                    .HasDefaultValue(true);
            });

            modelBuilder.Entity<Extension>(e =>
            {
                e.HasKey(extension => extension.Id);

                e.HasIndex(extension => extension.ExtensionName)
                    .IsUnique();

                e.HasMany(extension => extension.Files)
                    .WithOne(file => file.FileExtension)
                    .HasForeignKey(file => file.ExtensionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Category>(c =>
            {
                c.HasKey(category => category.Id);

                c.HasIndex(category => category.CategoryName)
                    .IsUnique();

                c.HasMany(category => category.Extensions)
                    .WithOne(extension => extension.Category)
                    .HasForeignKey(extension => extension.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                c.HasMany(category => category.Files)
                    .WithOne(file => file.Category)
                    .HasForeignKey(file => file.CategoryId)
                    .OnDelete(deleteBehavior: DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ApplicationInstance>(a =>
            {
                a.HasKey(instance => instance.ApplicationId);

                a.Property(instance => instance.InitiationTime)
                    .HasDefaultValueSql("GETDATE()");

                a.HasIndex(instance => instance.InitiationTime)
                    .IsUnique();

                a.HasMany(instance => instance.Files)
                    .WithOne(file => file.ApplicationInstance)
                    .HasForeignKey(file => file.ApplicationInstanceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<FailedMoves>(f =>
            {
                f.HasKey(failed =>  failed.Id);

                f.Property(failed => failed.FailedDate)
                    .HasDefaultValueSql("GETDATE()");
            });
        }
    }
}
