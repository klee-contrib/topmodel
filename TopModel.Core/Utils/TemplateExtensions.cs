using System.Text.RegularExpressions;
using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Core.Utils;

public static class TemplateExtensions
{
    public static string ParseTemplate(this string template, IProperty p, WatcherConfigBase config, string? tag = null)
    {
        return template.ParseTemplate(p, p.Domain?.TemplateParameters ?? [], p.DomainParameters, config, tag);
    }

    public static string ParseTemplate(this string template, IProperty p, IList<TemplateParameter> templateParameters, IDictionary<string, string> parameterValues, WatcherConfigBase config, string? tag = null)
    {
        if (string.IsNullOrEmpty(template) || !template.Contains('{'))
        {
            return template;
        }

        string result = template;
        foreach (var t in template.ExtractVariables())
        {
            result = result.Replace(t.Value, t.Value.Trim('{', '}').ResolveVariable(p, templateParameters, parameterValues, config, tag));
        }

        return result;
    }

    public static string ParseTemplate(this string template, IPropertyContainer c, IList<TemplateParameter> templateParameters, IDictionary<string, string> parameterValues, WatcherConfigBase config, string? tag = null)
    {
        if (string.IsNullOrEmpty(template) || !template.Contains('{'))
        {
            return template;
        }

        string result = template;
        foreach (var t in template.ExtractVariables())
        {
            result = result.Replace(t.Value, t.Value.Trim('{', '}').ResolveVariable(c, templateParameters, parameterValues, config, tag));
        }

        return result;
    }

    public static string ParseTemplate(this string template, Domain domainFrom, Domain domainTo, WatcherConfigBase config, string? tag = null)
    {
        if (string.IsNullOrEmpty(template) || !template.Contains('{'))
        {
            return template;
        }

        var result = template;
        foreach (var t in template.ExtractVariables())
        {
            result = result.Replace(t.Value, t.Value.Trim('{', '}').ResolveVariable(domainFrom, domainTo, config, tag));
        }

        return result;
    }

    public static string Transform(this string value, string input)
    {
        if (input.Contains(':'))
        {
            foreach (var transformName in input.Split(':').Skip(1))
            {
                switch (transformName)
                {
                    case "camel":
                        value = value.ApplyTransform(v => v.ToCamelCase());
                        break;
                    case "constant":
                        value = value.ApplyTransform(v => v.ToConstantCase());
                        break;
                    case "kebab":
                        value = value.ApplyTransform(v => v.ToKebabCase());
                        break;
                    case "lower":
                        value = value.ApplyTransform(v => v.ToLower());
                        break;
                    case "pascal":
                        value = value.ApplyTransform(v => v.ToPascalCase());
                        break;
                    case "snake":
                        value = value.ApplyTransform(v => v.ToSnakeCase());
                        break;
                    case "upper":
                        value = value.ApplyTransform(v => v.ToUpper());
                        break;
                    case "flat":
                        value = value.ToFlat();
                        break;
                    case "path":
                        value = value.ToPath();
                        break;
                    case "head":
                        value = value.Split('/', '\\', '.').First();
                        break;
                    case "last":
                        value = value.Split('/', '\\', '.').Last();
                        break;
                    case "tail":
                        value = value.Any(c => c == '/' || c == '\\' || c == '.')
                            ? string.Concat(value.SkipWhile(v => v != '/' && v != '\\' && v != '.'))[1..]
                            : string.Empty;
                        break;
                    default:
                        break;
                }
            }
        }

        return value;
    }

    private static IEnumerable<Match> ExtractVariables(this string input)
    {
        var regex = new Regex(@"(\{[$a-zA-Z0-9:.\[\]]+\})");
        return regex.Matches(input).Cast<Match>();
    }

    private static string ResolveCustomProperty(string input, Dictionary<string, string> customProperties)
    {
        var propertyName = input.Split(':').First();
        if (customProperties.TryGetValue(propertyName, out var value))
        {
            return value.Transform(input);
        }

        return string.Empty;
    }

    private static string ResolveVariable(this string input, Domain domain, WatcherConfigBase config, string? tag = null)
    {
        return (input.Split(':').First() switch
        {
            "mediaType" => domain.MediaType ?? string.Empty,
            "length" => domain.Length?.ToString() ?? string.Empty,
            "scale" => domain.Scale?.ToString() ?? string.Empty,
            "name" => domain.Name ?? string.Empty,
            "type" => config.GetImplementation(domain)?.Type ?? string.Empty,
            var i => config.ResolveVariables(config.ResolveGlobalVariables($@"{{{i}}}"), tag: tag)
        }).Transform(input);
    }

    private static string ResolveVariable(this string input, IPropertyContainer container, IList<TemplateParameter> templateParameters, IDictionary<string, string> parameterValues, WatcherConfigBase config, string? tag = null)
    {
        return container switch
        {
            Endpoint e => input.ResolveVariable(e, templateParameters, parameterValues, config, tag),
            Class c => input.ResolveVariable(c, templateParameters, parameterValues, config, tag),
            _ => string.Empty,
        };
    }

    private static string ResolveVariable(this string input, IProperty p, IList<TemplateParameter> templateParameters, IDictionary<string, string> parameterValues, WatcherConfigBase config, string? tag = null)
    {
        if (input == null || input.Length == 0)
        {
            return string.Empty;
        }

        if (input.StartsWith("parent."))
        {
            return input["parent.".Length..].ResolveVariable(p.Parent, templateParameters, parameterValues, config, tag);
        }

        if (input.StartsWith("class."))
        {
            return input["class.".Length..].ResolveVariable(p.Parent, templateParameters, parameterValues, config, tag);
        }

        if (input.StartsWith("endpoint."))
        {
            return input["endpoint.".Length..].ResolveVariable(p.Parent, templateParameters, parameterValues, config, tag);
        }

        if (input.StartsWith("domain."))
        {
            return input["domain.".Length..].ResolveVariable(p.Domain, config, tag);
        }

        if (input.StartsWith($"customProperties."))
        {
            return ResolveCustomProperty(input["customProperties.".Length..], p.CustomProperties);
        }

        if (input.StartsWith("association."))
        {
            var association = p switch
            {
                AssociationProperty ap => ap.Association,
                AliasProperty { Property: AssociationProperty ap } => ap.Association,
                _ => null // impossible
            };

            if (association != null)
            {
                return input["association.".Length..].ResolveVariable(association, templateParameters, parameterValues, config, tag);
            }
        }

        if (input.StartsWith("composition."))
        {
            var composition = p switch
            {
                CompositionProperty cp => cp.Composition,
                AliasProperty { Property: CompositionProperty cp } => cp.Composition,
                _ => null // impossible
            };

            if (composition != null)
            {
                return input["composition.".Length..].ResolveVariable(composition, templateParameters, parameterValues, config, tag);
            }
        }

        var result = (input.Split(':').First() switch
        {
            "name" => p.Name ?? string.Empty,
            "sqlName" => p.SqlName ?? string.Empty,
            "paramName" => p.GetParamName().ToString(),
            "trigram" => p.Trigram ?? p.Class?.Trigram ?? string.Empty,
            "label" => p.Label ?? string.Empty,
            "comment" => p.Comment,
            "required" => p.Required.ToString().ToLower(),
            "resourceKey" => p.ResourceKey.ToString(),
            "commentResourceKey" => p.CommentResourceKey.ToString(),
            "defaultValue" => p.DefaultValue?.ToString() ?? string.Empty,
            var i => i.TryResolveParameters(templateParameters, parameterValues) ?? config.ResolveVariables(config.ResolveGlobalVariables($@"{{{i}}}"), module: p.Parent.Namespace.Module, tag: tag)
        }).Transform(input);

        return result;
    }

    private static string ResolveVariable(this string input, Class c, IList<TemplateParameter> templateParameters, IDictionary<string, string> parameterValues, WatcherConfigBase config, string? tag = null)
    {
        if (input == null || input.Length == 0)
        {
            return string.Empty;
        }

        if (input.StartsWith("primaryKey."))
        {
            if (c.PrimaryKey.FirstOrDefault() == null)
            {
                return string.Empty;
            }

            return input["primaryKey.".Length..].ResolveVariable(c.PrimaryKey.FirstOrDefault()!, templateParameters, parameterValues, config, tag);
        }

        if (input.StartsWith("extends."))
        {
            if (c.Extends == null)
            {
                return string.Empty;
            }

            return input["extends.".Length..].ResolveVariable(c.Extends, templateParameters, parameterValues, config, tag);
        }

        if (input.StartsWith($"customProperties."))
        {
            return ResolveCustomProperty(input["customProperties.".Length..], c.CustomProperties);
        }

        if (input.StartsWith("properties["))
        {
            var indexSize = input["properties[".Length..].IndexOf(']');
            var indexString = input.Split("properties[")[1].Split("]")[0];
            if (int.TryParse(indexString, out var index))
            {
                var nextInput = input[("properties[].".Length + indexSize)..];
                if (c.Properties.Count < index)
                {
                    return string.Empty;
                }

                return nextInput.ResolveVariable(c.Properties[index], templateParameters, parameterValues, config, tag);
            }
            else
            {
                return string.Empty;
            }
        }

        var result = (input.Split(':').First() switch
        {
            "trigram" => c.Trigram ?? string.Empty,
            "name" => c.Name,
            "sqlName" => c.SqlName,
            "comment" => c.Comment,
            "label" => c.Label ?? string.Empty,
            "pluralName" => c.PluralName ?? string.Empty,
            "module" => c.Namespace.Module ?? string.Empty,
            var i => i.TryResolveParameters(templateParameters, parameterValues) ?? config.ResolveVariables(config.ResolveGlobalVariables($@"{{{i}}}"), module: c.Namespace.Module, tag: tag)
        }).Transform(input);

        return result;
    }

    private static string ResolveVariable(this string input, Endpoint e, IList<TemplateParameter> templateParameters, IDictionary<string, string> parameterValues, WatcherConfigBase config, string? tag = null)
    {
        if (input == null || input.Length == 0)
        {
            return string.Empty;
        }

        if (input.StartsWith("returns."))
        {
            if (e.Returns == null)
            {
                return string.Empty;
            }

            return input["returns.".Length..].ResolveVariable(e.Returns, templateParameters, parameterValues, config, tag);
        }

        if (input.StartsWith($"customProperties."))
        {
            return ResolveCustomProperty(input["customProperties.".Length..], e.CustomProperties);
        }

        if (input.StartsWith("params["))
        {
            var indexSize = input["params[".Length..].IndexOf(']');
            var indexString = input.Split("params[")[1].Split("]")[0];
            if (int.TryParse(indexString, out var index))
            {
                var nextInput = input[("params[].".Length + indexSize)..];
                if (e.Params.Count < index)
                {
                    return string.Empty;
                }

                return nextInput.ResolveVariable(e.Params[index], templateParameters, parameterValues, config, tag);
            }
            else
            {
                return string.Empty;
            }
        }

        var result = (input.Split(':').First() switch
        {
            "name" => e.Name,
            "method" => e.Method,
            "route" => e.Route,
            "description" => e.Description,
            "module" => e.Namespace.Module ?? string.Empty,
            var i => i.TryResolveParameters(templateParameters, parameterValues) ?? config.ResolveVariables(config.ResolveGlobalVariables($@"{{{i}}}"), module: e.Namespace.Module, tag: tag)
        }).Transform(input);

        return result;
    }

    private static string ResolveVariable(this string input, Domain domainFrom, Domain domainTo, WatcherConfigBase config, string? tag = null)
    {
        if (input.StartsWith("from."))
        {
            return input["from.".Length..].ResolveVariable(domainFrom, config, tag);
        }
        else
        {
            return input["to.".Length..].ResolveVariable(domainTo, config, tag);
        }
    }

    private static string? TryResolveParameters(this string input, IList<TemplateParameter> templateParameters, IDictionary<string, string> parameterValues)
    {
        foreach (var parameter in templateParameters)
        {
            if (input == parameter.Name)
            {
                return parameterValues.TryGetValue(parameter.Name, out var value) ? value : parameter.DefaultValue;
            }
        }

        return null;
    }
}