using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;

namespace TopModel.LanguageServer;

public class DefinitionHandler : DefinitionHandlerBase
{
    private readonly ModelConfig _config;
    private readonly ILanguageServerFacade _facade;
    private readonly ModelStore _modelStore;

    public DefinitionHandler(ModelStore modelStore, ILanguageServerFacade facade, ModelConfig config)
    {
        _config = config;
        _facade = facade;
        _modelStore = modelStore;
    }

    public override Task<LocationOrLocationLinks?> Handle(DefinitionParams request, CancellationToken cancellationToken)
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
                var selectionRange = objet.GetLocation().ToRange();
                if (selectionRange == null)
                {
                    return Task.FromResult<LocationOrLocationLinks?>(new());
                }

                return Task.FromResult<LocationOrLocationLinks?>(new(new LocationLink
                {
                    OriginSelectionRange = matchedReference.ToRange(),
                    TargetRange = objet switch
                    {
                        Class or Endpoint or Domain => selectionRange with { End = new() { Line = selectionRange.Start.Line + 2, Character = 200 } },
                        Decorator or (Decorator, _) or DataFlow => selectionRange with { End = new() { Line = selectionRange.Start.Line + 1, Character = 200 } },
                        _ => selectionRange with { End = new() { Line = selectionRange.Start.Line, Character = 200 } }
                    },
                    TargetSelectionRange = selectionRange,
                    TargetUri = _facade.GetFilePath(objet.GetFile())
                }));
            }

            var matchedUse = file.Uses.SingleOrDefault(use =>
                use.Start.Line - 1 <= request.Position.Line && request.Position.Line <= use.End.Line - 1
                && use.Start.Column - 1 <= request.Position.Character && request.Position.Character <= use.End.Column - 1);

            if (matchedUse != null)
            {
                var usedFile = _modelStore.Files.SingleOrDefault(f => f.Name == matchedUse.ReferenceName);
                if (usedFile != null)
                {
                    return Task.FromResult<LocationOrLocationLinks?>(new(new LocationLink
                    {
                        OriginSelectionRange = matchedReference.ToRange(),
                        TargetRange = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(0, 0, 5, 200),
                        TargetSelectionRange = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(0, 0, 0, 0),
                        TargetUri = _facade.GetFilePath(usedFile)
                    }));
                }
            }
        }

        return Task.FromResult<LocationOrLocationLinks?>(new());
    }

    protected override DefinitionRegistrationOptions CreateRegistrationOptions(DefinitionCapability capability, ClientCapabilities clientCapabilities)
    {
        return new DefinitionRegistrationOptions
        {
            DocumentSelector = _config.GetDocumentSelector()
        };
    }
}