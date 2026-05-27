using System.CommandLine;
using System.Reflection;
using System.Text.RegularExpressions;
using Spectre.Console;

namespace TopModel.Utils.Cli;

public static class TopModelCli
{
    public static Option<IEnumerable<FileInfo>> FileOption { get; } =
        new("--file", "-f") { Description = LocalizeUtils.Localize(CliMessage.FileOptionDescription) };

    public static Option<bool> WatchOption { get; } =
        new("--watch", "-w") { Description = LocalizeUtils.Localize(CliMessage.WatchOptionDescription) };

    public static Option<bool> CheckOption { get; } =
        new("--check", "-c") { Description = LocalizeUtils.Localize(CliMessage.CheckOptionDescription) };

    public static string[] Colors => LogUtils.Colors;

    public static void ListFoundFiles(IEnumerable<string> filePaths)
    {
        LogUtils.LogInformation(CliMessage.ConfigFilesFound);
        var i = 0;
        foreach (var path in filePaths)
        {
            var color = LogUtils.Colors[i % LogUtils.Colors.Length];
            AnsiConsole.MarkupLine(
                $"[{color}]#{i + 1} - {Path.GetRelativePath(Directory.GetCurrentDirectory(), path)}[/]"
            );
            i++;
        }
    }

    public static IEnumerable<FileInfo> ResolveFiles(IEnumerable<FileInfo> files, Regex pattern)
    {
        if (files.Any())
        {
            foreach (var file in files)
            {
                if (!file.Exists)
                {
                    LogUtils.LogError(CliMessage.ConfigFileNotFound, file.FullName);
                }
                else
                {
                    yield return file;
                }
            }
        }
        else
        {
            foreach (var file in ConfigUtils.FindConfigFiles(Directory.GetCurrentDirectory(), pattern))
            {
                yield return file;
            }
        }
    }

    public static async Task StartPackage(string nugetPackageName, CancellationToken cancellationToken)
    {
        var version = Assembly
            .GetEntryAssembly()!
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
            .InformationalVersion;

        AnsiConsole.MarkupLine($"========= {nugetPackageName} v{version} =========");
        AnsiConsole.WriteLine();

        var prerelease = version.Contains('-');
        var latestVersion = await NugetUtils.GetLatestVersionAsync(
            nugetPackageName,
            cancellationToken,
            prerelease: prerelease
        );
        if (latestVersion != null && latestVersion.Version != version)
        {
            LogUtils.LogWarning(CliMessage.NewVersionAvailable, latestVersion.Version!);
            LogUtils.LogWarning(CliMessage.DotnetUpdateCommand, nugetPackageName);
            AnsiConsole.WriteLine();
        }
    }
}
