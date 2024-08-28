using TopModel.Core;
using TopModel.Generator.Core;

namespace TopModel.Generator.Php;

public class PhpConfig : GeneratorConfigBase
{
    /// <summary>
    /// Localisation des classes persistées du modèle, relative au répertoire de génération. Par défaut, 'phpgen/{app}/entities/{module}'.
    /// </summary>
    public string EntitiesPath { get; set; } = "Entity/{module}";

    /// <summary>
    /// Localisation des Repositories, relative au répertoire de génération.
    /// </summary>
    public string RepositoriesPath { get; set; } = "Repository/{module}";

    /// <summary>
    /// Localisation des classses non persistées du modèle, relative au répertoire de génération. Par défaut, 'phpgen/{app}/dtos/{module}'.
    /// </summary>
    public string DtosPath { get; set; } = "Model/{module}";

    /// <summary>
    /// Mode de génération des séquences.
    /// </summary>
    public IdentityConfig Identity { get; set; } = new() { Mode = IdentityMode.IDENTITY };

    public override string[] PropertiesWithTagVariableSupport => new[]
    {
        nameof(EntitiesPath),
        nameof(RepositoriesPath),
        nameof(DtosPath),
    };

    public override string[] PropertiesWithModuleVariableSupport => new[]
    {
        nameof(EntitiesPath),
        nameof(RepositoriesPath),
        nameof(DtosPath),
    };

    public override bool CanClassUseEnums(Class classe, IEnumerable<Class>? availableClasses = null, IProperty? prop = null)
    {
        return false;
    }

    public string GetClassFileName(Class classe, string tag)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(classe.IsPersistent ? EntitiesPath : DtosPath, tag, module: classe.Namespace.Module).ToFilePath(),
            $"{classe.NamePascal}.php");
    }

    public string GetPackageName(Class classe, string tag, bool? isPersistent = null)
    {
        return GetPackageName(
            classe.Namespace,
            isPersistent.HasValue ? isPersistent.Value ? EntitiesPath : DtosPath : classe.IsPersistent ? EntitiesPath : DtosPath,
            tag);
    }

    public string GetPackageName(Namespace ns, string modelPath, string tag)
    {
        return ResolveVariables(modelPath, tag, module: ns.Module).ToPackageName();
    }

    protected override string GetEnumType(string className, string propName, bool isPrimaryKeyDef = false)
    {
        throw new NotImplementedException();
    }
}