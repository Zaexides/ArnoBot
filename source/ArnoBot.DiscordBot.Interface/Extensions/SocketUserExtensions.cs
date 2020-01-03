using System;

using Discord.WebSocket;

namespace ArnoBot.DiscordBot.Interface.Extensions
{
    public static class SocketUserExtensions
    {
        public static string GetNicknameOrUsername(this SocketUser socketUser)
        {
            if (socketUser is SocketGuildUser)
            {
                SocketGuildUser guildUser = socketUser as SocketGuildUser;
                if (guildUser.Nickname == null || guildUser.Nickname.Equals(string.Empty))
                    return guildUser.Username;
                else
                    return guildUser.Nickname;
            }
            else
                return socketUser.Username;
        }
    }
}
