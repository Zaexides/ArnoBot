using System;
using System.Threading.Tasks;

namespace ArnoBot.FrontEnd.DiscordBot
{
    class Program
    {
        static void Main(string[] args)
        {
            DiscordBot bot = new DiscordBot();
            Task.Run(() => bot.Run()).Wait();
        }
    }
}
