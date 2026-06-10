using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Spectre.Console;

namespace TopModel.Utils.Cli;

#pragma warning disable MA0048
public interface IConfigObserver : IDisposable
{
    Task Start(CancellationToken cancellationToken);
}

public class ConfigObserver<TConfig, TFileChecker, TWorker>(
    FileInfo configInfo,
    TFileChecker fileChecker,
    LoggerProvider loggerProvider,
    bool watchMode,
    bool parallelMode,
    int configIndex,
    Action<TWorker>? configurator = null,
    Action<TWorker>? onDispose = null,
    Action? onRestart = null
) : IConfigObserver
    where TConfig : ConfigBase
    where TFileChecker : AbstractFileChecker<TConfig>
    where TWorker : TopModelWorker<TConfig, TFileChecker>, new()
{
    private readonly MemoryCache fsCache = new(new MemoryCacheOptions());
    private FileSystemWatcher? ConfigWatcher;
    private TWorker? Worker;

    private CancellationTokenSource? ctsWorker;

    public bool HasError => Worker?.HasError ?? true;

    public bool NoLog { get; set; }

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        ctsWorker?.Dispose();
        if (Worker != null)
        {
            onDispose?.Invoke(Worker);
            Worker.Dispose();
        }
        ConfigWatcher?.Dispose();
        fsCache.Dispose();
    }

    /// <inheritdoc cref="IConfigObserver.Start" />
    public async Task Start(CancellationToken cancellationToken)
    {
        if (watchMode)
        {
            StartWatchConfig(cancellationToken);
        }

        await Run(cancellationToken);
    }

    private async Task Restart(CancellationToken cancellationToken)
    {
        if (ctsWorker != null)
        {
            await ctsWorker.CancelAsync();
        }

        if (Worker != null)
        {
            await Worker.WaitForFinished(cancellationToken);
            onDispose?.Invoke(Worker);
            Worker.Dispose();
        }

        if (!NoLog)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.LogConfig(configInfo.FullName, configIndex, changed: true);
        }

        await Run(cancellationToken);
        onRestart?.Invoke();
    }

    private async Task Run(CancellationToken cancellationToken)
    {
        ctsWorker?.Dispose();
        ctsWorker = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        try
        {
            fileChecker.CheckConfigFile(configInfo.FullName);
            using (var text = configInfo.OpenText())
            {
                var config = fileChecker
                    .DeserializeConfig(await text.ReadToEndAsync(ctsWorker.Token))
                    .Init(configInfo.DirectoryName!);
                Worker = new TWorker
                {
                    Config = (TConfig)config,
                    ConfigFullName = configInfo.FullName,
                    ConfigDirectoryName = configInfo.DirectoryName!,
                    ConfigIndex = configIndex,
                    LoggerProvider = loggerProvider,
                    FileChecker = fileChecker,
                    WatchMode = watchMode,
                    ParallelMode = parallelMode,
                };
            }

            configurator?.Invoke(Worker);
            Worker.Init();
            await Worker.Run(ctsWorker.Token);
        }
        catch (LegitException me)
        {
            if (!NoLog)
            {
                AnsiConsole.WriteLine($"[red]{me.Message}[/]");
            }
        }
        catch (OperationCanceledException)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
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
