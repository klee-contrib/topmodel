using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Javascript;

/// <summary>
/// Générateur des objets de traduction javascripts.
/// </summary>
public class JavascriptResourceGenerator : TranslationGeneratorBase
{
    private readonly JavascriptConfig _config;
    private readonly ILogger<JavascriptResourceGenerator> _logger;
    private readonly TranslationStore _translationStore;
    private readonly ModelConfig _modelConfig;

    public JavascriptResourceGenerator(ILogger<JavascriptResourceGenerator> logger, JavascriptConfig config, TranslationStore translationStore, ModelConfig modelConfig)
        : base(logger, config, translationStore)
    {
        _config = config;
        _logger = logger;
        _translationStore = translationStore;
        _modelConfig = modelConfig;
    }

    public override string Name => "JSResourceGen";

    protected override string? GetCommentResourceFilePath(IFieldProperty property, string tag, string lang)
    {
        if (!_config.GenerateComments)
        {
            return null;
        }

        return _config.GetCommentResourcesFilePath(property.Parent.Namespace, tag, _modelConfig.I18n.DefaultLang);
    }

    protected override string GetResourceFilePath(IFieldProperty property, string tag, string lang)
    {
        return _config.GetResourcesFilePath(property.Parent.Namespace, tag, lang);
    }

    protected override void HandleCommentResourceFile(string filePath, string lang, IEnumerable<IFieldProperty> properties)
    {
        using var fw = new FileWriter(filePath, _logger, encoderShouldEmitUTF8Identifier: false) { EnableHeader = _config.ResourceMode == ResourceMode.JS };

        var module = properties.First().Parent.Namespace.RootModule;

        if (_config.ResourceMode != ResourceMode.JS)
        {
            fw.WriteLine("{");
        }
        else
        {
            fw.WriteLine($"export const {module.ToCamelCase()} = {{");
        }

        WriteSubModule(fw, _modelConfig.I18n.DefaultLang, properties, true, 1);

        if (_config.ResourceMode != ResourceMode.JS)
        {
            fw.WriteLine("}");
        }
        else
        {
            fw.WriteLine("};");
        }
    }

    protected override void HandleResourceFile(string filePath, string lang, IEnumerable<IFieldProperty> properties)
    {
        using var fw = new FileWriter(filePath, _logger, encoderShouldEmitUTF8Identifier: false) { EnableHeader = _config.ResourceMode == ResourceMode.JS };

        var module = properties.First().Parent.Namespace.RootModule;

        if (_config.ResourceMode != ResourceMode.JS)
        {
            fw.WriteLine("{");
        }
        else
        {
            fw.WriteLine($"export const {module.ToCamelCase()} = {{");
        }

        WriteSubModule(fw, lang, properties, false, 1);

        if (_config.ResourceMode != ResourceMode.JS)
        {
            fw.WriteLine("}");
        }
        else
        {
            fw.WriteLine("};");
        }
    }

    private void WriteSubModule(FileWriter fw, string lang, IEnumerable<IFieldProperty> properties, bool isComment, int level)
    {
        var classes = properties.GroupBy(prop => prop.Parent);
        var modules = classes
            .GroupBy(c => c.Key.Namespace.Module.Split('.').Skip(level).ElementAtOrDefault(0));
        var u = 1;
        foreach (var submodule in modules.OrderBy(m => m.Key, StringComparer.Ordinal))
        {
            var isLast = u++ == modules.Count();
            if (submodule.Key == null)
            {
                var i = 1;
                foreach (var container in submodule.OrderBy(c => c.Key.NameCamel))
                {
                    WriteClasseNode(fw, container, isComment, classes.Count() == i++ && isLast, lang, level);
                }
            }
            else
            {
                fw.WriteLine(level, $@"""{submodule.Key.Split('.').First().ToCamelCase()}"": {{");
                WriteSubModule(fw, lang, submodule.Select(m => m.Key).SelectMany(c => c.Properties).OfType<IFieldProperty>(), isComment, level + 1);
                if (isLast)
                {
                    fw.WriteLine(level, "}");
                }
                else
                {
                    fw.WriteLine(level, "},");
                }
            }
        }
    }

    private void WriteClasseNode(FileWriter fw, IGrouping<IPropertyContainer, IFieldProperty> container, bool isComment, bool isLast, string lang, int indentLevel)
    {
        fw.WriteLine(indentLevel, $"{Quote(container.Key.NameCamel)}: {{");

        var i = 1;

        foreach (var property in container.OrderBy(p => p.NameCamel, StringComparer.Ordinal))
        {
            var translation = isComment
                ? property.CommentResourceProperty.Comment.Replace(Environment.NewLine, " ").Replace("\"", "'")
                : _translationStore.GetTranslation(property, lang);

            if (translation == string.Empty)
            {
                translation = property.Name;
            }

            fw.Write(indentLevel + 1, $"{Quote(property.NameCamel)}: ");
            fw.Write($@"""{translation}""");
            fw.WriteLine(container.Count() == i++ && !(_modelConfig.I18n.TranslateReferences && (container.Key as Class)?.DefaultProperty != null && ((container.Key as Class)?.Values.Any() ?? false)) ? string.Empty : ",");
        }

        if (container.Key is Class classe && _modelConfig.I18n.TranslateReferences && classe.EnumKey != null && classe.DefaultProperty != null)
        {
            i = 1;
            fw.WriteLine(indentLevel + 1, @$"{Quote("values")}: {{");
            foreach (var refValue in classe.Values)
            {
                fw.Write(indentLevel + 2, $@"{Quote(refValue.Name)}: ");
                fw.Write($@"""{_translationStore.GetTranslation(refValue, lang)}""");
                fw.WriteLine(classe.Values.Count == i++ ? string.Empty : ",");
            }

            fw.WriteLine(indentLevel + 1, "}");
        }

        fw.Write(indentLevel, "}");
        fw.WriteLine(!isLast ? "," : string.Empty);
    }

    private string Quote(string name)
    {
        return _config.ResourceMode == ResourceMode.JS ? name : $@"""{name}""";
    }
}