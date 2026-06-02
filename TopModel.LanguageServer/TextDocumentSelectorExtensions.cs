#pragma warning disable KTA1200

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace TopModel.LanguageServer;

public static class TextDocumentSelectorExtensions
{
    extension(TextDocumentSelector)
    {
        public static TextDocumentSelector TmdFiles => TextDocumentSelector.ForPattern("**/*.tmd");
    }
}
