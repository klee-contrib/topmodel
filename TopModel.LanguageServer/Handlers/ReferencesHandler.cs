using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;

namespace TopModel.LanguageServer.Handlers;

public class ReferencesHandler(LSWorkerStore workerStore, ILanguageServerFacade facade) : ReferencesHandlerBase
{
    private ModelStore? ModelStore => workerStore.ModelStore;

    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public override async Task<LocationContainer?> Handle(ReferenceParams request, CancellationToken cancellationToken)
    {
        await ModelStore.WaitForUpdates(cancellationToken);

        var file = ModelStore.Files.SingleOrDefault(f =>
            facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath()
        );
        if (file != null)
        {
            var references = ModelStore.GetReferencesForPositionInFile(request.Position, file);
            if (references != null)
            {
                return new(
                    references.Select(r => new Location
                    {
                        Uri = new Uri(facade.GetFilePath(r.File)),
                        Range = r.Reference.ToRange()!,
                    })
                );
            }
        }

        return new();
    }

    protected override ReferenceRegistrationOptions CreateRegistrationOptions(
        ReferenceCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new ReferenceRegistrationOptions() { DocumentSelector = TextDocumentSelector.TmdFiles };
    }
}
