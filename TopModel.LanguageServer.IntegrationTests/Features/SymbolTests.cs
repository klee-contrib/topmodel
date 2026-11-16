using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;

namespace TopModel.LanguageServer.IntegrationTests.Features;

[TestClass]
public class SymbolTests : LspTestBase
{
    [TestMethod]
    public async Task DocumentSymbols_AreReturned()
    {
        await using var doc = await OpenAsync(CommandeFile);

        var symbols = await Client.TextDocument.RequestDocumentSymbol(
            new DocumentSymbolParams { TextDocument = doc.Id },
            Ct
        );

        Assert.IsNotNull(symbols);
        Assert.IsNotEmpty(symbols);
    }

    [TestMethod]
    public async Task WorkspaceSymbols_FindClassesByName()
    {
        var symbols = await Client.TextDocument.RequestWorkspaceSymbols(
            new WorkspaceSymbolParams { Query = "Commande" },
            Ct
        );

        Assert.IsNotNull(symbols);
        Assert.IsNotEmpty(symbols);
    }
}
