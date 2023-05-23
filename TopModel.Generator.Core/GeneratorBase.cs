using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Generator.Core;

public abstract class GeneratorBase<T> : IModelWatcher
    where T : GeneratorConfigBase
{
    private readonly ILogger _logger;

    protected GeneratorBase(ILogger logger)
    {
        _logger = logger;
    }

    public abstract string Name { get; }

#nullable disable
    public T Config { get; internal set; }
#nullable enable

    public int Number { get; internal set; }

    public virtual IEnumerable<string> GeneratedFiles => new List<string>();

    public bool Disabled => Config.Disable?.Contains(Name) ?? false;

    protected Dictionary<string, ModelFile> Files { get; } = new();

    protected IEnumerable<Class> Classes => Files.SelectMany(f => f.Value.Classes).Distinct();

    protected virtual bool PersistentOnly => false;

    protected virtual bool NoLanguage => false;

    /// <inheritdoc cref="IModelWatcher.OnErrors" />
    public void OnErrors(IDictionary<ModelFile, IEnumerable<ModelError>> errors)
    {
    }

    /// <inheritdoc cref="IModelWatcher.OnFilesChanged" />
    public void OnFilesChanged(IEnumerable<ModelFile> files, LoggingScope? storeConfig = null)
    {
        using var scope = _logger.BeginScope(((IModelWatcher)this).FullName);
        using var scope2 = _logger.BeginScope(storeConfig!);

        var handledFiles = files.Where(file => Config.Tags.Intersect(file.Tags).Any());

        if (!NoLanguage)
        {
            var missingDomains = handledFiles.SelectMany(f => f.Properties).OfType<IFieldProperty>().Where(fp => !PersistentOnly || (fp.Class?.IsPersistent ?? false)).Select(fp => fp.Domain)
                .Concat(PersistentOnly ? Array.Empty<Domain>() : handledFiles.SelectMany(f => f.Properties).OfType<CompositionProperty>().Select(fp => fp.Domain!))
                .Where(domain => domain != null && Config.GetImplementation(domain) == null)
                .Distinct();

            if (missingDomains.Any())
            {
                throw new ModelException($"Pour utiliser le générateur '{Name}', les domaines suivants doivent définir une implémentation pour '{Config.Language}' : {string.Join(", ", missingDomains.Select(d => d.Name).OrderBy(x => x))}.");
            }
        }

        foreach (var file in handledFiles)
        {
            Files[file.Name] = file;
        }

        HandleFiles(handledFiles);
    }

    protected IEnumerable<string> GetClassTags(Class classe)
    {
        return Files.Values.Where(f => f.Classes.Contains(classe)).SelectMany(f => f.Tags).Distinct();
    }

    protected abstract void HandleFiles(IEnumerable<ModelFile> files);
}