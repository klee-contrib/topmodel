using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Translation;

/// <summary>
/// Générateur des objets de traduction javascripts.
/// </summary>
public class TranslationOutGenerator : TranslationGeneratorBase<TranslationConfig>
{
    private readonly ILogger<TranslationOutGenerator> _logger;
    private readonly ModelConfig _modelConfig;
    private readonly TranslationStore _translationStore;

    public TranslationOutGenerator(ILogger<TranslationOutGenerator> logger, ModelConfig modelConfig, TranslationStore translationStore)
        : base(logger, translationStore)
    {
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
                Config.OutputDirectory,
                Config.ResolveVariables(Config.RootPath, tag: tag, lang: lang),
                $"{p.Parent.Namespace.RootModule.ToKebabCase()}_{lang}.properties");
        }

        return null;
    }

    protected override void HandleResourceFile(string filePath, string lang, IEnumerable<IFieldProperty> properties)
    {
        using var fw = new FileWriter(filePath, _logger) { EnableHeader = false };
        var containers = properties.GroupBy(prop => prop.Parent);

        foreach (var container in containers.OrderBy(c => c.Key.NameCamel))
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
