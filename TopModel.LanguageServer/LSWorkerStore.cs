using Meziantou.Framework.Globbing;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.LanguageServer;

public class LSWorkerStore(ILanguageServerFacade facade)
{
    private readonly IList<LSWorker> _workers = [];

    public ModelStore? ModelStore => _workers.Count == 1 ? _workers[0].ModelStore : null;

    public IEnumerable<ModelStore> ModelStores => _workers.Select(w => w.ModelStore);

    public void AddWorker(LSWorker worker)
    {
        _workers.Add(worker);
    }

    public IEnumerable<(ModelFile File, ModelStore Store)> GetFiles(TextDocumentIdentifier textDocument)
    {
        return _workers
            .Select(w =>
                (
                    File: w.ModelStore.Files.SingleOrDefault(f =>
                        facade.GetFilePath(f) == textDocument.Uri.GetFileSystemPath()
                    )!,
                    Store: w.ModelStore
                )
            )
            .Where(w => w.File != null);
    }

    public async Task OnModelFileChange(string filePath, string content, CancellationToken ct = default)
    {
        var affectedWorkers = _workers.Where(worker =>
        {
            var relativePath = filePath.ToRelative(worker.Config.ModelRoot);
            return (
                !relativePath.StartsWith("..")
                && !Path.IsPathRooted(relativePath)
                && worker.ModelConfig.ModelFilePaths.IsMatch(relativePath[2..])
            );
        });

        foreach (var worker in affectedWorkers)
        {
            worker.ModelFileLoader.RemoveFromCache(filePath);
        }

        await Parallel.ForEachAsync(
            affectedWorkers,
            ct,
            async (worker, ct) =>
            {
                await worker.ModelStore.OnModelFileChange(filePath, content, ct);
            }
        );
    }

    public void RemoveWorker(LSWorker worker)
    {
        _workers.Remove(worker);
    }

    public async Task WaitForUpdates(CancellationToken ct = default)
    {
        await Parallel.ForEachAsync(_workers, ct, async (w, ct) => await w.ModelStore.WaitForUpdates(ct));
    }
}
