using Meziantou.Framework.Globbing;
using TopModel.Core;
using TopModel.Utils;

namespace TopModel.LanguageServer;

public class LSWorkerStore
{
    private readonly IList<LSWorker> _workers = [];

    public ModelStore? ModelStore => _workers.Count > 0 ? _workers[0].ModelStore : null;

    public void AddWorker(LSWorker worker)
    {
        _workers.Add(worker);
    }

    public async Task OnModelFileChange(string filePath, string content, CancellationToken ct = default)
    {
        foreach (var worker in _workers)
        {
            var relativePath = filePath.ToRelative(worker.Config.ModelRoot);
            if (
                !relativePath.StartsWith("..")
                && !Path.IsPathRooted(relativePath)
                && worker.ModelConfig.ModelFilePaths.IsMatch(relativePath[2..])
            )
            {
                await worker.ModelStore.OnModelFileChange(filePath, content, ct);
            }
        }
    }

    public void RemoveWorker(LSWorker worker)
    {
        _workers.Remove(worker);
    }
}
