using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.LanguageServer;

public class ModelWatcher(ILanguageServerFacade facade) : IModelWatcher
{
    public string Name => "Errors";

    public int Number { get; init; }

    public IEnumerable<string>? GeneratedFiles => null;

    public bool Disabled => false;

    /// <inheritdoc cref="IModelWatcher.OnErrors" />
    public void OnErrors(IDictionary<ModelFile, IEnumerable<ModelError>> errors)
    {
        foreach (var fileErrors in errors)
        {
            var diagnostics = new List<Diagnostic>();

            foreach (var error in fileErrors.Value)
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
                    Uri = new Uri(facade.GetFilePath(fileErrors.Key)),
                }
            );
        }
    }

    /// <inheritdoc cref="IModelWatcher.OnFilesChanged" />
    public void OnFilesChanged(IEnumerable<ModelFile> files, LoggingScope? storeConfig = null)
    {
        facade.SendNotification("filesChanged");
    }

    /// <inheritdoc cref="IModelWatcher.OnFilesDeleted" />
    public void OnFilesDeleted(IEnumerable<string> fileNames)
    {
        facade.SendNotification("filesChanged");
    }
}
