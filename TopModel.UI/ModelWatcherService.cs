using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace TopModel.UI
{
    public class ModelWatcherService : IHostedService
    {
        private readonly ModelStore _modelStore;
        private IDisposable? _watcher;

        public ModelWatcherService(ModelStore modelStore)
        {
            _modelStore = modelStore;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _watcher = _modelStore.LoadFromConfig(true);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _watcher?.Dispose();
            return Task.CompletedTask;
        }
    }
}