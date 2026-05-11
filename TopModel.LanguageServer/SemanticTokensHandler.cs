using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core.FileModel;

namespace TopModel.LanguageServer;

public class SemanticTokensHandler(ModelStoreRegistry registry, ILanguageServerFacade facade)
    : SemanticTokensHandlerBase
{
    protected override SemanticTokensRegistrationOptions CreateRegistrationOptions(
        SemanticTokensCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new SemanticTokensRegistrationOptions
        {
            DocumentSelector = registry.GetCombinedDocumentSelector(),
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
        var filePath = identifier.TextDocument.Uri.GetFileSystemPath();
        var entry = registry.GetPrimaryForFile(filePath);
        if (entry == null)
        {
            return;
        }

        var modelStore = entry.Store;
        await modelStore.WaitForUpdates(cancellationToken);

        var file = modelStore.Files.SingleOrDefault(f => facade.GetFilePath(f) == filePath);
        if (file != null)
        {
            foreach (var reference in file.Uses)
            {
                if (modelStore.Files.Any(f => f.Name == reference.ReferenceName))
                {
                    builder.Push(reference.ToRange()!, SemanticTokenType.Parameter, SemanticTokenModifier.Definition);
                }
            }

            foreach (var reference in file.References.Keys.OrderBy(r => r.Start.Line).ThenBy(r => r.Start.Column))
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
