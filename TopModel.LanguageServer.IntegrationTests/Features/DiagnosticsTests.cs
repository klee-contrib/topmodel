namespace TopModel.LanguageServer.IntegrationTests.Features;

[TestClass]
public class DiagnosticsTests : LspTestBase
{
    [TestMethod]
    public void Pull_IsAdvertisedByServer()
    {
        var provider = Client.ServerSettings.Capabilities?.DiagnosticProvider;

        Assert.IsNotNull(provider, "Le serveur doit annoncer le diagnostic provider.");
        Assert.AreEqual("topmodel", provider.Identifier);
    }

    [TestMethod]
    public async Task Pull_ReportsUnknownDomainThenUnchangedThenCleared()
    {
        await using var doc = await OpenAsync(CommandeFile);

        static bool IsUnknownDomain(Diagnostic d) => d.Message.ToString().Contains("DO_INCONNU", StringComparison.Ordinal);

        // Le serveur attend lui-même la fin de l'analyse : pas besoin d'attendre le push avant de tirer.
        doc.Change(doc.Text.Replace("domain: DO_ID_2", "domain: DO_INCONNU", StringComparison.Ordinal));
        var full = await PullAsync<RelatedFullDocumentDiagnosticReport>(doc, previousResultId: null);

        Assert.IsFalse(string.IsNullOrEmpty(full.ResultId), "Le rapport doit porter un resultId.");
        Assert.IsTrue(full.Items.Any(IsUnknownDomain), "Le domaine inconnu doit être signalé.");
        Assert.IsTrue(
            full.Items.Where(IsUnknownDomain).All(d => d.Severity == DiagnosticSeverity.Error),
            "Un domaine inconnu doit être signalé comme une erreur."
        );

        // Sans modification, le même resultId donne un rapport inchangé.
        var unchanged = await PullAsync<RelatedUnchangedDocumentDiagnosticReport>(doc, full.ResultId);
        Assert.AreEqual(full.ResultId, unchanged.ResultId);

        // Après correction, le rapport est de nouveau complet, sans le diagnostic.
        doc.Change(doc.Text.Replace("domain: DO_INCONNU", "domain: DO_ID_2", StringComparison.Ordinal));
        var cleared = await PullAsync<RelatedFullDocumentDiagnosticReport>(doc, full.ResultId);

        Assert.IsFalse(cleared.Items.Any(IsUnknownDomain));
        Assert.AreNotEqual(full.ResultId, cleared.ResultId);
    }

    [TestMethod]
    public async Task Pull_ReturnsNoErrorForValidDocument()
    {
        await using var doc = await OpenAsync(CommandeFile);
        var report = await PullAsync<RelatedFullDocumentDiagnosticReport>(doc, previousResultId: null);

        Assert.IsFalse(string.IsNullOrEmpty(report.ResultId), "Le rapport doit porter un resultId.");
        Assert.IsFalse(report.Items.Any(d => d.Severity == DiagnosticSeverity.Error));
    }

    private async Task<TReport> PullAsync<TReport>(OpenDocument doc, string? previousResultId)
        where TReport : RelatedDocumentDiagnosticReport
    {
        var report = await Client.TextDocument.RequestDocumentDiagnostic(
            new DocumentDiagnosticParams { TextDocument = doc.Id, PreviousResultId = previousResultId },
            Ct
        );
        return Assert.IsInstanceOfType<TReport>(report);
    }
}
