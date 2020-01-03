using System;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace ArnoBot.DiscordBot.Interface
{
    public interface IMessageEventListenerModule
    {
        Task OnMessageReceived(SocketMessage message);
    }
}
