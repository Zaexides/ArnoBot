using System;

namespace ArnoBot.Core
{
    public class Bot
    {
        private static Bot singleton;

        public ModuleRegistry ModuleRegistry { get; }

        private Bot()
        {
        }

        public static Bot CreateOrGet()
        {
            if(singleton == null)
            {
                singleton = new Bot();
            }
            return singleton;
        }
    }
}
