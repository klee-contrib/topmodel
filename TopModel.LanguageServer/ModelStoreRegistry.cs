using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using TopModel.Core;

namespace TopModel.LanguageServer;

/// <summary>
/// Registre des ModelStore, un par fichier de configuration topmodel trouvé dans le workspace.
/// Fournit le routing document → store(s) utilisé par tous les handlers LSP.
/// </summary>
public class ModelStoreRegistry
{
    private readonly List<ModelStoreEntry> _entries = [];

    public bool IsEmpty => _entries.Count == 0;

    public IReadOnlyList<ModelStoreEntry> All => _entries;

    /// <summary>
    /// Retourne toutes les entrées dont le ModelRoot couvre un fichier donné.
    /// Utilisé pour les diagnostics et les références, où un fichier peut appartenir
    /// à plusieurs configs (racines imbriquées ou identiques).
    /// </summary>
    public IEnumerable<ModelStoreEntry> GetAllForFile(string filePath) =>
        _entries.Where(e =>
            !Path.GetRelativePath(Path.GetFullPath(e.Config.ModelRoot), Path.GetFullPath(filePath)).StartsWith("..")
        );

    /// <summary>
    /// Retourne un DocumentSelector couvrant les globs de tous les configs enregistrés.
    /// Utilisé dans CreateRegistrationOptions des handlers.
    /// </summary>
    public TextDocumentSelector GetCombinedDocumentSelector()
    {
        if (_entries.Count == 0)
        {
            return TextDocumentSelector.ForPattern("**/*.tmd");
        }

        var patterns = _entries
            .SelectMany(e => e.Config.ModelFilePathsGlobs.Select(g => $"{e.Config.ModelRoot}/{g}"))
            .Distinct()
            .ToArray();

        return TextDocumentSelector.ForPattern(patterns);
    }

    /// <summary>
    /// Retourne l'entrée la plus spécifique pour un fichier donné (prefixe ModelRoot le plus long).
    /// Utilisé pour les opérations document-scoped : hover, definition, completion, semantic tokens…
    /// </summary>
    public ModelStoreEntry? GetPrimaryForFile(string filePath) =>
        _entries
            .Where(e =>
                !Path.GetRelativePath(Path.GetFullPath(e.Config.ModelRoot), Path.GetFullPath(filePath)).StartsWith("..")
            )
            .MaxBy(e => Path.GetFullPath(e.Config.ModelRoot).Length);

    public void Register(ModelStoreEntry entry) => _entries.Add(entry);

    /// <summary>
    /// Attend que tous les stores aient terminé leurs mises à jour en cours.
    /// </summary>
    public Task WaitForAllUpdatesAsync(CancellationToken ct) =>
        Task.WhenAll(_entries.Select(e => e.Store.WaitForUpdates(ct)));
}
