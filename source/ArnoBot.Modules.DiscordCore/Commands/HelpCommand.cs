using System;
using ArnoBot.Core;
using ArnoBot.Core.Responses;
using ArnoBot.DiscordBot.Interface;

namespace ArnoBot.Modules.DiscordCore.Commands
{
    public class HelpCommand : IDiscordCommand
    {
        private string helpReferenceSite;

        public bool IsNSFW => false;

        public HelpCommand(string helpReferenceSite)
        {
            this.helpReferenceSite = helpReferenceSite;
        }

        public Response Execute(DiscordCommandContext commandContext)
        {
            if (helpReferenceSite == null || helpReferenceSite.Equals(string.Empty))
                return new TextResponse(Response.Type.Error, "The bot owner did not include a help reference site URL.");
            else
                return new ExtendedResponse(Response.Type.Executed,
                    new ExtendedResponse.Content.Builder()
                        .AddParagraph("Help Reference Site:", helpReferenceSite)
                        .Build()
                    );
        }

        public Response Execute(CommandContext context)
        {
            throw new NotImplementedException();
        }
    }
}
