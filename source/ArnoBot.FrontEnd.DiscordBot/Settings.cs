using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace ArnoBot.FrontEnd.DiscordBot
{
    public class Settings
    {
        private const string FILE_PATH = "Resources/Settings.json";

        private InternalSettings internalSettings;

        internal string BotToken { get => internalSettings.BotToken; }
        public string Prefix { get => internalSettings.Prefix; }

        public IReadOnlyList<ulong> Owners { get => internalSettings.Owners; }

        private Settings(InternalSettings internalSettings)
        {
            this.internalSettings = internalSettings;
        }

        internal static Settings Load()
        {
            InternalSettings internalSettings = JsonSerializer.Deserialize<InternalSettings>(File.ReadAllText(FILE_PATH));
            return new Settings(internalSettings);
        }

        private class InternalSettings
        {
            public string BotToken { get; set; }
            public string Prefix { get; set; }

            public ulong[] Owners { get; set; }
        }
    }
}
