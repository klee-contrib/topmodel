using System.Text.RegularExpressions;
using TopModel.Utils;

namespace TopModel.Core;

public static class TemplateExtension
{
    public static string ParseTemplate(this string template, IFieldProperty fp)
    {
        if (string.IsNullOrEmpty(template) || !template.Contains('{'))
        {
            return template;
        }

        var result = template;
        foreach (var t in template.ExtractVariables())
        {
            result = result.Replace(t.Value, ResolveVariable(t.Value.Trim('{', '}'), fp));
        }

        return result;
    }

    public static string ParseTemplate(this string template, IProperty p)
    {
        if (p is IFieldProperty fp)
        {
            return template.ParseTemplate(fp);
        }

        if (p is CompositionProperty cp)
        {
            return template.ParseTemplate(cp);
        }

        return template;
    }

    public static string ParseTemplate(this string template, CompositionProperty fp)
    {
        if (string.IsNullOrEmpty(template) || !template.Contains('{'))
        {
            return template;
        }

        var result = template;
        foreach (var t in template.ExtractVariables())
        {
            result = result.Replace(t.Value, ResolveVariable(t.Value.Trim('{', '}'), fp));
        }

        return result;
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

    private static Func<string, string> GetTransformation(this string input)
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
                    transform = (string a) => a.ToCamelCase();
                    break;
                case "constant":
                    transform = (string a) => a.ToConstantCase();
                    break;
                case "kebab":
                    transform = (string a) => a.ToKebabCase();
                    break;
                case "lower":
                    transform = (string a) => a.ToLower();
                    break;
                case "pascal":
                    transform = (string a) => a.ToPascalCase();
                    break;
                case "snake":
                    transform = (string a) => a.ToSnakeCase();
                    break;
                case "upper":
                    transform = (string a) => a.ToUpper();
                    break;
                default:
                    break;
            }
        }

        return transform;
    }

    private static string ResolveVariable(this string input, IFieldProperty rp)
    {
        var transform = input.GetTransformation();
        var result = input.Split(':').First()
            .Replace("class.name", transform(rp.Class?.Name.ToString() ?? string.Empty))
            .Replace("name", transform(rp.Name ?? string.Empty))
            .Replace("trigram", transform(rp.Trigram ?? rp.Class?.Trigram ?? string.Empty))
            .Replace("label", transform(rp.Label ?? string.Empty))
            .Replace("comment", transform(rp.Comment))
            .Replace("required", transform(rp.Required.ToString().ToLower()))
            .Replace("resourceKey", transform(rp.ResourceKey.ToString()))
            .Replace("defaultValue", transform(rp.DefaultValue?.ToString() ?? string.Empty));
        return result;
    }

    private static string ResolveVariable(this string input, CompositionProperty cp)
    {
        var transform = input.GetTransformation();
        return input.Split(':').First()
            .Replace("class.name", transform(cp.Class?.Name.ToString() ?? string.Empty))
            .Replace("composition.name", transform(cp.Composition?.Name.ToString() ?? string.Empty))
            .Replace("name", transform(cp.Name ?? string.Empty))
            .Replace("label", transform(cp.Label ?? string.Empty))
            .Replace("comment", transform(cp.Comment));
    }

    private static string ResolveVariable(this string input, Class c, string[] parameters)
    {
        var transform = input.GetTransformation();
        var result = input.Split(':').First()
             .Replace("primaryKey.name", transform(c.PrimaryKey?.Name ?? string.Empty))
             .Replace("trigram", transform(c.Trigram))
             .Replace("name", transform(c.Name))
             .Replace("comment", transform(c.Comment))
             .Replace("label", transform(c.Label ?? string.Empty))
             .Replace("pluralName", transform(c.PluralName ?? string.Empty))
             .Replace("module", transform(c.ModelFile.Module ?? string.Empty));

        for (var i = 0; i < parameters.Length; i++)
        {
            result = result.Replace($"${i}", parameters[i]);
        }

        return result;
    }

    private static string ResolveVariable(this string input, Endpoint e, string[] parameters)
    {
        var transform = input.GetTransformation();
        var result = input.Split(':').First()
             .Replace("name", transform(e.Name))
             .Replace("method", transform(e.Method))
             .Replace("route", transform(e.Route))
             .Replace("description", transform(e.Description))
             .Replace("module", transform(e.ModelFile.Module ?? string.Empty));

        for (var i = 0; i < parameters.Length; i++)
        {
            result = result.Replace($"${i}", parameters[i]);
        }

        return result;
    }

    private static IEnumerable<Match> ExtractVariables(this string input)
    {
        var regex = new Regex(@"(\{[$a-zA-Z0-9:.]+\})");
        return regex.Matches(input).Cast<Match>();
    }
}