using FileSort.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Data.Interfaces
{
    internal interface IExtensionRepository : IBaseRepository<Extension>
    {
        int GetExtensionId(string extensionName);
        string GetExtensionName(int  extensionId);
    }
}
