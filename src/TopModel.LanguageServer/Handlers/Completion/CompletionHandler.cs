using System.Text.RegularExpressions;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace TopModel.LanguageServer.Handlers.Completion;

public class CompletionHandler(LSWorkerStore workerStore, ModelFileCache fileCache) : CompletionHandlerBase
{
    /// <summary>
    /// Détermine le type de complétion à partir du chemin YAML du curseur (voir <see cref="YamlCursorContext.Path" />).
    /// Les cas sont évalués dans l'ordre. Dans les motifs, <c>(^|\.)cle</c> désigne une clé (racine ou non) et
    /// <c>(\[\d+\])*$</c> autorise un élément de liste. Un point final indique une nouvelle clé en cours de saisie.
    /// </summary>
    /// <param name="path">Chemin YAML, ou null.</param>
    /// <returns>Le type de complétion.</returns>
    public static CompletionKind GetCompletionKind(string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return CompletionKind.None;
        }

        bool Is(string pattern) => Regex.IsMatch(path, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(100));

        return path switch
        {
            _ when Is(@"\.(annotations|propertyAnnotations|decorators)\[\d+\]\.[^.\[]+\.$|\.domain\.parameters\.$") =>
                CompletionKind.Parameter,
            _ when Is(@"^class\.values\.[^.\[]+\.$") => CompletionKind.OwnProperty,
            _ when Is(@"^class\.mappers\..*\.mappings\.$") => CompletionKind.OwnProperty,
            _ when path.EndsWith('.') => CompletionKind.None,
            _ when Is(@"^domain\.asDomains\.[^.\[]+$") => CompletionKind.Domain,
            _ when Is(@"\.domain(\.name)?$") => CompletionKind.Domain,
            _ when Is(@"^converter\.(to|from)(\[\d+\])*$") => CompletionKind.Domain,

            _ when Is(@"\.(association|composition|class|extends)$") => CompletionKind.Class,
            _ when Is(@"\.implements(\[\d+\])*$") => CompletionKind.Class,

            _ when Is(@"\.endpoint$") => CompletionKind.Endpoint,
            _ when Is(@"(^|\.)tags(\[\d+\])*$") => CompletionKind.Tag,
            _ when Is(@"^uses(\[\d+\])*$") => CompletionKind.File,
            _ when Is(@"\.(decorator|decorators\[\d+\])$") => CompletionKind.Decorator,
            _ when Is(@"\.(annotations|excludedAnnotations|propertyAnnotations)\[\d+\]$") => CompletionKind.Annotation,
            _ when Is(@"\.dependsOn(\[\d+\])*$") => CompletionKind.DataFlow,

            _ when Is(@"^class\.(defaultProperty|orderProperty|flagProperty|localeProperty|discriminatorProperty)$") =>
                CompletionKind.OwnProperty,
            _ when Is(@"^class\.unique\[\d+\]\[\d+\]$") => CompletionKind.OwnProperty,
            _ when Is(@"^class\.indexes\[\d+\](\[\d+\]|\.properties\[\d+\])$") => CompletionKind.OwnProperty,
            _ when Is(@"\.alias\.(property|include|exclude)(\[\d+\])*$") => CompletionKind.TargetProperty,
            _ when Is(@"\.properties\[\d+\]\.property$") => CompletionKind.TargetProperty,
            _ when Is(@"^class\.mappers\.from\[\d+\]\.params\[\d+\]\.target$") => CompletionKind.OwnProperty,
            _ when Is(@"^class\.mappers\..*\.mappings\.[^.\[]+$") => CompletionKind.TargetProperty,
            _ when Is(@"^dataFlow\.activeProperty$") => CompletionKind.TargetProperty,
            _ when Is(@"^dataFlow\.sources\[\d+\]\.joinProperties(\[\d+\])+$") => CompletionKind.TargetProperty,

            _ => CompletionKind.None,
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
        var context = YamlCursorContext.Parse(text, request.Position.Line, request.Position.Character);
        var path = context.Path;

        // Étape 2 : type de complétion.
        var kind = GetCompletionKind(path);

        // Étape 3 : complétion.
        var provider = new CompletionProvider(text, request.Position, context, files);
        return kind switch
        {
            CompletionKind.Domain => provider.CompleteDomain(),
            CompletionKind.Class => provider.CompleteClass(),
            CompletionKind.Endpoint => provider.CompleteEndpoint(),
            CompletionKind.Tag => provider.CompleteTag(),
            CompletionKind.File => provider.CompleteFile(),
            CompletionKind.Decorator => provider.CompleteDecorator(),
            CompletionKind.Annotation => provider.CompleteAnnotation(),
            CompletionKind.Parameter => provider.CompleteParameter(path!),
            CompletionKind.DataFlow => provider.CompleteDataFlow(),
            CompletionKind.OwnProperty => provider.CompleteOwnProperty(path!),
            CompletionKind.TargetProperty => provider.CompleteTargetProperty(path!),
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
