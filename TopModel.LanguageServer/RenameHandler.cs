using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.FileModel;

class RenameHandler : RenameHandlerBase
{
    private readonly ModelStore _modelStore;
    private readonly ILanguageServerFacade _facade;

    public RenameHandler(ModelStore modelStore, ILanguageServerFacade facade)
    {
        _modelStore = modelStore;
        _facade = facade;
    }

    public override Task<WorkspaceEdit?> Handle(RenameParams request, CancellationToken cancellationToken)
    {
        var file = _modelStore.Files.SingleOrDefault(f => _facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath());
        if (file != null)
        {
            IEnumerable<(Reference Reference, ModelFile File)>? references = null;
            LocatedString? name = null;

            var clazz = file.Classes.SingleOrDefault(c =>
                c.Name.GetLocation()!.Start.Line - 1 == request.Position.Line
                || c.GetLocation()!.Start.Line - 1 == request.Position.Line);

            if (clazz != null)
            {
                name = clazz.Name;
                references = _modelStore.GetClassReferences(clazz).Select(r => ((Reference)r.Reference, r.File));
            }
            else
            {
                var domain = file.Domains.SingleOrDefault(d =>
                    d.Name.GetLocation()!.Start.Line - 1 == request.Position.Line
                    || d.GetLocation()!.Start.Line - 1 == request.Position.Line);

                if (domain != null)
                {
                    name = domain.Name;
                    references = _modelStore.GetDomainReferences(domain).Select(r => ((Reference)r.Reference, r.File));
                }
            };

            if (references != null)
            {
                return Task.FromResult<WorkspaceEdit?>(new WorkspaceEdit()
                {
                    Changes = references
                        .Select(r => new Location { Uri = new Uri(_facade.GetFilePath(r.File)), Range = r.Reference.ToRange()! })
                        .Concat(new List<Location>
                        {
                            new()
                            {
                                Uri = new Uri(_facade.GetFilePath(file)),
                                Range = name.GetLocation()!.ToRange()!
                            }
                        })
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
            DocumentSelector = DocumentSelector.ForLanguage("yaml")
        };
    }
}