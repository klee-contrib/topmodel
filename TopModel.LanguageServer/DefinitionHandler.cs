using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;

class DefinitionHandler : DefinitionHandlerBase
{
    private readonly ModelStore _modelStore;
    private readonly ILanguageServerFacade _facade;

    public DefinitionHandler(ModelStore modelStore, ILanguageServerFacade facade)
    {
        _modelStore = modelStore;
        _facade = facade;
    }

    public override Task<LocationOrLocationLinks> Handle(DefinitionParams request, CancellationToken cancellationToken)
    {
        var file = _modelStore.Files.SingleOrDefault(f => _facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath());
        if (file != null)
        {
            var matchedReference = file.References.Keys.SingleOrDefault(reference =>
                reference.Start.Line - 1 <= request.Position.Line && request.Position.Line <= reference.End.Line - 1
                && reference.Start.Column - 1 <= request.Position.Character && request.Position.Character <= reference.End.Column - 1);

            if (matchedReference != null)
            {
                var objet = file.References[matchedReference];
                var selectionRange = objet.GetLocation().ToRange()!;
                return Task.FromResult<LocationOrLocationLinks>(new(new LocationLink
                {
                    OriginSelectionRange = matchedReference.ToRange(),
                    TargetRange = objet switch
                    {
                        Class or Endpoint or Domain => selectionRange with { End = new() { Line = selectionRange.Start.Line + 2, Character = 200 } },
                        _ => selectionRange with { End = new() { Line = selectionRange.Start.Line, Character = 200 } }
                    },
                    TargetSelectionRange = selectionRange,
                    TargetUri = _facade.GetFilePath(objet.GetFile())
                }));
            }
        }

        return Task.FromResult<LocationOrLocationLinks>(new());
    }

    protected override DefinitionRegistrationOptions CreateRegistrationOptions(DefinitionCapability capability, ClientCapabilities clientCapabilities)
    {
        return new DefinitionRegistrationOptions
        {
            DocumentSelector = DocumentSelector.ForLanguage("yaml")
        };
    }
}