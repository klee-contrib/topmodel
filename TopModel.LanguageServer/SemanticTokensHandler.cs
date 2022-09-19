using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.FileModel;

class SemanticTokensHandler : SemanticTokensHandlerBase
{
    private readonly ModelStore _modelStore;
    private readonly ILanguageServerFacade _facade;

    public SemanticTokensHandler(ModelStore modelStore, ILanguageServerFacade facade)
    {
        _modelStore = modelStore;
        _facade = facade;
    }

    protected override SemanticTokensRegistrationOptions CreateRegistrationOptions(SemanticTokensCapability capability, ClientCapabilities clientCapabilities)
    {
        return new SemanticTokensRegistrationOptions
        {
            DocumentSelector = DocumentSelector.ForLanguage("yaml"),
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
                builder.Push(
                    reference.ToRange()!,
                    reference switch
                    {
                        ClassReference or DecoratorReference => SemanticTokenType.Class,
                        DomainReference => SemanticTokenType.EnumMember,
                        Reference r when r.ReferenceName == "this" || r.ReferenceName == "false" => SemanticTokenType.Keyword,
                        _ => SemanticTokenType.Function
                    },
                    SemanticTokenModifier.Definition);
            }
        }

        return Task.CompletedTask;
    }
}