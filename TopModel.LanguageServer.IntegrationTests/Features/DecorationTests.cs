namespace TopModel.LanguageServer.IntegrationTests.Features;

[TestClass]
public class DecorationTests : LspTestBase
{
    [TestMethod]
    public async Task CodeLens_AreReturned()
    {
        await using var doc = await OpenAsync(CommandeFile);

        var lenses = await Client.TextDocument.RequestCodeLens(new CodeLensParams { TextDocument = doc.Id }, Ct);

        Assert.IsNotNull(lenses);
        Assert.IsNotEmpty(lenses);
    }

    [TestMethod]
    public async Task DocumentLinks_AreReturnedForLockFile()
    {
        // Les liens de document ne sont produits que pour les fichiers .lock (generatedFiles:).
        await using var doc = await OpenAsync("topmodel.lock");

        var links = await Client.TextDocument.RequestDocumentLink(new DocumentLinkParams { TextDocument = doc.Id }, Ct);

        Assert.IsNotNull(links);
        Assert.IsNotEmpty(links);
    }

    [TestMethod]
    public async Task SemanticTokens_AreReturned()
    {
        await using var doc = await OpenAsync(CommandeFile);

        var tokens = await Client.TextDocument.RequestSemanticTokens(
            new SemanticTokensParams { TextDocument = doc.Id },
            Ct
        );

        Assert.IsNotNull(tokens);
        Assert.IsNotEmpty(tokens.Data);
    }
}
