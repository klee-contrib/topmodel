using Microsoft.Extensions.Logging;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Generator.Core;

public abstract class EndpointsGeneratorBase<T>(
    ILogger<EndpointsGeneratorBase<T>> logger,
    IFileWriterProvider writerProvider
) : GeneratorBase<T>(logger, writerProvider)
    where T : GeneratorConfigBase
{
    public override List<string> GeneratedFiles =>
        Config
            .Files.Values.SelectMany(file =>
                Config.Tags.Intersect(file.AllTags.Where(FilterTag)).Select(tag => (file, path: GetFilePath(file, tag)))
            )
            .Where(i => i.file.Endpoints.Any(Config.FilterEndpoint))
            .Select(i => i.path)
            .Distinct()
            .ToList();

    protected virtual bool FilterTag(string tag)
    {
        return true;
    }

    protected abstract string GetFilePath(ModelFile file, string tag);

    protected abstract Task HandleFile(
        string filePath,
        string fileName,
        string tag,
        IList<Endpoint> endpoints,
        CancellationToken ct = default
    );

    protected override async Task HandleFiles(IEnumerable<ModelFile> files, CancellationToken ct = default)
    {
        await Parallel.ForEachAsync(
            Config
                .Files.Values.SelectMany(file =>
                    Config
                        .Tags.Intersect(file.AllTags.Where(FilterTag))
                        .Select(tag => (tag, file, filePath: GetFilePath(file, tag)))
                )
                .GroupBy(file => file.filePath),
            ct,
            async (file, ct) =>
            {
                var endpoints = file.SelectMany(f => f.file.Endpoints.Where(Config.FilterEndpoint))
                    .Where(e => e.Tags.Intersect(file.Select(f => f.tag)).Any())
                    .Distinct()
                    .OrderBy(e => e.Name, StringComparer.Ordinal)
                    .ToList();

                if (endpoints.Any())
                {
                    await HandleFile(
                        file.Key,
                        file.First().file.Options.Endpoints.FileName,
                        file.First().tag,
                        endpoints,
                        ct
                    );
                }
            }
        );
    }
}
