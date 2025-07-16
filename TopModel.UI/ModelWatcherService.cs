using TopModel.Core;

namespace TopModel.UI;

public class ModelWatcherService : IHostedService
{
    private readonly ModelStore _modelStore;

    public ModelWatcherService(ModelStore modelStore)
    {
        _modelStore = modelStore;
    }

    /// <inheritdoc cref="IHostedService.StartAsync" />
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IHostedService.StopAsync" />
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _modelStore.Dispose();
        return Task.CompletedTask;
    }
}