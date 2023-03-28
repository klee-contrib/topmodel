using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Core;

namespace TopModel.Generator.Jpa;

/// <summary>
/// Générateur de DAOs JPA.
/// </summary>
public class JpaDaoGenerator : ClassGeneratorBase<JpaConfig>
{
    private readonly ILogger<JpaDaoGenerator> _logger;

    public JpaDaoGenerator(ILogger<JpaDaoGenerator> logger)
        : base(logger)
    {
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
            Config.OutputDirectory,
            Config.ResolveVariables(Config.DaosPath!, tag, module: classe.Namespace.Module).ToFilePath(),
            $"{classe.NamePascal}DAO.java");
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        // Ne génère le DAO qu'une seule fois
        if (File.Exists(fileName))
        {
            return;
        }

        var packageName = Config.ResolveVariables(
            Config.DaosPath!,
            tag,
            module: classe.Namespace.Module).ToPackageName();

        using var fw = new JavaWriter(fileName, _logger, packageName, null);
        fw.WriteLine();
        WriteImports(fw, classe, tag);
        fw.WriteLine();
        fw.WriteLine($"public interface {classe.NamePascal}DAO extends {(classe.Reference ? "CrudRepository" : "JpaRepository")}<{classe.NamePascal}, {classe.PrimaryKey.Single().GetJavaType()}> {{");
        fw.WriteLine();
        fw.WriteLine("}");
    }

    private void WriteImports(JavaWriter fw, Class classe, string tag)
    {
        var imports = new List<string>
        {
            classe.GetImport(Config, tag)
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