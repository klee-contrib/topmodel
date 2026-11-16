using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using TopModel.Core;

namespace TopModel.LanguageServer.Handlers;

public class CompletionHandler(LSWorkerStore workerStore, ModelFileCache fileCache) : CompletionHandlerBase
{
    /// <summary>
    /// Détermine le type de complétion à partir du chemin YAML du curseur (voir <see cref="YamlPathAnalyzer.GetPath" />).
    /// <c>rootObject</c> est le premier segment, <c>currentKey</c> la dernière clé (sans indices) et <c>parentKey</c>
    /// la clé précédente. Quand le curseur saisit une nouvelle clé (point final) ou un élément de liste,
    /// <c>parentKey</c> vaut <c>currentKey</c>, comme dans l'ancienne analyse par indentation.
    /// </summary>
    /// <param name="path">Chemin YAML, ou null.</param>
    /// <returns>Le type de complétion.</returns>
    public static CompletionKind GetCompletionKind(string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return CompletionKind.None;
        }

        var isNewKey = path.EndsWith('.');
        var keys = path.Split('.', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.IndexOf('[') is >= 0 and var bracket ? s[..bracket] : s)
            .Where(s => s.Length > 0)
            .ToList();
        if (keys.Count == 0)
        {
            return CompletionKind.None;
        }

        // Une clé en cours de saisie ne référence aucun objet du modèle : seules des propriétés peuvent être proposées
        // (clés de "values" ou de "mappings").
        if (isNewKey)
        {
            return CompletionKind.Property;
        }

        var rootObject = keys[0];
        var currentKey = keys[^1];

        // Écart connu : l'ancien code distinguait liste inline ("include: [a, |]", parentKey = clé englobante) et liste en
        // bloc ("- a", parentKey = currentKey) ; ici les deux donnent parentKey = currentKey.
        var parentKey =
            isNewKey || path.EndsWith(']') ? currentKey
            : keys.Count > 1 ? keys[^2]
            : string.Empty;

        if (
            parentKey == "asDomains" && !isNewKey
            || currentKey == "domain"
            || parentKey == "domain" && currentKey == "name" && !isNewKey
            || rootObject == "converter" && currentKey is "to" or "from"
        )
        {
            return CompletionKind.Domain;
        }

        if (
            currentKey is "association" or "composition" or "class" or "extends" && parentKey != currentKey
            || currentKey == "implements"
        )
        {
            return CompletionKind.Class;
        }

        return currentKey switch
        {
            "endpoint" => CompletionKind.Endpoint,
            "tags" => CompletionKind.Tag,
            "uses" => CompletionKind.File,
            _ when currentKey.Contains("decorator") => CompletionKind.Decorator,
            _ when currentKey.Contains("annotation", StringComparison.OrdinalIgnoreCase) => CompletionKind.Annotation,
            "dependsOn" => CompletionKind.DataFlow,
            _ => CompletionKind.Property,
        };
    }

    public override Task<CompletionItem> Handle(CompletionItem request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request);
    }

    public override async Task<CompletionList> Handle(CompletionParams request, CancellationToken cancellationToken)
    {
        await workerStore.WaitForUpdates(cancellationToken);

        var text = fileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath());
        var currentLine = text.ElementAtOrDefault(request.Position.Line);

        if (currentLine == null)
        {
            return new();
        }

        var files = workerStore.GetFiles(request.TextDocument).ToList();

        if (files.Count == 0 || currentLine == string.Empty)
        {
            return new();
        }

        // Étape 1 : position du curseur dans le YAML.
        var path = YamlPathAnalyzer.GetPath(text, request.Position.Line, request.Position.Character);

        // Étape 2 : type de complétion.
        var kind = GetCompletionKind(path);

        // Étape 3 : complétion.
        var provider = new CompletionProvider(text, request.Position, files);
        return kind switch
        {
            CompletionKind.Domain => provider.CompleteDomain(),
            CompletionKind.Class => provider.CompleteClass(),
            CompletionKind.Endpoint => provider.CompleteEndpoint(),
            CompletionKind.Tag => provider.CompleteTag(),
            CompletionKind.File => provider.CompleteFile(),
            CompletionKind.Decorator => provider.CompleteDecorator(),
            CompletionKind.Annotation => provider.CompleteAnnotation(),
            CompletionKind.DataFlow => provider.CompleteDataFlow(),
            CompletionKind.Property => provider.CompleteProperty(),
            _ => new(),
        };
    }

    protected override CompletionRegistrationOptions CreateRegistrationOptions(
        CompletionCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new CompletionRegistrationOptions { DocumentSelector = workerStore.TmdFiles };
    }
}
