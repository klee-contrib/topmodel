using Microsoft.Extensions.DependencyInjection;

namespace TopModel.Utils.Cli;

public abstract class TopModelWorker<TConfig, TFileChecker> : IDisposable
    where TConfig : ConfigBase
    where TFileChecker : AbstractFileChecker<TConfig>
{
    public LoggingScope StoreConfig { get; private set; } = null!;

    public bool HasError { get; protected set; }

    public TFileChecker FileChecker { get; init; } = null!;

    public LoggerProvider LoggerProvider { get; init; } = null!;

    public int ConfigIndex
    {
        get;
        init
        {
            field = value;
            StoreConfig = LogUtils.GetScope(value);
        }
    }

    public string ConfigFullName { get; init; } = null!;

    public string ConfigDirectoryName { get; init; } = null!;

    public TConfig Config { get; init; } = null!;

    public bool WatchMode { get; set; }

    public bool ParallelMode { get; set; }

    public IServiceCollection Services { get; } = new ServiceCollection();

    public ServiceProvider ServiceProvider
    {
        get { return field ??= Services.BuildServiceProvider(); }
    }

    /// <inheritdoc cref="IDisposable.Dispose" />
    public virtual void Dispose()
    {
        ServiceProvider.Dispose();
    }

    public abstract void Init();

    public abstract Task Run(CancellationToken cancellationToken);

    public abstract Task WaitForFinished(CancellationToken cancellationToken);
}
