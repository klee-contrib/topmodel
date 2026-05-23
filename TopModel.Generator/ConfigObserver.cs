using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Spectre.Console;
using TopModel.Core;
using TopModel.Core.Loaders;
using TopModel.Utils;
using TopModel.Utils.Cli;

namespace TopModel.Generator;

public class ConfigObserver(
    FileInfo configInfo,
    FileChecker fileChecker,
    int configIndex,
    Func<ModelConfig, FileInfo, int, TopModelWorker> createWorker
) : IDisposable
{
    private readonly MemoryCache fsCache = new(new MemoryCacheOptions());
    private FileSystemWatcher? ConfigWatcher;
    private TopModelWorker? Worker;

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
        LogConfigFound();
        StartWatchConfig(cancellationToken);
        await Run(cancellationToken);
    }

    private void LogConfigFound()
    {
        var color = LogUtils.Colors[configIndex % LogUtils.Colors.Length];
        AnsiConsole.MarkupLine(
            $"[{color}]#{configIndex + 1} - {Path.GetRelativePath(Directory.GetCurrentDirectory(), configInfo.FullName)}[/]"
        );
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
            Worker = createWorker(config, configInfo, configIndex);
        }
        catch (ModelException me)
        {
            AnsiConsole.WriteLine($"[red]{me.Message}[/]");
        }
    }

    private async Task Restart(CancellationToken cancellationToken)
    {
        Worker?.Dispose();
        Worker = null;
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
