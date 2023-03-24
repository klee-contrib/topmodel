using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Generator;

public abstract class GeneratorBase : IModelWatcher
{
    private readonly GeneratorConfigBase _config;
    private readonly ILogger _logger;

    protected GeneratorBase(ILogger logger, GeneratorConfigBase config)
    {
        _config = config;
        _logger = logger;
    }

    public abstract string Name { get; }

    public int Number { get; init; }

    public virtual IEnumerable<string> GeneratedFiles => new List<string>();

    protected Dictionary<string, ModelFile> Files { get; } = new();

    protected IEnumerable<Class> Classes => Files.SelectMany(f => f.Value.Classes).Distinct();

    public void OnErrors(IDictionary<ModelFile, IEnumerable<ModelError>> errors)
    {
    }

    public void OnFilesChanged(IEnumerable<ModelFile> files, LoggingScope? storeConfig = null)
    {
        using var scope = _logger.BeginScope(((IModelWatcher)this).FullName);
        using var scope2 = _logger.BeginScope(storeConfig);

        var handledFiles = files.Where(file => _config.Tags.Intersect(file.Tags).Any());

        var missingDomains = handledFiles.SelectMany(f => f.Properties).OfType<IFieldProperty>().Where(fp => GetDomainType(fp.Domain) == null).Select(fp => fp.Domain).Distinct();

        if (missingDomains.Any())
        {
            throw new ModelException($"Pour utiliser le générateur '{Name}', les domaines suivants doivent définir le type de son langage cible : {string.Join(", ", missingDomains.Select(d => d.Name).OrderBy(x => x))}.");
        }

        foreach (var file in handledFiles)
        {
            Files[file.Name] = file;
        }

        HandleFiles(handledFiles);
    }

    protected virtual object? GetDomainType(Domain domain)
    {
        return new object();
    }

    protected abstract void HandleFiles(IEnumerable<ModelFile> files);

    protected IEnumerable<string> GetClassTags(Class classe)
    {
        return Files.Values.Where(f => f.Classes.Contains(classe)).SelectMany(f => f.Tags).Distinct();
    }
}