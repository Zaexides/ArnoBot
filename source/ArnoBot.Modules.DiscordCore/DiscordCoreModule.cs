using System;
using System.Collections.Generic;
using ArnoBot.DiscordBot.Interface;
using ArnoBot.Interface;

namespace ArnoBot.Modules.DiscordCore
{
    public class DiscordCoreModule : IDiscordModule
    {
        private Dictionary<string, ICommand> commandRegistry = new Dictionary<string, ICommand>();

        public string Name => "Discord";

        public IReadOnlyDictionary<string, ICommand> CommandRegistry => commandRegistry;

        
    }
}
