#pragma warning disable KTA1200

using System.Resources;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace TopModel.Utils.Cli;

public static class LogUtils
{
    private static readonly string[] Colors = ["teal", "olive", "yellow", "aqua"];

    public static LoggingScope GetScope(int index)
    {
        return new LoggingScope(index + 1, Colors[index % Colors.Length]);
    }

    extension<T>(T messageType)
        where T : struct, Enum
    {
        public string GetMessage(params object[] args)
        {
            var format =
                ResourceManagerCache<T>.Instance.GetString(Enum.GetName(messageType)!) ?? Enum.GetName(messageType)!;
            return args.Length > 0 ? string.Format(format, args) : format;
        }
    }

    extension(AnsiConsole)
    {
        public static void LogConfig(string configPath, int index)
        {
            var color = Colors[index % Colors.Length];
            AnsiConsole.MarkupLine(
                $"[{color}]#{index + 1} - {Path.GetRelativePath(Directory.GetCurrentDirectory(), configPath)}[/]"
            );
        }

        public static void LogError<T>(T key, params object[] args)
            where T : struct, Enum
        {
            Log(key, "red", args);
        }

        public static void LogInformation<T>(T key, params object[] args)
            where T : struct, Enum
        {
            Log(key, "white", args);
        }

        public static void LogWarning<T>(T key, params object[] args)
            where T : struct, Enum
        {
            Log(key, "yellow", args);
        }

        private static void Log<T>(T key, string color, params object[] args)
            where T : struct, Enum
        {
            AnsiConsole.MarkupLine($"[{color}]{key.GetMessage(args)}[/]");
        }
    }

    extension(ILogger logger)
    {
        public void LogError<T>(T key, params object[] args)
            where T : struct, Enum
        {
            logger.LogError(key.GetMessage(args));
        }

        public void LogInformation<T>(T key, params object[] args)
            where T : struct, Enum
        {
            logger.LogInformation(key.GetMessage(args));
        }

        public void LogWarning<T>(T key, params object[] args)
            where T : struct, Enum
        {
            logger.LogWarning(key.GetMessage(args));
        }
    }

    private static class ResourceManagerCache<T>
        where T : struct, Enum
    {
        public static readonly ResourceManager Instance = new(typeof(T));
    }
}
