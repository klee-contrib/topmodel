using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core.Model;
using TopModel.Core.Utils;

namespace TopModel.LanguageServer.Handlers;

public class DefinitionHandler(LSWorkerStore workerStore, ILanguageServerFacade facade) : DefinitionHandlerBase
{
    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public override async Task<LocationOrLocationLinks?> Handle(
        DefinitionParams request,
        CancellationToken cancellationToken
    )
    {
        await workerStore.WaitForUpdates(cancellationToken);

        var files = workerStore.GetFiles(request.TextDocument);
        if (!files.Any())
        {
            return new();
        }

        var references = files
            .Select(f => f.File.GetObjetAtPosition(request.Position))
            .Select(r => (r.Reference, r.Objet, SelectionRange: r.Objet?.GetLocation()?.ToRange()!))
            .Where(r => r.Reference != null && r.Objet != null && r.SelectionRange != null)
            .DistinctBy(r => r.Reference)
            .Select(r => new LocationLink
            {
                OriginSelectionRange = r.Reference.ToRange(),
                TargetRange = r.Objet switch
                {
                    Class or Endpoint or Domain => r.SelectionRange with
                    {
                        End = new() { Line = r.SelectionRange.Start.Line + 2, Character = 200 },
                    },
                    Decorator or DecoratorInstance or Annotation or AnnotationInstance or DataFlow =>
                        r.SelectionRange with
                        {
                            End = new() { Line = r.SelectionRange.Start.Line + 1, Character = 200 },
                        },
                    _ => r.SelectionRange with
                    {
                        End = new() { Line = r.SelectionRange.Start.Line, Character = 200 },
                    },
                },
                TargetSelectionRange = r.SelectionRange,
                TargetUri = facade.GetFilePath(r.Objet.GetFile()),
            });

        if (references.Any())
        {
            return new(references.Select(r => new LocationOrLocationLink(r)));
        }

        var matchedUse = files
            .First()
            .File.Uses.SingleOrDefault(use =>
                use.Start.Line - 1 <= request.Position.Line
                && request.Position.Line <= use.End.Line - 1
                && use.Start.Column - 1 <= request.Position.Character
                && request.Position.Character <= use.End.Column - 1
            );

        if (matchedUse != null)
        {
            var usedFile = files.First().Store.Files.SingleOrDefault(f => f.Name == matchedUse.ReferenceName);
            if (usedFile != null)
            {
                return new(
                    new LocationLink
                    {
                        TargetRange = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(0, 0, 5, 200),
                        TargetSelectionRange = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(
                            0,
                            0,
                            0,
                            0
                        ),
                        TargetUri = facade.GetFilePath(usedFile),
                    }
                );
            }
        }

        return new();
    }

    protected override DefinitionRegistrationOptions CreateRegistrationOptions(
        DefinitionCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new DefinitionRegistrationOptions { DocumentSelector = workerStore.TmdFiles };
    }
}
