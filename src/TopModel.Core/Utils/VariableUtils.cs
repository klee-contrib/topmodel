using System.Diagnostics.CodeAnalysis;
using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Core.Utils;

internal static class VariableUtils
{
    internal static readonly Dictionary<string, VariableMessage> ClassProperties = new()
    {
        ["trigram"] = VariableMessage.ClassTrigram,
        ["name"] = VariableMessage.ClassName,
        ["sqlName"] = VariableMessage.ClassSqlName,
        ["comment"] = VariableMessage.ClassComment,
        ["label"] = VariableMessage.ClassLabel,
        ["pluralName"] = VariableMessage.ClassPluralName,
        ["module"] = VariableMessage.ClassModule,
    };

    internal static readonly Dictionary<string, VariableMessage> DomainProperties = new()
    {
        ["mediaType"] = VariableMessage.DomainMediaType,
        ["length"] = VariableMessage.DomainLength,
        ["scale"] = VariableMessage.DomainScale,
        ["name"] = VariableMessage.DomainName,
        ["type"] = VariableMessage.DomainType,
    };

    internal static readonly Dictionary<string, VariableMessage> EndpointProperties = new()
    {
        ["name"] = VariableMessage.EndpointName,
        ["method"] = VariableMessage.EndpointMethod,
        ["route"] = VariableMessage.EndpointRoute,
        ["description"] = VariableMessage.EndpointDescription,
        ["module"] = VariableMessage.EndpointModule,
    };

    internal static readonly Dictionary<string, VariableMessage> PropertyProperties = new()
    {
        ["T"] = VariableMessage.PropertyGenericType,
        ["value"] = VariableMessage.PropertyValue,
        ["name"] = VariableMessage.PropertyName,
        ["sqlName"] = VariableMessage.PropertySqlName,
        ["paramName"] = VariableMessage.PropertyParamName,
        ["trigram"] = VariableMessage.PropertyTrigram,
        ["label"] = VariableMessage.PropertyLabel,
        ["comment"] = VariableMessage.PropertyComment,
        ["required"] = VariableMessage.PropertyRequired,
        ["resourceKey"] = VariableMessage.PropertyResourceKey,
        ["commentResourceKey"] = VariableMessage.PropertyCommentResourceKey,
        ["defaultValue"] = VariableMessage.PropertyDefaultValue,
    };

    internal static readonly string[] Transforms =
    [
        "camel",
        "constant",
        "kebab",
        "lower",
        "pascal",
        "snake",
        "upper",
        "flat",
        "path",
        "head",
        "last",
        "tail",
    ];

    extension(string input)
    {
        public bool IsValidTransform => Transforms.Contains(input);

        public bool TryGetClassVariable(
            ModelConfig config,
            IList<TemplateParameter> templateParameters,
            [MaybeNullWhen(false)] out Variable variable
        )
        {
            if (input.StartsWith("primaryKey."))
            {
                return input["primaryKey.".Length..].TryGetPropertyVariable(config, templateParameters, out variable);
            }

            if (input.StartsWith("extends."))
            {
                return input["extends.".Length..].TryGetClassVariable(config, templateParameters, out variable);
            }

            if (input.StartsWith("customProperties."))
            {
                variable = new Variable { Description = VariableMessage.CustomProperty.GetMessage() };
                return true;
            }

            if (input.StartsWith("properties["))
            {
                return input[(input.IndexOf(']') + 2)..]
                    .TryGetPropertyVariable(config, templateParameters, out variable);
            }

            if (templateParameters.Any(tp => tp.Name == input))
            {
                variable = new Variable { TemplateParameter = templateParameters.First(tp => tp.Name == input) };
                return true;
            }

            if (ClassProperties.TryGetValue(input, out var description))
            {
                variable = new Variable { Description = description.GetMessage() };
                return true;
            }

            if (config.GlobalVariables.TryGetValue(input, out var gVariable))
            {
                variable = gVariable;
                return true;
            }

            if (config.TagVariables.TryGetValue(input, out var tVariable))
            {
                variable = tVariable;
                return true;
            }

            variable = null;
            return false;
        }

        public bool TryGetConverterVariable([MaybeNullWhen(false)] out Variable variable)
        {
            if (input.StartsWith("from."))
            {
                return input["from.".Length..].TryGetDomainVariable(out variable);
            }
            else if (input.StartsWith("to."))
            {
                return input["to.".Length..].TryGetDomainVariable(out variable);
            }

            if (input == "value")
            {
                variable = new Variable { Description = VariableMessage.PropertyValueToConvert.GetMessage() };
                return true;
            }

            variable = null;
            return false;
        }

        public bool TryGetDomainVariable([MaybeNullWhen(false)] out Variable variable)
        {
            if (DomainProperties.TryGetValue(input, out var description))
            {
                variable = new Variable { Description = description.GetMessage() };
                return true;
            }

            variable = null;
            return false;
        }

        public bool TryGetEndpointVariable(
            ModelConfig config,
            IList<TemplateParameter> templateParameters,
            [MaybeNullWhen(false)] out Variable variable
        )
        {
            if (input.StartsWith("returns."))
            {
                return input["returns.".Length..].TryGetClassVariable(config, templateParameters, out variable);
            }

            if (input.StartsWith("customProperties."))
            {
                variable = new Variable { Description = VariableMessage.CustomProperty.GetMessage() };
                return true;
            }

            if (input.StartsWith("params["))
            {
                return input[(input.IndexOf(']') + 2)..]
                    .TryGetPropertyVariable(config, templateParameters, out variable);
            }

            if (templateParameters.Any(tp => tp.Name == input))
            {
                variable = new Variable { TemplateParameter = templateParameters.First(tp => tp.Name == input) };
                return true;
            }

            if (EndpointProperties.TryGetValue(input, out var description))
            {
                variable = new Variable { Description = description.GetMessage() };
                return true;
            }

            if (config.GlobalVariables.TryGetValue(input, out var gVariable))
            {
                variable = gVariable;
                return true;
            }

            if (config.TagVariables.TryGetValue(input, out var tVariable))
            {
                variable = tVariable;
                return true;
            }

            variable = null;
            return false;
        }

        public bool TryGetPropertyVariable(
            ModelConfig config,
            IList<TemplateParameter> templateParameters,
            [MaybeNullWhen(false)] out Variable variable
        )
        {
            if (input.StartsWith("parent."))
            {
                return input["parent.".Length..].TryGetClassVariable(config, templateParameters, out variable)
                    || input["parent.".Length..].TryGetEndpointVariable(config, templateParameters, out variable);
            }

            if (input.StartsWith("class."))
            {
                return input["class.".Length..].TryGetClassVariable(config, templateParameters, out variable);
            }

            if (input.StartsWith("endpoint."))
            {
                return input["endpoint.".Length..].TryGetEndpointVariable(config, templateParameters, out variable);
            }

            if (input.StartsWith("domain."))
            {
                return input["domain.".Length..].TryGetDomainVariable(out variable);
            }

            if (input.StartsWith("customProperties."))
            {
                variable = new Variable { Description = VariableMessage.CustomProperty.GetMessage() };
                return true;
            }

            if (input.StartsWith("association."))
            {
                return input["association.".Length..].TryGetClassVariable(config, templateParameters, out variable);
            }

            if (input.StartsWith("composition."))
            {
                return input["composition.".Length..].TryGetClassVariable(config, templateParameters, out variable);
            }

            if (templateParameters.Any(tp => tp.Name == input))
            {
                variable = new Variable { TemplateParameter = templateParameters.First(tp => tp.Name == input) };
                return true;
            }

            if (PropertyProperties.TryGetValue(input, out var description))
            {
                variable = new Variable { Description = description.GetMessage() };
                return true;
            }

            if (config.GlobalVariables.TryGetValue(input, out var gVariable))
            {
                variable = gVariable;
                return true;
            }

            if (config.TagVariables.TryGetValue(input, out var tVariable))
            {
                variable = tVariable;
                return true;
            }

            variable = null;
            return false;
        }
    }
}
