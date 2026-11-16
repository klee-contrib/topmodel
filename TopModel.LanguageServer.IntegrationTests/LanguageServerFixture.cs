using System.Diagnostics;
using OmniSharp.Extensions.LanguageServer.Client;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client;

namespace TopModel.LanguageServer.IntegrationTests;

/// <summary>
/// Démarre une seule fois le language server sur "tests/model" pour toute la suite, et l'arrête proprement.
/// </summary>
/// <remarks>
/// Le serveur est lancé en processus enfant de testhost, sans dotnet-coverage : pour obtenir la couverture, il suffit
/// d'exécuter <c>dotnet-coverage collect -- "dotnet test"</c>, le profileur étant hérité par les processus enfants.
/// L'arrêt doit rester propre (shutdown, exit) : un processus tué ne produit pas de couverture.
/// </remarks>
[TestClass]
public static class LanguageServerFixture
{
    private static readonly Queue<string> _stderr = new();
    private static Process? _process;

    /// <summary>
    /// Client LSP connecté au serveur.
    /// </summary>
    public static LanguageClient Client { get; private set; } = null!;

    /// <summary>
    /// Répertoire du modèle de test, racine du workspace du serveur.
    /// </summary>
    public static string ModelDir { get; private set; } = null!;

    [AssemblyCleanup]
    public static async Task Cleanup()
    {
        if (_process == null)
        {
            return;
        }

        try
        {
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await Client.Shutdown();
            Client.SendNotification("exit");
            await _process.WaitForExitAsync(timeout.Token);
        }
        catch (Exception e) when (e is OperationCanceledException or IOException or InvalidOperationException)
        {
            // Arrêt propre impossible : on tue le processus (sans couverture pour cette exécution).
            _process.Kill(entireProcessTree: true);
        }
        finally
        {
            Client.Dispose();
            _process.Dispose();
        }
    }

    /// <summary>
    /// Dernières lignes écrites par le serveur sur sa sortie d'erreur.
    /// </summary>
    /// <returns>Les lignes.</returns>
    public static IReadOnlyList<string> GetServerLog()
    {
        lock (_stderr)
        {
            return [.. _stderr];
        }
    }

    [AssemblyInitialize]
    public static async Task Initialize(TestContext context)
    {
        var serverDll = Path.Combine(AppContext.BaseDirectory, "TopModel.LanguageServer.dll");
        Assert.IsTrue(File.Exists(serverDll), $"Language server introuvable : {serverDll}");

        ModelDir = Path.Combine(FindRepositoryRoot(), "tests", "model");

        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,

            // Le serveur calcule les chemins des fichiers relativement à son répertoire courant (racine du workspace).
            WorkingDirectory = ModelDir,
        };
        psi.ArgumentList.Add(serverDll);
        psi.ArgumentList.Add("-f");
        psi.ArgumentList.Add(Path.Combine(ModelDir, "topmodel.config"));

        _process = Process.Start(psi)!;
        _process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                lock (_stderr)
                {
                    _stderr.Enqueue(e.Data);
                    if (_stderr.Count > 50)
                    {
                        _stderr.Dequeue();
                    }
                }
            }
        };
        _process.BeginErrorReadLine();

        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(context.CancellationToken);
        timeout.CancelAfter(TimeSpan.FromMinutes(2));

        Client = await LanguageClient.From(
            options =>
                options
                    .WithInput(_process.StandardOutput.BaseStream)
                    .WithOutput(_process.StandardInput.BaseStream)
                    .WithRootUri(DocumentUri.FromFileSystemPath(ModelDir))
                    .OnNotification("filesChanged", () => { }),
            timeout.Token
        );
    }

    private static string FindRepositoryRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "TopModel.slnx")))
        {
            dir = dir.Parent;
        }

        return dir?.FullName ?? throw new InvalidOperationException("Racine du dépôt (TopModel.slnx) introuvable.");
    }
}

