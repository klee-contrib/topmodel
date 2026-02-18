using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
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
    public virtual EntityMode EntityMode { get; set; } = EntityMode.TYPED;

    /// <summary>
    /// Génère `isRequired`, `label` (et `comment`) sur les compositions dans les entitées typées.
    /// </summary>
    public virtual bool ExtendedCompositions { get; set; }

    /// <summary>
    /// Chemin (ou alias commençant par '@') vers les imports de types d'entités, relatif au répertoire de génération.
    /// </summary>
    public virtual string? EntityTypesPath { get; set; }

    /// <summary>
    /// Mode de génération (JS, JSON ou JSON Schema).
    /// </summary>
    public virtual ResourceMode ResourceMode { get; set; }

    /// <summary>
    /// Mode de génération des listes de références (définitions ou valeurs).
    /// </summary>
    public virtual ReferenceMode ReferenceMode { get; set; } = ReferenceMode.DEFINITION;

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

    public virtual string GetClassFileName(Class classe, string tag)
    {
        return Path.Combine(
                OutputDirectory,
                ResolveVariables(ModelRootPath!, tag, classe.Namespace.ModulePathKebab),
                $"{classe.Name.ToKebabCase()}.ts"
            )
            .Replace('\\', '/');
    }

    public virtual string GetCommentResourcesFilePath(Namespace ns, string tag, string lang)
    {
        return Path.Combine(
                OutputDirectory,
                ResolveVariables(ResourceRootPath!, tag, GetRootModule(ns).ToKebabCase(), lang),
                $"{GetRootModule(ns).ToKebabCase()}.comments{(ResourceMode == ResourceMode.JS ? ".ts" : ".json")}"
            )
            .Replace('\\', '/');
    }

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

    public virtual IList<(string Import, string Path)> GetEndpointImports(
        string fileName,
        IEnumerable<Endpoint> endpoints,
        string tag
    )
    {
        return endpoints
            .SelectMany(e => e.ClassDependencies)
            .Select(dep =>
                (
                    Import: dep is { Source: IProperty fp and not IProperty { Composition: not null } }
                        ? GetEnumType(fp)
                        : dep.Classe.NamePascal,
                    Path: GetImportPathForClass(
                        dep,
                        dep.Classe.Tags.Contains(tag) ? tag : dep.Classe.Tags.Intersect(Tags).FirstOrDefault() ?? tag,
                        tag
                    )!
                )
            )
            .Concat(endpoints.SelectMany(d => d.Properties).SelectMany(dep => GetDomainImportPaths(fileName, dep, tag)))
            .Concat(
                endpoints
                    .SelectMany(d => d.Params)
                    .Where(p => p.IsQueryParam())
                    .SelectMany(dep => GetValueImportPaths(fileName, dep))
            )
            .Where(i => i.Path != null)
            .GroupAndSort();
    }

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

    public virtual string? GetImportPathForClass(ClassDependency dep, string targetTag, string sourceTag)
    {
        string target;
        if (dep is { Source: IProperty and not { Composition: not null } })
        {
            if (dep.Classe.IsJSReference() && AvailableClasses.Contains(dep.Classe))
            {
                target = GetReferencesFileName(dep.Classe.Namespace, targetTag);
            }
            else
            {
                return null;
            }
        }
        else
        {
            target = dep.Classe.IsJSReference()
                ? GetReferencesFileName(dep.Classe.Namespace, targetTag)
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

    public virtual string GetMainResourceFilePath(string tag, string lang)
    {
        return Path.Combine(OutputDirectory, ResolveVariables(ResourceRootPath!, tag, lang: lang), "index.ts")
            .Replace('\\', '/');
    }

    public virtual string GetReferencesFileName(Namespace ns, string tag)
    {
        return Path.Combine(OutputDirectory, ResolveVariables(ModelRootPath!, tag, ns.ModulePathKebab), "references.ts")
            .Replace('\\', '/');
    }

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

    public virtual string GetResourcesFilePath(Namespace ns, string tag, string lang)
    {
        return Path.Combine(
                OutputDirectory,
                ResolveVariables(ResourceRootPath!, tag, GetRootModule(ns).ToKebabCase(), lang),
                $"{GetRootModule(ns).ToKebabCase()}{(ResourceMode == ResourceMode.JS ? ".ts" : ".json")}"
            )
            .Replace('\\', '/');
    }

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

    public virtual bool IsListComposition(IProperty property)
    {
        var cp = property.Composition != null ? property : null;

        return cp != null && cp.Domain != null && (GetImplementation(cp.Domain)?.GenericType?.EndsWith("[]") ?? false);
    }

    protected override string ResolveTagVariables(string value, string tag)
    {
        return base.ResolveTagVariables(value, tag).Trim('/');
    }
}
