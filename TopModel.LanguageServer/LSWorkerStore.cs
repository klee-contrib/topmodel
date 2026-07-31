using Meziantou.Framework.Globbing;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Core.Loaders;
using TopModel.Utils;

namespace TopModel.LanguageServer;

public class LSWorkerStore(ILanguageServerFacade facade, IModelReporter modelReporter, ModelFileLoader modelFileLoader)
{
    private readonly IList<LSWorker> _workers = [];

    public IEnumerable<ModelStore> ModelStores => _workers.Select(w => w.ModelStore);

    public TextDocumentSelector TmdFiles
    {
        get
        {
            var roots = _workers.Select(w => w.Config.ModelRoot.TrimEnd('/')).ToList();
            if (roots.Count == 0)
            {
                roots.Add(Directory.GetCurrentDirectory().Replace('\\', '/').TrimEnd('/'));
            }

            return TextDocumentSelector.ForPattern([.. roots.Select(root => $"{root}/**/*.tmd")]);
        }
    }

    public void AddWorker(LSWorker worker)
    {
        _workers.Add(worker);
    }

    public (ModelFile? File, ModelStore? Store, string? App) GetFileWithApp(string filePath)
    {
        foreach (var worker in _workers)
        {
            var file = worker.ModelStore.Files.SingleOrDefault(f => facade.GetFilePath(f) == filePath);
            if (file != null)
            {
                return (file, worker.ModelStore, worker.Config.App);
            }
        }

        return (null, null, null);
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
            modelFileLoader.RemoveFromCache(worker.ModelStore.ModelFileLoadConfig, filePath);
        }

        await Parallel.ForEachAsync(
            affectedWorkers,
            ct,
            async (worker, ct) =>
            {
                await worker.ModelStore.OnModelFileChange(filePath, content, ct);
            }
        );

        modelReporter.Report();
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
