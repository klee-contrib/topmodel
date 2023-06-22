using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;

namespace TopModel.LanguageServer;

public class HoverHandler : HoverHandlerBase
{
    private readonly ModelConfig _config;
    private readonly ILanguageServerFacade _facade;
    private readonly ModelStore _modelStore;

    public HoverHandler(ModelStore modelStore, ILanguageServerFacade facade, ModelConfig config)
    {
        _config = config;
        _facade = facade;
        _modelStore = modelStore;
    }

    public override Task<Hover?> Handle(HoverParams request, CancellationToken cancellationToken)
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
                return Task.FromResult<Hover?>(new Hover
                {
                    Range = matchedReference.ToRange(),
                    Contents = new(new MarkedString(objet switch
                    {
                        Class c => c.Comment,
                        Endpoint e => e.Description,
                        RegularProperty p => p.Comment,
                        AssociationProperty p => p.Comment,
                        CompositionProperty p => p.Comment,
                        AliasProperty p => p.Comment,
                        Domain d => d.Label,
                        Decorator d => d.Description,
                        DataFlow d => $"Flux de données '{d.Name}'",
                        (Decorator d, _) => d.Description,
                        _ => string.Empty
                    }))
                });
            }
        }

        return Task.FromResult<Hover?>(null);
    }

    protected override HoverRegistrationOptions CreateRegistrationOptions(HoverCapability capability, ClientCapabilities clientCapabilities)
    {
        return new HoverRegistrationOptions
        {
            DocumentSelector = _config.GetDocumentSelector()
        };
    }
}