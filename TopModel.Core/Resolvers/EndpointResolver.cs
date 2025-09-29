using TopModel.Core.FileModel;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Resolvers;

internal class EndpointResolver(IList<ModelFile> modelFiles)
{
    /// <summary>
    /// Effectue les vérifications de cohérence sur le résultat de la résolution des endpoints.
    /// </summary>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> CheckResult()
    {
        foreach (var modelFile in modelFiles)
        {
            foreach (var endpoint in modelFile.Endpoints)
            {
                foreach (
                    var property in endpoint.Params.Where(
                        (e, i) => endpoint.Params.Where((p, j) => p.GetParamName() == e.GetParamName() && j < i).Any()
                    )
                )
                {
                    yield return new ModelError(
                        ErrorType.TMD0001,
                        modelFile,
                        $"Le nom '{property.Name}' est déjà utilisé.",
                        property.Decorator is not null
                            ? endpoint.DecoratorReferences.FirstOrDefault(dr =>
                                dr.ReferenceName == property.Decorator.Name
                            )
                            : property.GetLocation()
                    );
                }

                foreach (var queryParam in endpoint.GetQueryAndMultipartParams())
                {
                    var index = endpoint.Params.IndexOf(queryParam);

                    if (
                        endpoint.Params.Any(param =>
                            !param.IsQueryOrMultipartParam() && endpoint.Params.IndexOf(param) > index
                        )
                    )
                    {
                        yield return new ModelError(
                            ErrorType.TMD7002,
                            endpoint,
                            $"Le paramètre de requête (ou multipart) '{queryParam.GetParamName()}' doit suivre tous les paramètres de route ou de body dans un endpoint.",
                            queryParam.GetLocation(),
                            isError: false
                        );
                    }
                }

                var split = endpoint.FullRoute.Split("/");

                for (var i = 0; i < split.Length; i++)
                {
                    if (split[i].StartsWith('{'))
                    {
                        var routeParamName = split[i][1..^1];
                        var param = endpoint.Params.FirstOrDefault(param => param.GetParamName() == routeParamName);

                        if (param == null)
                        {
                            yield return new ModelError(
                                ErrorType.TMD7003,
                                endpoint,
                                $"Le endpoint '{endpoint.Name}' définit un paramètre '{routeParamName}' dans sa route qui n'existe pas dans la liste des paramètres. Les valeurs possibles sont : {string.Join(", ", endpoint.Params.Select(p => p.GetParamName()))}."
                            );
                        }
                    }
                }
            }
        }
    }
}
