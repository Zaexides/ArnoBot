using System;
using System.Collections.Generic;
using System.Text;

using ArnoBot.Interface;

using ArnoBot.Modules.Core.Commands;

namespace ArnoBot.Modules.Core
{
    public class CoreModule : IModule
    {
        private Dictionary<string, ICommand> commandRegistry = new Dictionary<string, ICommand>();

        public string Name => "Core";

        public IReadOnlyDictionary<string, ICommand> CommandRegistry => commandRegistry;

        public CoreModule()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            commandRegistry.Add("about", new AboutCommand());
        }
    }
}
