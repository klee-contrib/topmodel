using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.Generator.Markdown;

/// <summary>
/// Générateur de documentation au format Markdown.
/// </summary>
public class MarkdownGenerator : GeneratorBase
{
    private readonly MarkdownConfig _config;
    private readonly ILogger<MarkdownGenerator> _logger;
    private readonly IDictionary<string, ModelFile> _files = new Dictionary<string, ModelFile>();

    public MarkdownGenerator(ILogger<MarkdownGenerator> logger, MarkdownConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "MarkdownGen";

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files)
        {
            _files[file.Name] = file;
        }

        var modules = files.SelectMany(f => f.Classes.Select(c => c.Namespace.Module)).Distinct();
        var destFolder = Path.Combine(_config.DocOutputDirectory);
        var dirInfo = Directory.CreateDirectory(_config.DocOutputDirectory);
        using var fw = new MarkdownFileWriter(destFolder + "/classDiagram" + ".md", _logger);
        fw.writeTitle(1, "Documentation technique");
        fw.WriteLine();
        fw.writeTitle(1, "Diagramme de classe");
        fw.WriteLine();
        fw.WriteLine("```mermaid");
        fw.WriteLine("classDiagram");
        foreach (var module in modules)
        {
            GenerateDiagramModule(module, fw);
        }
        fw.WriteLine("```");
        fw.WriteLine();
        foreach (var module in modules)
        {
            GenerateDocModule(module, fw);
        }
        fw.Close();

    }

    private void GenerateDiagramModule(string module, MarkdownFileWriter fw)
    {
        var classes = _files.Values
            .SelectMany(f => f.Classes)
            .Distinct()
            .Where(c => c.Namespace.Module == module && (c.IsPersistent || _config.GenerateNotPersisted));

        foreach (var classe in classes)
        {
            fw.WriteLine("");
            fw.WriteLine(@$"%% {classe.Comment}");
            fw.WriteLine(@$"class {classe.Name}{{");
            if (classe.Reference)
            {
                fw.WriteLine("<<Enum>>");
                foreach (var refValue in classe.ReferenceValues.OrderBy(x => x.Name, StringComparer.Ordinal))
                {
                    var code = classe.PrimaryKey == null || classe.PrimaryKey.Domain.Name == "DO_CODE"
                        ? (string)refValue.Value[classe.PrimaryKey ?? classe.Properties.OfType<IFieldProperty>().First()]
                        : (string)refValue.Value[classe.UniqueKeys!.First().First()];

                    fw.WriteLine(code);
                }
                fw.WriteLine("}");
                continue;
            }
            foreach (var property in classe.Properties.OfType<RegularProperty>())
            {
                fw.WriteLine($" {property.Domain.Name} {property.Name}");
            }
            fw.WriteLine("}");

            foreach (var property in classe.Properties.OfType<AssociationProperty>())
            {
                if (property.Type != null)
                {
                    var from = property.Type == AssociationType.ManyToOne || property.Type == AssociationType.ManyToMany ? "*" : property.Required ? "1" : "0";
                    var to = property.Type == AssociationType.OneToOne || property.Type == AssociationType.ManyToOne ? property.Required ? "1" : "0" : "*";
                    fw.WriteLine(@$"{property.Class.Name} ""{from}"" --> ""{to}"" {property.Association.Name}");
                }
                else
                {
                    fw.WriteLine($"{property.Class.Name} --> {property.Association.Name}{(property.Role != null ? property.Role : "")}");
                }
            }
            foreach (var property in classe.Properties.OfType<CompositionProperty>())
            {
                fw.WriteLine($"{property.Class.Name} --* {property.Composition.Name}");
            }
        }
    }
    private void GenerateDocModule(string module, MarkdownFileWriter fw)
    {
        var classes = _files.Values
            .SelectMany(f => f.Classes)
            .Distinct()
            .Where(c => c.Namespace.Module == module && c.IsPersistent);

        fw.writeTitle(2, module);
        fw.WriteLine();
        foreach (var classe in classes)
        {
            fw.writeTitle(3, classe.Name);
            fw.WriteLine($"_{classe.Comment}_");
            fw.WriteLine($"| Name | Comment  | Domain | Required |");
            fw.WriteLine($"| --- | --- | --- | --- |");
            if (classe.PrimaryKey != null)
            {
                fw.WriteLine($"| **{classe.PrimaryKey.Name}** | **{classe.PrimaryKey.Comment}** | **{classe.PrimaryKey.Domain.Name}** | :heavy_check_mark: |");
            }
            foreach (var property in classe.Properties.OfType<AssociationProperty>())
            {
                fw.WriteLine($"| _{property.Name}_ | _{property.Comment}_ | _{property.Domain.Name}_ | {(property.Required ? ":heavy_check_mark:": "")}|");
            }
            foreach (var property in classe.Properties.OfType<RegularProperty>().Where(p => !p.PrimaryKey))
            {
                fw.WriteLine($"| {property.Name} | {property.Comment} | {property.Domain.Name} | {(property.Required ? ":heavy_check_mark:": "")}|");
            }
            if (classe.UniqueKeys != null && classe.UniqueKeys.Count > 0)
            {
                foreach (var u in classe.UniqueKeys)
                {

                    fw.writeTitle(4, "Contraintes d'unicité");
                    fw.WriteLine($">    {String.Join(", ", u.Select(t => t.Name + " (" + t.Comment + ")"))}");
                    fw.WriteLine();

                }
            }
        }
    }
}