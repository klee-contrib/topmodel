using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Translation;

/// <summary>
/// Générateur des objets de traduction javascripts.
/// </summary>
public class TranslationOutGenerator(
    ILogger<TranslationOutGenerator> logger,
    ModelConfig modelConfig,
    TranslationStore translationStore,
    IFileWriterProvider writerProvider
) : TranslationGeneratorBase<TranslationConfig>(logger, translationStore, writerProvider)
{
    private readonly ModelConfig _modelConfig = modelConfig;
    private readonly TranslationStore _translationStore = translationStore;

    public override string Name => "TranslationOutGen";

    protected override string? GetResourceFilePath(IProperty property, string tag, string lang)
    {
        if (lang == _modelConfig.I18n.DefaultLang)
        {
            return null;
        }

        var p = property.ResourceProperty;
        if (
            p.Label != null && !ExistsInStore(lang, p.ResourceKey)
            || !(
                p.Class?.DefaultProperty == null
                || (p.Class?.Values.All(r => ExistsInStore(lang, r.ResourceKey)) ?? false)
            )
        )
        {
            return Path.Combine(
                Config.OutputDirectory,
                Config.ResolveVariables(Config.RootPath, tag: tag, lang: lang),
                $"{string.Join('_', Config.GetRootModule(p.Parent.Namespace).Split(".").Select(part => part.ToKebabCase()))}_{lang}.properties"
            );
        }

        return null;
    }

    protected override void HandleResourceFile(
        string filePath,
        string tag,
        string lang,
        IEnumerable<IProperty> properties
    )
    {
        using var fw = OpenFileWriter(filePath);
        fw.EnableHeader = false;

        var containers = properties.GroupBy(prop => prop.Parent);

        foreach (var container in containers.OrderBy(c => c.Key.NameCamel))
        {
            WriteClasse(fw, container, lang);
        }
    }

    private bool ExistsInStore(string lang, string key)
    {
        return _translationStore.Translations.TryGetValue(lang, out var langDict) && langDict.ContainsKey(key);
    }

    private void WriteClasse(IFileWriter fw, IGrouping<IPropertyContainer, IProperty> container, string lang)
    {
        foreach (var property in container.OrderBy(p => p.NameCamel, StringComparer.Ordinal))
        {
            if (
                property.Label != null
                && !(
                    _translationStore.Translations.TryGetValue(lang, out var langDict)
                    && langDict.ContainsKey(property.ResourceKey)
                )
                && !ExistsInStore(lang, property.ResourceKey)
            )
            {
                fw.WriteLine($"{property.ResourceKey}={property.Label}");
            }
        }

        if (container.Key is Class classe && classe.DefaultProperty != null)
        {
            foreach (var reference in classe.Values.OrderBy(p => p.ResourceKey, StringComparer.Ordinal))
            {
                if (!ExistsInStore(lang, reference.ResourceKey))
                {
                    fw.WriteLine($"{reference.ResourceKey}={reference.Value[classe.DefaultProperty]}");
                }
            }
        }
    }
}
