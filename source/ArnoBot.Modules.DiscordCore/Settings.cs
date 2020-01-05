using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace ArnoBot.Modules.DiscordCore
{
    public class Settings
    {
        private const string FILE_PATH = "Resources/DiscordCoreSettings.json";

        private InternalSettings internalSettings;

        public string HelpReferenceSite { get => internalSettings.HelpReferenceSite; }

        private Settings(InternalSettings internalSettings)
        {
            this.internalSettings = internalSettings;
        }

        public static Settings Load()
        {
            InternalSettings internalSettings = JsonSerializer.Deserialize<InternalSettings>(File.ReadAllText(FILE_PATH));
            return new Settings(internalSettings);
        }

        private class InternalSettings
        {
            public string HelpReferenceSite { get; set; }
        }
    }
}
