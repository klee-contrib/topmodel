using Spectre.Console;

namespace TopModel.Utils.Cli;

public static class LogUtils
{
    public static readonly string[] Colors = ["teal", "olive", "yellow", "aqua"];

    public static void Log<T>(T key, string color, params object[] args)
        where T : struct, Enum
    {
        AnsiConsole.MarkupLine($"[{color}]{LocalizeUtils.Localize(key, args)}[/]");
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

    public static void LogSuccess<T>(T key, params object[] args)
        where T : struct, Enum
    {
        Log(key, "green", args);
    }

    public static void LogWarning<T>(T key, params object[] args)
        where T : struct, Enum
    {
        Log(key, "yellow", args);
    }
}
