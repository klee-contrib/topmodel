namespace TopModel.Utils;

/// <summary>
/// Writer pour les fichiers dans les générateurs.
/// </summary>
public interface IFileWriter : IAsyncDisposable
{
    /// <summary>
    /// Nom du fichier à écrire.
    /// </summary>
    string FileName { get; }

    /// <summary>
    /// Langue technique du contenu généré.
    /// </summary>
    string? Language { get; }

    /// <summary>
    /// Options pour le FileWriter.
    /// </summary>
    FileWriterOptions Options { get; }

    /// <summary>
    /// Ecrit un string dans le stream.
    /// </summary>
    /// <param name="value">Chaîne de caractère.</para
    void Write(string? value);
}
