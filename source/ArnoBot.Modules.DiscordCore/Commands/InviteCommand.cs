using System;
using ArnoBot.Core;
using ArnoBot.Core.Responses;
using ArnoBot.DiscordBot.Interface;

namespace ArnoBot.Modules.DiscordCore.Commands
{
    public class InviteCommand : IDiscordCommand
    {
        private const string INVITE_LINK_FORMAT = "https://discordapp.com/api/oauth2/authorize?client_id={0}&scope=bot&permissions=8";

        public bool IsNSFW => false;

        public Response Execute(DiscordCommandContext commandContext)
        {
            return new TextResponse(Response.Type.Executed,
                string.Format(INVITE_LINK_FORMAT, commandContext.Self.Id)
                );
        }

        public Response Execute(CommandContext context)
        {
            throw new NotImplementedException();
        }
    }
}
