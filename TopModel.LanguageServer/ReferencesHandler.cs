using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;
using TopModel.Core;

class ReferencesHandler : ReferencesHandlerBase
{
    private readonly ModelStore _modelStore;
    private readonly ILanguageServerFacade _facade;

    public ReferencesHandler(ModelStore modelStore, ILanguageServerFacade facade)
    {
        _modelStore = modelStore;
        _facade = facade;
    }
    public override Task<LocationContainer> Handle(ReferenceParams request, CancellationToken cancellationToken)
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
                return Task.FromResult<LocationContainer>(new LocationContainer(this.findClassReferences(clazz)));
            }
        }
        return Task.FromResult<LocationContainer>(new());
    }

    public IEnumerable<Location> findClassReferences(Class clazz)
    {
        return _modelStore.Classes
        .SelectMany(c => c.Properties)
        .Concat(_modelStore.Files.SelectMany(f => f.Endpoints).SelectMany(e => e.Params))
        .Concat(_modelStore.Files.SelectMany(f => f.Endpoints).Where(e => e.Returns != null).Select(e => e.Returns!))
        .Where(p =>
        p is AliasProperty al && al.Property.Class == clazz
        || p is AssociationProperty asp && asp.Association == clazz
        || p is CompositionProperty cp && cp.Composition == clazz)
        .Select(p =>
        {
            OmniSharp.Extensions.LanguageServer.Protocol.Models.Range range;
            if (p is AssociationProperty ap)
            {
                range = ap.Reference.ToRange()!;
            }

            else if (p is CompositionProperty cp)
            {
                range = cp.Reference.ToRange()!;
            }
            else
            {
                range = ((AliasProperty)p).ClassReference.ToRange()!;
            }

            return new Location()
            {
                Range = range!,
                Uri = new Uri(_facade.GetFilePath(p.GetFile()))
            };
        }).DistinctBy(l => l.Uri.ToString() + l.Range.Start);
    }

    protected override ReferenceRegistrationOptions CreateRegistrationOptions(ReferenceCapability capability, ClientCapabilities clientCapabilities)
    {
        return new ReferenceRegistrationOptions()
        {
            DocumentSelector = DocumentSelector.ForLanguage("yaml")
        };
    }
}