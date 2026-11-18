using Spectre.Console;

namespace TopModel.Utils.Cli;

public static class LogUtils
{
    private static readonly string[] Colors = ["teal", "olive", "yellow", "aqua"];

    extension(AnsiConsole)
    {
        public static void LogConfig(string configPath, int index, bool changed = false)
        {
            var color = Colors[index % Colors.Length];
            AnsiConsole.MarkupLine(
                $"[{color}]#{index + 1} - {Path.GetRelativePath(Directory.GetCurrentDirectory(), configPath)}[/]{(changed ? $": [teal]{CliMessage.ConfigChanged.GetMessage()}[/]" : string.Empty)}"
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
            AnsiConsole.MarkupLine(key.GetMessage(args));
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

    public static LoggingScope GetScope(int index)
    {
        return new LoggingScope(index + 1, Colors[index % Colors.Length]);
    }
}
