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
    private readonly IDictionary<string, ModelFile> _files = new Dictionary<string, ModelFile>();

    public JpaResourceGenerator(ILogger<JpaResourceGenerator> logger, JpaConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "JpaResourceGenerator";

    public override IEnumerable<string> GeneratedFiles => _files
        .SelectMany(file => file.Value.Classes.SelectMany(c => c.Properties.OfType<IFieldProperty>()))
        .Select(c => c.ResourceProperty)
        .Distinct()
        .GroupBy(prop => prop.Class.Namespace.Module).Select(m => GetFilePath(m));

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

    private string GetFilePath(IGrouping<string, IFieldProperty> module)
    {
        return _config.ResourcesOutputDirectory!
            + "\\"
            + string.Join('\\', module.Key.Split(".").Select(part => part.ToDashCase()))
            + "_fr_FR.properties";
    }

    private void GenerateModule(IGrouping<string, IFieldProperty> module)
    {
        if (_config.ResourcesOutputDirectory == null)
        {
            return;
        }

        var dirInfo = Directory.CreateDirectory(_config.ResourcesOutputDirectory);
        var filePath = GetFilePath(module);

        using var fw = new FileWriter(filePath, _logger, encoderShouldEmitUTF8Identifier: false) { EnableHeader = false };
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
                    name = ap.GetAssociationName().ToSnakeCase();
                }
                else
                {
                    name = property.Name.ToSnakeCase();
                }

                fw.WriteLine($"{classe.Key.Name.ToString().ToSnakeCase()}.{name}={property.Label}");
            }
        }

        if (classe.Key.DefaultProperty != null)
        {
            foreach (var val in classe.Key.ReferenceValues)
            {
                var key = val.Value[classe.Key.PrimaryKey];
                var value = val.Value[classe.Key.DefaultProperty];
                fw.WriteLine($"{classe.Key.Name.ToString().ToSnakeCase()}.VALUES.{key}={value}");
            }
        }
    }
}