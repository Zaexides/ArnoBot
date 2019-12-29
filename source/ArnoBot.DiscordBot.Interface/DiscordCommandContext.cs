using System;
using ArnoBot.Core;
using Discord.WebSocket;

namespace ArnoBot.DiscordBot.Interface
{
    public class DiscordCommandContext : CommandContext
    {
        public ISocketMessageChannel MessageChannel { get; }

        public SocketUser User { get; }

        public SocketUser Self { get; }
        
        protected DiscordCommandContext(string commandName, object[] parameters, ISocketMessageChannel messageChannel, SocketUser user, SocketUser self) : base(commandName, parameters)
        {
            this.MessageChannel = messageChannel;
            this.User = user;
            this.Self = self;
        }

        public DiscordCommandContext(CommandContext commandContext, ISocketMessageChannel messageChannel, SocketUser user, SocketUser self)
            : this(commandContext.CommandName, commandContext.Parameters, messageChannel, user, self)
        {
        }
    }
}
