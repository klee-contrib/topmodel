using MediatR;
using OmniSharp.Extensions.JsonRpc;
using TopModel.Core.Loaders;
using TopModel.LanguageServer.Handlers.Schema;
using TopModel.Utils;

namespace TopModel.LanguageServer.Handlers;

/// <summary>
/// Sert le schéma JSON des fichiers de modèle livré avec TopModel, pour que les clients n'aient plus
/// à en embarquer leur propre copie : celle-ci est figée à la version du client (l'extension VSCode
/// par exemple) et peut donc diverger de la version de TopModel réellement utilisée sur le modèle.
///
/// La requête ne dépend pas du modèle chargé : elle peut donc être servie dès le démarrage, sans
/// attendre la fin du chargement.
/// </summary>
public class SchemaHandler : IRequestHandler<SchemaRequest, SchemaResponse?>, IJsonRpcHandler
{
    /// <inheritdoc cref="IRequestHandler{TRequest, TResponse}.Handle" />
    public async Task<SchemaResponse?> Handle(SchemaRequest request, CancellationToken cancellationToken)
    {
        // Le schéma est porté par l'assembly de TopModel.Core, qui le copie dans le répertoire de sortie.
        var filePath = typeof(FileChecker).Assembly.GetFilePath("schema.json");
        if (!File.Exists(filePath))
        {
            return null;
        }

        return new SchemaResponse(await File.ReadAllTextAsync(filePath, cancellationToken));
    }
}
