using System;
using System.Collections.Generic;
using System.Linq;

namespace TopModel.Generator
{
    public static class ApiGeneratorExtensions
    {
        public static IProperty? GetBodyParam(this Endpoint endpoint)
        {
            var bodyParams = endpoint.Params.Where(param => param is CompositionProperty);
            return bodyParams.Count() > 1
                ? throw new Exception($"L'endpoint '{endpoint.Name}' doit avoir une seule propriété dans le body. Propriétés trouvées : {string.Join(", ", bodyParams)}")
                : bodyParams.SingleOrDefault();
        }

        public static IEnumerable<IProperty> GetQueryParams(this Endpoint endpoint)
        {
            return endpoint.Params.Where(param => param != endpoint.GetBodyParam()).Except(endpoint.GetRouteParams());
        }

        public static IEnumerable<IProperty> GetRouteParams(this Endpoint endpoint)
        {
            return endpoint.Params.Where(param => endpoint.Route.Contains($"{{{param.GetParamName()}}}"));
        }

        public static string GetParamName(this IProperty property)
        {
            return !(property is AliasProperty alp) || !alp.Property.PrimaryKey
                ? property.Name
                : $"{alp.Property.Class.Trigram?.ToLower() ?? alp.Property.Class.Name.ToFirstLower()}{property.Name}";
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
}
