using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace TopModel.LanguageServer;

/// <summary>
/// Diagnostics d'un fichier à un instant donné.
/// </summary>
/// <param name="Diagnostics">Diagnostics du fichier.</param>
/// <param name="ResultId">Identifiant du résultat, qui change à chaque modification des diagnostics du fichier.</param>
public record FileDiagnostics(Diagnostic[] Diagnostics, string ResultId);
