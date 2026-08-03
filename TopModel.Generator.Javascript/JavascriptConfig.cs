using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Javascript;

/// <summary>
/// Paramètres pour la génération du Javascript.
/// </summary>
public class JavascriptConfig : GeneratorConfigBase
{
    /// <summary>
    /// Localisation du modèle, relative au répertoire de génération. Si non renseigné, aucun modèle ne sera généré. Si '{module}' n'est pas présent dans le chemin, alors il sera ajouté à la fin.
    /// </summary>
    public virtual string? ModelRootPath { get; set; }

    /// <summary>
    /// Nom du fichier généré contenant les enums d'un module. Par défaut : `enums`.
    /// </summary>
    public virtual string EnumsFileName { get; set; } = "enums";

    /// <summary>
    /// Localisation des ressources i18n, relative au répertoire de génération. Si non renseigné, aucun fichier ne sera généré. Si '{lang}' n'est pas présent dans le chemin, alors il sera ajouté à la fin.
    /// </summary>
    public virtual string? ResourceRootPath { get; set; }

    /// <summary>
    /// Localisation des clients d'API, relative au répertoire de génération. Si non renseigné, aucun fichier ne sera généré.
    /// </summary>
    public virtual string? ApiClientRootPath { get; set; }

    /// <summary>
    /// Chemin vers lequel sont créés les fichiers d'endpoints générés, relatif à la racine de l'API.
    /// </summary>
    public virtual string? ApiClientFilePath { get; set; }

    /// <summary>
    /// Chemin (ou alias commençant par '@') vers un 'fetch' personnalisé, relatif au répertoire de génération.
    /// </summary>
    public virtual string? FetchPath { get; set; }

    /// <summary>
    /// Chemin (ou alias commençant par '@') vers le fichier 'domain', relatif au répertoire de génération.
    /// </summary>
    public virtual string DomainPath { get; set; } = "./domains";

    /// <summary>
    /// Framework cible pour la génération.
    /// </summary>
    public virtual TargetFramework ApiMode { get; set; }

    /// <summary>
    /// Typage des entités générées
    /// </summary>
    public virtual EntityMode EntityMode { get; set; }

    /// <summary>
    /// Génère `isRequired`, `label` (et `comment`) sur les compositions dans les entitées typées.
    /// </summary>
    public virtual bool ExtendedCompositions { get; set; }

    /// <summary>
    /// Génère les (alias de) clés primaires simples comme optionnelles dans les définitions d'entités. Il s'agit d'un paramètre de compatibilité avec l'existant, qui ne devrait plus être utilisé dans les nouveaux projets.
    /// </summary>
    public virtual bool OptionalPrimaryKeys { get; set; }

    /// <summary>
    /// Chemin (ou alias commençant par '@') vers les imports de types d'entités, relatif au répertoire de génération.
    /// </summary>
    public virtual string? EntityTypesPath { get; set; }

    /// <summary>
    /// Mode de génération (JS, JSON ou JSON Schema).
    /// </summary>
    public virtual ResourceMode ResourceMode { get; set; }

    /// <summary>
    /// Ajoute les commentaires dans les entités JS générées.
    /// </summary>
    public virtual bool GenerateComments { get; set; }

    /// <summary>
    /// Génère un fichier 'index.ts' qui importe et réexporte tous les fichiers de resources générés par langue. Uniquement compatible avec `resourceMode: js`.
    /// </summary>
    public virtual bool GenerateMainResourceFiles { get; set; }

    public override string? DefaultLanguage => "ts";

    public override string[] PropertiesWithModuleVariableSupport =>
        [nameof(ModelRootPath), nameof(ApiClientFilePath), nameof(ResourceRootPath)];

    public override string[] PropertiesWithFileNameVariableSupport => [nameof(ApiClientFilePath)];

    public override string[] PropertiesWithTagVariableSupport =>
        [
            nameof(ModelRootPath),
            nameof(ResourceRootPath),
            nameof(ApiClientRootPath),
            nameof(FetchPath),
            nameof(DomainPath),
        ];

    public override string[] PropertiesWithLangVariableSupport => [nameof(ResourceRootPath)];

    protected override bool UseValueNameForValues => false;

    protected override string NullValue => "undefined";

    /// <summary>
    /// Retourne le nom du fichier généré pour une classe.
    /// </summary>
    /// <param name="classe">Classe.</param>
    /// <param name="tag">Tag.</param>
    /// <returns>Nom du fichier généré.</returns>
    public virtual string GetClassFileName(Class classe, string tag)
    {
        return Path.Combine(
                OutputDirectory,
                ResolveVariables(ModelRootPath!, tag, classe.Namespace.ModulePathKebab),
                $"{classe.Name.ToKebabCase()}.ts"
            )
            .Replace('\\', '/');
    }

    /// <summary>
    /// Retourne le chemin du fichier de ressources pour les commentaires.
    /// </summary>
    /// <param name="ns">Namespace.</param>
    /// <param name="tag">Tag.</param>
    /// <param name="lang">Langue.</param>
    /// <returns>Chemin du fichier.</returns>
    public virtual string GetCommentResourcesFilePath(Namespace ns, string tag, string lang)
    {
        return Path.Combine(
                OutputDirectory,
                ResolveVariables(ResourceRootPath!, tag, GetRootModule(ns).ToKebabCase(), lang),
                $"{GetRootModule(ns).ToKebabCase()}.comments{(ResourceMode == ResourceMode.JS ? ".ts" : ".json")}"
            )
            .Replace('\\', '/');
    }

    /// <summary>
    /// Retourne les imports de domaines requis pour une propriété.
    /// </summary>
    /// <param name="fileName">Nom du fichier source contenant la propriété.</param>
    /// <param name="prop">Propriété.</param>
    /// <param name="tag">Tag.</param>
    /// <returns>Les imports.</returns>
    public virtual IEnumerable<(string Import, string Path)> GetDomainImportPaths(
        string fileName,
        IProperty prop,
        string tag
    )
    {
        return GetDomainImports(prop, tag)
            .Select(import =>
                (Import: import.Split("/")[^1], Path: GetRelativePath(import[..import.LastIndexOf('/')], fileName))
            );
    }

    /// <summary>
    /// Retourne les imports requis par un fichier d'endpoints.
    /// </summary>
    /// <param name="fileName">Nom du fichier.</param>
    /// <param name="endpoints">Endpoints.</param>
    /// <param name="tag">Tag.</param>
    /// <returns>Les imports.</returns>
    public virtual IList<(string Import, string Path)> GetEndpointImports(
        string fileName,
        IEnumerable<Endpoint> endpoints,
        string tag
    )
    {
        return endpoints
            .SelectMany(GetClassDependencies)
            .Select(dep =>
                (
                    Import: dep is { Source: IProperty p and not IProperty { Composition: not null } }
                        ? GetEnumType(p)
                        : dep.Classe.NamePascal,
                    Path: GetImportPathForClass(
                        dep,
                        dep.Classe.Tags.Contains(tag) ? tag : dep.Classe.Tags.Intersect(Tags).FirstOrDefault() ?? tag,
                        tag
                    )!
                )
            )
            .Concat(endpoints.SelectMany(e => e.Properties).SelectMany(dep => GetDomainImportPaths(fileName, dep, tag)))
            .Concat(
                endpoints
                    .SelectMany(GetParams)
                    .Where(p => p.ParamLocation == ParamLocation.Query)
                    .SelectMany(dep => GetValueImportPaths(fileName, dep))
            )
            .Where(import => import.Path != null)
            .GroupAndSort();
    }

    /// <summary>
    /// Retourne le nom du fichier d'endpoints correspondant à un fichier de modèle.
    /// </summary>
    /// <param name="file">Fichier de modèle.</param>
    /// <param name="tag">Tag.</param>
    /// <returns>Nom du fichier d'endpoints correspondant.</returns>
    public virtual string GetEndpointsFileName(ModelFile file, string tag)
    {
        return Path.Combine(
                OutputDirectory,
                ResolveVariables(ApiClientRootPath!, tag),
                ResolveVariables(ApiClientFilePath!, module: file.Namespace.ModulePathKebab)
                    .Replace("{fileName}", file.Options.Endpoints.FileName.ToKebabCase()) + ".ts"
            )
            .Replace('\\', '/');
    }

    /// <summary>
    /// Retourne le nom du fichier d'enums correspondant à un namespace.
    /// </summary>
    /// <param name="ns">Namespace.</param>
    /// <param name="tag">Tag.</param>
    /// <returns>Nom du fichier d'enums correspondant.</returns>
    public virtual string GetEnumsFileName(Namespace ns, string tag)
    {
        return Path.Combine(
                OutputDirectory,
                ResolveVariables(ModelRootPath!, tag, ns.ModulePathKebab),
                $"{EnumsFileName}.ts"
            )
            .Replace('\\', '/');
    }

    /// <summary>
    /// Retourne le chemin d'import correspondant à la dépendance de classe donnée.
    /// </summary>
    /// <param name="dep">Dépendance de classe.</param>
    /// <param name="targetTag">Tag cible.</param>
    /// <param name="sourceTag">Tag source.</param>
    /// <returns>Chemin d'import, ou <see langword="null" /> si aucun import n'est nécessaire.</returns>
    public virtual string? GetImportPathForClass(ClassDependency dep, string targetTag, string sourceTag)
    {
        string target;
        if (dep is { Source: IProperty and not { Composition: not null } })
        {
            if (dep.Classe.Enum != null && AvailableClasses.Contains(dep.Classe))
            {
                target = GetEnumsFileName(dep.Classe.Namespace, targetTag);
            }
            else
            {
                return null;
            }
        }
        else
        {
            target =
                dep.Classe.Enum != null
                    ? GetEnumsFileName(dep.Classe.Namespace, targetTag)
                    : GetClassFileName(dep.Classe, targetTag);
        }

        var source = dep.Source switch
        {
            IProperty { Class: Class classe } => GetClassFileName(classe, sourceTag),
            IProperty { Endpoint: Endpoint endpoint } => GetEndpointsFileName(endpoint.ModelFile, sourceTag),
            Class classe => GetClassFileName(classe, sourceTag),
            _ => null,
        };

        if (source == null)
        {
            return null;
        }

        var path = Path.GetRelativePath(string.Join('/', source.Split('/').SkipLast(1)), target)[..^3]
            .Replace('\\', '/');

        if (!path.StartsWith('.'))
        {
            path = $"./{path}";
        }

        return path;
    }

    /// <summary>
    /// Retourne le chemin du fichier principal de ressources.
    /// </summary>
    /// <param name="tag">Tag.</param>
    /// <param name="lang">Langue.</param>
    /// <returns>Chemin du fichier.</returns>
    public virtual string GetMainResourceFilePath(string tag, string lang)
    {
        return Path.Combine(OutputDirectory, ResolveVariables(ResourceRootPath!, tag, lang: lang), "index.ts")
            .Replace('\\', '/');
    }

    /// <summary>
    /// Calcule le chemin relatif d'un chemin depuis un fichier source.
    /// </summary>
    /// <param name="path">Chemin à rendre relatif.</param>
    /// <param name="fileName">Nom du fichier source.</param>
    /// <returns>Chemin relatif.</returns>
    public virtual string GetRelativePath(string path, string fileName)
    {
        return !path.StartsWith('.')
            ? path
            : Path.GetRelativePath(
                    string.Join('/', fileName.Split('/').SkipLast(1)),
                    Path.Combine(OutputDirectory, path)
                )
                .Replace('\\', '/');
    }

    /// <summary>
    /// Retourne le chemin du fichier de ressources.
    /// </summary>
    /// <param name="ns">Namespace.</param>
    /// <param name="tag">Tag.</param>
    /// <param name="lang">Langue.</param>
    /// <returns>Chemin du fichier.</returns>
    public virtual string GetResourcesFilePath(Namespace ns, string tag, string lang)
    {
        return Path.Combine(
                OutputDirectory,
                ResolveVariables(ResourceRootPath!, tag, GetRootModule(ns).ToKebabCase(), lang),
                $"{GetRootModule(ns).ToKebabCase()}{(ResourceMode == ResourceMode.JS ? ".ts" : ".json")}"
            )
            .Replace('\\', '/');
    }

    /// <summary>
    /// Retourne les imports requis par une valeur de propriété.
    /// </summary>
    /// <param name="fileName">Nom du fichier source.</param>
    /// <param name="prop">Propriété.</param>
    /// <param name="value">Valeur.</param>
    /// <returns>Imports des valeurs.</returns>
    public virtual IEnumerable<(string Import, string Path)> GetValueImportPaths(
        string fileName,
        IProperty prop,
        string? value = null
    )
    {
        return GetValueImports(prop, value)
            .Select(import =>
                (Import: import.Split("/")[^1], Path: GetRelativePath(import[..import.LastIndexOf('/')], fileName))
            );
    }

    /// <summary>
    /// Indique si une propriété est une composition typée comme une liste.
    /// </summary>
    /// <param name="property">Propriété.</param>
    /// <returns><see langword="true" /> si la propriété est une composition de liste.</returns>
    public virtual bool IsListComposition(IProperty property)
    {
        return property is { Composition: Class c, Domain: Domain d }
            && c.Enum == null
            && (GetImplementation(d)?.GenericType?.EndsWith("[]") ?? false);
    }

    protected override string ResolveTagVariables(string value, string tag)
    {
        return base.ResolveTagVariables(value, tag).Trim('/');
    }
}
