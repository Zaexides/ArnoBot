using System;

using ArnoBot.Core;
using ArnoBot.Core.Responses;

using ArnoBot.Modules.Core;

using ArnoBot.ModuleLoader;

namespace ArnoBot.FrontEnd.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot bot = InitializeBot();
            ModuleLoader.ModuleLoader.LoadModules(bot.ModuleRegistry);

            if (args.Length > 0)
                ExecuteSingleCommand(bot, args);
            else
                RunLoop(bot);
        }

        static void ExecuteSingleCommand(Bot bot, string[] args)
        {
            Response response = bot.Query(string.Join(" ", args));
            Console.WriteLine(response);
        }

        static void RunLoop(Bot bot)
        {
            string input;

            Console.WriteLine("Awaiting input.");
            Console.Write("> ");
            while ((input = Console.ReadLine()) != string.Empty)
            {
                Console.WriteLine(bot.Query(input));
                Console.Write("> ");
            }
        }

        static Bot InitializeBot()
        {
            Bot bot = Bot.CreateOrGet();
            bot.ModuleRegistry.RegisterModule(new CoreModule());
            return bot;
        }
    }
}
