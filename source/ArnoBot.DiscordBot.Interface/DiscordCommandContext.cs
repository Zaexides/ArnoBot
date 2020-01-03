using System;
using ArnoBot.Core;
using Discord.WebSocket;

namespace ArnoBot.DiscordBot.Interface
{
    public class DiscordCommandContext : CommandContext
    {
        public SocketMessage TriggeringMessage { get; }

        public ISocketMessageChannel MessageChannel { get; }

        public SocketUser User { get; }

        public SocketUser Self { get; }
        
        protected DiscordCommandContext(string commandName, object[] parameters, SocketMessage triggeringMessage, ISocketMessageChannel messageChannel, SocketUser user, SocketUser self) : base(commandName, parameters)
        {
            this.TriggeringMessage = triggeringMessage;
            this.MessageChannel = messageChannel;
            this.User = user;
            this.Self = self;
        }

        public DiscordCommandContext(CommandContext commandContext, SocketMessage triggeringMessage, ISocketMessageChannel messageChannel, SocketUser user, SocketUser self)
            : this(commandContext.CommandName, commandContext.Parameters, triggeringMessage, messageChannel, user, self)
        {
        }
    }
}
