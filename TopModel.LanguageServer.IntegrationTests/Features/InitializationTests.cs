using TopModel.LanguageServer.Handlers.Schema;

namespace TopModel.LanguageServer.IntegrationTests.Features;

[TestClass]
public class InitializationTests : LspTestBase
{
    [TestMethod]
    public async Task Schema_IsReturned()
    {
        var schema = await Client.SendRequest("schema", new SchemaRequest()).Returning<SchemaResponse>(Ct);

        Assert.IsNotNull(schema);
        Assert.IsGreaterThan(100, schema.Content.Length);
    }

    [TestMethod]
    public void Server_AdvertisesCapabilities()
    {
        var caps = Client.ServerSettings.Capabilities;

        Assert.IsNotNull(caps.HoverProvider, "hover");
        Assert.IsNotNull(caps.DefinitionProvider, "definition");
        Assert.IsNotNull(caps.CompletionProvider, "completion");
        Assert.IsNotNull(caps.ReferencesProvider, "references");
        Assert.IsNotNull(caps.RenameProvider, "rename");
        Assert.IsNotNull(caps.SemanticTokensProvider, "semanticTokens");
    }
}

