using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.Utils;

namespace TopModel.LanguageServer.Handlers;

public class DocumentSymbolHandler(LSWorkerStore workerStore, ILanguageServerFacade facade) : DocumentSymbolHandlerBase
{
    private ModelStore? ModelStore => workerStore.ModelStore;

    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public override async Task<SymbolInformationOrDocumentSymbolContainer?> Handle(
        DocumentSymbolParams request,
        CancellationToken cancellationToken
    )
    {
        await workerStore.WaitForUpdates(cancellationToken);

        var file = ModelStore?.Files.SingleOrDefault(f => facade.GetFilePath(f) == request.TextDocument.Uri);
        if (file == null)
        {
            return new();
        }

        return new(
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
                            Uri = request.TextDocument.Uri,
                        },
                    };
                })
                .Concat(
                    file.Endpoints.Select(e =>
                    {
                        return new SymbolInformation
                        {
                            Deprecated = false,
                            Kind = SymbolKind.Method,
                            Name = e.Name,
                            Location = new Location
                            {
                                Range = e.Name.GetLocation()?.ToRange()!,
                                Uri = request.TextDocument.Uri,
                            },
                        };
                    })
                )
                .Concat(
                    file.Domains.Select(d =>
                    {
                        return new SymbolInformation
                        {
                            Deprecated = false,
                            Kind = SymbolKind.Struct,
                            Name = d.Name,
                            Location = new Location
                            {
                                Range = d.GetLocation()?.ToRange()!,
                                Uri = request.TextDocument.Uri,
                            },
                        };
                    })
                )
                .Concat(
                    file.Annotations.Select(d =>
                    {
                        return new SymbolInformation
                        {
                            Deprecated = false,
                            Kind = SymbolKind.Interface,
                            Name = d.Name,
                            Location = new Location
                            {
                                Range = d.GetLocation()?.ToRange()!,
                                Uri = request.TextDocument.Uri,
                            },
                        };
                    })
                )
                .Concat(
                    file.Decorators.Select(d =>
                    {
                        return new SymbolInformation
                        {
                            Deprecated = false,
                            Kind = SymbolKind.Interface,
                            Name = d.Name,
                            Location = new Location
                            {
                                Range = d.GetLocation()?.ToRange()!,
                                Uri = request.TextDocument.Uri,
                            },
                        };
                    })
                )
                .Concat(
                    file.DataFlows.Select(d =>
                    {
                        return new SymbolInformation
                        {
                            Deprecated = false,
                            Kind = SymbolKind.Operator,
                            Name = d.Name,
                            Location = new Location
                            {
                                Range = d.GetLocation()?.ToRange()!,
                                Uri = request.TextDocument.Uri,
                            },
                        };
                    })
                )
                .Concat(
                    file.Converters.Select(c =>
                    {
                        return new SymbolInformation
                        {
                            Deprecated = false,
                            Kind = SymbolKind.Function,
                            Name = "Converter#" + file.Converters.IndexOf(c),
                            Location = new Location
                            {
                                Range = c.GetLocation()?.ToRange()!,
                                Uri = request.TextDocument.Uri,
                            },
                        };
                    })
                )
                .Where(d => d.Location.Range != null)
                .Select(t => new SymbolInformationOrDocumentSymbol(t))
        );
    }

    protected override DocumentSymbolRegistrationOptions CreateRegistrationOptions(
        DocumentSymbolCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new DocumentSymbolRegistrationOptions { DocumentSelector = TextDocumentSelector.TmdFiles };
    }
}
