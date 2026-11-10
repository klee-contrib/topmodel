using Meziantou.Framework.Globbing;

namespace TopModel.Utils;

/// <summary>
/// Configuration commune aux générateurs.
/// </summary>
public abstract class ConfigBase
{
    /// <summary>
    /// Répertoire contenant le fichier de config.
    /// </summary>
    public string ConfigRoot { get; set; } = null!;

    /// <summary>
    /// Chemin du dossier racine qui contient tous les fichiers tmd du modèle, qui seront chargés par le générateur (à l'exception du fichier de domaines qui est chargé à part). Si non renseigné, le dossier contenant le fichier de config sera utilisé.
    /// </summary>
    public string ModelRoot { get; set; } = null!;

    /// <summary>
    /// Nom du fichier de lock.
    /// </summary>
    public string LockFileName { get; set; } = null!;

    /// <summary>
    /// Si renseigné, seuls les fichiers de modèle matchant les patterns (globs) listés (relativement au <c>modelRoot</c>) seront chargés par TopModel.
    /// </summary>
    public GlobCollection ModelFilePaths =>
        new(ModelFilePathsGlobs.Select(g => Glob.Parse(g, GlobDialect.Standard, GlobOptions.IgnoreCase)).ToArray());

    /// <summary>
    /// Si renseigné, seuls les fichiers de modèle matchant les patterns (globs) listés (relativement au <c>modelRoot</c>) seront chargés par TopModel.
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

    public abstract ConfigBase Init(string rootDir);
}
