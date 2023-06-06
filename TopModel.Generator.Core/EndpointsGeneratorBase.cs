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
        .SelectMany(file => Config.Tags.Intersect(file.Tags.Where(FilterTag)).Select(tag => GetFilePath(file, tag)))
        .Distinct()
        .ToList();

    private IEnumerable<ModelFile> EndpointsFiles => Files.Values
        .Where(f => f.Tags.Any(FilterTag)
            && f.Endpoints.Any(endpoint => endpoint.ModelFile == f || !Files.ContainsKey(endpoint.ModelFile.Name)));

    protected virtual bool FilterTag(string tag)
    {
        return true;
    }

    protected abstract string GetFilePath(ModelFile file, string tag);

    protected abstract void HandleFile(string filePath, string fileName, string tag, IList<Endpoint> endpoints);

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files
            .Where(file => EndpointsFiles.Contains(file))
            .SelectMany(file => Config.Tags.Intersect(file.Tags.Where(FilterTag))
                .Select(tag => (tag, file, filePath: GetFilePath(file, tag))))
            .GroupBy(file => file.filePath))
        {
            var endpoints = file
                .SelectMany(f => f.file.Endpoints)
                .Distinct()
                .Where(endpoint => file.Select(f => f.file).Contains(endpoint.ModelFile) || !Files.ContainsKey(endpoint.ModelFile.Name))
                .OrderBy(e => e.Name, StringComparer.Ordinal)
                .ToList();

            HandleFile(file.Key, file.First().file.Options.Endpoints.FileName, file.First().tag, endpoints);
        }
    }
}