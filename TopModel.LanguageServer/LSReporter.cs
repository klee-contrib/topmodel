using System.Collections.Concurrent;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.LanguageServer;

public class LSReporter(ILanguageServerFacade facade) : IModelReporter
{
    private readonly HashSet<string> _filesWithErrors = [];

    private readonly ConcurrentBag<KeyValuePair<ModelFile, IEnumerable<ModelError>>> _pendingErrors = [];

    private bool _hasChange;

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

        var diagnosticsToSend = _pendingErrors
            .GroupBy(e => (e.Key.Name, e.Key.Path))
            .ToDictionary(g => g.Key, g => g.SelectMany(e => e.Value).DistinctBy(e => (e.Message, e.Location)))
            .Where(e => _filesWithErrors.Contains(e.Key.Name) || e.Value.Any())
            .ToList();

        foreach (var fileDiagnostics in diagnosticsToSend)
        {
            var diagnostics = new List<Diagnostic>();

            if (fileDiagnostics.Value.Any())
            {
                _filesWithErrors.Add(fileDiagnostics.Key.Name);
            }
            else
            {
                _filesWithErrors.Remove(fileDiagnostics.Key.Name);
            }

            foreach (var error in fileDiagnostics.Value)
            {
                var loc = error.Location;
                diagnostics.Add(
                    new()
                    {
                        Code = error.ErrorType.ToString(),
                        Severity = error.IsError ? DiagnosticSeverity.Error : DiagnosticSeverity.Warning,
                        Message = error.Message,
                        Range =
                            loc.ToRange()! ?? new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(0, 0, 0, 0),
                        Source = "TopModel",
                    }
                );
            }

            facade.TextDocument.PublishDiagnostics(
                new()
                {
                    Diagnostics = new Container<Diagnostic>(diagnostics.ToArray()),
                    Uri = new Uri(facade.GetFilePath(fileDiagnostics.Key.Path)),
                }
            );

            _pendingErrors.Clear();
        }

        if (refresh)
        {
            facade.Workspace.SendSemanticTokensRefresh(new());
        }
    }
}
