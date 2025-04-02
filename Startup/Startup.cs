using FileSort.Data;
using FileSort.Data.Interfaces;
using FileSort.DataModels;
using FileSort.Display;
using FileSort.Migrations;
using FileSort.Data.Repositories;
using FileSort.Services;
using FileSort.Settings;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Startup
{
    internal class Startup
    {
        // properties
        private readonly IConfigurationService _configurationService;
        private readonly IDataService _dataService;

        public IApplicationInstanceRepository ApplicationInstanceRepository { get; }
        public IFileDataModelRepository FileDataModelRepository { get; }
        public IExtensionRepository ExtensionRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IFailedMovesRepository FailedMovesRepository { get; }

        public ApplicationInstance ApplicationInstance { get; private set; }
        public AppSettings AppSettings => _configurationService.AppSettings;
        public List<Category> Categories => _dataService.Categories;
        public List<Extension> Extensions => _dataService.Extensions;

        public Startup(
            IConfigurationService configurationService, 
            IDataService dataService, 
            IApplicationInstanceRepository applicationInstanceRepository, 
            IFileDataModelRepository fileDataModelRepository, 
            IExtensionRepository extensionRepository, 
            ICategoryRepository categoryRepository, 
            IFailedMovesRepository failedMovesRepository)
        {
            _configurationService = configurationService;
            _dataService = dataService;
            ApplicationInstanceRepository = applicationInstanceRepository;
            FileDataModelRepository = fileDataModelRepository;
            ExtensionRepository = extensionRepository;
            CategoryRepository = categoryRepository;
            FailedMovesRepository = failedMovesRepository;
        }

        public void Initialize()
        {
            ApplicationInstance = _dataService.CreateApplicationInstance(DateTime.Now);
            _dataService.EnsureDatabaseSeeded();
            _dataService.LoadInitialData();
        }
    }
}
