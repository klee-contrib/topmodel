using System.Text.Json;

namespace TopModel.LanguageServer.IntegrationTests.Features;

[TestClass]
public class NavigationTests : LspTestBase
{
    [TestMethod]
    public async Task Definition_OfDomainReference_PointsToDomainFile()
    {
        await using var doc = await OpenAsync(CommandeFile);

        var definition = await Client.TextDocument.RequestDefinition(
            new DefinitionParams
            {
                TextDocument = doc.Id,
                Position = doc.PositionOf("domain: DO_I|D_2", occurrence: 0),
            },
            Ct
        );

        var first = definition?.FirstOrDefault();
        var target = (first?.Location?.Uri ?? first?.LocationLink?.TargetUri)?.GetFileSystemPath();
        Assert.IsNotNull(target);
        Assert.AreEqual(ModelPath(DomainFile), target, ignoreCase: true);
    }

    [TestMethod]
    public async Task Hover_OnDomainReference_ReturnsDomainLabel()
    {
        await using var doc = await OpenAsync(CommandeFile);

        var hover = await Client.TextDocument.RequestHover(
            new HoverParams { TextDocument = doc.Id, Position = doc.PositionOf("domain: DO_I|D_2", occurrence: 0) },
            Ct
        );

        Assert.IsNotNull(hover);

        // Le hover sur une référence de domaine renvoie son libellé (Label ?? Name) : DO_ID_2 -> "Identifiant".
        Assert.Contains("Identifiant", JsonSerializer.Serialize(hover.Contents), StringComparison.Ordinal);
    }

    [TestMethod]
    public async Task PrepareRename_OnDomainReference_ReturnsRange()
    {
        await using var doc = await OpenAsync(CommandeFile);

        var range = await Client.SendRequest(
            new PrepareRenameParams
            {
                TextDocument = doc.Id,
                Position = doc.PositionOf("domain: DO_I|D_2", occurrence: 0),
            },
            Ct
        );

        Assert.IsNotNull(range);
    }

    [TestMethod]
    public async Task References_OfDomain_IncludeDeclarationAndUsages()
    {
        await using var doc = await OpenAsync(DomainFile);

        var references = await Client.TextDocument.RequestReferences(
            new ReferenceParams
            {
                TextDocument = doc.Id,
                Position = doc.PositionOf("name: DO_I|D_2"),
                Context = new ReferenceContext { IncludeDeclaration = true },
            },
            Ct
        );

        Assert.IsNotNull(references);
        Assert.IsGreaterThan(1, references.Count());
    }
}

