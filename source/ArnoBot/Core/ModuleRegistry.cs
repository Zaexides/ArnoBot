using System;
using System.Collections.Generic;
using System.Text;

using ArnoBot.Interface;

namespace ArnoBot.Core
{
    public class ModuleRegistry
    {
        private Dictionary<string, IModule> moduleRegistry = new Dictionary<string, IModule>();

        internal ModuleRegistry()
        {
        }

        public IModule this[string moduleName]
        {
            get => null;
        }

        public void RegisterModule(IModule module)
        {
        }
    }
}
