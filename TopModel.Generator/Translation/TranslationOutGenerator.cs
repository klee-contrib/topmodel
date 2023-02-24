using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Utils;

namespace TopModel.Generator.Translation;

/// <summary>
/// Générateur des objets de traduction javascripts.
/// </summary>
public class TranslationOutGenerator : TranslationGeneratorBase
{
    private readonly TranslationConfig _config;
    private readonly ILogger<TranslationOutGenerator> _logger;
    private readonly ModelConfig _modelConfig;
    private readonly TranslationStore _translationStore;

    public TranslationOutGenerator(ILogger<TranslationOutGenerator> logger, TranslationConfig config, ModelConfig modelConfig, TranslationStore translationStore)
        : base(logger, config, translationStore)
    {
        _config = config;
        _logger = logger;
        _modelConfig = modelConfig;
        _translationStore = translationStore;
    }

    public override string Name => "TranslationOutGen";

    protected override string? GetResourceFilePath(IFieldProperty property, string tag, string lang)
    {
        if (lang == _modelConfig.I18n.DefaultLang)
        {
            return null;
        }

        var p = property.ResourceProperty;
        if (p.Label != null
            && !ExistsInStore(lang, p.ResourceKey)
            || !(
                p.Class?.DefaultProperty == null ||
                (p.Class?.Values.TrueForAll(r => ExistsInStore(lang, r.ResourceKey)) ?? false)))
        {
            return Path.Combine(
                _config.OutputDirectory,
                _config.ResolveVariables(_config.RootPath, tag: tag, lang: lang),
                Path.Combine(p.Parent.Namespace.Module.Split(".").Select(part => part.ToKebabCase()).ToArray()) + "_" + lang + ".properties");
        }

        return null;
    }

    protected override void HandleResourceFile(string filePath, string lang, IEnumerable<IFieldProperty> properties)
    {
        using var fw = new FileWriter(filePath, _logger) { EnableHeader = false };
        var containers = properties.GroupBy(prop => prop.Parent);

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
}
