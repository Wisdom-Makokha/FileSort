﻿using FileSort.Data;
using FileSort.DataModels;
using FileSort.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Repositories
{
    internal class ExtensionRepository : BaseRepository<Extension>, IExtensionRepository
    {
        public ExtensionRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
        }

        public int GetExtensionId(string extensionName)
        {
            return _dbContext.Extensions.Single(e => e.ExtensionName == extensionName).Id;
        }

        public string GetExtensionName(int extensionId)
        {
            return GetById(extensionId)!.ExtensionName;
        }
    }
}
