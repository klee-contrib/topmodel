using Microsoft.Extensions.Logging;
using TopModel.Core;

namespace TopModel.Generator.Jpa;

/// <summary>
/// Générateur de DAOs JPA.
/// </summary>
public class JpaDaoGenerator : ClassGeneratorBase
{
    private readonly JpaConfig _config;
    private readonly ILogger<JpaDaoGenerator> _logger;

    public JpaDaoGenerator(ILogger<JpaDaoGenerator> logger, JpaConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "JpaDaoGen";

    protected override bool FilterClass(Class classe)
    {
        return classe.IsPersistent;
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return Path.Combine(
            _config.OutputDirectory,
            _config.ResolveTagVariables(tag, _config.ModelRootPath),
            Path.Combine(_config.ResolveTagVariables(tag, _config.DaosPackageName).Split(".")),
            classe.Namespace.Module.Replace('.', Path.DirectorySeparatorChar).ToLower(),
            $"{classe.Name}DAO.java");
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        // Ne génère le DAO qu'une seule fois
        if (File.Exists(fileName))
        {
            return;
        }

        var packageName = $"{_config.ResolveTagVariables(tag, _config.DaosPackageName)}.{classe.Namespace.Module.ToLower()}";

        using var fw = new JavaWriter(fileName, _logger, packageName, null);
        fw.WriteLine();
        WriteImports(fw, classe, tag);
        fw.WriteLine();
        fw.WriteLine($"public interface {classe.Name}DAO extends {(classe.Reference ? "CrudRepository" : "JpaRepository")}<{classe.Name}, {classe.PrimaryKey.Single().GetJavaType()}> {{");
        fw.WriteLine();
        fw.WriteLine("}");
    }

    private void WriteImports(JavaWriter fw, Class classe, string tag)
    {
        var imports = new List<string>
        {
            $"{_config.ResolveTagVariables(tag, _config.EntitiesPackageName)}.{classe.Namespace.Module.ToLower()}.{classe.Name}"
        };

        if (classe.Reference)
        {
            imports.Add("org.springframework.data.repository.CrudRepository");
        }
        else
        {
            imports.Add("org.springframework.data.jpa.repository.JpaRepository");
        }

        fw.AddImports(imports);
    }
}