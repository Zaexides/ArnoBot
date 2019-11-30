using System;
using System.Collections.Generic;
using System.Text;

using ArnoBot.Interface;

namespace ArnoBot.Modules.Core
{
    public class CoreModule : IModule
    {
        private Dictionary<string, ICommand> commandRegistry;

        public string Name => "Core";

        public IReadOnlyDictionary<string, ICommand> CommandRegistry => commandRegistry;

        public CoreModule()
        {

        }
    }
}
