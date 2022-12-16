using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;

namespace TopModel.LanguageServer;

public class DocumentSymbolHandler : DocumentSymbolHandlerBase
{
    private readonly ModelStore _modelStore;
    private readonly ILanguageServerFacade _facade;
    private readonly ModelConfig _config;

    public DocumentSymbolHandler(ModelStore modelStore, ILanguageServerFacade facade, ModelConfig config)
    {
        _modelStore = modelStore;
        _facade = facade;
        _config = config;
    }

    public override Task<SymbolInformationOrDocumentSymbolContainer> Handle(DocumentSymbolParams request, CancellationToken cancellationToken)
    {

        var file = _modelStore.Files.SingleOrDefault(f => _facade.GetFilePath(f) == request.TextDocument.Uri);
        if (file == null)
        {
            return Task.FromResult(new SymbolInformationOrDocumentSymbolContainer());
        }

        return Task.FromResult(new SymbolInformationOrDocumentSymbolContainer(
            file.Classes.Select(c =>
            {
                return new SymbolInformation
                {
                    Deprecated = false,
                    Kind = SymbolKind.Class,
                    Name = c.Name,
                    Location = new Location
                    {
                        Range = c.Name.GetLocation()?.ToRange()!,
                        Uri = request.TextDocument.Uri
                    }
                };
            })
            .Concat(file.Endpoints.Select(e =>
            {
                return new SymbolInformation
                {
                    Deprecated = false,
                    Kind = SymbolKind.Method,
                    Name = e.Name,
                    Location = new Location
                    {
                        Range = e.Name.GetLocation()?.ToRange()!,
                        Uri = request.TextDocument.Uri
                    }
                };
            }))
            .Concat(file.Domains.Select(d =>
            {
                return new SymbolInformation
                {
                    Deprecated = false,
                    Kind = SymbolKind.Struct,
                    Name = d.Name,
                    Location = new Location
                    {
                        Range = d.GetLocation()?.ToRange()!,
                        Uri = request.TextDocument.Uri
                    }
                };
            }))
            .Concat(file.Decorators.Select(d =>
            {
                return new SymbolInformation
                {
                    Deprecated = false,
                    Kind = SymbolKind.Interface,
                    Name = d.Name,
                    Location = new Location
                    {
                        Range = d.GetLocation()?.ToRange()!,
                        Uri = request.TextDocument.Uri
                    }
                };
            }))
            .Where(d => d.Location.Range != null)
            .Select(t => new SymbolInformationOrDocumentSymbol(t))));
    }

    protected override DocumentSymbolRegistrationOptions CreateRegistrationOptions(DocumentSymbolCapability capability, ClientCapabilities clientCapabilities)
    {
        return new DocumentSymbolRegistrationOptions
        {
            DocumentSelector = _config.GetDocumentSelector()
        };
    }
}