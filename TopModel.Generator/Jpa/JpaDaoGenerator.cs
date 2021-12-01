using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.Generator.Jpa;

/// <summary>
/// Générateur de DAOs JPA.
/// </summary>
public class JpaDaoGenerator : GeneratorBase
{
    private readonly JpaConfig _config;
    private readonly ILogger<JpaDaoGenerator> _logger;
    private readonly IDictionary<string, ModelFile> _files = new Dictionary<string, ModelFile>();

    public JpaDaoGenerator(ILogger<JpaDaoGenerator> logger, JpaConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "JpaDaoGen";

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
        var classes = _files.Values
            .SelectMany(f => f.Classes)
            .Distinct()
            .Where(c => c.Namespace.Module == module);

        foreach (var classe in classes.Where(c => !c.Reference && c.IsPersistent))
        {
            var destFolder = Path.Combine(_config.ModelOutputDirectory, Path.Combine(_config.DaoPackageName.Split(".")), "daos", classe.Namespace.Module.Replace('.', '/').ToLower());
            var dirInfo = Directory.CreateDirectory(destFolder);
            var packageName = $"{_config.DaoPackageName}.daos.{classe.Namespace.Module.ToLower()}";
            var fileName = $"{destFolder}/{classe.Name}DAO.java";

            var fileExists = File.Exists(fileName);

            // Ne génère le DAO qu'une seule fois
            if (fileExists)
            {
                continue;
            }

            using var fw = new JavaWriter(fileName, _logger, null);
            fw.WriteLine($"package {packageName};");
            fw.WriteLine();
            WriteImports(fw, classe);
            fw.WriteLine();
            fw.WriteLine("@Generated(\"TopModel : https://github.com/JabX/topmodel\")");
            fw.WriteLine($"public interface {classe.Name}DAO extends PagingAndSortingRepository<{classe.Name}, Long> {{");
            fw.WriteLine();
            fw.WriteLine("}");
        }
    }

    private void WriteImports(JavaWriter fw, Class classe)
    {
        var imports = new List<string>
            {
                "org.springframework.data.repository.PagingAndSortingRepository",
                "javax.annotation.Generated",
                $"{_config.DaoPackageName}.entities.{classe.Namespace.Module.ToLower()}.{classe.Name}"
            };
        fw.WriteImports(imports.Distinct().ToArray());
    }
}