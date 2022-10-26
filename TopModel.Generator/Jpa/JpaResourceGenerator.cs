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

    public JpaResourceGenerator(ILogger<JpaResourceGenerator> logger, JpaConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "JpaResourceGen";

    public override IEnumerable<string> GeneratedFiles => GetModules().Select(m => GetFilePath(m));

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        var modules = GetModules();

        foreach (var module in modules)
        {
            GenerateModule(module);
        }
    }

    private string GetFilePath(IGrouping<string, IFieldProperty> module)
    {
        return Path.Combine(_config.OutputDirectory, _config.ResourceRootPath, Path.Combine(module.Key.Split(".").Select(part => part.ToKebabCase()).ToArray()) + "_fr_FR.properties");
    }

    private IEnumerable<IGrouping<string, IFieldProperty>> GetModules()
    {
        return Files
                    .SelectMany(file => file.Value.Classes.SelectMany(c => c.Properties.OfType<IFieldProperty>()))
                    .Select(c => c.ResourceProperty)
                    .Where(p => p.Label != null || p.Class.ReferenceValues.Any())
                    .Distinct()
                    .GroupBy(prop => prop.Class.Namespace.Module);
    }

    private void GenerateModule(IGrouping<string, IFieldProperty> module)
    {
        if (_config.ResourceRootPath == null)
        {
            return;
        }

        var dirInfo = Directory.CreateDirectory(Path.Combine(_config.OutputDirectory, _config.ResourceRootPath));
        var filePath = GetFilePath(module);

        using var fw = new FileWriter(filePath, _logger, Encoding.Latin1) { EnableHeader = false };
        var classes = module.GroupBy(prop => prop.Class);

        foreach (var classe in classes.OrderBy(c => c.Key.Name))
        {
            WriteClasse(fw, classe);
        }
    }

    /// <summary>
    /// Générère le noeus de classe.
    /// </summary>
    /// <param name="fw">Flux de sortie.</param>
    /// <param name="classe">Classe.</param>
    private void WriteClasse(FileWriter fw, IGrouping<Class, IFieldProperty> classe)
    {
        foreach (var property in classe)
        {
            if (property.Label != null)
            {
                string name;
                if (property is AssociationProperty ap)
                {
                    name = ap.GetAssociationName().ToConstantCase();
                }
                else
                {
                    name = property.Name.ToConstantCase();
                }

                fw.WriteLine($"{classe.Key.Name.ToString().ToConstantCase()}.{name}={property.Label}");
            }
        }

        if (classe.Key.DefaultProperty != null)
        {
            foreach (var val in classe.Key.ReferenceValues)
            {
                var key = val.Value[classe.Key.PrimaryKey];
                var value = val.Value[classe.Key.DefaultProperty];
                fw.WriteLine($"{classe.Key.Name.ToString().ToConstantCase()}.VALUES.{key}={value}");
            }
        }
    }
}