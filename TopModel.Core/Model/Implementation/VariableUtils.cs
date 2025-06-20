namespace TopModel.Core.Model.Implementation;

internal static class VariableUtils
{
    internal static readonly string[] ClassProperties = ["trigram", "name", "sqlName", "comment", "label", "pluralName", "module"];
    internal static readonly string[] DomainProperties = ["mediaType", "length", "scale", "name", "type"];
    internal static readonly string[] EndpointProperties = ["name", "method", "route", "description", "module"];
    internal static readonly string[] PropertyProperties = ["T", "value", "name", "sqlName", "paramName", "trigram", "label", "comment", "required", "resourceKey", "commentResourceKey", "defaultValue"];

    public static bool IsValidClassVariable(this string input, IList<TemplateParameter> templateParameters)
    {
        if (input.StartsWith("primaryKey."))
        {
            return input["primaryKey.".Length..].IsValidPropertyVariable(templateParameters);
        }

        if (input.StartsWith("extends."))
        {
            return input["extends.".Length..].IsValidClassVariable(templateParameters);
        }

        if (input.StartsWith($"customProperties."))
        {
            return true;
        }

        if (input.StartsWith("properties["))
        {
            return input[(input.IndexOf(']') + 2)..].IsValidPropertyVariable(templateParameters);
        }

        if (templateParameters.Any(tp => tp.Name == input))
        {
            return true;
        }

        return ClassProperties.Contains(input);
    }

    public static bool IsValidConverterVariable(this string input)
    {
        if (input.StartsWith("from."))
        {
            return input["from.".Length..].IsValidDomainVariable();
        }
        else if (input.StartsWith("to."))
        {
            return input["to.".Length..].IsValidDomainVariable();
        }

        return input == "value";
    }

    public static bool IsValidDomainVariable(this string input)
    {
        return DomainProperties.Contains(input);
    }

    public static bool IsValidEndpointVariable(this string input, IList<TemplateParameter> templateParameters)
    {
        if (input.StartsWith("returns."))
        {
            return input["returns.".Length..].IsValidClassVariable(templateParameters);
        }

        if (input.StartsWith("customProperties."))
        {
            return true;
        }

        if (input.StartsWith("params["))
        {
            return input[(input.IndexOf(']') + 2)..].IsValidPropertyVariable(templateParameters);
        }

        if (templateParameters.Any(tp => tp.Name == input))
        {
            return true;
        }

        return EndpointProperties.Contains(input);
    }

    public static bool IsValidPropertyVariable(this string input, IList<TemplateParameter> templateParameters)
    {
        if (input.StartsWith("parent."))
        {
            return input["parent.".Length..].IsValidClassVariable(templateParameters) || input["parent.".Length..].IsValidEndpointVariable(templateParameters);
        }

        if (input.StartsWith("class."))
        {
            return input["class.".Length..].IsValidClassVariable(templateParameters);
        }

        if (input.StartsWith("endpoint."))
        {
            return input["endpoint.".Length..].IsValidEndpointVariable(templateParameters);
        }

        if (input.StartsWith("domain."))
        {
            return input["domain.".Length..].IsValidDomainVariable();
        }

        if (input.StartsWith($"customProperties."))
        {
            return true;
        }

        if (input.StartsWith("association."))
        {
            return input["association.".Length..].IsValidClassVariable(templateParameters);
        }

        if (input.StartsWith("composition."))
        {
            return input["composition.".Length..].IsValidClassVariable(templateParameters);
        }

        if (templateParameters.Any(tp => tp.Name == input))
        {
            return true;
        }

        return PropertyProperties.Contains(input);
    }
}
