namespace TopModel.Generator.Javascript;

/// <summary>
/// Paramètres pour la génération du Javascript.
/// </summary>
public class JavascriptConfig : GeneratorConfigBase
{
    /// <summary>
    /// Dossier de sortie pour le modèle.
    /// </summary>
    public string? ModelOutputDirectory { get; set; }

    /// <summary>
    /// Dossier de sortie pour les ressources.
    /// </summary>
    public string? ResourceOutputDirectory { get; set; }

    /// <summary>
    /// Dossier de sortie pour le client d'API.
    /// </summary>
    public string? ApiClientOutputDirectory { get; set; }

    /// <summary>
    /// Chemin vers lequel sont créés les fichiers générés du client d'API
    /// </summary>
    public string ApiClientFilePath { get; set; } = "{module}";

    /// <summary>
    /// Chemin vers un "fetch" personnalisé.
    /// </summary>
    public string? FetchImportPath { get; set; }

    /// <summary>
    /// Chemin vers le fichier "domain".
    /// </summary>
    public string DomainImportPath { get; set; } = "../domains";

    /// <summary>
    /// Génère des modèles pour Focus (par défaut : true).
    /// </summary>
    public TargetFramework TargetFramework { get; set; } = TargetFramework.FOCUS;

    /// <summary>
    /// Mode de génération (JS ou JSON Schema).
    /// </summary>
    public ResourceMode ResourceMode { get; set; }

    /// <summary>
    /// Si la liste des valeurs de la liste de référence doit être générée
    /// </summary>
    public ReferenceMode ReferenceMode { get; set; } = ReferenceMode.DEFINITION;
}
