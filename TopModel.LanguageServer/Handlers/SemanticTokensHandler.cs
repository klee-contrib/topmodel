using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using TopModel.Core.FileModel;

namespace TopModel.LanguageServer.Handlers;

public class SemanticTokensHandler(LSWorkerStore workerStore) : SemanticTokensHandlerBase
{
    private static readonly SemanticTokensLegend ServerLegend = new()
    {
        TokenTypes = new Container<SemanticTokenType>(SemanticTokenType.Defaults),
        TokenModifiers = new Container<SemanticTokenModifier>(SemanticTokenModifier.Defaults),
    };

    protected override SemanticTokensRegistrationOptions CreateRegistrationOptions(
        SemanticTokensCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new SemanticTokensRegistrationOptions
        {
            DocumentSelector = workerStore.TmdFiles,
            Legend = ServerLegend,
            Full = new SemanticTokensCapabilityRequestFull { Delta = true },
            Range = true,
        };
    }

    protected override Task<SemanticTokensDocument> GetSemanticTokensDocument(
        ITextDocumentIdentifierParams @params,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new SemanticTokensDocument(ServerLegend));
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
                    builder.Push(
                        reference.ToRange()!,
                        SemanticTokenType.Parameter,
                        Array.Empty<SemanticTokenModifier>()
                    );
                }
            }

            foreach (
                var reference in files
                    .SelectMany(f => f.File.References.Keys)
                    .Distinct()
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

                builder.Push(reference.ToRange()!, type, Array.Empty<SemanticTokenModifier>());
            }
        }
    }
}
