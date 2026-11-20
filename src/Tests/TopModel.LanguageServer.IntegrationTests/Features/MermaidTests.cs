using TopModel.LanguageServer.Handlers.Mermaid;

namespace TopModel.LanguageServer.IntegrationTests.Features;

[TestClass]
public class MermaidTests : LspTestBase
{
    [TestMethod]
    public async Task Mermaid_ReturnsDiagramForFile()
    {
        var path = ModelPath(CommandeFile);

        var mermaid = await Client
            .SendRequest("mermaid", new MermaidRequest(path, scope: null))
            .Returning<MermaidResponse>(Ct);

        Assert.IsNotNull(mermaid);
        Assert.IsFalse(string.IsNullOrWhiteSpace(mermaid.Diagram));
    }
}
