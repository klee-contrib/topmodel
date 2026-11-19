using System.Text;

namespace TopModel.Utils;

public class FileWriterOptions
{
    /// <summary>
    /// Encodage du fichier à écrire.
    /// </summary>
    public Encoding Encoding { get; init; } = Encoding.UTF8;

    /// <summary>
    /// Active la lecture et l'écriture d'un entête avec un hash du fichier.
    /// </summary>
    public bool EnableHeader { get; init; } = true;

    /// <summary>
    /// Message à mettre dans le header.
    /// </summary>
    public string? HeaderMessage { get; init; }

    /// <summary>
    /// Indentation.
    /// </summary>
    public string IndentValue { get; init; } = "    ";

    /// <summary>
    /// Renvoie le token de début de ligne de commentaire dans le langage du fichier.
    /// </summary>
    public string StartCommentToken { get; init; } = "////";
}
