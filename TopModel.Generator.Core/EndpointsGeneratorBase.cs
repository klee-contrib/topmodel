using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.Generator.Core;

public abstract class EndpointsGeneratorBase : GeneratorBase
{
    private readonly GeneratorConfigBase _config;

    public EndpointsGeneratorBase(ILogger<EndpointsGeneratorBase> logger, GeneratorConfigBase config)
        : base(logger, config)
    {
        _config = config;
    }

    public override List<string> GeneratedFiles => EndpointsFiles
        .SelectMany(file => _config.Tags.Intersect(file.Tags.Where(FilterTag)).Select(tag => GetFileName(file, tag)))
        .Distinct()
        .ToList();

    private IEnumerable<ModelFile> EndpointsFiles => Files.Values
        .Where(f => f.Tags.Any(FilterTag)
            && f.Endpoints.Any(endpoint => endpoint.ModelFile == f || !Files.ContainsKey(endpoint.ModelFile.Name)));

    protected abstract string GetFileName(ModelFile file, string tag);

    protected abstract void HandleFile(string filePath, string fileName, string tag, IList<Endpoint> endpoints);

    protected virtual bool FilterTag(string tag)
    {
        return true;
    }

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files.Where(file => EndpointsFiles.Contains(file)).GroupBy(file => new { file.Options.Endpoints.FileName, file.Namespace.Module }))
        {
            HandleFileBase(file.First(), file.SelectMany(f => f.Tags.Where(FilterTag)).Distinct());
        }
    }

    private void HandleFileBase(ModelFile file, IEnumerable<string> tags)
    {
        foreach (var (tag, fileName) in _config.Tags.Intersect(tags)
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