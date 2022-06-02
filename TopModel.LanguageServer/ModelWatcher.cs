using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.LanguageServer;

public class ModelWatcher : IModelWatcher
{
    private readonly ILanguageServerFacade _facade;

    public ModelWatcher(ILanguageServerFacade facade)
    {
        _facade = facade;
    }

    public string Name => "Errors";

    public int Number { get; set; }

    public IEnumerable<string>? GeneratedFiles => null;

    public void OnErrors(IDictionary<ModelFile, IEnumerable<ModelError>> errors)
    {
        foreach (var fileErrors in errors)
        {
            var diagnostics = new List<Diagnostic>();

            foreach (var error in fileErrors.Value)
            {
                var loc = error.Location;
                diagnostics.Add(new()
                {
                    Code = error.ModelErrorType.ToString(),
                    Severity = error.IsError ? DiagnosticSeverity.Error : DiagnosticSeverity.Warning,
                    Message = error.Message,
                    Range = loc.ToRange()! ?? new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(0, 0, 0, 0),
                    Source = "TopModel"
                });
            }

            _facade.TextDocument.PublishDiagnostics(new()
            {
                Diagnostics = new Container<Diagnostic>(diagnostics.ToArray()),
                Uri = new Uri(_facade.GetFilePath(fileErrors.Key))
            });
        }
    }

    public void OnFilesChanged(IEnumerable<ModelFile> files)
    {
        _facade.SendNotification("filesChanged");
    }
}