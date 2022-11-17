using System.Text;
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

    private readonly IDictionary<string, ModelFile> _files = new Dictionary<string, ModelFile>();

    public TranslationOutGenerator(ILogger<TranslationOutGenerator> logger, TranslationConfig config, TranslationStore translationStore)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
        _translationStore = translationStore;
    }

    public override string Name => "ResourcesGenerator";

    public override IEnumerable<string> GeneratedFiles => _config.Langs.SelectMany(lang => GetModules(lang.Key).Select(m => GetFilePath(m, lang.Key)));

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var lang in _config.Langs)
        {
            var modules = GetModules(lang.Key);
            foreach (var module in modules)
            {
                GenerateModule(module, lang.Key);
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
                        p.Class.DefaultProperty == null ||
                        p.Class.ReferenceValues.TrueForAll(r => ExistsInStore(lang, r.ResourceKey))))
                    .Distinct()
                    .GroupBy(prop => prop.Class.Namespace.Module);
    }

    private void GenerateModule(IGrouping<string, IFieldProperty> module, string lang)
    {
        var filePath = GetFilePath(module, lang);

        using var fw = new FileWriter(filePath, _logger) { EnableHeader = false };
        var classes = module.GroupBy(prop => prop.Class);

        foreach (var classe in classes.OrderBy(c => c.Key.Name))
        {
            WriteClasse(fw, classe, lang);
        }
    }

    private void WriteClasse(FileWriter fw, IGrouping<Class, IFieldProperty> classe, string lang)
    {
        foreach (var property in classe)
        {
            if (property.Label != null
                && !(_translationStore.Translations.TryGetValue(lang, out var langDict)
                && langDict.TryGetValue(property.ResourceKey, out var label)))
            {
                if (!ExistsInStore(lang, property.ResourceKey))
                {
                    fw.WriteLine($"{property.ResourceKey}={property.Label}");
                }
            }
        }

        if (classe.Key.DefaultProperty != null)
        {
            foreach (var reference in classe.Key.ReferenceValues)
            {
                if (!ExistsInStore(lang, reference.ResourceKey))
                {
                    fw.WriteLine($"{reference.ResourceKey}={reference.Value[classe.Key.DefaultProperty]}");
                }
            }
        }
    }

    private bool ExistsInStore(string lang, string key)
    {
        return _translationStore.Translations.TryGetValue(lang, out var langDict)
                && langDict.TryGetValue(key, out var label);
    }

    private string GetFilePath(IGrouping<string, IFieldProperty> module, string lang)
    {
        return Path.Combine(_config.OutputDirectory, _config.Langs[lang], Path.Combine(module.Key.Split(".").Select(part => part.ToKebabCase()).ToArray()) + "_" + lang + ".properties");
    }
}
