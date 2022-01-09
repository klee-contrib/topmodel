using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.Generator.Javascript;

/// <summary>
/// Générateur des objets de traduction javascripts.
/// </summary>
public class JavascriptResourceGenerator : GeneratorBase
{
    private readonly JavascriptConfig _config;
    private readonly ILogger<JavascriptResourceGenerator> _logger;
    private readonly IDictionary<string, ModelFile> _files = new Dictionary<string, ModelFile>();

    public JavascriptResourceGenerator(ILogger<JavascriptResourceGenerator> logger, JavascriptConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "JSResourceGen";

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files)
        {
            _files[file.Name] = file;
        }

        var modules = _files
            .SelectMany(file => file.Value.Classes.SelectMany(c => c.Properties.OfType<IFieldProperty>()))
            .Select(c => c.ResourceProperty)
            .Distinct()
            .GroupBy(prop => prop.Class.Namespace.Module);

        foreach (var module in modules)
        {
            GenerateModule(module);
        }
    }

    private void GenerateModule(IGrouping<string, IFieldProperty> module)
    {
        if (_config.ResourceOutputDirectory == null)
        {
            return;
        }

        var dirInfo = Directory.CreateDirectory(_config.ResourceOutputDirectory);
        var filePath = dirInfo.FullName + "/" + string.Join("/", module.Key.Split(".").Select(part => part.ToDashCase())) + (_config.ResourceMode == ResourceMode.JS ? ".ts" : ".json");

        using var fw = new FileWriter(filePath, _logger, encoderShouldEmitUTF8Identifier: false) { EnableHeader = _config.ResourceMode == ResourceMode.JS };

        if (_config.ResourceMode != ResourceMode.JS)
        {
            fw.WriteLine("{");

            if (_config.ResourceMode == ResourceMode.Schema)
            {
                fw.WriteLine($"  \"$id\": \"{module.Key.Replace('.', '/').ToDashCase()}_translation.json\",");
                fw.WriteLine("  \"$schema\": \"http://json-schema.org/draft-07/schema#\",");
                fw.WriteLine("  \"properties\": {");
            }
        }
        else
        {
            fw.WriteLine($"export const {module.Key.ToFirstLower()} = {{");
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