using System;
using System.Linq;
using Discord;
using ArnoBot.Core;
using ArnoBot.Core.Responses;
using ArnoBot.DiscordBot.Interface;

namespace ArnoBot.Modules.DiscordCore.Commands
{
    public class SetPlayingCommand : IDiscordCommand
    {
        public bool IsNSFW => false;

        public Response Execute(DiscordCommandContext commandContext)
        {
            if (!DiscordUtils.Main.IsUserBotOwner(commandContext.User))
                return new TextResponse(Response.Type.Error, "This command can only be used by a bot owner.");

            string playingStatus = string.Join(" ", commandContext.Parameters);
            DiscordUtils.Main.BotActivity = new Game(playingStatus, ActivityType.Playing);
            return new TextResponse(Response.Type.Executed, "Playing status changed.");
        }

        public Response Execute(CommandContext context)
        {
            throw new NotImplementedException();
        }
    }
}
