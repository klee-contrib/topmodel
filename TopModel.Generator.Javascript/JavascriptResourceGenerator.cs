using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Javascript;

/// <summary>
/// Générateur des objets de traduction javascripts.
/// </summary>
public class JavascriptResourceGenerator : TranslationGeneratorBase<JavascriptConfig>
{
    private readonly ILogger<JavascriptResourceGenerator> _logger;
    private readonly ModelConfig _modelConfig;
    private readonly TranslationStore _translationStore;

    public JavascriptResourceGenerator(ILogger<JavascriptResourceGenerator> logger, TranslationStore translationStore, ModelConfig modelConfig)
        : base(logger, translationStore)
    {
        _logger = logger;
        _modelConfig = modelConfig;
        _translationStore = translationStore;
    }

    public override string Name => "JSResourceGen";

    protected override string? GetCommentResourceFilePath(IFieldProperty property, string tag, string lang)
    {
        if (!Config.GenerateComments)
        {
            return null;
        }

        return Config.GetCommentResourcesFilePath(property.Parent.Namespace, tag, _modelConfig.I18n.DefaultLang);
    }

    protected override string? GetMainResourceFilePath(string tag, string lang)
    {
        if (!Config.GenerateMainResourceFiles || Config.ResourceMode != ResourceMode.JS)
        {
            return null;
        }

        return Config.GetMainResourceFilePath(tag, lang);
    }

    protected override string GetResourceFilePath(IFieldProperty property, string tag, string lang)
    {
        return Config.GetResourcesFilePath(property.Parent.Namespace, tag, lang);
    }

    protected override void HandleCommentResourceFile(string filePath, string lang, IEnumerable<IFieldProperty> properties)
    {
        using var fw = new FileWriter(filePath, _logger, encoderShouldEmitUTF8Identifier: false) { EnableHeader = Config.ResourceMode == ResourceMode.JS };

        var module = properties.First().Parent.Namespace.RootModule;

        if (Config.ResourceMode != ResourceMode.JS)
        {
            fw.WriteLine("{");
        }
        else
        {
            fw.WriteLine($"export const {module.ToCamelCase()}Comments = {{");
        }

        WriteSubModule(fw, _modelConfig.I18n.DefaultLang, properties, true, 1);

        if (Config.ResourceMode != ResourceMode.JS)
        {
            fw.WriteLine("}");
        }
        else
        {
            fw.WriteLine("};");
        }
    }

    protected override void HandleMainResourceFile(string mainFilePath, IEnumerable<(string ModuleFilePath, string ModuleName)> modules)
    {
        using var fw = new FileWriter(mainFilePath, _logger, encoderShouldEmitUTF8Identifier: false) { EnableHeader = true };

        foreach (var (moduleFilePath, moduleName) in modules)
        {
            fw.WriteLine($"import {{{moduleName.ToCamelCase()}}} from \"./{Path.GetRelativePath(Path.GetDirectoryName(mainFilePath)!, moduleFilePath).Replace("\\", "/").Replace(".ts", string.Empty)}\";");
        }

        fw.WriteLine();
        fw.WriteLine($"export const all = {{{string.Join(", ", modules.Where(m => !m.ModuleFilePath.EndsWith(".comments.ts")).Select(m => m.ModuleName.ToCamelCase()))}}};");

        var comments = modules.Where(m => m.ModuleFilePath.EndsWith(".comments.ts")).Select(m => m.ModuleName.ToCamelCase());
        if (comments.Any())
        {
            fw.WriteLine($@"export const allComments = {{
    {string.Join($",{Environment.NewLine}    ", comments.Select(c => $"{c[0..^8]}: {c}"))}
}};");
        }
    }

    protected override void HandleResourceFile(string filePath, string lang, IEnumerable<IFieldProperty> properties)
    {
        using var fw = new FileWriter(filePath, _logger, encoderShouldEmitUTF8Identifier: false) { EnableHeader = Config.ResourceMode == ResourceMode.JS };

        var module = properties.First().Parent.Namespace.RootModule;

        if (Config.ResourceMode != ResourceMode.JS)
        {
            fw.WriteLine("{");
        }
        else
        {
            fw.WriteLine($"export const {module.ToCamelCase()} = {{");
        }

        WriteSubModule(fw, lang, properties, false, 1);

        if (Config.ResourceMode != ResourceMode.JS)
        {
            fw.WriteLine("}");
        }
        else
        {
            fw.WriteLine("};");
        }
    }

    private string Quote(string name)
    {
        return Config.ResourceMode == ResourceMode.JS ? name : $@"""{name}""";
    }

    private void WriteClasseNode(FileWriter fw, IGrouping<IPropertyContainer, IFieldProperty> container, bool isComment, bool isLast, string lang, int indentLevel)
    {
        fw.WriteLine(indentLevel, $"{Quote(container.Key.NameCamel)}: {{");

        var i = 1;
        if (Config.TranslateProperties == true)
        {
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
                fw.WriteLine(container.Count() == i++ && !(Config.TranslateReferences == true && container.Key is Class { DefaultProperty: not null, Enum: true } && ((container.Key as Class)?.Values.Any() ?? false)) ? string.Empty : ",");
            }
        }

        if (Config.TranslateReferences == true && container.Key is Class { DefaultProperty: not null, Enum: true } classe && (classe?.Values.Any() ?? false))
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
}