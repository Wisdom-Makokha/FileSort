using FileSort.AppDirectory;
using FileSort.Data.Interfaces;
using FileSort.DataModels;
using FileSort.Display.Interfaces;
using FileSort.FileHandling;
using FileSort.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Display.Managers
{
    internal class SortManager : ISortManager
    {
        private readonly Guid _applicationInstanceId;
        private readonly string _destinationDirectoryPath;
        private readonly string _sourceDirectoryPath;
        private readonly List<Extension> _extensions;
        private readonly List<Category> _categories;
        private readonly List<Extension> _excludedExtensions;

        private readonly IFileDataModelRepository _fileRepository;
        private readonly IFailedMovesRepository _failedMovesRepository;
        private readonly IExtensionRepository _extensionRepository;

        public SortManager(
            IConfigurationService configurationService,
            IFileDataModelRepository fileRepository,
            IExtensionRepository extensionRepository,
            IFailedMovesRepository failedMovesRepository,
            ICategoryRepository categoryRepository,
            IApplicationInstanceRepository applicationInstanceRepository)
        {
            _applicationInstanceId = applicationInstanceRepository
                                        .GetAll().ToList()
                                        .OrderByDescending(i => i.InitiationTime)
                                        .FirstOrDefault()!.ApplicationId;
            _fileRepository = fileRepository;
            _failedMovesRepository = failedMovesRepository;
            _extensionRepository = extensionRepository;

            _destinationDirectoryPath = configurationService.AppSettings.DestinationFolder;
            _sourceDirectoryPath = configurationService.AppSettings.SourceFolder;

            _extensions = extensionRepository.GetAll().ToList();
            _categories = categoryRepository.GetAll().ToList();

            _excludedExtensions = _categories.FirstOrDefault(c => c.CategoryName == "excludedextensions")!.Extensions;
        }

        public void SortFiles()
        {
            var categoryNames = _categories.Select(c => c.CategoryName).ToList();
            
            var sourceDirectory = new SourceDirectory(_excludedExtensions, _sourceDirectoryPath);
            var destinationDirectory = new DestinationDirectory(categoryNames, _destinationDirectoryPath);

            Sort sort = new Sort(
                sourceDirectory,
                _destinationDirectoryPath,
                _applicationInstanceId,
                _fileRepository,
                _extensionRepository,
                _failedMovesRepository,
                _extensions,
                _categories);

            sort.SortFiles();
        }

        //private void ReverseSort()
        //{
        //    Reverse reverse = new Reverse(Startup.FailedMovesRepository, Startup.ApplicationInstance, Startup.FileDataModelRepository);

        //    reverse.ReverseSort();
        //}
    }
}
