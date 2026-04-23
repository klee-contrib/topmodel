namespace TopModel.Generator.Documentation;

using TopModel.Utils;

/// <summary>
/// Utilitaire pour écrire des documents Markdown en s'appuyant sur un <see cref="IFileWriter"/>.
/// </summary>
public sealed class MarkdownWriter : IDisposable
{
    private readonly IFileWriter _fileWriter;
    private bool HasLevel1Header = false;

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="MarkdownWriter"/>.
    /// </summary>
    /// <param name="fileWriter">Writer sous-jacent dans lequel écrire le Markdown.</param>
    public MarkdownWriter(IFileWriter fileWriter)
    {
        _fileWriter = fileWriter;
        _fileWriter.EnableHeader = false;
    }

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        _fileWriter.Dispose();
    }

    /// <summary>
    /// Écrit une ligne vide ou le texte fourni suivi d'un saut de ligne.
    /// </summary>
    /// <param name="text">Texte à écrire (optionnel).</param>
    public void WriteLine(string? text = null)
    {
        _fileWriter.WriteLine(text);
    }

    /// <summary>
    /// Écrit un tableau Markdown complet (entête, séparateur et lignes).
    /// Les colonnes sont alignées : chaque cellule est complétée avec des espaces
    /// et les tirets du séparateur sont ajustés à la largeur maximale de chaque colonne.
    /// </summary>
    /// <param name="table">Tableau à écrire.</param>
    public void WriteTable(MarkdownTable table)
    {
        var columns = table.Columns.ToList();
        var rows = table.Rows.Select(r => r.Select(c => c ?? string.Empty).ToList()).ToList();

        var widths = new int[columns.Count];
        for (var i = 0; i < columns.Count; i++)
        {
            widths[i] = Math.Max(3, columns[i].Length);
        }

        foreach (var row in rows)
        {
            for (var i = 0; i < Math.Min(row.Count, widths.Length); i++)
            {
                if (row[i].Length > widths[i])
                {
                    widths[i] = row[i].Length;
                }
            }
        }

        _fileWriter.WriteLine($"| {string.Join(" | ", columns.Select((c, i) => c.PadRight(widths[i])))} |");
        _fileWriter.WriteLine($"| {string.Join(" | ", widths.Select(w => new string('-', w)))} |");

        foreach (var row in rows)
        {
            _fileWriter.WriteLine(
                $"| {string.Join(
                    " | ",
                    Enumerable.Range(0, columns.Count)
                        .Select(i => (i < row.Count ? row[i] : string.Empty).PadRight(widths[i])))} |"
            );
        }
    }

    /// <summary>
    /// Écrit un titre Markdown au niveau indiqué (1 = H1, 2 = H2, ...).
    /// </summary>
    /// <param name="level">Niveau de titre entre 1 et 6.</param>
    /// <param name="text">Texte du titre.</param>
    public void WriteTitle(int level, string text)
    {
        if (level is < 1 or > 6)
        {
            throw new ArgumentOutOfRangeException(nameof(level), "Le niveau de titre doit être compris entre 1 et 6.");
        }
        if (level == 1 && HasLevel1Header)
        {
            throw new InvalidOperationException("Un document Markdown ne peut contenir qu'un seul titre de niveau 1.");
        }
        HasLevel1Header |= level == 1;
        _fileWriter.WriteLine($"{new string('#', level)} {text}");
    }
}
