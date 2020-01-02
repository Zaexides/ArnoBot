using System;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

namespace ArnoBot.DiscordBot.Interface
{
    public class DiscordUtils
    {
        private DiscordSocketClient client;

        public static DiscordUtils Main { get; private set; }

        public ulong BotID { get => client.CurrentUser.Id; }

        public UserStatus BotUserStatus
        {
            get => client.Status;
            set
            {
                Task.Run(async () =>
                {
                    await client.SetStatusAsync(value);
                });
            }
        }

        public IActivity BotActivity
        {
            get => client.Activity;
            set
            {
                Task.Run(async () =>
                {
                    await client.SetActivityAsync(value);
                });
            }
        }

        public int Latency => client.Latency;

        public DiscordUtils(DiscordSocketClient client)
        {
            Main = this;
            this.client = client;
        }

        public SocketChannel GetChannelByID(ulong channelId) => client.GetChannel(channelId);
        public SocketGuild GetGuildByID(ulong guildId) => client.GetGuild(guildId);
        public SocketUser GetUserByID(ulong userId) => client.GetUser(userId);
    }
}
