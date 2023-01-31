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
    private readonly ModelConfig _modelConfig;

    public JavascriptResourceGenerator(ILogger<JavascriptResourceGenerator> logger, JavascriptConfig config, TranslationStore translationStore, ModelConfig modelConfig)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
        _translationStore = translationStore;
        _modelConfig = modelConfig;
    }

    public override string Name => "JSResourceGen";

    public override IEnumerable<string> GeneratedFiles => _config.Tags
        .SelectMany(tag =>
        {
            var properties = Classes
                .Where(c => c.ModelFile.Tags.Contains(tag))
                .SelectMany(c => c.Properties.OfType<IFieldProperty>());

            return properties
                .Select(c => c.ResourceProperty)
                .SelectMany(c => _translationStore.Translations.Select(lang => _config.GetResourcesFilePath(c.Parent.Namespace.Module, tag, lang.Key)))
                .Concat(_config.GenerateComments
                    ? properties.Select(c => _config.GetCommentResourcesFilePath(c.CommentResourceProperty.Parent.Namespace.Module, tag, _modelConfig.I18n.DefaultLang))
                    : Array.Empty<string>());
        })
        .Distinct();

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        var modules = Classes
            .SelectMany(c => c.Properties.OfType<IFieldProperty>())
            .Select(c => c.ResourceProperty.Parent.Namespace.Module.Split('.').First())
            .Distinct();
        foreach (var lang in _translationStore.Translations)
        {
            foreach (var module in modules)
            {
                GenerateModule(module, lang.Key);
            }
        }

        if (_config.GenerateComments)
        {
            foreach (var module in modules)
            {
                GenerateCommentModule(module);
            }
        }
    }

    private void GenerateModule(string module, string lang)
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
                .Where(prop => prop.Parent.Namespace.Module.Split('.').First() == module);
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
        }
    }

    private void GenerateCommentModule(string module)
    {
        if (_config.ResourceRootPath == null)
        {
            return;
        }

        foreach (var group in _config.Tags
            .Select(tag => (tag, fileName: _config.GetCommentResourcesFilePath(module, tag, _modelConfig.I18n.DefaultLang)))
            .GroupBy(t => t.fileName))
        {
            var properties = Classes
                .Where(c => c.ModelFile.Tags.Intersect(group.Select(t => t.tag)).Any())
                .SelectMany(c => c.Properties.OfType<IFieldProperty>())
                .Select(c => c.CommentResourceProperty)
                .Distinct()
                .Where(prop => prop.Parent.Namespace.Module.Split('.').First() == module);

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
                foreach (var container in submodule.OrderBy(c => c.Key.Name))
                {
                    WriteClasseNode(fw, container, isComment, classes.Count() == i++ && isLast, lang, level);
                }
            }
            else
            {
                fw.WriteLine(level, $@"""{submodule.Key.Split('.').First().ToFirstLower()}"": {{");
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
        fw.WriteLine(indentLevel, $"{Quote(container.Key.Name)}: {{");

        var i = 1;

        foreach (var property in container.OrderBy(p => p.Name, StringComparer.Ordinal))
        {
            var translation = isComment
                ? property.CommentResourceProperty.Comment.Replace(Environment.NewLine, " ").Replace("\"", "'")
                : _translationStore.GetTranslation(property, lang);

            if (translation == string.Empty)
            {
                translation = property.Name;
            }

            fw.Write(indentLevel + 1, $"{Quote(property.Name)}: ");
            fw.Write($@"""{translation}""");
            fw.WriteLine(container.Count() == i++ && !(_modelConfig.I18n.TranslateReferences && (container.Key as Class)?.DefaultProperty != null && ((container.Key as Class)?.ReferenceValues.Any() ?? false)) ? string.Empty : ",");
        }

        if (container.Key is Class classe && _modelConfig.I18n.TranslateReferences && classe.PrimaryKey?.Domain.AutoGeneratedValue != true && classe.DefaultProperty != null && classe.ReferenceValues.Any())
        {
            i = 1;
            fw.WriteLine(indentLevel + 1, @$"{Quote("values")}: {{");
            foreach (var refValue in classe.ReferenceValues)
            {
                fw.Write(indentLevel + 2, $@"{QuoteOnly(refValue.Name)}: ");
                fw.Write($@"""{_translationStore.GetTranslation(refValue, lang)}""");
                fw.WriteLine(classe.ReferenceValues.Count == i++ ? string.Empty : ",");
            }

            fw.WriteLine(indentLevel + 1, "}");
        }

        fw.Write(indentLevel, "}");
        fw.WriteLine(!isLast ? "," : string.Empty);
    }

    private string Quote(string name)
    {
        return _config.ResourceMode == ResourceMode.JS
            ? name.ToFirstLower()
            : $@"""{name.ToFirstLower()}""";
    }

    private string QuoteOnly(string name)
    {
        return _config.ResourceMode == ResourceMode.JS
            ? name
            : $@"""{name}""";
    }
}