using System.Text.RegularExpressions;
using TopModel.Core;
using TopModel.Utils;

namespace TopModel.Generator.Core;

internal static class TemplateExtensions
{
    public static Func<string, string> GetTransformation(this string input)
    {
        var transform = (string a) => a;
        var value = input;
        if (input.Contains(':'))
        {
            var splitted = input.Split(':');
            value = splitted[0];
            var transformationName = input.Split(':')[1];
            switch (transformationName)
            {
                case "camel":
                    transform = a => a.ToCamelCase();
                    break;
                case "constant":
                    transform = a => a.ToConstantCase();
                    break;
                case "kebab":
                    transform = a => a.ToKebabCase();
                    break;
                case "lower":
                    transform = a => a.ToLower();
                    break;
                case "pascal":
                    transform = a => a.ToPascalCase();
                    break;
                case "snake":
                    transform = a => a.ToSnakeCase();
                    break;
                case "upper":
                    transform = a => a.ToUpper();
                    break;
                default:
                    break;
            }
        }

        return transform;
    }

    public static string ParseTemplate(this string template, IProperty p, GeneratorConfigBase config, string? tag = null)
    {
        if (p is IFieldProperty fp)
        {
            if (string.IsNullOrEmpty(template) || !template.Contains('{'))
            {
                return template;
            }

            var result = template;
            foreach (var t in template.ExtractVariables())
            {
                result = result.Replace(t.Value, ResolveVariable(t.Value.Trim('{', '}'), fp, fp.DomainParameters, config, tag));
            }

            return result;
        }
        else if (p is CompositionProperty cp)
        {
            if (string.IsNullOrEmpty(template) || !template.Contains('{'))
            {
                return template;
            }

            var result = template;
            foreach (var t in template.ExtractVariables())
            {
                result = result.Replace(t.Value, ResolveVariable(t.Value.Trim('{', '}'), cp, cp.DomainParameters, config, tag));
            }

            return result;
        }

        return template;
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

    private static IEnumerable<Match> ExtractVariables(this string input)
    {
        var regex = new Regex(@"(\{[$a-zA-Z0-9:.\[\]]+\})");
        return regex.Matches(input).Cast<Match>();
    }

    private static string ResolveVariable(this string input, Domain domain, GeneratorConfigBase config, string? tag = null)
    {
        var transform = input.GetTransformation();
        var variable = input.Split(':').First();

        return input.Split(':').First() switch
        {
            "mediaType" => transform(domain.MediaType ?? string.Empty),
            "length" => transform(domain.Length?.ToString() ?? string.Empty),
            "scale" => transform(domain.Scale?.ToString() ?? string.Empty),
            "name" => transform(domain.Name ?? string.Empty),
            "type" => transform(domain.Implementations.GetValueOrDefault(config.Language)?.Type ?? string.Empty),
            var i => config.ResolveVariables(config.ResolveGlobalVariables($@"{{{i}}}").Trim('{', '}'), tag: tag)
        };
    }

    private static string ResolveVariable(this string input, IProperty c, string[] parameters, GeneratorConfigBase config, string? tag = null)
    {
        switch (c)
        {
            case IFieldProperty fp: return ResolveVariable(input, fp, parameters, config, tag);
            case CompositionProperty cp: return ResolveVariable(input, cp, parameters, config, tag);
            default: return string.Empty;
        }
    }

    private static string ResolveVariable(this string input, IPropertyContainer container, string[] parameters, GeneratorConfigBase config, string? tag = null)
    {
        switch (container)
        {
            case Endpoint e:
                return ResolveVariable(input, e, parameters, config, tag);
            case Class c: return ResolveVariable(input, c, parameters, config, tag);
            default: return string.Empty;
        }
    }

    private static string ResolveVariable(this string input, IFieldProperty fp, string[] parameters, GeneratorConfigBase config, string? tag = null)
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
            return ResolveVariable(input["parent.".Length..], fp.Parent, parameters, config, tag);
        }

        if (input.StartsWith("class."))
        {
            return ResolveVariable(input["class.".Length..], fp.Parent, parameters, config, tag);
        }

        if (input.StartsWith("endpoint."))
        {
            return ResolveVariable(input["endpoint.".Length..], fp.Parent, parameters, config, tag);
        }

        if (input.StartsWith("domain."))
        {
            return ResolveVariable(input["domain.".Length..], fp.Domain, config, tag);
        }

        if (input.StartsWith("association.") && (fp is AssociationProperty ap || fp is AliasProperty alp && alp.Property is AssociationProperty asp))
        {
            var asso = fp switch
            {
                AssociationProperty assop => assop.Association,
                AliasProperty { Property: AssociationProperty alip } => alip.Association,
                _ => null // impossible
            };
            return ResolveVariable(input["association.".Length..], asso!, parameters, config, tag);
        }

        var transform = input.GetTransformation();
        var result = input.Split(':').First() switch
        {
            "name" => transform(fp.Name ?? string.Empty),
            "sqlName" => transform(fp.SqlName ?? string.Empty),
            "trigram" => transform(fp.Trigram ?? fp.Class?.Trigram ?? string.Empty),
            "label" => transform(fp.Label ?? string.Empty),
            "comment" => transform(fp.Comment),
            "required" => transform(fp.Required.ToString().ToLower()),
            "resourceKey" => transform(fp.ResourceKey.ToString()),
            "commentResourceKey" => transform(fp.CommentResourceKey.ToString()),
            "defaultValue" => transform(fp.DefaultValue?.ToString() ?? string.Empty),
            var i => config.ResolveVariables(config.ResolveGlobalVariables($@"{{{i}}}").Trim('{', '}'), module: fp.Parent.Namespace.Module, tag: tag)
        };

        return result;
    }

    private static string ResolveVariable(this string input, CompositionProperty cp, string[] parameters, GeneratorConfigBase config, string? tag = null)
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
            return ResolveVariable(input["parent.".Length..], cp.Class, parameters, config, tag);
        }

        if (input.StartsWith("class."))
        {
            return ResolveVariable(input["class.".Length..], cp.Class, parameters, config, tag);
        }

        if (input.StartsWith("endpoint."))
        {
            return ResolveVariable(input["endpoint.".Length..], cp.Class, parameters, config, tag);
        }

        if (input.StartsWith("composition."))
        {
            return ResolveVariable(input["composition.".Length..], cp.Composition, parameters, config, tag);
        }

        var transform = input.GetTransformation();
        var result = input.Split(':').First() switch
        {
            "name" => transform(cp.Name ?? string.Empty),
            "label" => transform(cp.Label ?? string.Empty),
            "comment" => transform(cp.Comment),
            var i => config.ResolveVariables(config.ResolveGlobalVariables($@"{{{i}}}").Trim('{', '}'), module: cp.Class.Namespace.Module, tag: tag)
        };

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

        if (input.StartsWith("properties["))
        {
            var indexSize = input["properties[".Length..].IndexOf("]");
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

        var transform = input.GetTransformation();
        var result = input.Split(':').First() switch
        {
            "trigram" => transform(c.Trigram),
            "name" => transform(c.Name),
            "sqlName" => transform(c.SqlName),
            "comment" => transform(c.Comment),
            "label" => transform(c.Label ?? string.Empty),
            "pluralName" => transform(c.PluralName ?? string.Empty),
            "module" => transform(c.Namespace.Module ?? string.Empty),
            var i => config.ResolveVariables(config.ResolveGlobalVariables($@"{{{i}}}").Trim('{', '}'), module: c.Namespace.Module, tag: tag)
        };

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

        if (input.StartsWith("params["))
        {
            var indexSize = input["params[".Length..].IndexOf("]");
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

        var transform = input.GetTransformation();
        var result = input.Split(':').First() switch
        {
            "name" => transform(e.Name),
            "method" => transform(e.Method),
            "route" => transform(e.Route),
            "description" => transform(e.Description),
            "module" => transform(e.Namespace.Module ?? string.Empty),
            var i => config.ResolveVariables(config.ResolveGlobalVariables($@"{{{i}}}").Trim('{', '}'), module: e.Namespace.Module, tag: tag)
        };

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
}