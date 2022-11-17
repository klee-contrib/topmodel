using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Generator.Javascript;

/// <summary>
/// Générateur des objets de traduction javascripts.
/// </summary>
public class JavascriptResourceGenerator : GeneratorBase
{
    private readonly JavascriptConfig _config;
    private readonly ILogger<JavascriptResourceGenerator> _logger;
    private readonly TranslationStore _translationStore;

    public JavascriptResourceGenerator(ILogger<JavascriptResourceGenerator> logger, JavascriptConfig config, TranslationStore translationStore)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
        _translationStore = translationStore;
    }

    public override string Name => "JSResourceGen";

    public override IEnumerable<string> GeneratedFiles => _config.Tags
        .SelectMany(tag =>
            Classes
                .Where(c => c.ModelFile.Tags.Contains(tag))
                .SelectMany(c => c.Properties.OfType<IFieldProperty>())
                .Select(c => c.ResourceProperty)
                .SelectMany(c =>
                new List<string> { _config.GetResourcesFilePath(c.Class.Namespace.Module, tag, string.Empty) }.Union(
                _translationStore.Translations.Select(lang => _config.GetResourcesFilePath(c.Class.Namespace.Module, tag, lang.Key)))))
                .Distinct();

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var module in files.SelectMany(f => f.Classes.Select(c => c.Namespace.Module)).Distinct())
        {
            GenerateModule(module);
            foreach (var lang in _translationStore.Translations)
            {
                GenerateModule(module, lang.Key);
            }
        }
    }

    private void GenerateModule(string module, string lang = "")
    {
        if (_config.ResourceRootPath == null)
        {
            return;
        }

        foreach (var group in _config.Tags
            .Select(tag => (tag, fileName: _config.GetResourcesFilePath(module, tag, lang)))
            .GroupBy(t => t.fileName))
        {
            var properties = Classes
                .Where(c => c.ModelFile.Tags.Intersect(group.Select(t => t.tag)).Any())
                .SelectMany(c => c.Properties.OfType<IFieldProperty>())
                .Select(c => c.ResourceProperty)
                .Distinct()
                .Where(prop => prop.Class.Namespace.Module == module);

            if (properties.Any())
            {
                using var fw = new FileWriter(group.Key, _logger, encoderShouldEmitUTF8Identifier: false) { EnableHeader = _config.ResourceMode == ResourceMode.JS };

                if (_config.ResourceMode != ResourceMode.JS)
                {
                    fw.WriteLine("{");
                }
                else
                {
                    fw.WriteLine($"export const {module.Split('.').Last().ToFirstLower()} = {{");
                }

                var classes = properties.GroupBy(prop => prop.Class);

                var i = 1;
                foreach (var classe in classes.OrderBy(c => c.Key.Name))
                {
                    WriteClasseNode(fw, classe, classes.Count() == i++, lang);
                }

                if (_config.ResourceMode != ResourceMode.JS)
                {
                    fw.WriteLine("}");
                }
                else
                {
                    fw.WriteLine("};");
                }
            }
        }
    }

    /// <summary>
    /// Générère le noeus de classe.
    /// </summary>
    /// <param name="fw">Flux de sortie.</param>
    /// <param name="classe">Classe.</param>
    /// <param name="isLast">True s'il s'agit de al dernière classe du namespace.</param>
    private void WriteClasseNode(FileWriter fw, IGrouping<Class, IFieldProperty> classe, bool isLast, string lang = "")
    {
        fw.WriteLine($"    {Quote(classe.Key.Name)}: {{");

        var i = 1;

        foreach (var property in classe.OrderBy(p => p.Name, StringComparer.Ordinal))
        {
            fw.Write($"        {Quote(property.Name)}: ");
            fw.Write($@"""{(_translationStore.Translations.TryGetValue(lang, out var dict) && dict.TryGetValue(property.ResourceKey, out var translatedValue) ? translatedValue : property.Label ?? property.Name)}""");
            fw.WriteLine(classe.Count() == i++
                && !(_config.TranslateReferences && classe.Key.DefaultProperty != null && classe.Key.ReferenceValues.Any()) ? string.Empty : ",");
        }

        if (_config.TranslateReferences && classe.Key.DefaultProperty != null)
        {
            i = 1;
            fw.WriteLine(@$"        ""values"": {{");
            foreach (var refValue in classe.Key.ReferenceValues)
            {
                fw.Write($@"            ""{refValue.Name}"": ");
                fw.Write($@"""{(_translationStore.Translations.TryGetValue(lang, out var dict) && dict.TryGetValue(refValue.ResourceKey, out var translatedValue) ? translatedValue : refValue.Value[classe.Key.DefaultProperty])}""");
                fw.WriteLine(classe.Key.ReferenceValues.Count() == i++ ? string.Empty : ",");
            }

            fw.WriteLine("        }");
        }

        fw.Write("    }");
        fw.WriteLine(!isLast ? "," : string.Empty);
    }

    private string Quote(string name)
    {
        return _config.ResourceMode == ResourceMode.JS
            ? name.ToFirstLower()
            : $@"""{name.ToFirstLower()}""";
    }
}