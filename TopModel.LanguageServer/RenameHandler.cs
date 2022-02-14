using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;

class RenameHandler : RenameHandlerBase
{
    private readonly ModelStore _modelStore;
    private readonly ILanguageServerFacade _facade;

    private readonly ReferencesHandler _referencesHandler;

    public RenameHandler(ModelStore modelStore, ILanguageServerFacade facade, ReferencesHandler referencesHandler)
    {
        _modelStore = modelStore;
        _facade = facade;
        _referencesHandler = referencesHandler;
    }

    public override Task<WorkspaceEdit?> Handle(RenameParams request, CancellationToken cancellationToken)
    {
        var file = _modelStore.Files.SingleOrDefault(f => _facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath());
        if (file != null)
        {
            var clazz = file.Classes.Where(c =>
            c.Name.GetLocation()!.Start.Line - 1 == request.Position.Line
            || c.GetLocation()!.Start.Line - 1 == request.Position.Line
            ).SingleOrDefault();
            if (clazz != null)
            {
                return Task.FromResult<WorkspaceEdit?>(new WorkspaceEdit()
                {
                    Changes = _referencesHandler.findClassReferences(clazz)
                    .Concat(new List<Location>(){
                        new Location(){
                            Uri= new Uri(_facade.GetFilePath(file)),
                            Range = clazz.Name.GetLocation()!.ToRange()!
                        }
                    })
                    .Select(c => new
                    {
                        Uri = c.Uri,
                        TextEdit = new TextEdit()
                        {
                            NewText = request.NewName,
                            Range = c.Range
                        }
                    }).GroupBy(t => t.Uri, t => t).ToDictionary(x => x.Key, x => x.Select(y => y.TextEdit))
                });
            }
        }
        return Task.FromResult<WorkspaceEdit?>(null);
    }

    protected override RenameRegistrationOptions CreateRegistrationOptions(RenameCapability capability, ClientCapabilities clientCapabilities)
    {
        return new RenameRegistrationOptions()
        {
            DocumentSelector = DocumentSelector.ForLanguage("yaml")
        };
    }
}