using System;
using System.Linq;
using System.Threading.Tasks;

using Discord.WebSocket;

using ArnoBot.Core;
using ArnoBot.ModuleLoader;
using ArnoBot.Modules.Core;
using ArnoBot.Modules.DiscordCore;
using ArnoBot.DiscordBot.Interface;

namespace ArnoBot.FrontEnd.DiscordBot
{
    public class DiscordBot
    {
        private const int RECONNECT_DELAY = 10000;
        private DiscordSocketClient discordClient;

        public Settings Settings { get; }

        public DiscordBot()
        {
            Bot bot = InitializeBot();
            Settings = Settings.Load();
            InitializeDiscordIntegration(bot);
        }

        private Bot InitializeBot()
        {
            Bot bot = Bot.CreateOrGet();
            bot.ModuleRegistry.RegisterModule(new CoreModule());
            bot.ModuleRegistry.RegisterModule(new DiscordCoreModule());
            ModuleLoader.ModuleLoader.LoadModules(bot.ModuleRegistry);
            return bot;
        }

        private void InitializeDiscordIntegration(Bot bot)
        {
            discordClient = new DiscordSocketClient();
            new MessageHandler(discordClient, bot, Settings.Prefixes, Settings.ReactOnMention);
            new DiscordUtils(discordClient, Settings.Owners);

            DiscordUtils.Main.BotActivity = new Discord.Game(Settings.PlayingStatus);
            DiscordUtils.Main.activityChanged += OnActivityChanged;

            RegisterMessageReceivedEvents(bot.ModuleRegistry, discordClient);
        }

        private void OnActivityChanged(object sender, DiscordUtils.ActivityChangedEventArgs e)
            => Settings.PlayingStatus = e.NewActivity.Name;

        private void RegisterMessageReceivedEvents(ModuleRegistry moduleRegistry, DiscordSocketClient discordClient)
        {
            foreach (IMessageEventListenerModule messageListenerModule in moduleRegistry.GetModules().Where((module) => module is IMessageEventListenerModule))
                discordClient.MessageReceived += messageListenerModule.OnMessageReceived;
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
