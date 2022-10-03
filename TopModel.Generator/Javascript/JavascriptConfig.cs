namespace TopModel.Generator.Javascript;

/// <summary>
/// Paramètres pour la génération du Javascript.
/// </summary>
public class JavascriptConfig : GeneratorConfigBase
{
    /// <summary>
    /// Localisation du modèle, relative au répertoire de génération. Si non renseigné, aucun fichier ne sera généré.
    /// </summary>
    public string? ModelRootPath { get; set; }

    /// <summary>
    /// Localisation des ressources i18n, relative au répertoire de génération. Si non renseigné, aucun fichier ne sera généré.
    /// </summary>
    public string? ResourceRootPath { get; set; }

    /// <summary>
    /// Localisation des clients d'API, relative au répertoire de génération. Si non renseigné, aucun fichier ne sera généré.
    /// </summary>
    public string? ApiClientRootPath { get; set; }

    /// <summary>
    /// Chemin vers lequel sont créés les fichiers d'endpoints générés, relatif à la racine de l'API.
    /// </summary>
    public string ApiClientFilePath { get; set; } = "{module}";

    /// <summary>
    /// Chemin (ou alias commençant par '@') vers un 'fetch' personnalisé, relatif à la racine de l'API.
    /// </summary>
    public string? FetchImportPath { get; set; }

    /// <summary>
    /// Chemin (ou alias commençant par '@') vers le fichier 'domain', relatif à la racine du modèle.
    /// </summary>
    public string DomainImportPath { get; set; } = "../domains";

    /// <summary>
    /// Framework cible pour la génération.
    /// </summary>
    public TargetFramework TargetFramework { get; set; } = TargetFramework.FOCUS;

    /// <summary>
    /// Mode de génération (JS, JSON ou JSON Schema).
    /// </summary>
    public ResourceMode ResourceMode { get; set; }

    /// <summary>
    /// Mode de génération des listes de références (définitions ou valeurs).
    /// </summary>
    public ReferenceMode ReferenceMode { get; set; } = ReferenceMode.DEFINITION;
}
