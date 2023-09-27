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

    public static string ParseTemplate(this string template, IProperty p)
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
                result = result.Replace(t.Value, ResolveVariable(t.Value.Trim('{', '}'), fp, fp.DomainParameters));
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
                result = result.Replace(t.Value, ResolveVariable(t.Value.Trim('{', '}'), cp, cp.DomainParameters));
            }

            return result;
        }

        return template;
    }

    public static string ParseTemplate(this string template, Class c, string[] parameters)
    {
        if (string.IsNullOrEmpty(template) || !template.Contains('{'))
        {
            return template;
        }

        var result = template;
        foreach (var t in template.ExtractVariables())
        {
            result = result.Replace(t.Value, ResolveVariable(t.Value.Trim('{', '}'), c, parameters));
        }

        return result;
    }

    public static string ParseTemplate(this string template, Endpoint e, string[] parameters)
    {
        if (string.IsNullOrEmpty(template) || !template.Contains('{'))
        {
            return template;
        }

        var result = template;
        foreach (var t in template.ExtractVariables())
        {
            result = result.Replace(t.Value, ResolveVariable(t.Value.Trim('{', '}'), e, parameters));
        }

        return result;
    }

    public static string ParseTemplate(this string template, Domain domainFrom, Domain domainTo, string targetLanguage)
    {
        if (string.IsNullOrEmpty(template) || !template.Contains('{'))
        {
            return template;
        }

        var result = template;
        foreach (var t in template.ExtractVariables())
        {
            result = result.Replace(t.Value, ResolveVariable(t.Value.Trim('{', '}'), domainFrom, domainTo, targetLanguage));
        }

        return result;
    }

    private static IEnumerable<Match> ExtractVariables(this string input)
    {
        var regex = new Regex(@"(\{[$a-zA-Z0-9:.\[\]]+\})");
        return regex.Matches(input).Cast<Match>();
    }

    private static string ResolveVariable(this string input, IPropertyContainer container, string[] parameters)
    {
        switch (container)
        {
            case Endpoint e:
                return ResolveVariable(input, e, parameters);
            case Class c: return ResolveVariable(input, c, parameters);
            default: return string.Empty;
        }
    }

    private static string ResolveVariable(this string input, IFieldProperty rp, string[] parameters)
    {
        if (input == null || input.Length == 0)
        {
            return string.Empty;
        }

        if (input.StartsWith("parent."))
        {
            return ResolveVariable(input["parent.".Length..], rp.Parent, parameters);
        }

        if (input.StartsWith("association.") && rp is AssociationProperty ap)
        {
            return ResolveVariable(input["association.".Length..], ap.Association, parameters);
        }

        var transform = input.GetTransformation();
        var result = input.Split(':').First() switch
        {
            "name" => transform(rp.Name ?? string.Empty),
            "sqlName" => transform(rp.SqlName ?? string.Empty),
            "trigram" => transform(rp.Trigram ?? rp.Class?.Trigram ?? string.Empty),
            "label" => transform(rp.Label ?? string.Empty),
            "comment" => transform(rp.Comment),
            "required" => transform(rp.Required.ToString().ToLower()),
            "resourceKey" => transform(rp.ResourceKey.ToString()),
            "commentResourceKey" => transform(rp.CommentResourceKey.ToString()),
            "defaultValue" => transform(rp.DefaultValue?.ToString() ?? string.Empty),
            var i => i
        };

        for (var i = 0; i < parameters.Length; i++)
        {
            result = result.Replace($"${i}", parameters[i]);
        }

        return result;
    }

    private static string ResolveVariable(this string input, CompositionProperty cp, string[] parameters)
    {
        if (input == null || input.Length == 0)
        {
            return string.Empty;
        }

        if (input.StartsWith("parent."))
        {
            return ResolveVariable(input["parent.".Length..], cp.Class, parameters);
        }

        if (input.StartsWith("composition."))
        {
            return ResolveVariable(input["composition.".Length..], cp.Composition, parameters);
        }

        var transform = input.GetTransformation();
        var result = input.Split(':').First() switch
        {
            "name" => transform(cp.Name ?? string.Empty),
            "label" => transform(cp.Label ?? string.Empty),
            "comment" => transform(cp.Comment),
            var i => i
        };

        for (var i = 0; i < parameters.Length; i++)
        {
            result = result.Replace($"${i}", parameters[i]);
        }

        return result;
    }

    private static string ResolveVariable(this string input, IProperty c, string[] parameters)
    {
        switch (c)
        {
            case IFieldProperty fp: return ResolveVariable(input, fp, parameters);
            case CompositionProperty cp: return ResolveVariable(input, cp, parameters);
            default: return string.Empty;
        }
    }

    private static string ResolveVariable(this string input, Class c, string[] parameters)
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

            return ResolveVariable(input["primaryKey.".Length..], c.PrimaryKey.FirstOrDefault()!, parameters);
        }

        if (input.StartsWith("properties["))
        {
            var indexSize = input["properties[".Length..].IndexOf("]");
            var index = int.Parse(input.Split("properties[")[1].Split("]")[0]);
            var nextInput = input[("properties[].".Length + indexSize)..];
            if (c.Properties.Count < index)
            {
                return string.Empty;
            }

            return ResolveVariable(nextInput, c.Properties[index], parameters);
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
            var i => i
        };

        for (var i = 0; i < parameters.Length; i++)
        {
            result = result.Replace($"${i}", parameters[i]);
        }

        return result;
    }

    private static string ResolveVariable(this string input, Endpoint e, string[] parameters)
    {
        if (input.StartsWith("returns."))
        {
            if (e.Returns == null)
            {
                return string.Empty;
            }

            return ResolveVariable(input["returns.".Length..], e.Returns, parameters);
        }

        if (input.StartsWith("params["))
        {
            var indexSize = input["params[".Length..].IndexOf("]");
            var index = int.Parse(input.Split("params[")[1].Split("]")[0]);
            var nextInput = input[("params[].".Length + indexSize)..];
            if (e.Params.Count < index)
            {
                return string.Empty;
            }

            return ResolveVariable(nextInput, e.Params[index], parameters);
        }

        var transform = input.GetTransformation();
        var result = input.Split(':').First() switch
        {
            "name" => transform(e.Name),
            "method" => transform(e.Method),
            "route" => transform(e.Route),
            "description" => transform(e.Description),
            "module" => transform(e.Namespace.Module ?? string.Empty),
            var i => i
        };

        for (var i = 0; i < parameters.Length; i++)
        {
            result = result.Replace($"${i}", parameters[i]);
        }

        return result;
    }

    private static string ResolveVariable(this string input, Domain domain, string targetLanguage)
    {
        var transform = input.GetTransformation();
        var variable = input.Split(':').First();

        return input.Split(':').First() switch
        {
            "mediaType" => transform(domain.MediaType ?? string.Empty),
            "length" => transform(domain.Length?.ToString() ?? string.Empty),
            "scale" => transform(domain.Scale?.ToString() ?? string.Empty),
            "name" => transform(domain.Name ?? string.Empty),
            "type" => transform(domain.Implementations.GetValueOrDefault(targetLanguage)?.Type ?? string.Empty),
            var i => i
        };
    }

    private static string ResolveVariable(this string input, Domain domainFrom, Domain domainTo, string targetLanguage)
    {
        if (input.StartsWith("from."))
        {
            return ResolveVariable(input.Split("from.")[1], domainFrom, targetLanguage);
        }
        else
        {
            return ResolveVariable(input.Split("to.")[1], domainTo, targetLanguage);
        }
    }
}