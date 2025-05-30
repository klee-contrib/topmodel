using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration;

/// <summary>
/// Générateur de DAOs JPA.
/// </summary>
public class JpaDaoGenerator(ILogger<JpaDaoGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGeneratorBase<JpaConfig>(logger, writerProvider)
{
    public override string Name => "JpaDaoGen";

    protected override bool FilterClass(Class classe)
    {
        return classe.IsPersistent && (!Config.UseJdbc || classe.PrimaryKey.Count() <= 1) && !Config.CanClassUseEnums(classe, Classes);
    }

    protected override string GetFileName(Class classe, string tag)
    {
        string className = GetClassName(classe);
        return Path.Combine(
            Config.OutputDirectory,
            Config.ResolveVariables(Config.DaosPath!, tag, module: classe.Namespace.Module).ToFilePath(),
            $"{className}.java");
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        // Ne génère le DAO qu'une seule fois
        if (!Config.DaosAbstract && File.Exists(fileName))
        {
            return;
        }

        var packageName = Config.ResolveVariables(
            Config.DaosPath!,
            tag,
            module: classe.Namespace.Module).ToPackageName();
        var javaClass = GetJavaClass(classe, tag);

        using var fw = this.OpenJavaWriter(fileName, packageName, null);
        fw.Write(0, javaClass);
    }

    private string GetClassName(Class classe)
    {
        return Config.DaosName != null ? Config.DaosName.Replace("{class}", classe.NamePascal) : $"{(Config.DaosAbstract ? "Abstract" : string.Empty)}{classe.NamePascal}DAO";
    }

    private JavaClass GetJavaClass(Class classe, string tag)
    {
        var packageName = Config.ResolveVariables(
            Config.DaosPath!,
            tag,
            module: classe.Namespace.Module).ToPackageName();
        var javaClass = new JavaClass(GetClassName(classe))
        {
            Package = packageName,
            Interface = true,
            Visibility = "public"
        };
        javaClass.Imports.Add(classe.GetImport(Config, tag));

        if (Config.CanClassUseEnums(classe))
        {
            javaClass.Imports.Add($"{Config.GetEnumPackageName(classe, tag)}.{Config.GetType(classe.PrimaryKey.SingleOrDefault() ?? classe.Extends!.PrimaryKey.Single())}");
        }

        string pk;
        if (!classe.PrimaryKey.Any() && classe.Extends != null)
        {
            pk = Config.GetType(classe.ExtendedProperties.Single(p => p.PrimaryKey));
            javaClass.Imports.AddRange(classe.ExtendedProperties.Single(p => p.PrimaryKey).GetTypeImports(Config, tag));
        }
        else
        {
            if (classe.PrimaryKey.Count() > 1)
            {
                pk = $"{classe.NamePascal}.{classe.NamePascal}Id";
            }
            else
            {
                pk = Config.GetType(classe.PrimaryKey.Single());
                javaClass.Imports.AddRange(classe.PrimaryKey.Single().GetTypeImports(Config, tag));
            }
        }

        string daosInterface = $"JpaRepository";
        string daosInterfaceImport = "org.springframework.data.jpa.repository.JpaRepository";
        if (Config.DaosInterface != null)
        {
            int lastIndexOf = Config.DaosInterface.LastIndexOf('.');
            string daosInterfaceName = lastIndexOf > -1 ? Config.DaosInterface[(lastIndexOf + 1)..] : Config.DaosInterface;
            daosInterface = daosInterfaceName;
            daosInterfaceImport = Config.DaosInterface;
        }
        else if (classe.Reference || Config.UseJdbc)
        {
            daosInterface = "CrudRepository";
            daosInterfaceImport = "org.springframework.data.repository.CrudRepository";
        }

        javaClass.Extends = $"{daosInterface}<{classe.NamePascal}, {pk}>";
        javaClass.Imports.Add(daosInterfaceImport);

        if (Config.DaosAbstract)
        {
            javaClass.Add(new JavaAnnotation("NoRepositoryBean", imports: "org.springframework.data.repository.NoRepositoryBean"));
        }

        return javaClass;
    }
}