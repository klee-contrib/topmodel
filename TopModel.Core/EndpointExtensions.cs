using TopModel.Utils;

namespace TopModel.Core;

public static class EndpointExtensions
{
    public static IProperty? GetBodyParam(this Endpoint endpoint)
    {
        var bodyParams = endpoint.Params.Where(param => param is CompositionProperty || param is IFieldProperty { Domain.BodyParam: true }).Where(p => p.Domain?.MediaType != "multipart/form-data");
        return bodyParams.Count() > 1
            ? throw new ModelException(endpoint, $"L'endpoint '{endpoint.Name}' doit avoir une seule propriété dans le body. Propriétés trouvées : {string.Join(", ", bodyParams)}")
            : bodyParams.SingleOrDefault();
    }

    public static string GetParamName(this IProperty property)
    {
        if (property is AliasProperty { Property: null })
        {
            return string.Empty;
        }

        return property is not AliasProperty alp || !alp.Property.PrimaryKey
            ? property.NameCamel
            : $"{alp.Property.Class.Trigram?.ToLower() ?? alp.Property.Class.NameCamel}{property.NameCamel.ToFirstUpper()}";
    }

    public static IEnumerable<IProperty> GetQueryParams(this Endpoint endpoint)
    {
        return endpoint.Params.Where(param => !(param is CompositionProperty || param is IFieldProperty { Domain.BodyParam: true })).Except(endpoint.GetRouteParams());
    }

    public static IEnumerable<IProperty> GetRouteParams(this Endpoint endpoint)
    {
        return endpoint.Params.Where(param => endpoint.Route.Contains($"{{{param.GetParamName()}}}"));
    }

    public static bool IsBodyParam(this IProperty property)
    {
        return property.Endpoint.GetBodyParam() == property;
    }

    public static bool IsQueryParam(this IProperty property)
    {
        return property.Endpoint.GetQueryParams().Contains(property);
    }

    public static bool IsRouteParam(this IProperty property)
    {
        return property.Endpoint.GetRouteParams().Contains(property);
    }
}