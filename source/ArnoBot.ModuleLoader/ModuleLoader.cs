using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using ArnoBot.Core;
using ArnoBot.Interface;

namespace ArnoBot.ModuleLoader
{
    public static class ModuleLoader
    {
        private const string MODULE_FOLDER = "/modules";

        public static void LoadModules(ModuleRegistry moduleRegistry)
        {
            IEnumerable<Assembly> libaries = LoadLibraries();
            IEnumerable<IModule> modules = LoadModules(libaries);
            RegisterModules(modules, moduleRegistry);
        }

        private static IEnumerable<Assembly> LoadLibraries()
        {
            string directoryPath = Environment.CurrentDirectory + MODULE_FOLDER;
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            string[] libraryPaths = Directory.GetFiles(
                Environment.CurrentDirectory + MODULE_FOLDER,
                "*.dll",
                SearchOption.AllDirectories
                );

            foreach(string path in libraryPaths)
            {
                Assembly loadedLibrary = LoadLibrary(path);
                if (loadedLibrary != null)
                    yield return loadedLibrary;
            }
        }

        private static Assembly LoadLibrary(string path)
        {
            Assembly library;

            try
            {
                library = Assembly.LoadFrom(path);
            }
            catch(IOException ioEx)
            {
                Console.WriteLine(ioEx);
                library = null;
            }
            return library;
        }

        private static IEnumerable<IModule> LoadModules(IEnumerable<Assembly> libraries)
        {
            foreach(Assembly assembly in libraries)
            {
                IEnumerable<IModule> modules = LoadModulesInAssembly(assembly);
                foreach (IModule module in modules)
                    yield return module;
            }
        }

        private static IEnumerable<IModule> LoadModulesInAssembly(Assembly assembly)
        {
            IEnumerable<Type> moduleTypes =
                assembly.GetTypes()
                .Where((type) => (typeof(IModule)).IsAssignableFrom(type))
                .Where((type) => (!type.IsAbstract && !type.IsInterface));

            foreach (Type moduleType in moduleTypes)
            {
                IModule module;
                try
                {
                    module = (IModule)Activator.CreateInstance(moduleType);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    module = null;
                }

                if(module != null)
                    yield return module;
            }
        }

        private static void RegisterModules(IEnumerable<IModule> modules, ModuleRegistry moduleRegistry)
        {
            foreach (IModule module in modules)
                moduleRegistry.RegisterModule(module);
        }
    }
}
