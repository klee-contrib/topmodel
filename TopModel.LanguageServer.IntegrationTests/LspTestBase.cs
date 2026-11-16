using OmniSharp.Extensions.LanguageServer.Client;

namespace TopModel.LanguageServer.IntegrationTests;

/// <summary>
/// Base des tests d'intégration : accès au client partagé, jeton d'annulation par test et log du serveur en cas d'échec.
/// </summary>
public abstract class LspTestBase
{
    private CancellationTokenSource? _timeout;

    /// <summary>
    /// Contexte de test MSTest.
    /// </summary>
    public TestContext TestContext { get; set; } = null!;

    /// <summary>
    /// Fichier de modèle principal utilisé par les tests (entité avec des références de domaines).
    /// </summary>
    protected static string CommandeFile => Path.Combine("Restaurant", "Entities", "Commande.tmd");

    /// <summary>
    /// Fichier de modèle contenant les domaines.
    /// </summary>
    protected static string DomainFile => Path.Combine("meta", "domain.tmd");

    /// <summary>
    /// Client LSP partagé.
    /// </summary>
    protected static LanguageClient Client => LanguageServerFixture.Client;

    /// <summary>
    /// Jeton d'annulation du test (60 secondes maximum).
    /// </summary>
    protected CancellationToken Ct => _timeout!.Token;

    [TestCleanup]
    public void BaseCleanup()
    {
        _timeout?.Dispose();
        if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
        {
            TestContext.WriteLine("--- Sortie d'erreur du language server ---");
            foreach (var line in LanguageServerFixture.GetServerLog())
            {
                TestContext.WriteLine(line);
            }
        }
    }

    [TestInitialize]
    public void BaseInitialize()
    {
        _timeout = CancellationTokenSource.CreateLinkedTokenSource(TestContext.CancellationToken);
        _timeout.CancelAfter(TimeSpan.FromSeconds(60));
    }

    /// <summary>
    /// Chemin système d'un fichier du modèle de test.
    /// </summary>
    /// <param name="relativePath">Chemin relatif à "tests/model".</param>
    /// <returns>Le chemin complet.</returns>
    protected static string ModelPath(string relativePath) =>
        Path.Combine(LanguageServerFixture.ModelDir, relativePath);

    /// <summary>
    /// Ouvre un fichier du modèle de test.
    /// </summary>
    /// <param name="relativePath">Chemin relatif à "tests/model".</param>
    /// <returns>Le document ouvert (à libérer en fin de test).</returns>
    protected Task<OpenDocument> OpenAsync(string relativePath) => OpenDocument.OpenAsync(relativePath, Ct);
}

