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
        private CommandRegistry commandRegistry = new CommandRegistry();

        public string Name => "Discord";

        public IReadOnlyCommandRegistry CommandRegistry => commandRegistry;

        public Settings ModuleSettings { get; }

        public DiscordCoreModule()
        {
            ModuleSettings = Settings.Load();

            commandRegistry.Add("invite", new InviteCommand());
            commandRegistry.Add("ping", new PingCommand());
            commandRegistry.Add("setplaying", new SetPlayingCommand());
            commandRegistry.Add("help", new HelpCommand(ModuleSettings.HelpReferenceSite));
        }
    }
}
