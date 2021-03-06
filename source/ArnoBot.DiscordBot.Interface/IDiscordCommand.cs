﻿using System;
using ArnoBot.Interface;
using ArnoBot.Core;
using ArnoBot.Core.Responses;
using Discord.WebSocket;

namespace ArnoBot.DiscordBot.Interface
{
    public interface IDiscordCommand : ICommand
    {
        bool IsNSFW { get; }
        Response Execute(DiscordCommandContext commandContext);
    }
}
