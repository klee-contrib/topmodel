using System.Text.RegularExpressions;
using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Core.Utils;

public static class TemplateExtensions
{
    extension(string template)
    {
        public string ParseTemplate(IProperty p, WatcherConfigBase config, string tag)
        {
            return template.ParseTemplate(
                p,
                p.Domain?.TemplateParameters ?? [],
                p.Domain?.Parameters ?? new Dictionary<string, string>(),
                config,
                tag
            );
        }

        public string ParseTemplate(
            IProperty p,
            IList<TemplateParameter> templateParameters,
            IDictionary<string, string> parameterValues,
            WatcherConfigBase config,
            string tag
        )
        {
            if (string.IsNullOrEmpty(template) || !template.Contains('{'))
            {
                return template;
            }

            string result = template;
            foreach (var t in template.Variables)
            {
                result = result.Replace(
                    t.Value,
                    t.Value.Trim('{', '}').ResolveVariable(p, templateParameters, parameterValues, config, tag)
                );
            }

            return result;
        }

        public string ParseTemplate(
            IPropertyContainer c,
            IList<TemplateParameter> templateParameters,
            IDictionary<string, string> parameterValues,
            WatcherConfigBase config,
            string tag
        )
        {
            if (string.IsNullOrEmpty(template) || !template.Contains('{'))
            {
                return template;
            }

            string result = template;
            foreach (var t in template.Variables)
            {
                result = result.Replace(
                    t.Value,
                    t.Value.Trim('{', '}').ResolveVariable(c, templateParameters, parameterValues, config, tag)
                );
            }

            return result;
        }

        public string ParseTemplate(Domain domainFrom, Domain domainTo, WatcherConfigBase config, string tag)
        {
            if (string.IsNullOrEmpty(template) || !template.Contains('{'))
            {
                return template;
            }

            var result = template;
            foreach (var t in template.Variables)
            {
                result = result.Replace(
                    t.Value,
                    t.Value.Trim('{', '}').ResolveVariable(domainFrom, domainTo, config, tag)
                );
            }

            return result;
        }
    }

    extension(string input)
    {
        private IEnumerable<Match> Variables
        {
            get
            {
                var regex = new Regex(@"(\{[$a-zA-Z0-9:.\[\]]+\})");
                return regex.Matches(input).Cast<Match>();
            }
        }

        private string ResolveCustomProperty(IDictionary<string, string> customProperties)
        {
            var propertyName = input.Split(':')[0];
            if (customProperties.TryGetValue(propertyName, out var value))
            {
                return value.Transform(input);
            }

            return string.Empty;
        }

        private string ResolveVariable(Domain domain, WatcherConfigBase config, string tag)
        {
            return (
                input.Split(':')[0] switch
                {
                    "mediaType" => domain.MediaType ?? string.Empty,
                    "length" => domain.Length?.ToString() ?? string.Empty,
                    "scale" => domain.Scale?.ToString() ?? string.Empty,
                    "name" => domain.Name ?? string.Empty,
                    "type" => config.GetImplementation(domain, tag)?.Type ?? string.Empty,
                    var i => config.ResolveVariables(config.ResolveGlobalVariables($@"{{{i}}}"), tag: tag),
                }
            ).Transform(input);
        }

        private string ResolveVariable(
            IPropertyContainer container,
            IList<TemplateParameter> templateParameters,
            IDictionary<string, string> parameterValues,
            WatcherConfigBase config,
            string tag
        )
        {
            return container switch
            {
                Endpoint e => input.ResolveVariable(e, templateParameters, parameterValues, config, tag),
                Class c => input.ResolveVariable(c, templateParameters, parameterValues, config, tag),
                _ => string.Empty,
            };
        }

        private string ResolveVariable(
            IProperty p,
            IList<TemplateParameter> templateParameters,
            IDictionary<string, string> parameterValues,
            WatcherConfigBase config,
            string tag
        )
        {
            if (input == null || input.Length == 0)
            {
                return string.Empty;
            }

            if (input.StartsWith("parent."))
            {
                return input["parent.".Length..]
                    .ResolveVariable(p.Parent, templateParameters, parameterValues, config, tag);
            }

            if (input.StartsWith("class."))
            {
                return input["class.".Length..]
                    .ResolveVariable(p.Parent, templateParameters, parameterValues, config, tag);
            }

            if (input.StartsWith("endpoint."))
            {
                return input["endpoint.".Length..]
                    .ResolveVariable(p.Parent, templateParameters, parameterValues, config, tag);
            }

            if (input.StartsWith("domain."))
            {
                return input["domain.".Length..].ResolveVariable(p.Domain.Domain, config, tag);
            }

            if (input.StartsWith($"customProperties."))
            {
                return ResolveCustomProperty(input["customProperties.".Length..], p.CustomProperties);
            }

            if (input.StartsWith("association.") && p.Association != null)
            {
                return input["association.".Length..]
                    .ResolveVariable(p.Association!, templateParameters, parameterValues, config, tag);
            }

            if (input.StartsWith("composition.") && p.Composition != null)
            {
                return input["composition.".Length..]
                    .ResolveVariable(p.Composition!, templateParameters, parameterValues, config, tag);
            }

            var result = (
                input.Split(':')[0] switch
                {
                    "name" => p.Name ?? string.Empty,
                    "sqlName" => p.SqlName ?? string.Empty,
                    "paramName" => p.GetParamName(),
                    "trigram" => p.Trigram ?? p.Class?.Trigram ?? string.Empty,
                    "label" => p.Label ?? string.Empty,
                    "comment" => p.Comment,
                    "required" => p.Required.ToString().ToLower(),
                    "resourceKey" => p.ResourceKey,
                    "commentResourceKey" => p.CommentResourceKey,
                    "defaultValue" => p.DefaultValue ?? string.Empty,
                    var i => i.TryResolveParameters(templateParameters, parameterValues)
                        ?? config.ResolveVariables(
                            config.ResolveGlobalVariables($@"{{{i}}}"),
                            module: p.Parent.Namespace.Module,
                            tag: tag
                        ),
                }
            ).Transform(input);

            return result;
        }

        private string ResolveVariable(
            Class c,
            IList<TemplateParameter> templateParameters,
            IDictionary<string, string> parameterValues,
            WatcherConfigBase config,
            string tag
        )
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

                return input["primaryKey.".Length..]
                    .ResolveVariable(c.PrimaryKey.FirstOrDefault()!, templateParameters, parameterValues, config, tag);
            }

            if (input.StartsWith("extends."))
            {
                if (c.Extends == null)
                {
                    return string.Empty;
                }

                return input["extends.".Length..]
                    .ResolveVariable(c.Extends, templateParameters, parameterValues, config, tag);
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

                    return nextInput.ResolveVariable(
                        c.Properties[index],
                        templateParameters,
                        parameterValues,
                        config,
                        tag
                    );
                }

                return string.Empty;
            }

            var result = (
                input.Split(':')[0] switch
                {
                    "trigram" => c.Trigram ?? string.Empty,
                    "name" => c.Name,
                    "sqlName" => c.SqlName,
                    "comment" => c.Comment,
                    "label" => c.Label ?? string.Empty,
                    "pluralName" => c.PluralName ?? string.Empty,
                    "module" => c.Namespace.Module ?? string.Empty,
                    var i => i.TryResolveParameters(templateParameters, parameterValues)
                        ?? config.ResolveVariables(
                            config.ResolveGlobalVariables($@"{{{i}}}"),
                            module: c.Namespace.Module,
                            tag: tag
                        ),
                }
            ).Transform(input);

            return result;
        }

        private string ResolveVariable(
            Endpoint e,
            IList<TemplateParameter> templateParameters,
            IDictionary<string, string> parameterValues,
            WatcherConfigBase config,
            string tag
        )
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

                return input["returns.".Length..]
                    .ResolveVariable(e.Returns, templateParameters, parameterValues, config, tag);
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

                return string.Empty;
            }

            var result = (
                input.Split(':')[0] switch
                {
                    "name" => e.Name,
                    "method" => e.Method,
                    "route" => e.Route,
                    "description" => e.Description,
                    "module" => e.Namespace.Module ?? string.Empty,
                    var i => i.TryResolveParameters(templateParameters, parameterValues)
                        ?? config.ResolveVariables(
                            config.ResolveGlobalVariables($@"{{{i}}}"),
                            module: e.Namespace.Module,
                            tag: tag
                        ),
                }
            ).Transform(input);

            return result;
        }

        private string ResolveVariable(Domain domainFrom, Domain domainTo, WatcherConfigBase config, string tag)
        {
            if (input.StartsWith("from."))
            {
                return input["from.".Length..].ResolveVariable(domainFrom, config, tag);
            }

            return input["to.".Length..].ResolveVariable(domainTo, config, tag);
        }

        private string? TryResolveParameters(
            IList<TemplateParameter> templateParameters,
            IDictionary<string, string> parameterValues
        )
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
                        value = value.Split('/', '\\', '.')[0];
                        break;
                    case "last":
                        value = value.Split('/', '\\', '.')[^1];
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
}
