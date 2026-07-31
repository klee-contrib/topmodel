using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core.FileModel;

namespace TopModel.LanguageServer.Handlers;

public class RenameHandler(LSWorkerStore workerStore, ILanguageServerFacade facade) : RenameHandlerBase
{
    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public override async Task<WorkspaceEdit?> Handle(RenameParams request, CancellationToken cancellationToken)
    {
        await workerStore.WaitForUpdates(cancellationToken);

        var files = workerStore.GetFiles(request.TextDocument);

        if (!files.Any())
        {
            return null;
        }

        var allReferences = files.Select(f =>
            f.Store.GetReferencesForPositionInFile(request.Position, f.File, includeTransitive: true)
        );
        var objetName = allReferences.First().Objet.GetName();
        var references = allReferences.SelectMany(r => r);
        if (
            references.All(r =>
                r.Reference.ReferenceName == objetName
                || r.Reference is ClassReference
                || r.Reference is EndpointReference
            )
        )
        {
            return new WorkspaceEdit
            {
                Changes = references
                    .Where(r => r.Reference.ReferenceName == objetName)
                    .Select(r => new { Uri = facade.GetFilePath(r.File), r.Reference })
                    .Distinct()
                    .Select(c => new Location { Uri = c.Uri, Range = c.Reference.ToRange()! })
                    .Select(c => new { c.Uri, TextEdit = new TextEdit { NewText = request.NewName, Range = c.Range } })
                    .GroupBy(t => t.Uri, t => t)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.TextEdit)),
            };
        }

        return null;
    }

    protected override RenameRegistrationOptions CreateRegistrationOptions(
        RenameCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new RenameRegistrationOptions { DocumentSelector = workerStore.TmdFiles };
    }
}
