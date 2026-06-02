using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using TopModel.Core.FileModel;

namespace TopModel.LanguageServer.Handlers;

public class SemanticTokensHandler(LSWorkerStore workerStore) : SemanticTokensHandlerBase
{
    protected override SemanticTokensRegistrationOptions CreateRegistrationOptions(
        SemanticTokensCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new SemanticTokensRegistrationOptions
        {
            DocumentSelector = TextDocumentSelector.TmdFiles,
            Legend = new()
            {
                TokenModifiers = capability?.TokenModifiers ?? [],
                TokenTypes = capability?.TokenTypes ?? [],
            },
            Full = new SemanticTokensCapabilityRequestFull { Delta = true },
            Range = true,
        };
    }

    protected override Task<SemanticTokensDocument> GetSemanticTokensDocument(
        ITextDocumentIdentifierParams @params,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new SemanticTokensDocument(RegistrationOptions.Legend));
    }

    protected override async Task Tokenize(
        SemanticTokensBuilder builder,
        ITextDocumentIdentifierParams identifier,
        CancellationToken cancellationToken
    )
    {
        await workerStore.WaitForUpdates(cancellationToken);

        var files = workerStore.GetFiles(identifier.TextDocument);

        if (files.Any())
        {
            foreach (var reference in files.First().File.Uses)
            {
                if (files.All(f => f.Store.Files.Any(f => f.Name == reference.ReferenceName)))
                {
                    builder.Push(reference.ToRange()!, SemanticTokenType.Parameter, SemanticTokenModifier.Definition);
                }
            }

            foreach (
                var reference in files
                    .SelectMany(f => f.File.References.Keys)
                    .DistinctBy(k => new { k.Start, k.End })
                    .OrderBy(r => r.Start.Line)
                    .ThenBy(r => r.Start.Column)
            )
            {
                var type = reference switch
                {
                    AnnotationReference or ClassReference or DecoratorReference or EndpointReference =>
                        SemanticTokenType.Class,
                    DataFlowReference => SemanticTokenType.Operator,
                    DomainReference => SemanticTokenType.EnumMember,
                    Reference r when r.ReferenceName == "false" => SemanticTokenType.Keyword,
                    ParameterReference => SemanticTokenType.Parameter,
                    _ => SemanticTokenType.Function,
                };

                builder.Push(reference.ToRange()!, type, SemanticTokenModifier.Definition);
            }
        }
    }
}
