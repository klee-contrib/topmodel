using System.Text.RegularExpressions;
using TopModel.Core;
using TopModel.Utils;

namespace TopModel.Generator.Core;

internal static class TemplateExtensions
{
    public static string ParseTemplate(this string template, IProperty p, GeneratorConfigBase config, string? tag = null)
    {
        if (string.IsNullOrEmpty(template) || !template.Contains('{'))
        {
            return template;
        }

        var result = template;
        foreach (var t in template.ExtractVariables())
        {
            result = result.Replace(t.Value, ResolveVariable(t.Value.Trim('{', '}'), p, p.DomainParameters, config, tag));
        }

        return result;
    }

    public static string ParseTemplate(this string template, Class c, string[] parameters, GeneratorConfigBase config, string? tag = null)
    {
        if (string.IsNullOrEmpty(template) || !template.Contains('{'))
        {
            return template;
        }

        var result = template;
        foreach (var t in template.ExtractVariables())
        {
            result = result.Replace(t.Value, ResolveVariable(t.Value.Trim('{', '}'), c, parameters, config, tag));
        }

        return result;
    }

    public static string ParseTemplate(this string template, Endpoint e, string[] parameters, GeneratorConfigBase config, string? tag = null)
    {
        if (string.IsNullOrEmpty(template) || !template.Contains('{'))
        {
            return template;
        }

        var result = template;
        foreach (var t in template.ExtractVariables())
        {
            result = result.Replace(t.Value, ResolveVariable(t.Value.Trim('{', '}'), e, parameters, config, tag));
        }

        return result;
    }

    public static string ParseTemplate(this string template, Domain domainFrom, Domain domainTo, GeneratorConfigBase config, string? tag = null)
    {
        if (string.IsNullOrEmpty(template) || !template.Contains('{'))
        {
            return template;
        }

        var result = template;
        foreach (var t in template.ExtractVariables())
        {
            result = result.Replace(t.Value, ResolveVariable(t.Value.Trim('{', '}'), domainFrom, domainTo, config, tag));
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
                        value = value.StringTransform(v => v.ToCamelCase());
                        break;
                    case "constant":
                        value = value.StringTransform(v => v.ToConstantCase());
                        break;
                    case "kebab":
                        value = value.StringTransform(v => v.ToKebabCase());
                        break;
                    case "lower":
                        value = value.StringTransform(v => v.ToLower());
                        break;
                    case "pascal":
                        value = value.StringTransform(v => v.ToPascalCase());
                        break;
                    case "snake":
                        value = value.StringTransform(v => v.ToSnakeCase());
                        break;
                    case "upper":
                        value = value.StringTransform(v => v.ToUpper());
                        break;
                    case "flat":
                        value = Regex.Replace(value, @"[./\\]", string.Empty);
                        break;
                    case "path":
                        value = Regex.Replace(value, @"[./\\]", Path.DirectorySeparatorChar.ToString());
                        break;
                    case "head":
                        value = value.Split('/', '\\', '.').First();
                        break;
                    case "last":
                        value = value.Split('/', '\\', '.').Last();
                        break;
                    case "tail":
                        value = string.Concat(value.SkipWhile(v => v != '/' && v != '\\' && v != '.'))[1..];
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

    private static string ResolveVariable(this string input, Domain domain, GeneratorConfigBase config, string? tag = null)
    {
        return (input.Split(':').First() switch
        {
            "mediaType" => domain.MediaType ?? string.Empty,
            "length" => domain.Length?.ToString() ?? string.Empty,
            "scale" => domain.Scale?.ToString() ?? string.Empty,
            "name" => domain.Name ?? string.Empty,
            "type" => domain.Implementations.GetValueOrDefault(config.Language)?.Type ?? string.Empty,
            var i => config.ResolveVariables(config.ResolveGlobalVariables($@"{{{i}}}").Trim('{', '}'), tag: tag)
        }).Transform(input);
    }

    private static string ResolveVariable(this string input, IPropertyContainer container, string[] parameters, GeneratorConfigBase config, string? tag = null)
    {
        return container switch
        {
            Endpoint e => ResolveVariable(input, e, parameters, config, tag),
            Class c => ResolveVariable(input, c, parameters, config, tag),
            _ => string.Empty,
        };
    }

    private static string ResolveVariable(this string input, IProperty p, string[] parameters, GeneratorConfigBase config, string? tag = null)
    {
        if (input == null || input.Length == 0)
        {
            return string.Empty;
        }

        for (var i = 0; i < parameters.Length; i++)
        {
            input = input.Replace($"${i}", parameters[i]);
        }

        if (input.StartsWith("parent."))
        {
            return ResolveVariable(input["parent.".Length..], p.Parent, parameters, config, tag);
        }

        if (input.StartsWith("class."))
        {
            return ResolveVariable(input["class.".Length..], p.Parent, parameters, config, tag);
        }

        if (input.StartsWith("endpoint."))
        {
            return ResolveVariable(input["endpoint.".Length..], p.Parent, parameters, config, tag);
        }

        if (input.StartsWith("domain."))
        {
            return ResolveVariable(input["domain.".Length..], p.Domain, config, tag);
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
                return ResolveVariable(input["association.".Length..], association, parameters, config, tag);
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
                return ResolveVariable(input["composition.".Length..], composition, parameters, config, tag);
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
            var i => config.ResolveVariables(config.ResolveGlobalVariables($@"{{{i}}}").Trim('{', '}'), module: p.Parent.Namespace.Module, tag: tag)
        }).Transform(input);

        return result;
    }

    private static string ResolveVariable(this string input, Class c, string[] parameters, GeneratorConfigBase config, string? tag = null)
    {
        if (input == null || input.Length == 0)
        {
            return string.Empty;
        }

        for (var i = 0; i < parameters.Length; i++)
        {
            input = input.Replace($"${i}", parameters[i]);
        }

        if (input.StartsWith("primaryKey."))
        {
            if (c.PrimaryKey.FirstOrDefault() == null)
            {
                return string.Empty;
            }

            return ResolveVariable(input["primaryKey.".Length..], c.PrimaryKey.FirstOrDefault()!, parameters, config, tag);
        }

        if (input.StartsWith("extends."))
        {
            if (c.Extends == null)
            {
                return string.Empty;
            }

            return ResolveVariable(input["extends.".Length..], c.Extends, parameters, config, tag);
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

                return ResolveVariable(nextInput, c.Properties[index], parameters, config, tag);
            }
            else
            {
                return string.Empty;
            }
        }

        var result = (input.Split(':').First() switch
        {
            "trigram" => c.Trigram,
            "name" => c.Name,
            "sqlName" => c.SqlName,
            "comment" => c.Comment,
            "label" => c.Label ?? string.Empty,
            "pluralName" => c.PluralName ?? string.Empty,
            "module" => c.Namespace.Module ?? string.Empty,
            var i => config.ResolveVariables(config.ResolveGlobalVariables($@"{{{i}}}").Trim('{', '}'), module: c.Namespace.Module, tag: tag)
        }).Transform(input);

        return result;
    }

    private static string ResolveVariable(this string input, Endpoint e, string[] parameters, GeneratorConfigBase config, string? tag = null)
    {
        if (input == null || input.Length == 0)
        {
            return string.Empty;
        }

        for (var i = 0; i < parameters.Length; i++)
        {
            input = input.Replace($"${i}", parameters[i]);
        }

        if (input.StartsWith("returns."))
        {
            if (e.Returns == null)
            {
                return string.Empty;
            }

            return ResolveVariable(input["returns.".Length..], e.Returns, parameters, config, tag);
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

                return ResolveVariable(nextInput, e.Params[index], parameters, config, tag);
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
            var i => config.ResolveVariables(config.ResolveGlobalVariables($@"{{{i}}}").Trim('{', '}'), module: e.Namespace.Module, tag: tag)
        }).Transform(input);

        return result;
    }

    private static string ResolveVariable(this string input, Domain domainFrom, Domain domainTo, GeneratorConfigBase config, string? tag = null)
    {
        if (input.StartsWith("from."))
        {
            return ResolveVariable(input["from.".Length..], domainFrom, config, tag);
        }
        else
        {
            return ResolveVariable(input["to.".Length..], domainTo, config, tag);
        }
    }

    private static string StringTransform(this string value, Func<string, string> transform)
    {
        return Regex.Replace(value, @"[^./\\]+", match => transform(match.Value));
    }
}