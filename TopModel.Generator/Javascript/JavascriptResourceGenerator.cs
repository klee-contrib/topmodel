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

        var modules = files.SelectMany(f => f.Classes.Select(c => c.Namespace.Module)).Distinct();

        foreach (var module in modules)
        {
            GenerateModule(module);
        }
    }

    private void GenerateModule(string module)
    {
        if (_config.ResourceOutputDirectory == null)
        {
            return;
        }

        var classes = _files.Values
            .SelectMany(f => f.Classes)
            .Distinct()
            .Where(c => c.Namespace.Module == module);

        var dirInfo = Directory.CreateDirectory(_config.ResourceOutputDirectory);
        var fileName = module.ToDashCase();
        var filePath = dirInfo.FullName + "/" + fileName + (_config.ResourceMode == ResourceMode.JS ? ".ts" : ".json");

        using var fw = new FileWriter(filePath, _logger, encoderShouldEmitUTF8Identifier: false) { EnableHeader = _config.ResourceMode == ResourceMode.JS };

        if (_config.ResourceMode != ResourceMode.JS)
        {
            fw.WriteLine("{");

            if (_config.ResourceMode == ResourceMode.Schema)
            {
                fw.WriteLine($"  \"$id\": \"{module.ToDashCase()}_translation.json\",");
                fw.WriteLine("  \"$schema\": \"http://json-schema.org/draft-07/schema#\",");
                fw.WriteLine("  \"properties\": {");
            }
        }
        else
        {
            fw.WriteLine($"export const {module.ToFirstLower()} = {{");
        }

        var i = 1;
        foreach (var classe in classes.OrderBy(c => c.Name))
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
    private void WriteClasseNode(FileWriter fw, Class classe, bool isLast)
    {
        fw.WriteLine($"    {Quote(classe.Name)}: {{");

        if (_config.ResourceMode == ResourceMode.Schema)
        {
            fw.WriteLine("      \"type\": \"object\",");
            fw.WriteLine($"      \"description\": \"{classe.Comment.Replace("\"", "\\\"")}\",");
            fw.WriteLine("      \"properties\": {");
        }

        var i = 1;

        foreach (var property in classe.Properties)
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

            fw.WriteLine(classe.Properties.Count == i++ ? string.Empty : ",");
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