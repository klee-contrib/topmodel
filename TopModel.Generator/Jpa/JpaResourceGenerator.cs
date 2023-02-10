using System.Text;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

/// <summary>
/// Générateur des objets de traduction javascripts.
/// </summary>
public class JpaResourceGenerator : GeneratorBase
{
    private readonly JpaConfig _config;
    private readonly ILogger<JpaResourceGenerator> _logger;
    private readonly TranslationStore _translationStore;

    public JpaResourceGenerator(ILogger<JpaResourceGenerator> logger, JpaConfig config, TranslationStore translationStore)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
        _translationStore = translationStore;
    }

    public override string Name => "JpaResourceGen";

    public override IEnumerable<string> GeneratedFiles => _translationStore.Translations.SelectMany(dict => GetModules().Select(module => GetFilePath(module, dict.Key)));

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        var modules = GetModules();

        foreach (var module in modules)
        {
            foreach (var lang in _translationStore.Translations)
            {
                GenerateModule(module, lang.Key);
            }
        }
    }

    private string GetFilePath(IGrouping<string, IFieldProperty> module, string lang)
    {
        return Path.Combine(_config.OutputDirectory, _config.ResourceRootPath.Replace("{lang}", lang).Replace("{module}", module.Key.Replace(".", "/")).ToLower(), Path.Combine(module.Key.Split(".").Last().ToKebabCase()) + $"_{lang}.properties");
    }

    private IEnumerable<IGrouping<string, IFieldProperty>> GetModules()
    {
        return Files
            .SelectMany(file => file.Value.Classes.SelectMany(c => c.Properties.OfType<IFieldProperty>()))
            .Select(c => c.ResourceProperty)
            .Where(p => p.Label != null || (p.Class?.Values.Any() ?? false) && p.Class?.DefaultProperty != null)
            .Distinct()
            .GroupBy(prop => prop.Parent.Namespace.Module.Split('.').First());
    }

    private void GenerateModule(IGrouping<string, IFieldProperty> module, string lang)
    {
        if (_config.ResourceRootPath == null)
        {
            return;
        }

        var filePath = GetFilePath(module, lang);

        using var fw = new FileWriter(filePath, _logger, Encoding.Latin1) { EnableHeader = false };
        var containers = module.GroupBy(prop => prop.Parent);

        foreach (var container in containers.OrderBy(c => c.Key.Name))
        {
            WriteClasse(fw, container, lang);
        }
    }

    /// <summary>
    /// Générère le noeus de classe.
    /// </summary>
    /// <param name="fw">Flux de sortie.</param>
    /// <param name="container">Classe.</param>
    private void WriteClasse(FileWriter fw, IGrouping<IPropertyContainer, IFieldProperty> container, string lang)
    {
        foreach (var property in container)
        {
            if (property.Label != null)
            {
                fw.WriteLine($"{property.ResourceKey}={_translationStore.GetTranslation(property, lang)}");
            }
        }

        if (container.Key is Class classe && classe.DefaultProperty != null)
        {
            foreach (var val in classe.Values)
            {
                fw.WriteLine($"{val.ResourceKey}={_translationStore.GetTranslation(val, lang)}");
            }
        }
    }
}