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
        var regex = new Regex(@"(\{[$a-zA-Z0-9:.]+\})");
        return regex.Matches(input).Cast<Match>();
    }

    private static string ResolveVariable(this string input, IFieldProperty rp, string[] parameters)
    {
        var transform = input.GetTransformation();
        var result = input.Split(':').First() switch
        {
            "class.name" => transform(rp.Class?.Name.ToString() ?? string.Empty),
            "class.sqlName" => transform(rp.Class?.SqlName ?? string.Empty),
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
        var transform = input.GetTransformation();
        var result = input.Split(':').First() switch
        {
            "class.name" => transform(cp.Class?.Name.ToString() ?? string.Empty),
            "composition.name" => transform(cp.Composition?.Name.ToString() ?? string.Empty),
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

    private static string ResolveVariable(this string input, Class c, string[] parameters)
    {
        var transform = input.GetTransformation();
        var result = input.Split(':').First() switch
        {
            "primaryKey.name" => transform(c.PrimaryKey.FirstOrDefault()?.Name ?? string.Empty),
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

    private static string ResolveVariable(this string input, Domain domainFrom, Domain domainTo, string targetLanguage)
    {
        var transform = input.GetTransformation();
        var variable = input.Split(':').First();

        return input.Split(':').First() switch
        {
            "from.mediaType" => transform(domainFrom.MediaType ?? string.Empty),
            "from.length" => transform(domainFrom.Length?.ToString() ?? string.Empty),
            "from.scale" => transform(domainFrom.Scale?.ToString() ?? string.Empty),
            "from.name" => transform(domainFrom.Name ?? string.Empty),
            "from.type" => transform(domainFrom.Implementations.GetValueOrDefault(targetLanguage)?.Type ?? string.Empty),
            "to.mediaType" => transform(domainTo.MediaType ?? string.Empty),
            "to.length" => transform(domainTo.Length?.ToString() ?? string.Empty),
            "to.scale" => transform(domainTo.Scale?.ToString() ?? string.Empty),
            "to.name" => transform(domainTo.Name ?? string.Empty),
            "to.type" => transform(domainTo.Implementations.GetValueOrDefault(targetLanguage)?.Type ?? string.Empty),
            var i => i
        };
    }
}