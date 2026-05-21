using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using TopModel.Core.Model;
using TopModel.Core.Utils;

namespace TopModel.LanguageServer;

public class DefinitionHandler(ModelStoreRegistry registry) : DefinitionHandlerBase
{
    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public override async Task<LocationOrLocationLinks?> Handle(
        DefinitionParams request,
        CancellationToken cancellationToken
    )
    {
        var filePath = request.TextDocument.Uri.GetFileSystemPath();
        var entry = registry.GetPrimaryForFile(filePath);
        if (entry == null)
        {
            return new();
        }

        var modelStore = entry.Store;
        await modelStore.WaitForUpdates(cancellationToken);

        var file = modelStore.Files.SingleOrDefault(f => f.GetFilePath() == filePath);
        if (file != null)
        {
            var (reference, objet) = file.GetObjetAtPosition(request.Position);

            if (reference != null && objet != null)
            {
                var selectionRange = objet.GetLocation().ToRange();
                if (selectionRange == null)
                {
                    return new();
                }

                return new(
                    new LocationLink
                    {
                        OriginSelectionRange = reference.ToRange(),
                        TargetRange = objet switch
                        {
                            Class or Endpoint or Domain => selectionRange with
                            {
                                End = new() { Line = selectionRange.Start.Line + 2, Character = 200 },
                            },
                            Decorator or DecoratorInstance or Annotation or AnnotationInstance or DataFlow =>
                                selectionRange with
                                {
                                    End = new() { Line = selectionRange.Start.Line + 1, Character = 200 },
                                },
                            _ => selectionRange with
                            {
                                End = new() { Line = selectionRange.Start.Line, Character = 200 },
                            },
                        },
                        TargetSelectionRange = selectionRange,
                        TargetUri = objet.GetFile().GetFilePath(),
                    }
                );
            }

            var matchedUse = file.Uses.SingleOrDefault(use =>
                use.Start.Line - 1 <= request.Position.Line
                && request.Position.Line <= use.End.Line - 1
                && use.Start.Column - 1 <= request.Position.Character
                && request.Position.Character <= use.End.Column - 1
            );

            if (matchedUse != null)
            {
                var usedFile = modelStore.Files.SingleOrDefault(f => f.Name == matchedUse.ReferenceName);
                if (usedFile != null)
                {
                    return new(
                        new LocationLink
                        {
                            OriginSelectionRange = reference.ToRange(),
                            TargetRange = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(0, 0, 5, 200),
                            TargetSelectionRange = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(
                                0,
                                0,
                                0,
                                0
                            ),
                            TargetUri = usedFile.GetFilePath(),
                        }
                    );
                }
            }
        }

        return new();
    }

    protected override DefinitionRegistrationOptions CreateRegistrationOptions(
        DefinitionCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new DefinitionRegistrationOptions { DocumentSelector = registry.GetCombinedDocumentSelector() };
    }
}
