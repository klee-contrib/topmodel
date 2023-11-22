using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using TopModel.Core;

namespace TopModel.LanguageServer;

public static class ModelConfigExtensions
{
    public static TextDocumentSelector GetDocumentSelector(this ModelConfig config)
    {
        return TextDocumentSelector.ForPattern($"{config.ModelRoot}/**/*.tmd");
    }
}