using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.Generator.Core;

public abstract class EndpointsGeneratorBase<T> : GeneratorBase<T>
    where T : GeneratorConfigBase
{
    public EndpointsGeneratorBase(ILogger<EndpointsGeneratorBase<T>> logger)
        : base(logger)
    {
    }

    public override List<string> GeneratedFiles => EndpointsFiles
        .SelectMany(file => Config.Tags.Intersect(file.Tags.Where(FilterTag)).Select(tag => GetFileName(file, tag)))
        .Distinct()
        .ToList();

    private IEnumerable<ModelFile> EndpointsFiles => Files.Values
        .Where(f => f.Tags.Any(FilterTag)
            && f.Endpoints.Any(endpoint => endpoint.ModelFile == f || !Files.ContainsKey(endpoint.ModelFile.Name)));

    protected virtual bool FilterTag(string tag)
    {
        return true;
    }

    protected abstract string GetFileName(ModelFile file, string tag);

    protected abstract void HandleFile(string filePath, string fileName, string tag, IList<Endpoint> endpoints);

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files.Where(file => EndpointsFiles.Contains(file)).GroupBy(file => new { file.Options.Endpoints.FileName, file.Namespace.Module }))
        {
            HandleFileBase(file.First(), file.SelectMany(f => f.Tags.Where(FilterTag)).Distinct());
        }
    }

    private void HandleFileBase(ModelFile file, IEnumerable<string> tags)
    {
        foreach (var (tag, fileName) in Config.Tags.Intersect(tags)
           .Select(tag => (tag, fileName: GetFileName(file, tag)))
           .DistinctBy(t => t.fileName))
        {
            var files = Files.Values.Where(f => f.Options.Endpoints.FileName == file.Options.Endpoints.FileName && f.Namespace.Module == file.Namespace.Module && f.Tags.Contains(tag));
            var endpoints = files
                .SelectMany(f => f.Endpoints)
                .Where(endpoint => files.Contains(endpoint.ModelFile) || !Files.ContainsKey(endpoint.ModelFile.Name))
                .OrderBy(e => e.Name, StringComparer.Ordinal)
                .ToList();

            HandleFile(fileName, files.First().Options.Endpoints.FileName, tag, endpoints);
        }
    }
}