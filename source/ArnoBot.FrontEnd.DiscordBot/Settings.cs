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
        [Obsolete("Use Settings.Prefixes instead.")]
        public string Prefix { get => internalSettings.Prefixes[0]; }
        public string[] Prefixes { get => internalSettings.Prefixes; }
        public bool ReactOnMention { get => internalSettings.ReactOnMention; }

        public string PlayingStatus
        {
            get => internalSettings.PlayingStatus;
            set
            {
                internalSettings.PlayingStatus = value;
                Save();
            }
        }

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

        internal void Save()
        {
            File.WriteAllText(FILE_PATH,
                JsonSerializer.Serialize(internalSettings, new JsonSerializerOptions() {
                    WriteIndented = true,
                }));
        }

        private class InternalSettings
        {
            public string BotToken { get; set; }
            public string[] Prefixes { get; set; }
            public bool ReactOnMention { get; set; }
            public string PlayingStatus { get; set; }
            public ulong[] Owners { get; set; }
        }
    }
}
