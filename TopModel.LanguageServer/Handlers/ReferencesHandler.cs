using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;

namespace TopModel.LanguageServer.Handlers;

public class ReferencesHandler(LSWorkerStore workerStore, ILanguageServerFacade facade) : ReferencesHandlerBase
{
    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public override async Task<LocationContainer?> Handle(ReferenceParams request, CancellationToken cancellationToken)
    {
        await workerStore.WaitForUpdates(cancellationToken);

        var files = workerStore.GetFiles(request.TextDocument);
        return new(
            files
                .SelectMany(f =>
                    f.Store.GetReferencesForPositionInFile(request.Position, f.File)
                        .Select(r => new { Uri = facade.GetFilePath(r.File), r.Reference })
                )
                .Distinct()
                .Select(r => new Location { Range = r.Reference.ToRange()!, Uri = new Uri(r.Uri) })
        );
    }

    protected override ReferenceRegistrationOptions CreateRegistrationOptions(
        ReferenceCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new ReferenceRegistrationOptions() { DocumentSelector = workerStore.TmdFiles };
    }
}
