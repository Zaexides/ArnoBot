using System;

namespace ArnoBot.Core
{
    public class Bot : IDisposable
    {
        private static Bot singleton;

        public ModuleRegistry ModuleRegistry { get; }

        private Bot()
        {
            ModuleRegistry = new ModuleRegistry();
        }

        public static Bot CreateOrGet()
        {
            if(singleton == null)
            {
                singleton = new Bot();
            }
            return singleton;
        }

        public void Dispose()
        {
            singleton = null;
        }
    }
}
