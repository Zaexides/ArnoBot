using System;
using System.Collections.Generic;
using System.Text;

using ArnoBot.Interface;

using ArnoBot.Modules.Core.Commands;

namespace ArnoBot.Modules.Core
{
    public class CoreModule : IModule
    {
        private CommandRegistry commandRegistry = new CommandRegistry();

        public string Name => "Core";

        public IReadOnlyCommandRegistry CommandRegistry => commandRegistry;

        public CoreModule()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            commandRegistry.Add("about", new AboutCommand());
            commandRegistry.Add("echo", new EchoCommand());
        }
    }
}
