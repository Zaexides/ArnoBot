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
            get => moduleRegistry[moduleName];
        }

        public void RegisterModule(IModule module)
        {
            if (moduleRegistry.ContainsKey(module.Name))
                throw new DuplicateRegistryException($"I was asked to register a module with the name \"{module.Name}\", but there already is a module registered with that name.\nEither a module was being registered a second time, or two modules share the same name.");
            moduleRegistry.Add(module.Name, module);
        }
    }
}
