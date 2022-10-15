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

    public JavascriptResourceGenerator(ILogger<JavascriptResourceGenerator> logger, JavascriptConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "JSResourceGen";

    public override IEnumerable<string> GeneratedFiles => Files
        .SelectMany(file => file.Value.Classes.SelectMany(c => c.Properties.OfType<IFieldProperty>()))
        .Select(c => c.ResourceProperty)
        .Distinct()
        .GroupBy(prop => prop.Class.Namespace.Module).Select(m => GetFilePath(m));

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        var modules = Classes
            .SelectMany(c => c.Properties.OfType<IFieldProperty>())
            .Select(c => c.ResourceProperty)
            .Distinct()
            .GroupBy(prop => prop.Class.Namespace.Module);

        foreach (var module in modules)
        {
            GenerateModule(module);
        }
    }

    private string GetFilePath(IGrouping<string, IFieldProperty> module)
    {
        return Path.Combine(_config.OutputDirectory, _config.ResourceRootPath!, Path.Combine(module.Key.Split(".").Select(part => part.ToDashCase()).ToArray())) + (_config.ResourceMode == ResourceMode.JS ? ".ts" : ".json");
    }

    private void GenerateModule(IGrouping<string, IFieldProperty> module)
    {
        if (_config.ResourceRootPath == null)
        {
            return;
        }

        var dirInfo = Directory.CreateDirectory(Path.Combine(_config.OutputDirectory, _config.ResourceRootPath));
        var filePath = GetFilePath(module);

        using var fw = new FileWriter(filePath, _logger, encoderShouldEmitUTF8Identifier: false) { EnableHeader = _config.ResourceMode == ResourceMode.JS };

        if (_config.ResourceMode != ResourceMode.JS)
        {
            fw.WriteLine("{");

            if (_config.ResourceMode == ResourceMode.Schema)
            {
                fw.WriteLine($"  \"$id\": \"{string.Join('/', module.Key.Split('.').Select(m => m.ToDashCase()))}_translation.json\",");
                fw.WriteLine("  \"$schema\": \"http://json-schema.org/draft-07/schema#\",");
                fw.WriteLine("  \"properties\": {");
            }
        }
        else
        {
            fw.WriteLine($"export const {module.Key.Split('.').Last().ToFirstLower()} = {{");
        }

        var classes = module.GroupBy(prop => prop.Class);

        var i = 1;
        foreach (var classe in classes.OrderBy(c => c.Key.Name))
        {
            WriteClasseNode(fw, classe, classes.Count() == i++);
        }

        if (_config.ResourceMode == ResourceMode.Schema)
        {
            fw.WriteLine("  }");
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

    /// <summary>
    /// Générère le noeus de classe.
    /// </summary>
    /// <param name="fw">Flux de sortie.</param>
    /// <param name="classe">Classe.</param>
    /// <param name="isLast">True s'il s'agit de al dernière classe du namespace.</param>
    private void WriteClasseNode(FileWriter fw, IGrouping<Class, IFieldProperty> classe, bool isLast)
    {
        fw.WriteLine($"    {Quote(classe.Key.Name)}: {{");

        if (_config.ResourceMode == ResourceMode.Schema)
        {
            fw.WriteLine("      \"type\": \"object\",");
            fw.WriteLine($"      \"description\": \"{classe.Key.Comment.Replace("\"", "\\\"")}\",");
            fw.WriteLine("      \"properties\": {");
        }

        var i = 1;

        foreach (var property in classe)
        {
            fw.Write($"        {Quote(property.Name)}: ");

            if (_config.ResourceMode == ResourceMode.Schema)
            {
                fw.WriteLine("{");
                fw.WriteLine("          \"type\": \"string\",");
                fw.WriteLine($"          \"description\": \"{property.Comment.Replace("\"", "\\\"")}\"");
                fw.Write("        }");
            }
            else
            {
                fw.Write($@"""{property.Label ?? property.Name}""");
            }

            fw.WriteLine(classe.Count() == i++ ? string.Empty : ",");
        }

        if (_config.ResourceMode == ResourceMode.Schema)
        {
            fw.WriteLine("      }");
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