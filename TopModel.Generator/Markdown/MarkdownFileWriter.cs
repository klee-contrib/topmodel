using Microsoft.Extensions.Logging;

namespace TopModel.Generator.Markdown;

/// <summary>
/// Writer pour l'écriture de fichier.
/// Spécifique pour les fichiers Markdown (usage du token commentaire Markdown).
/// </summary>
public class MarkdownFileWriter : FileWriter
{
    public MarkdownFileWriter(string fileName, ILogger logger)
        : base(fileName, logger)
    {
    }

    /// <summary>
    /// Renvoie le token de début de ligne de commentaire dans le langage du fichier.
    /// </summary>
    /// <returns>Toket de début de ligne de commentaire.</returns>
    protected override string StartCommentToken => "";

    public void writeTitle(int level, string title)
    {
        this.WriteLine();
        for (int i = 0; i < level; i++)
        {
            this.Write("#");
        }
        this.Write(" " + title + "\n");
    }
}