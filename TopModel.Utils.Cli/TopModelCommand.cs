using System.CommandLine;
using System.CommandLine.Help;
using System.Reflection;
using System.Text.RegularExpressions;
using Spectre.Console;

namespace TopModel.Utils.Cli;

public class TopModelCommand<TDescription> : IDisposable
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
    private readonly IList<IDisposable> _configObservers = [];
    private readonly CancellationTokenSource _cts = new();
    private readonly bool _noLog;
    private List<FileInfo> _configs = [];

    public TopModelCommand(TDescription description, IEnumerable<string> args, params IEnumerable<Option> options)
        : this(description.GetMessage(), args, noLog: false, options) { }

    public TopModelCommand(
        string description,
        IEnumerable<string> args,
        bool noLog = true,
        params IEnumerable<Option> options
    )
    {
        _noLog = noLog;
        _command = new RootCommand(description) { FileOption, WatchOption, CheckOption };
        _command.Options.AddRange(options);
        Args = _command.Parse(args.ToList());

        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            eventArgs.Cancel = true;
            _cts.Cancel();
            AnsiConsole.LogWarning(CliMessage.CancellationRequested);
        };
    }

    public CancellationToken CancellationToken => _cts.Token;

    public ParseResult Args { get; }

    /// <summary>
    /// Vérifie la version de l'outil et récupère les configs à traiter.
    /// </summary>
    /// <param name="nugetPackageName">Nom de l'outil dans Nuget.</param>
    /// <param name="configFilePattern">Pattern pour les fichiers de config.</param>
    /// <returns>True si aucun fichier de config n'a été trouvé.</returns>
    public async Task<bool> CheckVersionAndFindConfigs(string nugetPackageName, Regex configFilePattern)
    {
        try
        {
            if (!_noLog)
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
                    _cts.Token,
                    prerelease: prerelease
                );
                if (latestVersion != null && latestVersion.Version != version)
                {
                    AnsiConsole.LogWarning(CliMessage.NewVersionAvailable, latestVersion.Version!);
                    AnsiConsole.LogWarning(CliMessage.DotnetUpdateCommand, nugetPackageName);
                    AnsiConsole.WriteLine();
                }
            }

            var files = Args.GetValue(FileOption) ?? [];
            if (files.Any())
            {
                foreach (var file in files)
                {
                    if (!file.Exists)
                    {
                        if (!_noLog)
                        {
                            AnsiConsole.LogError(CliMessage.ConfigFileNotFound, file.FullName);
                        }
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
                if (!_noLog)
                {
                    AnsiConsole.LogError(CliMessage.NoConfigFileFound);
                }
                return true;
            }

            return false;
        }
        catch (OperationCanceledException)
        {
            return true;
        }
    }

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        _cts.Dispose();
        foreach (var configObserver in _configObservers)
        {
            configObserver.Dispose();
        }
    }

    /// <summary>
    /// Regarde si on a demandé l'aide ou le numéro de version (--help ou --version).
    /// </summary>
    /// <returns>True si c'est le cas.</returns>
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

    /// <summary>
    /// Lance les configs trouvées.
    /// </summary>
    /// <typeparam name="TConfig">Type de la config.</typeparam>
    /// <typeparam name="TFileChecker">Type de désérialiseur / vérificateur de schéma.</typeparam>
    /// <typeparam name="TWorker">Type du worker associé aux configs.</typeparam>
    /// <param name="configurator">Configurateur pour le worker.</param>
    /// <param name="onDispose">Appelé lorsque que le worker est arrêté/redémarré.</param>
    /// <returns>Exit Code.</returns>
    public async Task<int> RunConfigs<TConfig, TFileChecker, TWorker>(
        Action<TWorker>? configurator = null,
        Action<TWorker>? onDispose = null
    )
        where TConfig : ConfigBase
        where TFileChecker : AbstractFileChecker<TConfig>, new()
        where TWorker : TopModelWorker<TConfig, TFileChecker>, new()
    {
        var loggerProvider = new LoggerProvider();

        var watchMode = Args.GetValue(WatchOption);
        var checkMode = Args.GetValue(CheckOption);

        if (!_noLog)
        {
            if (watchMode)
            {
                AnsiConsole.LogInformation(CliMessage.WatchModeEnabled);
            }

            if (checkMode)
            {
                AnsiConsole.LogInformation(CliMessage.CheckModeEnabled);
            }

            AnsiConsole.LogInformation(CliMessage.ConfigFilesFound);
        }

        var fileChecker = new TFileChecker();

        for (var i = 0; i < _configs.Count; i++)
        {
            var config = _configs[i];
            if (!_noLog)
            {
                AnsiConsole.LogConfig(config.FullName, i);
            }
            _configObservers.Add(
                new ConfigObserver<TConfig, TFileChecker, TWorker>(
                    config,
                    fileChecker,
                    loggerProvider,
                    i,
                    configurator,
                    onDispose
                )
                {
                    NoLog = _noLog,
                }
            );
        }

        try
        {
            foreach (var configObserver in _configObservers)
            {
                await ((ConfigObserver<TConfig, TFileChecker, TWorker>)configObserver).Start(watchMode, _cts.Token);
            }

            if (_noLog)
            {
                return 0;
            }

            if (watchMode)
            {
                _cts.Token.WaitHandle.WaitOne();
            }

            if (_configObservers.Any(w => ((ConfigObserver<TConfig, TFileChecker, TWorker>)w).HasError))
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
        catch (OperationCanceledException)
        {
            return 1;
        }
    }
}
