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

    public override bool CanClassUseEnums(Class classe, IEnumerable<Class>? availableClasses = null, IFieldProperty? prop = null)
    {
        return false;
    }

    protected override string GetEnumType(string className, string propName, bool asList = false, bool isPrimaryKeyDef = false)
    {
        throw new NotImplementedException();
    }

    protected override string GetListType(string name, bool useIterable = true)
    {
        return "Collection";
    }
}