using FileSort.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSort.Display.Interfaces
{
    internal interface IExtensionManager
    {
        void ShowExtensions();
        void AddExtension();
        void ShowExtension(Extension extension);
        bool EditExtensionName(Extension extension);
        bool ChangeExtensionCategory(Extension extension);
        bool RemoveExtension(Extension extension);
    }
}
