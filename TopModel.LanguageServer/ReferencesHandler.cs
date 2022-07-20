using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
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
            var classe = file.Classes.SingleOrDefault(c => c.Name.GetLocation()!.Start.Line - 1 == request.Position.Line || c.GetLocation()!.Start.Line - 1 == request.Position.Line);
            if (classe != null)
            {
                return Task.FromResult(new LocationContainer(
                    _modelStore.GetClassReferences(classe)
                        .Select(r => new Location
                        {
                            Uri = new Uri(_facade.GetFilePath(r.File)),
                            Range = r.Reference.ToRange()!
                        })));
            }

            var domain = file.Domains.SingleOrDefault(d => d.Name.GetLocation()!.Start.Line - 1 == request.Position.Line || d.GetLocation()!.Start.Line - 1 == request.Position.Line);
            if (domain != null)
            {
                return Task.FromResult(new LocationContainer(
                    _modelStore.GetDomainReferences(domain)
                        .Select(r => new Location
                        {
                            Uri = new Uri(_facade.GetFilePath(r.File)),
                            Range = r.Reference.ToRange()!
                        })));
            }

            var decorator = file.Decorators.SingleOrDefault(d => d.GetLocation()!.Start.Line - 1 == request.Position.Line);
            if (decorator != null)
            {
                return Task.FromResult(new LocationContainer(
                    _modelStore.GetDecoratorReferences(decorator)
                        .Select(r => new Location
                        {
                            Uri = new Uri(_facade.GetFilePath(r.File)),
                            Range = r.Reference.ToRange()!
                        })));
            }
        }

        return Task.FromResult<LocationContainer>(new());
    }

    protected override ReferenceRegistrationOptions CreateRegistrationOptions(ReferenceCapability capability, ClientCapabilities clientCapabilities)
    {
        return new ReferenceRegistrationOptions()
        {
            DocumentSelector = DocumentSelector.ForLanguage("yaml")
        };
    }
}