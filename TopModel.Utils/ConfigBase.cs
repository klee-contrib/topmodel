using Meziantou.Framework.Globbing;

namespace TopModel.Utils;

#nullable disable

/// <summary>
/// Configuration commune aux générateurs.
/// </summary>
public class ConfigBase
{
    /// <summary>
    /// Répertoire contenant le fichier de config.
    /// </summary>
    public string ConfigRoot { get; set; }

    /// <summary>
    /// Racine du modèle.
    /// </summary>
    public string ModelRoot { get; set; }

    /// <summary>
    /// Nom du lockfile.
    /// </summary>
    public string LockFileName { get; set; }

    /// <summary>
    /// Si renseigné, seuls les fichiers de modèle matchant les patterns (globs) listés (relativement au `ModelRoot`) seront chargés par TopModel.
    /// </summary>
    public GlobCollection ModelFilePaths =>
        new(ModelFilePathsGlobs.Select(g => Glob.Parse(g, GlobOptions.IgnoreCase)).ToArray());

    /// <summary>
    /// Si renseigné, seuls les fichiers de modèle matchant les patterns (globs) listés (relativement au `ModelRoot`) seront chargés par TopModel.
    /// </summary>
    public IList<string> ModelFilePathsGlobs { get; set; } = ["**/*.tmd"];

    /// <summary>
    /// Liste des fichiers à ignorer après première génération (relatif au fichier de config).
    /// </summary>
    public IList<IgnoredFile> IgnoredFiles { get; set; } = [];

    /// <summary>
    /// Liste des warnings à ignorer.
    /// </summary>
    public IList<ErrorType> NoWarn { get; set; } = [];
}
