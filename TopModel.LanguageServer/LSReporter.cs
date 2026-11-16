using System.Collections.Concurrent;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.LanguageServer;

/// <summary>
/// Reporter du language server : transmet les erreurs du modèle au client par push (<c>textDocument/publishDiagnostics</c>),
/// et les conserve en cache pour répondre aux requêtes pull par document (<c>textDocument/diagnostic</c>).
/// </summary>
public class LSReporter(ILanguageServerFacade facade) : IModelReporter
{
    /// <summary>
    /// Etat courant des diagnostics par fichier (chemin système), complet et mis à jour à chaque <see cref="Report" />.
    /// </summary>
    private readonly ConcurrentDictionary<string, FileDiagnostics> _diagnosticsCache = new(
        OmnisharpExtensions.PathComparer
    );

    private readonly ConcurrentBag<KeyValuePair<ModelFile, IEnumerable<ModelError>>> _pendingErrors = [];

    private bool _hasChange;

    private long _lastResultId;

    /// <summary>
    /// Diagnostics d'un fichier, ou <c>null</c> si aucun rapport n'a encore été fait pour ce fichier.
    /// </summary>
    /// <param name="filePath">Chemin système du fichier.</param>
    public FileDiagnostics? GetDiagnostics(string filePath)
    {
        return _diagnosticsCache.TryGetValue(filePath, out var value) ? value : null;
    }

    /// <inheritdoc cref="IModelReporter.RegisterChange" />
    public void RegisterChange()
    {
        _hasChange = true;
    }

    /// <inheritdoc cref="IModelReporter.RegisterErrors" />
    public void RegisterErrors(IDictionary<ModelFile, IEnumerable<ModelError>> errors)
    {
        foreach (var error in errors)
        {
            _pendingErrors.Add(error);
        }
    }

    /// <inheritdoc cref="IModelReporter.Report" />
    public void Report(bool refresh = false)
    {
        if (_hasChange)
        {
            facade.SendNotification("filesChanged");
            _hasChange = false;
        }

        var pendingErrors = _pendingErrors
            .GroupBy(e => e.Key.Path)
            .Select(g =>
                (
                    FilePath: facade.GetFilePath(g.Key),
                    Diagnostics: g.SelectMany(e => e.Value)
                        .DistinctBy(e => (e.Message, e.Location))
                        .Select(ToDiagnostic)
                        .ToArray()
                )
            )
            .ToList();

        _pendingErrors.Clear();

        var cacheChanged = false;

        foreach (var (filePath, diagnostics) in pendingErrors)
        {
            var existing = GetDiagnostics(filePath);
            if (existing is not null && existing.Diagnostics.SequenceEqual(diagnostics))
            {
                continue;
            }

            // Tous les fichiers analysés sont en cache (y compris sans erreur), pour que le pull ait toujours un resultId.
            _diagnosticsCache[filePath] = new(diagnostics, (++_lastResultId).ToString());

            // Push uniquement des diagnostics qui ont changé (un fichier inconnu sans erreur n'a rien à effacer).
            if (existing is not null || diagnostics.Length > 0)
            {
                facade.TextDocument.PublishDiagnostics(
                    new() { Diagnostics = new Container<Diagnostic>(diagnostics), Uri = new Uri(filePath) }
                );
                cacheChanged = true;
            }
        }

        // Le client fusionne les diagnostics poussés et tirés : sans refresh, le résultat pull d'un document ouvert
        // resterait périmé quand la modification d'un autre fichier change ses erreurs.
        if (
            cacheChanged
            && facade.ClientSettings?.Capabilities?.Workspace?.Diagnostics.Value?.RefreshSupport == true
        )
        {
            facade.Workspace.SendDiagnosticRefresh(new());
        }

        if (refresh)
        {
            facade.Workspace.SendSemanticTokensRefresh(new());
        }
    }

    private static Diagnostic ToDiagnostic(ModelError error)
    {
        return new()
        {
            Code = error.ErrorType.ToString(),
            Severity = error.IsError ? DiagnosticSeverity.Error : DiagnosticSeverity.Warning,
            Message = error.Message,
            Range = error.Location.ToRange()!
                ?? new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(0, 0, 0, 0),
            Source = "TopModel",
        };
    }
}
