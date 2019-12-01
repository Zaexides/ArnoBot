using System;

using ArnoBot.Core;
using ArnoBot.Core.Responses;

using ArnoBot.Modules.Core;

namespace ArnoBot.FrontEnd.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
                ExecuteSingleCommand(args);
            else
                RunLoop();
        }

        static void ExecuteSingleCommand(string[] args)
        {
            Response response = InitializeBot().Query(string.Join(" ", args));
            Console.WriteLine(response);
        }

        static void RunLoop()
        {
            string input;
            Bot bot = InitializeBot();

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
