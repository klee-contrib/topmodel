using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace TopModel.LanguageServer;

public class ReferencesHandler(ModelStoreRegistry registry) : ReferencesHandlerBase
{
    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public override async Task<LocationContainer?> Handle(ReferenceParams request, CancellationToken cancellationToken)
    {
        var filePath = request.TextDocument.Uri.GetFileSystemPath();

        // Les références sont agrégées depuis tous les stores qui couvrent ce fichier.
        var entries = registry.GetAllForFile(filePath).ToList();
        if (entries.Count == 0)
        {
            return new();
        }

        await registry.WaitForAllUpdatesAsync(cancellationToken);

        var locations = new List<Location>();

        foreach (var entry in entries)
        {
            var modelStore = entry.Store;
            var file = modelStore.Files.SingleOrDefault(f => f.GetFilePath() == filePath);
            if (file == null)
            {
                continue;
            }

            var references = modelStore.GetReferencesForPositionInFile(request.Position, file);
            if (references != null)
            {
                locations.AddRange(
                    references.Select(r => new Location
                    {
                        Uri = new Uri(r.File.GetFilePath()),
                        Range = r.Reference.ToRange()!,
                    })
                );
            }
        }

        return locations.Count > 0 ? new(locations) : new();
    }

    protected override ReferenceRegistrationOptions CreateRegistrationOptions(
        ReferenceCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new ReferenceRegistrationOptions { DocumentSelector = registry.GetCombinedDocumentSelector() };
    }
}
