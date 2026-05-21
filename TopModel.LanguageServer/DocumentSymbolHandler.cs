using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using TopModel.Core.Utils;

namespace TopModel.LanguageServer;

public class DocumentSymbolHandler(ModelStoreRegistry registry) : DocumentSymbolHandlerBase
{
    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public override async Task<SymbolInformationOrDocumentSymbolContainer?> Handle(
        DocumentSymbolParams request,
        CancellationToken cancellationToken
    )
    {
        var filePath = request.TextDocument.Uri.GetFileSystemPath();
        var entry = registry.GetPrimaryForFile(filePath);
        if (entry == null)
        {
            return new();
        }

        var modelStore = entry.Store;
        await modelStore.WaitForUpdates(cancellationToken);

        var file = modelStore.Files.SingleOrDefault(f => f.GetFilePath() == request.TextDocument.Uri);
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
        return new DocumentSymbolRegistrationOptions { DocumentSelector = registry.GetCombinedDocumentSelector() };
    }
}
