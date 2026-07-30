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

    [Obsolete("Utiliser la surcharge avec la config en paramètre.")]
    public static IProperty? GetJsonBodyParam(this Endpoint endpoint)
    {
        return GetJsonBodyParam(endpoint, config: null);
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

    [Obsolete("Utiliser la surcharge avec la config en paramètre.")]
    public static IEnumerable<IProperty> GetQueryParams(this Endpoint endpoint)
    {
        return GetQueryParams(endpoint, config: null);
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

    [Obsolete("Utiliser ParamLocation == ParamLocation.JsonBody")]
    public static bool IsJsonBodyParam(this IProperty property)
    {
        return IsJsonBodyParam(property, config: null);
    }

    [Obsolete("Utiliser ParamLocation == ParamLocation.JsonBody")]
    public static bool IsJsonBodyParam(this IProperty property, WatcherConfigBase? config)
    {
        return property.ParamLocation == ParamLocation.JsonBody;
    }

    [Obsolete("Utiliser ParamLocation == ParamLocation.Query")]
    public static bool IsQueryParam(this IProperty property)
    {
        return IsQueryParam(property, config: null);
    }

    [Obsolete("Utiliser ParamLocation == ParamLocation.Query")]
    public static bool IsQueryParam(this IProperty property, WatcherConfigBase? config)
    {
        return property.ParamLocation == ParamLocation.Query;
    }

    [Obsolete("Utiliser ParamLocation == ParamLocation.Route")]
    public static bool IsRouteParam(this IProperty property)
    {
        return property.ParamLocation == ParamLocation.Route;
    }
}
