using TopModel.LanguageServer.Handlers;

namespace TopModel.LanguageServer.Tests;

/// <summary>
/// Texte de test contenant un curseur, marqué par <see cref="Marker" />.
/// </summary>
/// <param name="Lines">Lignes du texte, sans le marqueur.</param>
/// <param name="Line">Ligne du curseur (base 0).</param>
/// <param name="Character">Colonne du curseur (base 0).</param>
public record CursorText(string[] Lines, int Line, int Character)
{
    /// <summary>
    /// Marqueur de la position du curseur.
    /// </summary>
    public const char Marker = '|';

    /// <summary>
    /// Vérifie le chemin YAML calculé pour la position du curseur d'un texte.
    /// </summary>
    /// <param name="expected">Chemin attendu.</param>
    /// <param name="text">Texte avec curseur.</param>
    public static void AssertPath(string? expected, string text)
    {
        Assert.AreEqual(expected, ParseContext(text).Path);
    }

    /// <summary>
    /// Extrait la position du curseur d'un texte contenant exactement un <see cref="Marker" />.
    /// </summary>
    /// <param name="text">Texte avec curseur.</param>
    /// <returns>Le texte sans le marqueur et la position du curseur.</returns>
    public static CursorText Parse(string text)
    {
        var lines = text.Replace("\r\n", "\n").Split('\n');
        var line = Array.FindIndex(lines, l => l.Contains(Marker));
        if (line < 0 || lines.Sum(l => l.Count(c => c == Marker)) != 1)
        {
            throw new ArgumentException($"Le texte doit contenir exactement un curseur '{Marker}'.", nameof(text));
        }

        var character = lines[line].IndexOf(Marker);
        lines[line] = lines[line].Remove(character, 1);
        return new(lines, line, character);
    }

    /// <summary>
    /// Calcule le contexte YAML de la position du curseur d'un texte.
    /// </summary>
    /// <param name="text">Texte avec curseur.</param>
    /// <returns>Le contexte du curseur.</returns>
    public static YamlCursorContext ParseContext(string text)
    {
        var cursor = Parse(text);
        return YamlCursorContext.Parse(cursor.Lines, cursor.Line, cursor.Character);
    }
}
