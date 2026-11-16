namespace TopModel.LanguageServer.IntegrationTests.Features;

[TestClass]
public class CompletionTests : LspTestBase
{
    [TestMethod]
    public async Task Completion_OnDomain_ProposesDomains()
    {
        await using var doc = await OpenAsync(CommandeFile);

        var completion = await Client.TextDocument.RequestCompletion(
            new CompletionParams
            {
                TextDocument = doc.Id,
                Position = doc.PositionOf("domain: DO_|ID_2", occurrence: 0),
            },
            Ct
        );

        Assert.IsNotNull(completion);
        Assert.Contains(i => i.Label == "DO_ID_2", completion);
    }
}

