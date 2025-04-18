﻿using FileSort.Data;
using FileSort.Data.Interfaces;
using FileSort.DataModels;

namespace FileSort.Services
{
    internal class DataService :IDataService
    {
        //private readonly ApplicationDBContext _dbContext;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IExtensionRepository _extensionRepository;
        private readonly IApplicationInstanceRepository _instanceRepository;

        public DataService(ICategoryRepository categoryRepository, IExtensionRepository extensionRepository, IApplicationInstanceRepository instanceRepository)
        {
            _categoryRepository = categoryRepository;
            _extensionRepository = extensionRepository;
            _instanceRepository = instanceRepository;
        }

        //public ApplicationDBContext Context => _dbContext;
        public List<Category> Categories { get; private set; } = new List<Category>();
        public List<Extension> Extensions { get; private set; } = new List<Extension>();


        public ApplicationInstance CreateApplicationInstance(DateTime dateTime)
        {
            var instance = new ApplicationInstance { InitiationTime = dateTime };
            _instanceRepository.AddEntity(instance);
            _instanceRepository.SaveChanges();

            return instance;
        }

        public void EnsureDatabaseSeeded()
        {
            DbInitializer.SeedDatabase(_categoryRepository, _extensionRepository);
        }

        public void LoadInitialData()
        {
            Categories = _categoryRepository.GetAll().ToList();
            Extensions = _extensionRepository.GetAll().ToList();
        }
    }
}
