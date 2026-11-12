using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Core.Utils;

public static class EndpointExtensions
{
    public static IEnumerable<IProperty> GetFormDataParams(this Endpoint endpoint, WatcherConfigBase? config)
    {
        return (config?.GetParams(endpoint) ?? endpoint.Params).Where(param =>
            param.ParamLocation == ParamLocation.FormData
        );
    }

    public static IProperty? GetJsonBodyParam(this Endpoint endpoint, WatcherConfigBase? config)
    {
        return (config?.GetParams(endpoint) ?? endpoint.Params).SingleOrDefault(p =>
            p.ParamLocation == ParamLocation.JsonBody
        );
    }

    public static string GetParamName(this IProperty property)
    {
        if (property is AliasProperty { Property: null })
        {
            return string.Empty;
        }

        if (property is not AliasProperty alp || !alp.Property.PrimaryKey)
        {
            return property.NameCamel;
        }

        var trigram = alp.Trigram?.ToLower() ?? alp.FinalTrigram?.ToLower() ?? alp.Property.Class.NameCamel;

        if (string.IsNullOrWhiteSpace(trigram))
        {
            return property.NameCamel;
        }

        return $"{trigram}{property.NameCamel.ToFirstUpper()}";
    }

    public static IEnumerable<IProperty> GetQueryParams(this Endpoint endpoint, WatcherConfigBase? config)
    {
        return (config?.GetParams(endpoint) ?? endpoint.Params).Where(param =>
            param.ParamLocation == ParamLocation.Query
        );
    }

    public static IEnumerable<IProperty> GetRouteParams(this Endpoint endpoint)
    {
        return endpoint.Params.Where(param => param.ParamLocation == ParamLocation.Route);
    }

    public static bool HasInRoute(this Endpoint endpoint, IProperty param)
    {
        return endpoint.Route.Contains($"{{{param.GetParamName()}}}");
    }
}
