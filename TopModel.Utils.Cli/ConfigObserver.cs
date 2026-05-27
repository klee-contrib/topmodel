using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Spectre.Console;

namespace TopModel.Utils.Cli;

public class ConfigObserver<TConfig, TFileChecker, TWorker>(
    FileInfo configInfo,
    TFileChecker fileChecker,
    LoggerProvider loggerProvider,
    int configIndex,
    Action<TWorker>? configurator = null
) : IDisposable
    where TConfig : ConfigBase
    where TFileChecker : AbstractFileChecker<TConfig>
    where TWorker : TopModelWorker<TConfig, TFileChecker>, new()
{
    private readonly MemoryCache fsCache = new(new MemoryCacheOptions());
    private FileSystemWatcher? ConfigWatcher;
    private TWorker? Worker;

    public bool HasError => Worker?.HasError ?? true;

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        Worker?.Dispose();
        ConfigWatcher?.Dispose();
        fsCache.Dispose();
    }

    public async Task Start(CancellationToken cancellationToken)
    {
        StartWatchConfig(cancellationToken);
        await Run(cancellationToken);
    }

    private async Task ReadConfig(CancellationToken cancellationToken)
    {
        try
        {
            fileChecker.CheckConfigFile(configInfo.FullName);
            using var text = configInfo.OpenText();
            var config = fileChecker
                .DeserializeConfig(await text.ReadToEndAsync(cancellationToken))
                .Init(configInfo.DirectoryName!);
            Worker = new TWorker
            {
                Config = (TConfig)config,
                ConfigFullName = configInfo.FullName,
                ConfigDirectoryName = configInfo.DirectoryName!,
                ConfigIndex = configIndex,
                LoggerProvider = loggerProvider,
                FileChecker = fileChecker,
            };
            configurator?.Invoke(Worker);
            Worker.Init();
        }
        catch (LegitException me)
        {
            AnsiConsole.WriteLine($"[red]{me.Message}[/]");
        }
    }

    private async Task Restart(CancellationToken cancellationToken)
    {
        Worker?.Dispose();
        Worker = null;
        AnsiConsole.WriteLine();
        AnsiConsole.LogConfig(configInfo.FullName, configIndex, changed: true);
        await Run(cancellationToken);
    }

    private async Task Run(CancellationToken cancellationToken)
    {
        await ReadConfig(cancellationToken);
        if (Worker != null)
        {
            await Worker.Run(cancellationToken);
        }
    }

    private void StartWatchConfig(CancellationToken cancellationToken)
    {
        var fsWatcher = new FileSystemWatcher(configInfo.DirectoryName!, configInfo.Name);
        fsWatcher.Changed += (sender, args) =>
        {
            fsCache.Set(
                args.FullPath,
                args,
                new MemoryCacheEntryOptions()
                    .AddExpirationToken(
                        new CancellationChangeToken(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token)
                    )
                    .RegisterPostEvictionCallback(
                        async (k, v, r, a) =>
                        {
                            if (r != EvictionReason.TokenExpired)
                            {
                                return;
                            }

                            await Restart(cancellationToken);
                        }
                    )
            );
        };
        fsWatcher.IncludeSubdirectories = true;
        fsWatcher.EnableRaisingEvents = true;
        ConfigWatcher = fsWatcher;
    }
}
