using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.LanguageServer;

public class ModelErrorWatcher : IModelWatcher
{
    private readonly ILanguageServerFacade _facade;

    public ModelErrorWatcher(ILanguageServerFacade facade)
    {
        _facade = facade;
    }

    public string Name => "Errors";

    public int Number { get; set; }

    public void OnErrors(IDictionary<ModelFile, IEnumerable<ModelError>> errors)
    {
        foreach (var fileErrors in errors)
        {
            var diagnostics = new List<Diagnostic>();

            foreach (var error in fileErrors.Value)
            {
                var loc = error.Location!;
                diagnostics.Add(new()
                {
                    Code = "000",
                    Severity = DiagnosticSeverity.Error,
                    Message = error.Message,
                    Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(loc.Start.Line - 1, loc.Start.Column - 1, loc.End.Line - 1, loc.End.Column - 1),
                    Source = "TopModel"
                });
            }

            _facade.TextDocument.PublishDiagnostics(new()
            {
                Diagnostics = new Container<Diagnostic>(diagnostics.ToArray()),
                Uri = new Uri(_facade.Workspace.ClientSettings.RootPath + fileErrors.Key.Path[1..])
            });
        }
    }

    public void OnFilesChanged(IEnumerable<ModelFile> files)
    {
    }
}