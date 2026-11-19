using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using Location = TopModel.Core.FileModel.Location;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace TopModel.LanguageServer.Handlers;

public class RenameHandler(LSWorkerStore workerStore, ILanguageServerFacade facade)
    : RenameHandlerBase,
        IPrepareRenameHandler
{
    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public override async Task<WorkspaceEdit?> Handle(RenameParams request, CancellationToken cancellationToken)
    {
        var rename = await TryGetRenameTargets(request.TextDocument, request.Position, cancellationToken);
        if (rename is null)
        {
            return null;
        }

        var (objetName, references) = rename.Value;

        return facade.ToWorkspaceEdit(
            references.Where(r => r.Reference.Value == objetName).Select(r => (r.File, r.Reference, request.NewName))
        );
    }

    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public async Task<RangeOrPlaceholderRange?> Handle(PrepareRenameParams request, CancellationToken cancellationToken)
    {
        var rename = await TryGetRenameTargets(request.TextDocument, request.Position, cancellationToken);
        if (rename is null)
        {
            return null;
        }

        var (objetName, references) = rename.Value;
        var range = references
            .Where(r => r.Reference.Value == objetName)
            .Select(r => r.Reference.ToRange())
            .FirstOrDefault(r => Contains(r, request.Position))
            ?? references.First(r => r.Reference.Value == objetName).Reference.ToRange();

        return new RangeOrPlaceholderRange(new PlaceholderRange { Range = range, Placeholder = objetName });
    }

    protected override RenameRegistrationOptions CreateRegistrationOptions(
        RenameCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new RenameRegistrationOptions { DocumentSelector = workerStore.TmdFiles, PrepareProvider = true };
    }

    private static bool Contains(Range range, Position position)
    {
        return (
                range.Start.Line < position.Line
                || (range.Start.Line == position.Line && range.Start.Character <= position.Character)
            )
            && (
                range.End.Line > position.Line
                || (range.End.Line == position.Line && range.End.Character >= position.Character)
            );
    }

    private async Task<(
        string ObjetName,
        IReadOnlyList<(Location Reference, ModelFile File)> References
    )?> TryGetRenameTargets(TextDocumentIdentifier textDocument, Position position, CancellationToken cancellationToken)
    {
        await workerStore.WaitForUpdates(cancellationToken);

        var files = workerStore.GetFiles(textDocument);
        if (!files.Any())
        {
            return null;
        }

        var allReferences = files
            .Select(f => f.Store.GetLocationsForObjetAtPosition(position, f.File, includeTransitive: true))
            .ToList();
        var objet = allReferences[0].Objet;
        var objetName = objet.GetName();
        if (objetName is null)
        {
            return null;
        }

        if (
            objet is IProperty { NameSegments.Count: > 1 }
            || objet is IProperty { NameSegments: [NameSegment.ClassName, ..] }
        )
        {
            return null;
        }

        var references = allReferences.SelectMany(r => r).ToList();
        if (references.Count == 0)
        {
            return null;
        }

        return (objetName, references);
    }
}
