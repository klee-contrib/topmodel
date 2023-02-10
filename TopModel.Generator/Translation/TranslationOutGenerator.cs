using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Generator.Translation;

/// <summary>
/// Générateur des objets de traduction javascripts.
/// </summary>
public class TranslationOutGenerator : GeneratorBase
{
    private readonly TranslationConfig _config;

    private readonly ILogger<TranslationOutGenerator> _logger;

    private readonly TranslationStore _translationStore;

    public TranslationOutGenerator(ILogger<TranslationOutGenerator> logger, TranslationConfig config, TranslationStore translationStore)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
        _translationStore = translationStore;
    }

    public override string Name => "TranslationOutGen";

    public override IEnumerable<string> GeneratedFiles => _config.Langs.SelectMany(lang => GetModules(lang).Select(m => GetFilePath(m, lang)));

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var lang in _config.Langs)
        {
            var modules = GetModules(lang);
            foreach (var module in modules)
            {
                GenerateModule(module, lang);
            }
        }
    }

    private IEnumerable<IGrouping<string, IFieldProperty>> GetModules(string lang)
    {
        return Files
            .SelectMany(file => file.Value.Classes.SelectMany(c => c.Properties.OfType<IFieldProperty>()))
            .Select(c => c.ResourceProperty)
            .Where(p => p.Label != null)
            .Where(p => !ExistsInStore(lang, p.ResourceKey)
            || !(
                p.Class?.DefaultProperty == null ||
                (p.Class?.Values.TrueForAll(r => ExistsInStore(lang, r.ResourceKey)) ?? false)))
            .Distinct()
            .GroupBy(prop => prop.Parent.Namespace.Module);
    }

    private void GenerateModule(IGrouping<string, IFieldProperty> module, string lang)
    {
        var filePath = GetFilePath(module, lang);

        using var fw = new FileWriter(filePath, _logger) { EnableHeader = false };
        var containers = module.GroupBy(prop => prop.Parent);

        foreach (var container in containers.OrderBy(c => c.Key.Name))
        {
            WriteClasse(fw, container, lang);
        }
    }

    private void WriteClasse(FileWriter fw, IGrouping<IPropertyContainer, IFieldProperty> container, string lang)
    {
        foreach (var property in container)
        {
            if (property.Label != null
                && !(_translationStore.Translations.TryGetValue(lang, out var langDict)
                && langDict.ContainsKey(property.ResourceKey)))
            {
                if (!ExistsInStore(lang, property.ResourceKey))
                {
                    fw.WriteLine($"{property.ResourceKey}={property.Label}");
                }
            }
        }

        if (container.Key is Class classe && classe.DefaultProperty != null)
        {
            foreach (var reference in classe.Values)
            {
                if (!ExistsInStore(lang, reference.ResourceKey))
                {
                    fw.WriteLine($"{reference.ResourceKey}={reference.Value[classe.DefaultProperty]}");
                }
            }
        }
    }

    private bool ExistsInStore(string lang, string key)
    {
        return _translationStore.Translations.TryGetValue(lang, out var langDict)
                && langDict.ContainsKey(key);
    }

    private string GetFilePath(IGrouping<string, IFieldProperty> module, string lang)
    {
        return Path.Combine(_config.OutputDirectory, _config.RootPath.Replace("{lang}", lang), Path.Combine(module.Key.Split(".").Select(part => part.ToKebabCase()).ToArray()) + "_" + lang + ".properties");
    }
}
