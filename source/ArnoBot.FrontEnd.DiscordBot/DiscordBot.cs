using System;
using System.Threading.Tasks;

using Discord.WebSocket;

using ArnoBot.Core;
using ArnoBot.ModuleLoader;
using ArnoBot.Modules.Core;

namespace ArnoBot.FrontEnd.DiscordBot
{
    public class DiscordBot
    {
        private const int RECONNECT_DELAY = 10000;

        private Bot bot;
        private DiscordSocketClient discordClient;

        public Settings Settings { get; }

        public DiscordBot()
        {
            bot = InitializeBot();
            Settings = Settings.Load();
            InitializeDiscordIntegration();
        }

        private Bot InitializeBot()
        {
            Bot bot = Bot.CreateOrGet();
            bot.ModuleRegistry.RegisterModule(new CoreModule());
            ModuleLoader.ModuleLoader.LoadModules(bot.ModuleRegistry);
            return bot;
        }

        private void InitializeDiscordIntegration()
        {
            discordClient = new DiscordSocketClient();
        }

        public async Task Run()
        {
            while(true)
            {
                try
                {
                    await discordClient.LoginAsync(Discord.TokenType.Bot, Settings.BotToken);
                    await discordClient.StartAsync();
                    await Task.Delay(-1);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                    await Task.Delay(RECONNECT_DELAY);
                }
            }
        }
    }
}
