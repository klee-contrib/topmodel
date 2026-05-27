using System.CommandLine;
using System.Reflection;
using System.Text.RegularExpressions;
using Spectre.Console;

namespace TopModel.Utils.Cli;

public static class TopModelCli
{
    public static Option<IEnumerable<FileInfo>> FileOption { get; } =
        new("--file", "-f") { Description = CliMessage.FileOptionDescription.GetMessage() };

    public static Option<bool> WatchOption { get; } =
        new("--watch", "-w") { Description = CliMessage.WatchOptionDescription.GetMessage() };

    public static Option<bool> CheckOption { get; } =
        new("--check", "-c") { Description = CliMessage.CheckOptionDescription.GetMessage() };

    public static void ListFoundFiles(IEnumerable<string> filePaths)
    {
        AnsiConsole.LogInformation(CliMessage.ConfigFilesFound);
        var i = 0;
        foreach (var path in filePaths)
        {
            AnsiConsole.LogConfig(path, i);
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
                    AnsiConsole.LogError(CliMessage.ConfigFileNotFound, file.FullName);
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
            AnsiConsole.LogWarning(CliMessage.NewVersionAvailable, latestVersion.Version!);
            AnsiConsole.LogWarning(CliMessage.DotnetUpdateCommand, nugetPackageName);
            AnsiConsole.WriteLine();
        }
    }
}
