using Microsoft.Extensions.Localization;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Resolvers;

internal class EndpointResolver(IStringLocalizer localizer, IList<ModelFile> modelFiles)
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
                foreach (var property in endpoint.Params.GetDuplicates(p => p.GetParamName()))
                {
                    yield return new ModelError(
                        localizer,
                        ErrorType.TMD0001,
                        [property.Name],
                        modelFile,
                        property.Decorator is not null
                            ? endpoint.DecoratorReferences.FirstOrDefault(dr =>
                                dr.ReferenceName == property.Decorator.Name
                            )
                            : property.GetLocation()
                    );
                }

                if (
                    endpoint.Params.Count(p => p.ParamLocation == ParamLocation.JsonBody) > 1
                    || endpoint.Params.Any(p => p.ParamLocation == ParamLocation.JsonBody)
                        && endpoint.Params.Any(p => p.ParamLocation == ParamLocation.FormData)
                )
                {
                    yield return new ModelError(
                        localizer,
                        ErrorType.TMD7007,
                        [endpoint.Name.Value],
                        modelFile,
                        endpoint.GetLocation()
                    );
                }

                foreach (var param in endpoint.Params.Where(p => p.OwnLocation != null))
                {
                    if (param.Composition != null)
                    {
                        if (param.ParamLocation == ParamLocation.Query || param.ParamLocation == ParamLocation.Route)
                        {
                            yield return new ModelError(
                                localizer,
                                ErrorType.TMD7008,
                                [param.Name],
                                modelFile,
                                param.GetLocation()
                            );
                        }

                        if (
                            param.OwnLocation == ParamLocation.JsonBody
                            && (
                                param.Composition!.Properties.Any(cpp => cpp.ParamLocation == ParamLocation.FormData)
                                || endpoint.Params.Any(p => p != this && p.ParamLocation == ParamLocation.FormData)
                            )
                        )
                        {
                            yield return new ModelError(
                                localizer,
                                ErrorType.TMD7009,
                                [param.Name],
                                modelFile,
                                param.GetLocation()
                            );
                        }
                    }
                    else if (
                        !endpoint.Route.Contains($"{{{param.GetParamName()}}}")
                        && param.ParamLocation == ParamLocation.Route
                    )
                    {
                        yield return new ModelError(
                            localizer,
                            ErrorType.TMD7010,
                            [param.Name],
                            modelFile,
                            param.GetLocation()
                        );
                    }
                }

                var split = endpoint.FullRoute.Split("/");

                for (var i = 0; i < split.Length; i++)
                {
                    if (split[i].StartsWith('{'))
                    {
                        var routeParamName = split[i][1..^1];
                        var param = endpoint.Params.FirstOrDefault(param =>
                            param.ParamLocation == ParamLocation.Route && param.GetParamName() == routeParamName
                        );

                        if (param == null)
                        {
                            yield return new ModelError(
                                localizer,
                                ErrorType.TMD7005,
                                [
                                    endpoint.Name.Value,
                                    routeParamName,
                                    string.Join(
                                        ", ",
                                        endpoint.Params.Where(p => p.OwnLocation == null).Select(p => p.GetParamName())
                                    ),
                                ],
                                endpoint
                            );
                        }
                    }
                }

                foreach (
                    var queryParam in endpoint.Params.Where(param =>
                        param.ParamLocation == ParamLocation.Query || param.ParamLocation == ParamLocation.FormData
                    )
                )
                {
                    var index = endpoint.Params.IndexOf(queryParam);

                    if (
                        endpoint.Params.Any(param =>
                            param.DefaultValue == null
                            && param.Required
                            && (queryParam.DefaultValue != null || !queryParam.Required)
                            && endpoint.Params.IndexOf(param) > index
                        )
                    )
                    {
                        yield return new ModelError(
                            localizer,
                            ErrorType.TMD7004,
                            [queryParam.GetParamName()],
                            endpoint,
                            queryParam.GetLocation(),
                            isError: false
                        );
                    }
                }
            }
        }
    }
}
