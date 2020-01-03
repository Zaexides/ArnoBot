using System;
using ArnoBot.Interface;

namespace ArnoBot.DiscordBot.Interface
{
    public static class Logger
    {
        public static void Log(IModule module, string message)
            => Log(LogType.Info, module, message);

        public static void LogDebug(IModule module, string message)
            => Log(LogType.Debug, module, message);

        public static void LogWarning(IModule module, string message)
            => Log(LogType.Warning, module, message);

        public static void LogError(IModule module, string message)
            => Log(LogType.Error, module, message);

        public static void LogError(IModule module, Exception exception)
            => Log(LogType.Error, module, exception.ToString());

        private static void Log(LogType logType, IModule module, string message)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = GetColorFromLogType(logType);
            Console.WriteLine($"({DateTime.Now.ToString()}) [{logType.ToString()}]:[{module.Name}] - {message}");
            Console.ForegroundColor = originalColor;
        }

        private static ConsoleColor GetColorFromLogType(LogType logType)
        {
            switch(logType)
            {
                case LogType.Debug:
                    return ConsoleColor.Gray;
                case LogType.Warning:
                    return ConsoleColor.Yellow;
                case LogType.Error:
                    return ConsoleColor.Red;
                default:
                    return ConsoleColor.White;
            }
        }

        private enum LogType
        {
            Debug,
            Info,
            Warning,
            Error,
        }
    }
}
