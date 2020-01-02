using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArnoBot.DiscordBot.Interface;
using ArnoBot.Interface;
using Discord.WebSocket;

using ArnoBot.Modules.DiscordCore.Commands;

namespace ArnoBot.Modules.DiscordCore
{
    public class DiscordCoreModule : IDiscordModule
    {
        private Dictionary<string, ICommand> commandRegistry = new Dictionary<string, ICommand>();

        public string Name => "Discord";

        public IReadOnlyDictionary<string, ICommand> CommandRegistry => commandRegistry;

        public DiscordCoreModule()
        {
            commandRegistry.Add("invite", new InviteCommand());
            commandRegistry.Add("ping", new PingCommand());
        }
    }
}
