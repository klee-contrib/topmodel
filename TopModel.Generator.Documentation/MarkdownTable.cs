namespace TopModel.Generator.Documentation;

/// <summary>
/// Représente un tableau Markdown : un ensemble de colonnes et des lignes associées.
/// </summary>
/// <remarks>
/// Initialise un nouveau tableau Markdown avec les colonnes indiquées.
/// </remarks>
/// <param name="columns">Libellés des colonnes.</param>
public sealed class MarkdownTable
{
    private readonly List<IEnumerable<string?>> _rows = [];

    /// <summary>
    /// Libellés des colonnes du tableau.
    /// </summary>
    public IEnumerable<string> Columns { get; init; } = [];

    /// <summary>
    /// Lignes du tableau.
    /// </summary>
    public IEnumerable<IEnumerable<string?>> Rows => _rows;

    /// <summary>
    /// Ajoute une ligne au tableau.
    /// </summary>
    /// <param name="cells">Valeurs des cellules. Les valeurs <c>null</c> seront rendues comme une chaîne vide.</param>
    /// <returns>Le tableau courant, pour permettre le chaînage.</returns>
    public MarkdownTable AddRow(IEnumerable<string?> cells)
    {
        _rows.Add([.. cells]);
        return this;
    }
}
