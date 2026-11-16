using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client;

namespace TopModel.LanguageServer.IntegrationTests;

/// <summary>
/// Document ouvert dans le serveur. À la libération, son contenu d'origine est restauré avant sa fermeture, pour que
/// le serveur (partagé entre tous les tests) retrouve un état sain.
/// </summary>
public sealed class OpenDocument : IAsyncDisposable
{
    private readonly string _originalText;
    private int _version = 1;

    private OpenDocument(string path, string text)
    {
        _originalText = text;
        Path = path;
        Text = text;
        Id = new TextDocumentIdentifier(DocumentUri.FromFileSystemPath(path));
    }

    /// <summary>
    /// Chemin système du fichier.
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// Identifiant du document pour les requêtes LSP.
    /// </summary>
    public TextDocumentIdentifier Id { get; }

    /// <summary>
    /// Contenu courant du document.
    /// </summary>
    public string Text { get; private set; }

    /// <summary>
    /// Ouvre un fichier du modèle de test.
    /// </summary>
    /// <param name="relativePath">Chemin relatif à "tests/model".</param>
    /// <param name="cancellationToken">Jeton d'annulation.</param>
    /// <returns>Le document ouvert.</returns>
    public static async Task<OpenDocument> OpenAsync(string relativePath, CancellationToken cancellationToken)
    {
        var path = System.IO.Path.Combine(LanguageServerFixture.ModelDir, relativePath);
        var doc = new OpenDocument(path, await File.ReadAllTextAsync(path, cancellationToken));
        LanguageServerFixture.Client.TextDocument.DidOpenTextDocument(
            new DidOpenTextDocumentParams
            {
                TextDocument = new TextDocumentItem
                {
                    Uri = doc.Id.Uri,
                    LanguageId = "yaml",
                    Version = doc._version,
                    Text = doc.Text,
                },
            }
        );
        return doc;
    }

    /// <summary>
    /// Remplace le contenu du document (didChange). Il sera restauré à la libération.
    /// </summary>
    /// <param name="newText">Nouveau contenu.</param>
    public void Change(string newText)
    {
        Text = newText;
        SendText(newText);
    }

    /// <inheritdoc cref="IAsyncDisposable.DisposeAsync" />
    public ValueTask DisposeAsync()
    {
        if (Text != _originalText)
        {
            SendText(_originalText);
        }

        LanguageServerFixture.Client.TextDocument.DidCloseTextDocument(
            new DidCloseTextDocumentParams { TextDocument = Id }
        );
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Position du marqueur <c>|</c> d'un extrait du document, par exemple <c>"domain: DO_I|D_2"</c>.
    /// Sans <paramref name="occurrence" /> (base 0), l'extrait doit apparaître exactement une fois.
    /// </summary>
    public Position PositionOf(string snippet, int? occurrence = null)
    {
        var markerIndex = snippet.IndexOf('|', StringComparison.Ordinal);
        Assert.IsGreaterThanOrEqualTo(0, markerIndex, $"L'extrait '{snippet}' doit contenir un marqueur '|'.");

        var needle = snippet.Remove(markerIndex, 1);
        var found = -1;
        for (var i = 0; i <= (occurrence ?? 0); i++)
        {
            found = Text.IndexOf(needle, found + 1, StringComparison.Ordinal);
            Assert.IsGreaterThanOrEqualTo(0, found, $"'{needle}' introuvable (occurrence {i}) dans {Path}.");
        }

        if (occurrence == null)
        {
            Assert.AreEqual(
                -1,
                Text.IndexOf(needle, found + 1, StringComparison.Ordinal),
                $"'{needle}' n'est pas unique dans {Path} : précisez une occurrence."
            );
        }

        var before = Text[..(found + markerIndex)];
        return new Position(before.Count(c => c == '\n'), before.Length - (before.LastIndexOf('\n') + 1));
    }

    private void SendText(string text)
    {
        _version++;
        LanguageServerFixture.Client.TextDocument.DidChangeTextDocument(
            new DidChangeTextDocumentParams
            {
                TextDocument = new OptionalVersionedTextDocumentIdentifier { Uri = Id.Uri, Version = _version },
                ContentChanges = new Container<TextDocumentContentChangeEvent>(
                    new TextDocumentContentChangeEvent { Text = text }
                ),
            }
        );
    }
}

