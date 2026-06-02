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
                        .Select(r => new Location
                        {
                            Uri = new Uri(facade.GetFilePath(r.File)),
                            Range = r.Reference.ToRange()!,
                        })
                )
                .DistinctBy(r => new
                {
                    r.Uri.Path,
                    r.Range.Start.Line,
                    r.Range.Start.Character,
                })
        );
    }

    protected override ReferenceRegistrationOptions CreateRegistrationOptions(
        ReferenceCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new ReferenceRegistrationOptions() { DocumentSelector = TextDocumentSelector.TmdFiles };
    }
}
