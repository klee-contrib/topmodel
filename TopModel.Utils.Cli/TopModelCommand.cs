using System.CommandLine;
using System.CommandLine.Help;
using System.Reflection;
using System.Text.RegularExpressions;
using Spectre.Console;

namespace TopModel.Utils.Cli;

public class TopModelCommand<TDescription>
    where TDescription : struct, Enum
{
    private readonly Option<bool> CheckOption = new("--check", "-c")
    {
        Description = CliMessage.CheckOptionDescription.GetMessage(),
    };
    private readonly Option<IEnumerable<FileInfo>> FileOption = new("--file", "-f")
    {
        Description = CliMessage.FileOptionDescription.GetMessage(),
    };
    private readonly Option<bool> WatchOption = new("--watch", "-w")
    {
        Description = CliMessage.WatchOptionDescription.GetMessage(),
    };

    private readonly RootCommand _command;

    private List<FileInfo> _configs = [];

    public TopModelCommand(TDescription description, IReadOnlyList<string> args, params IEnumerable<Option> options)
    {
        _command = new RootCommand(description.GetMessage()) { FileOption, WatchOption, CheckOption };
        _command.Options.AddRange(options);
        Args = _command.Parse(args);
    }

    public ParseResult Args { get; }

    public async Task<bool> FindConfigs(
        string nugetPackageName,
        Regex configFilePattern,
        CancellationToken cancellationToken
    )
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

        var files = Args.GetValue(FileOption) ?? [];
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
                    _configs.Add(file);
                }
            }
        }
        else
        {
            _configs = ConfigUtils.FindConfigFiles(Directory.GetCurrentDirectory(), configFilePattern).ToList();
        }

        if (_configs.Count == 0)
        {
            AnsiConsole.LogError(CliMessage.NoConfigFileFound);
            return true;
        }

        return false;
    }

    public async Task<bool> IsHelpOrVersionRequested()
    {
        var helpOption = _command.Options.OfType<HelpOption>().Single();
        var versionOption = _command.Options.OfType<VersionOption>().Single();

        if (Args?.GetResult(helpOption) != null || Args?.GetResult(versionOption) != null)
        {
            await Args.InvokeAsync(cancellationToken: default);
            return true;
        }

        return false;
    }

    public async Task<int> RunConfigs<TConfig, TFileChecker, TWorker>(
        TFileChecker fileChecker,
        CancellationToken ct,
        Action<TWorker>? configurator = null
    )
        where TConfig : ConfigBase
        where TFileChecker : AbstractFileChecker<TConfig>
        where TWorker : TopModelWorker<TConfig, TFileChecker>, new()
    {
        var loggerProvider = new LoggerProvider();

        var watchMode = Args.GetValue(WatchOption);
        var checkMode = Args.GetValue(CheckOption);

        if (watchMode)
        {
            AnsiConsole.LogInformation(CliMessage.WatchModeEnabled);
        }

        if (checkMode)
        {
            AnsiConsole.LogInformation(CliMessage.CheckModeEnabled);
        }

        AnsiConsole.LogInformation(CliMessage.ConfigFilesFound);

        IList<ConfigObserver<TConfig, TFileChecker, TWorker>> configObservers = [];

        for (var i = 0; i < _configs.Count; i++)
        {
            var config = _configs[i];
            AnsiConsole.LogConfig(config.FullName, i);
            configObservers.Add(
                new ConfigObserver<TConfig, TFileChecker, TWorker>(config, fileChecker, loggerProvider, i, configurator)
            );
        }

        try
        {
            foreach (var configObserver in configObservers)
            {
                await configObserver.Start(ct);
            }

            if (watchMode)
            {
                ct.WaitHandle.WaitOne();
            }

            if (configObservers.Any(w => w.HasError))
            {
                return 1;
            }

            if (checkMode && loggerProvider.Changes > 0)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.LogError(
                    loggerProvider.Changes == 1
                        ? CliMessage.OneFileModifiedInCheckMode
                        : CliMessage.MultipleFilesModifiedInCheckMode,
                    loggerProvider.Changes
                );

                return 1;
            }

            return 0;
        }
        finally
        {
            foreach (var configObserver in configObservers)
            {
                configObserver.Dispose();
            }
        }
    }
}
