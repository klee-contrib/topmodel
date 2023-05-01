using TopModel.Core;

namespace TopModel.UI;

public class ModelWatcherService : IHostedService
{
    private readonly ModelStore _modelStore;
    private IDisposable? _watcher;

    public ModelWatcherService(ModelStore modelStore)
    {
        _modelStore = modelStore;
    }

    /// <inheritdoc cref="IHostedService.StartAsync" />
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _watcher = _modelStore.LoadFromConfig(true);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IHostedService.StopAsync" />
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _watcher?.Dispose();
        return Task.CompletedTask;
    }
}