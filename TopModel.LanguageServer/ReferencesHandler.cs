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
            var references = _modelStore.GetReferencesForPositionInFile(request.Position, file);
            if (references != null)
            {
                return Task.FromResult(new LocationContainer(
                    references.Select(r => new Location
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