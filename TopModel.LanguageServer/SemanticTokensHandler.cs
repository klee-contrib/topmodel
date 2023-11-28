using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.LanguageServer;

public class SemanticTokensHandler : SemanticTokensHandlerBase
{
    private readonly ModelConfig _config;
    private readonly ILanguageServerFacade _facade;
    private readonly ModelStore _modelStore;

    public SemanticTokensHandler(ModelStore modelStore, ILanguageServerFacade facade, ModelConfig config)
    {
        _config = config;
        _facade = facade;
        _modelStore = modelStore;
    }

    protected override SemanticTokensRegistrationOptions CreateRegistrationOptions(SemanticTokensCapability capability, ClientCapabilities clientCapabilities)
    {
        return new SemanticTokensRegistrationOptions
        {
            DocumentSelector = _config.GetDocumentSelector(),
            Legend = new()
            {
                TokenModifiers = capability.TokenModifiers,
                TokenTypes = capability.TokenTypes
            },
            Full = new SemanticTokensCapabilityRequestFull
            {
                Delta = true
            },
            Range = true
        };
    }

    protected override Task<SemanticTokensDocument> GetSemanticTokensDocument(ITextDocumentIdentifierParams @params, CancellationToken cancellationToken)
    {
        return Task.FromResult(new SemanticTokensDocument(RegistrationOptions.Legend));
    }

    protected override Task Tokenize(SemanticTokensBuilder builder, ITextDocumentIdentifierParams identifier, CancellationToken cancellationToken)
    {
        var file = _modelStore.Files.SingleOrDefault(f => _facade.GetFilePath(f) == identifier.TextDocument.Uri.GetFileSystemPath());
        if (file != null)
        {
            foreach (var reference in file.Uses)
            {
                if (_modelStore.Files.Any(f => f.Name == reference.ReferenceName))
                {
                    builder.Push(reference.ToRange()!, SemanticTokenType.Parameter, SemanticTokenModifier.Definition);
                }
            }

            foreach (var reference in file.References.Keys.OrderBy(r => r.Start.Line).ThenBy(r => r.Start.Column))
            {
                var type = reference switch
                {
                    ClassReference or DecoratorReference => SemanticTokenType.Class,
                    DataFlowReference => SemanticTokenType.Operator,
                    DomainReference => SemanticTokenType.EnumMember,
                    Reference r when r.ReferenceName == "false" => SemanticTokenType.Keyword,
                    _ => SemanticTokenType.Function
                };

                builder.Push(reference.ToRange()!, type, SemanticTokenModifier.Definition);
            }
        }

        return Task.CompletedTask;
    }
}