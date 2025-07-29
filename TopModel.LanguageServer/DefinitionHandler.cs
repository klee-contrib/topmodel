using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.Model;
using TopModel.Core.Utils;

namespace TopModel.LanguageServer;

public class DefinitionHandler(ModelStore modelStore, ILanguageServerFacade facade, ModelConfig config) : DefinitionHandlerBase
{
    public override async Task<LocationOrLocationLinks?> Handle(DefinitionParams request, CancellationToken cancellationToken)
    {
        await modelStore.WaitForUpdates();

        var file = modelStore.Files.SingleOrDefault(f => facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath());
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
                    return new();
                }

                return new(new LocationLink
                {
                    OriginSelectionRange = matchedReference.ToRange(),
                    TargetRange = objet switch
                    {
                        Class or Endpoint or Domain => selectionRange with { End = new() { Line = selectionRange.Start.Line + 2, Character = 200 } },
                        Decorator or DecoratorInstance or Annotation or AnnotationInstance or DataFlow => selectionRange with { End = new() { Line = selectionRange.Start.Line + 1, Character = 200 } },
                        _ => selectionRange with { End = new() { Line = selectionRange.Start.Line, Character = 200 } }
                    },
                    TargetSelectionRange = selectionRange,
                    TargetUri = facade.GetFilePath(objet.GetFile())
                });
            }

            var matchedUse = file.Uses.SingleOrDefault(use =>
                use.Start.Line - 1 <= request.Position.Line && request.Position.Line <= use.End.Line - 1
                && use.Start.Column - 1 <= request.Position.Character && request.Position.Character <= use.End.Column - 1);

            if (matchedUse != null)
            {
                var usedFile = modelStore.Files.SingleOrDefault(f => f.Name == matchedUse.ReferenceName);
                if (usedFile != null)
                {
                    return new(new LocationLink
                    {
                        OriginSelectionRange = matchedReference.ToRange(),
                        TargetRange = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(0, 0, 5, 200),
                        TargetSelectionRange = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(0, 0, 0, 0),
                        TargetUri = facade.GetFilePath(usedFile)
                    });
                }
            }
        }

        return new();
    }

    protected override DefinitionRegistrationOptions CreateRegistrationOptions(DefinitionCapability capability, ClientCapabilities clientCapabilities)
    {
        return new DefinitionRegistrationOptions
        {
            DocumentSelector = config.GetDocumentSelector()
        };
    }
}