using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Core.Utils;

public static class EndpointExtensions
{
    [Obsolete("Utiliser la surcharge avec la config en paramètre.")]
    public static IProperty? GetJsonBodyParam(this Endpoint endpoint)
    {
        return GetJsonBodyParam(endpoint, config: null);
    }

    public static IProperty? GetJsonBodyParam(this Endpoint endpoint, WatcherConfigBase? config)
    {
        if (endpoint.IsMultipart)
        {
            return null;
        }

        var bodyParams = (config?.GetParams(endpoint) ?? endpoint.Params).Where(param =>
            param is IProperty { Composition: not null } or { Domain.BodyParam: true }
        );
        return bodyParams.Count() > 1
            ? throw new ModelException(
                endpoint,
                $"L'endpoint '{endpoint.Name}' doit avoir une seule propriété dans le body. Propriétés trouvées : {string.Join(", ", bodyParams)}"
            )
            : bodyParams.SingleOrDefault();
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

    public static IEnumerable<IProperty> GetQueryAndMultipartParams(this Endpoint endpoint)
    {
        return endpoint
            .Params.Where(param =>
                !(param is IProperty { Composition: not null } || (param.Domain?.BodyParam ?? false))
            )
            .Except(endpoint.GetRouteParams());
    }

    [Obsolete("Utiliser la surcharge avec la config en paramètre.")]
    public static IEnumerable<IProperty> GetQueryParams(this Endpoint endpoint)
    {
        return GetQueryParams(endpoint, config: null);
    }

    public static IEnumerable<IProperty> GetQueryParams(this Endpoint endpoint, WatcherConfigBase? config)
    {
        return (config?.GetParams(endpoint) ?? endpoint.Params)
            .Where(param =>
                !(
                    param is { Composition: not null }
                    || (param.Domain?.BodyParam ?? false)
                    || (param.Domain?.IsMultipart ?? false)
                )
            )
            .Except(endpoint.GetRouteParams());
    }

    public static IEnumerable<IProperty> GetRouteParams(this Endpoint endpoint)
    {
        return endpoint.Params.Where(param => endpoint.Route.Contains($"{{{param.GetParamName()}}}"));
    }

    [Obsolete("Utiliser la surcharge avec la config en paramètre.")]
    public static bool IsJsonBodyParam(this IProperty property)
    {
        return IsJsonBodyParam(property, config: null);
    }

    public static bool IsJsonBodyParam(this IProperty property, WatcherConfigBase? config)
    {
        return property.Endpoint.GetJsonBodyParam(config) == property;
    }

    public static bool IsQueryOrMultipartParam(this IProperty property)
    {
        return property.Endpoint.GetQueryAndMultipartParams().Contains(property);
    }

    [Obsolete("Utiliser la surcharge avec la config en paramètre.")]
    public static bool IsQueryParam(this IProperty property)
    {
        return IsQueryParam(property, config: null);
    }

    public static bool IsQueryParam(this IProperty property, WatcherConfigBase? config)
    {
        return property.Endpoint.GetQueryParams(config).Contains(property);
    }

    public static bool IsRouteParam(this IProperty property)
    {
        return property.Endpoint.GetRouteParams().Contains(property);
    }
}
