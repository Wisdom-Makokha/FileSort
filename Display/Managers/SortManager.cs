using FileSort.AppDirectory;
using FileSort.Data.Interfaces;
using FileSort.DataModels;
using FileSort.Display.Interfaces;
using FileSort.FileHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Display.Managers
{
    internal class SortManager : ISortManager
    {
        private readonly SourceDirectory _sourceDirectory;
        private readonly Guid _applicationInstanceId;
        private readonly string _destinationDirectoryPath;
        private readonly IFileDataModelRepository _fileRepository;
        private readonly IExtensionRepository _extensionRepository;
        private readonly IFailedMovesRepository _failedMovesRepository;
        private readonly List<Extension> _extensions;
        private readonly List<Category> _categories;

        public SortManager(
            SourceDirectory sourceDirectory,
            Guid applicationInstanceId, 
            string destinationDirectoryPath,
            IFileDataModelRepository fileRepository,
            IExtensionRepository extensionRepository,
            IFailedMovesRepository failedMovesRepository,
            List<Extension> extensions,
            List<Category> categories)
        {
            _sourceDirectory = sourceDirectory;
            _applicationInstanceId = applicationInstanceId;
            _destinationDirectoryPath = destinationDirectoryPath;
            _fileRepository = fileRepository;
            _extensionRepository = extensionRepository;
            _failedMovesRepository = failedMovesRepository;
            _extensions = extensions;
            _categories = categories;
        }

        public void SortFiles()
        {
            Sort sort = new Sort(
                _sourceDirectory,
                _destinationDirectoryPath,
                _applicationInstanceId,
                _fileRepository,
                _extensionRepository,
                _failedMovesRepository,
                _extensions,
                _categories);

            sort.SortFiles();
        }
    }
}
