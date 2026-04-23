using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Documentation;

/// <summary>
/// Paramètres pour la génération du Documentation.
/// </summary>
public class DocumentationConfig : GeneratorConfigBase
{
    /// <summary>
    /// Chemin de génération du fichier listant les endpoints, relatif au répertoire de génération.
    /// La finesse de génération est déduite de la présence de variables dans le chemin :
    /// un fichier par fichier de modèle si <c>{fileName}</c> est présent, un fichier par module si <c>{module}</c> est présent, un unique fichier global sinon.
    /// Par défaut : <c>endpoints.md</c>.
    /// </summary>
    public virtual string EndpointsFilePath { get; set; } = "endpoints.md";

    /// <summary>
    /// Chemin de génération du fichier de dictionnaire des classes, relatif au répertoire de génération.
    /// La finesse de génération est déduite de la présence de variables dans le chemin :
    /// un fichier par fichier de modèle si <c>{fileName}</c> est présent, un fichier par module si <c>{module}</c> est présent, un unique fichier global sinon.
    /// Par défaut : <c>classes.md</c>.
    /// </summary>
    public virtual string ClassesFilePath { get; set; } = "classes.md";

    /// <summary>
    /// Chemin de génération des fichiers de diagrammes Mermaid, relatif au répertoire de génération.
    /// La finesse de génération est déduite de la présence de variables dans le chemin :
    /// un diagramme par fichier de modèle si <c>{fileName}</c> est présent, un diagramme par module si <c>{module}</c> est présent, un unique diagramme global sinon.
    /// Par défaut : <c>{module}.md</c>.
    /// </summary>
    public virtual string MermaidFilePath { get; set; } = "{module}.md";

    public virtual IDictionary<string, string[]> Schemas { get; set; } =
        new Dictionary<string, string[]>(StringComparer.Ordinal);

    /// <summary>
    /// Liste des noms d'annotations (ex. <c>RoleRequired</c>, <c>HasRole</c>) dont les paramètres doivent être agrégés
    /// pour alimenter la colonne « Autorisation » dans la liste des endpoints.
    /// Si la liste est vide ou non renseignée, la colonne « Autorisation » n'est pas générée.
    /// </summary>
    public virtual IList<string> AuthorizationAnnotations { get; set; } = [];

    public override string[] PropertiesWithModuleVariableSupport =>
        [nameof(EndpointsFilePath), nameof(ClassesFilePath), nameof(MermaidFilePath)];

    public override string[] PropertiesWithFileNameVariableSupport =>
        [nameof(EndpointsFilePath), nameof(ClassesFilePath), nameof(MermaidFilePath)];

    public override string[] PropertiesWithTagVariableSupport =>
        [nameof(EndpointsFilePath), nameof(ClassesFilePath), nameof(MermaidFilePath)];

    /// <summary>
    /// Mode de découpage du dictionnaire de classes, déduit de <see cref="ClassesFilePath"/>.
    /// </summary>
    public virtual DocumentationGenerationMode ClassesMode => GetGenerationMode(ClassesFilePath);

    /// <summary>
    /// Mode de découpage de la liste des endpoints, déduit de <see cref="EndpointsFilePath"/>.
    /// </summary>
    public virtual DocumentationGenerationMode EndpointsMode => GetGenerationMode(EndpointsFilePath);

    /// <summary>
    /// Mode de découpage des diagrammes Mermaid, déduit de <see cref="MermaidFilePath"/>.
    /// </summary>
    public virtual DocumentationGenerationMode MermaidMode => GetGenerationMode(MermaidFilePath);

    /// <summary>
    /// Déduit le mode de découpage à partir d'un chemin de fichier configuré.
    /// </summary>
    /// <param name="filePath">Chemin configuré.</param>
    public static DocumentationGenerationMode GetGenerationMode(string filePath)
    {
        return filePath.Contains("{fileName}") ? DocumentationGenerationMode.File
            : filePath.Contains("{module}") ? DocumentationGenerationMode.Module
            : DocumentationGenerationMode.All;
    }

    /// <summary>
    /// Récupère le chemin de génération du fichier de dictionnaire des classes.
    /// </summary>
    /// <param name="tag">Tag courant.</param>
    /// <param name="classe">Classe cible (utilisée pour résoudre <c>{module}</c> et <c>{fileName}</c>).</param>
    public virtual string GetClassesFilePath(string tag, Class? classe = null)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveDocFilePath(
                ClassesFilePath,
                tag,
                classe?.Namespace.ModulePathKebab,
                GetModelFileName(classe?.ModelFile)
            )
        );
    }

    /// <summary>
    /// Récupère le chemin de génération du fichier listant les endpoints.
    /// </summary>
    /// <param name="tag">Tag courant.</param>
    /// <param name="file">Fichier de modèle (utilisé pour résoudre <c>{module}</c> et <c>{fileName}</c>).</param>
    public virtual string GetEndpointsFilePath(string tag, ModelFile? file = null)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveDocFilePath(EndpointsFilePath, tag, file?.Namespace.ModulePathKebab, GetModelFileName(file))
        );
    }

    /// <summary>
    /// Récupère le chemin de génération d'un diagramme Mermaid.
    /// </summary>
    /// <param name="tag">Tag courant.</param>
    /// <param name="classe">Classe cible (utilisée pour résoudre <c>{module}</c> et <c>{fileName}</c>).</param>
    public virtual string GetMermaidFilePath(string tag, Class? classe = null)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveDocFilePath(
                MermaidFilePath,
                tag,
                classe?.Namespace.ModulePathKebab,
                GetModelFileName(classe?.ModelFile)
            )
        );
    }

    /// <summary>
    /// Résout un chemin de fichier de documentation en appliquant les variables de tag, module et fichier.
    /// </summary>
    protected virtual string ResolveDocFilePath(string pattern, string tag, string? module, string? fileName)
    {
        return ResolveVariables(pattern, tag: tag, module: module ?? string.Empty)
            .Replace("{fileName}", fileName ?? string.Empty)
            .Replace('\\', '/');
    }

    private static string? GetModelFileName(ModelFile? file)
    {
        return file?.Name.Split('/')[^1].ToKebabCase();
    }
}
