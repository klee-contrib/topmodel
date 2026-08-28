using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace TopModel.LanguageServer.Handlers;

public class DocumentSymbolHandler(LSWorkerStore workerStore) : DocumentSymbolHandlerBase
{
    private bool _supportsHierarchy;

    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public override async Task<SymbolInformationOrDocumentSymbolContainer?> Handle(
        DocumentSymbolParams request,
        CancellationToken cancellationToken
    )
    {
        await workerStore.WaitForUpdates(cancellationToken);

        var file = workerStore.GetFiles(request.TextDocument).FirstOrDefault().File;
        if (file == null)
        {
            return new();
        }

        var symbols = GetSymbols(file).OrderBy(s => s.Range.Start.Line).ToList();

        if (_supportsHierarchy)
        {
            return new(symbols.Select(s => new SymbolInformationOrDocumentSymbol(s)));
        }

        // `SymbolInformation` est déprécié depuis LSP 3.16, mais reste le seul format compris des
        // clients qui n'annoncent pas `hierarchicalDocumentSymbolSupport`.
        return new(
            symbols
                .SelectMany(s => Flatten(s, request.TextDocument.Uri))
                .Select(s => new SymbolInformationOrDocumentSymbol(s))
        );
    }

    protected override DocumentSymbolRegistrationOptions CreateRegistrationOptions(
        DocumentSymbolCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        _supportsHierarchy = capability?.HierarchicalDocumentSymbolSupport == true;
        return new DocumentSymbolRegistrationOptions { DocumentSelector = workerStore.TmdFiles };
    }

    /// <summary>
    /// Construit un symbole, en garantissant l'invariant LSP « <c>Range</c> contient
    /// <c>SelectionRange</c> et les ranges de tous les enfants ».
    /// </summary>
    private static DocumentSymbol? CreateSymbol(
        Reference? nameLocation,
        Reference? objectLocation,
        string name,
        SymbolKind kind,
        string? detail,
        IEnumerable<DocumentSymbol>? children = null
    )
    {
        var selectionRange = nameLocation.ToRange() ?? objectLocation.ToRange();
        if (selectionRange == null)
        {
            return null;
        }

        var childList = children?.ToList() ?? [];

        var range = childList
            .Select(c => c.Range)
            .Append(objectLocation.ToRange())
            .Where(r => r != null)
            .Aggregate(selectionRange, (acc, r) => Union(acc, r!));

        return new DocumentSymbol
        {
            Name = name,
            Detail = string.IsNullOrWhiteSpace(detail) ? null : detail.ReplaceLineEndings(" ").Trim(),
            Kind = kind,
            Range = range,
            SelectionRange = selectionRange,
            Children = childList.Count > 0 ? new Container<DocumentSymbol>(childList) : null,
        };
    }

    private static IEnumerable<SymbolInformation> Flatten(
        DocumentSymbol symbol,
        DocumentUri uri,
        string? containerName = null
    )
    {
        yield return new SymbolInformation
        {
            Kind = symbol.Kind,
            Name = symbol.Name,
            ContainerName = containerName,
            Location = new Location { Range = symbol.SelectionRange, Uri = uri },
        };

        foreach (var child in symbol.Children ?? Enumerable.Empty<DocumentSymbol>())
        {
            foreach (var descendant in Flatten(child, uri, symbol.Name))
            {
                yield return descendant;
            }
        }
    }

    /// <summary>
    /// Symboles enfants pour les propriétés d'un conteneur. Les propriétés héritées d'un décorateur
    /// ou d'un mapper défini dans un autre fichier sont écartées : leur emplacement ne pointe pas
    /// dans le document courant.
    /// </summary>
    private static IEnumerable<DocumentSymbol> GetPropertySymbols(ModelFile file, IEnumerable<IProperty?> properties)
    {
        return properties
            .OfType<IProperty>()
            .Select(p => TryCreatePropertySymbol(file, p))
            .OfType<DocumentSymbol>()
            .OrderBy(s => s.Range.Start.Line);
    }

    private static IEnumerable<DocumentSymbol> GetSymbols(ModelFile file)
    {
        var symbols = file
            .Classes.Select(c =>
                CreateSymbol(
                    c.Name.GetLocation(),
                    c.GetLocation(),
                    c.Name,
                    SymbolKind.Class,
                    c.Comment,
                    GetPropertySymbols(file, c.Properties)
                )
            )
            .Concat(
                file.Endpoints.Select(e =>
                    CreateSymbol(
                        e.Name.GetLocation(),
                        e.GetLocation(),
                        e.Name,
                        SymbolKind.Method,
                        e.Description,
                        GetPropertySymbols(file, e.Params.Append(e.Returns))
                    )
                )
            )
            .Concat(
                file.Domains.Select(d =>
                    CreateSymbol(d.Name.GetLocation(), d.GetLocation(), d.Name, SymbolKind.Struct, d.Label ?? d.Name)
                )
            )
            .Concat(
                file.Annotations.Select(a =>
                    CreateSymbol(a.Name.GetLocation(), a.GetLocation(), a.Name, SymbolKind.Interface, a.Description)
                )
            )
            .Concat(
                file.Decorators.Select(d =>
                    CreateSymbol(
                        d.Name.GetLocation(),
                        d.GetLocation(),
                        d.Name,
                        SymbolKind.Interface,
                        d.Description,
                        GetPropertySymbols(file, d.Properties)
                    )
                )
            )
            .Concat(
                file.DataFlows.Select(d =>
                    CreateSymbol(d.Name.GetLocation(), d.GetLocation(), d.Name, SymbolKind.Operator, detail: null)
                )
            )
            .Concat(
                file.Converters.Select(
                    (c, i) =>
                        CreateSymbol(
                            c.GetLocation(),
                            c.GetLocation(),
                            $"Converter#{i}",
                            SymbolKind.Function,
                            detail: null
                        )
                )
            );

        return symbols.OfType<DocumentSymbol>();
    }

    private static bool IsBefore(Position first, Position second)
    {
        return first.Line != second.Line ? first.Line < second.Line : first.Character < second.Character;
    }

    /// <summary>
    /// Décrit une propriété, ou renvoie <see langword="null" /> si le modèle est trop partiellement
    /// résolu pour pouvoir le faire.
    /// </summary>
    /// <remarks>
    /// Le language server interroge volontairement des modèles en erreur
    /// (<c>KeepFileErrorsInReferenceResolution</c>), or plusieurs accesseurs de <see cref="IProperty" />
    /// supposent l'alias résolu : <c>AliasProperty.Comment</c> déréférence la propriété cible sans la
    /// protéger, et <c>GetFile()</c> lève si le conteneur n'est pas encore rattaché. Un symbole
    /// manquant vaut mieux qu'une requête <c>textDocument/documentSymbol</c> en échec, qui vide tout
    /// l'outline. À supprimer si ces accesseurs deviennent null-safe dans TopModel.Core.
    /// </remarks>
    private static DocumentSymbol? TryCreatePropertySymbol(ModelFile file, IProperty property)
    {
        try
        {
            if (property.GetFile() != file)
            {
                return null;
            }

            return CreateSymbol(
                property.GetLocation(),
                property.GetLocation(),
                property.Name,
                SymbolKind.Field,
                property.Comment
            );
        }
        catch (Exception e) when (e is NullReferenceException or InvalidOperationException)
        {
            return null;
        }
    }

    private static Range Union(Range first, Range second)
    {
        return new Range(
            IsBefore(first.Start, second.Start) ? first.Start : second.Start,
            IsBefore(first.End, second.End) ? second.End : first.End
        );
    }
}
