using Microsoft.Extensions.Logging;

namespace TopModel.Utils;

/// <summary>
/// Provider pour récupérer des File Writers.
/// </summary>
public interface IFileWriterProvider
{
    /// <summary>
    /// Crée une nouvelle instance.
    /// </summary>
    /// <param name="fileName">Nom du fichier à écrire.</param>
    /// <param name="logger">Logger.</param>
    /// <param name="options">Options d'écriture du fichier.</param>
    /// <returns>GeneratedFileWriter</returns>
    IFileWriter OpenFileWriter(string fileName, ILogger logger, FileWriterOptions options);
}
