using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

using ArnoBot.Interface;
using ArnoBot.Core.Responses;
using ArnoBot.Core;

namespace ArnoBot.Modules.Core.Commands
{
    public class AboutCommand : ICommand
    {
        private const string BOT_INFO_FILEPATH = "Resources/BotInfo.json";
        private static BotInfo botInfo = null;

        public Response Execute(CommandContext context)
        {
            if (botInfo == null)
                botInfo = JsonSerializer.Deserialize<BotInfo>(File.ReadAllText(BOT_INFO_FILEPATH));

            ExtendedResponse.Content responseContent = new ExtendedResponse.Content.Builder()
                .SetTitle("Bot Info")
                .AddParagraph("Bot Name", botInfo.Name)
                .AddParagraph("Version", botInfo.Version)
                .AddParagraph("Bot Owner", botInfo.Owner)
                .AddParagraph("Site", botInfo.Site)
                .Build();

            return new ExtendedResponse(Response.Type.Executed, responseContent);
        }

        private class BotInfo
        {
            public string Name { get; set; }
            public string Owner { get; set; }
            public string Version { get; set; }
            public string Site { get; set; }
        }
    }
}
