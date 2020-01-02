using System;
using ArnoBot.Core;
using ArnoBot.Core.Responses;
using ArnoBot.DiscordBot.Interface;

namespace ArnoBot.Modules.DiscordCore.Commands
{
    public class PingCommand : IDiscordCommand
    {
        public bool IsNSFW => false;

        public Response Execute(DiscordCommandContext commandContext)
        {
            return new TextResponse(Response.Type.Executed, "Pong! My latency is: " + DiscordUtils.Main.Latency + "ms");
        }

        public Response Execute(CommandContext context)
        {
            throw new NotImplementedException();
        }
    }
}
