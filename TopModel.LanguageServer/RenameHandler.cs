using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.LanguageServer;

public class RenameHandler : RenameHandlerBase
{
    private readonly ModelStore _modelStore;
    private readonly ILanguageServerFacade _facade;
    private readonly ModelConfig _config;

    public RenameHandler(ModelStore modelStore, ILanguageServerFacade facade, ModelConfig config)
    {
        _modelStore = modelStore;
        _facade = facade;
        _config = config;
    }

    public override Task<WorkspaceEdit?> Handle(RenameParams request, CancellationToken cancellationToken)
    {
        var file = _modelStore.Files.SingleOrDefault(f => _facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath());
        if (file != null)
        {
            var references = _modelStore.GetReferencesForPositionInFile(request.Position, file);
            if (references != null && references.All(r => r.Reference.ReferenceName == references.Objet.GetName() || r.Reference is ClassReference))
            {
                return Task.FromResult<WorkspaceEdit?>(new WorkspaceEdit
                {
                    Changes = references
                        .Where(r => r.Reference.ReferenceName == references.Objet.GetName())
                        .Select(r => new Location { Uri = new Uri(_facade.GetFilePath(r.File)), Range = r.Reference.ToRange()! })
                        .Select(c => new
                        {
                            c.Uri,
                            TextEdit = new TextEdit
                            {
                                NewText = request.NewName,
                                Range = c.Range
                            }
                        })
                        .GroupBy(t => t.Uri, t => t)
                        .ToDictionary(x => x.Key, x => x.Select(y => y.TextEdit))
                });
            }
        }

        return Task.FromResult<WorkspaceEdit?>(null);
    }

    protected override RenameRegistrationOptions CreateRegistrationOptions(RenameCapability capability, ClientCapabilities clientCapabilities)
    {
        return new RenameRegistrationOptions
        {
            DocumentSelector = _config.GetDocumentSelector()
        };
    }
}