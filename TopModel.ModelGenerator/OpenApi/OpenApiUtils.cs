using Microsoft.OpenApi;
using TopModel.Utils;

namespace TopModel.ModelGenerator.OpenApi;

public static class OpenApiUtils
{
    public static string Format(this string? description, bool quote = false)
    {
        if (description == null)
        {
            return quote ? "\"\"" : string.Empty;
        }

        if (
            description.Contains('"')
            || description.Contains('\n')
            || description.Contains(':')
            || description.Contains('#')
        )
        {
            var lines = description.ReplaceLineEndings().Split(Environment.NewLine);
            if (string.IsNullOrWhiteSpace(lines[^1]))
            {
                lines = lines.SkipLast(1).ToArray();
            }

            var indent = quote ? "    " : "        ";

            return $"|{Environment.NewLine}{string.Join(Environment.NewLine, lines.Select(line => $"{indent}{line}"))}";
        }
        else
        {
            return quote ? $"\"{description}\"" : description;
        }
    }

    public static (string? Kind, IOpenApiSchema? Schema) GetComposition(IOpenApiSchema schema)
    {
        if ((schema.AnyOf?.Any() ?? false) || (schema.OneOf?.Any() ?? false))
        {
            return ("object", schema);
        }

        return schema?.Items != null ? ("list", schema.Items)
            : schema?.Type == JsonSchemaType.Object && schema.AdditionalProperties != null
                ? ("map", schema.AdditionalProperties)
            : schema?.Type == JsonSchemaType.Object && schema.AdditionalProperties?.Items != null
                ? ("list-map", schema.AdditionalProperties.Items)
            : schema != null
                ? schema.Type == JsonSchemaType.Array ? ("list", schema)
                    : ("object", schema)
            : (null, null);
    }

    public static string GetDomain(this OpenApiConfig config, string name, IOpenApiSchema schema)
    {
        var resolvedDomain = TmdGenUtils.GetDomainString(config.Domains, name: name);
        if (resolvedDomain == name)
        {
            return GetDomainSchema(config, schema);
        }

        return resolvedDomain;
    }

    public static string GetOperationId(
        this OpenApiDocument model,
        KeyValuePair<HttpMethod, OpenApiOperation> operation
    )
    {
        if (operation.Value.OperationId != null)
        {
            return operation.Value.OperationId.ApplyTransform(p => p.ToPascalCase()).ToFlat();
        }

        var path = model.GetOperationPath(operation.Value).Replace("api/", string.Empty).Trim('/');

        if (!path.Contains('/'))
        {
            return path.ToPascalCase();
        }

        var id = operation.Key.ToString().ToPascalCase(strictIfUppercase: true);

        if (operation.Key == HttpMethod.Get || operation.Key == HttpMethod.Head)
        {
            var responseSchema = model.GetResponseSchema(operation.Value);
            if (responseSchema.Key != null)
            {
                id += responseSchema.Key;
            }
        }
        else
        {
            var bodySchema = operation.Value.GetRequestBodySchema();
            if (bodySchema != null)
            {
                var (kind, schema) = GetComposition(bodySchema);
                id += schema switch
                {
                    OpenApiSchemaReference schemaRef => schemaRef.Reference.Id,
                    _ => string.Empty,
                };

                if (kind != null && kind != "object")
                {
                    id += kind.ToPascalCase();
                }
            }
        }

        return id;
    }

    public static string GetOperationPath(this OpenApiDocument model, OpenApiOperation operation)
    {
        return model.Paths.Single(p => p.Value.Operations!.Any(o => o.Value == operation)).Key[1..];
    }

    public static IDictionary<string, IOpenApiSchema> GetProperties(this IOpenApiSchema schema)
    {
        if (schema.Type == JsonSchemaType.Array)
        {
            return schema.Items?.GetProperties() ?? new Dictionary<string, IOpenApiSchema>();
        }

        return (schema.Properties ?? new Dictionary<string, IOpenApiSchema>())
                .Concat(
                    (schema.AllOf ?? [])
                        .Where(a => a.Type == JsonSchemaType.Object)
                        .SelectMany(a => a.Properties ?? new Dictionary<string, IOpenApiSchema>())
                )
                .DistinctBy(a => a.Key)
                .ToDictionary(a => a.Key, a => a.Value)
            ?? [];
    }

    public static IOpenApiSchema? GetRequestBodySchema(this OpenApiOperation operation)
    {
        return operation.RequestBody?.Content?.FirstOrDefault().Value.Schema;
    }

    public static KeyValuePair<string, IOpenApiSchema> GetResponseSchema(
        this OpenApiDocument model,
        OpenApiOperation operation
    )
    {
        var response = operation.Responses?.FirstOrDefault(r => r.Key == "200" || r.Key == "201").Value;
        if (response?.Content?.Any() ?? false)
        {
            var contentSchema = response.Content.First().Value.Schema;
            return new(
                (contentSchema as OpenApiSchemaReference)?.Reference.Id
                    ?? model
                        .Components?.Schemas?.FirstOrDefault(s => s.Value == response.Content.First().Value.Schema)
                        .Key!,
                contentSchema!
            );
        }

        return default;
    }

    public static IDictionary<string, OpenApiSchema> GetSchemas(this OpenApiDocument model)
    {
        var schemas = model.Components?.Schemas ?? new Dictionary<string, IOpenApiSchema>();
        foreach (
            var s in model.Components?.RequestBodies?.ToDictionary(
                r => r.Key,
                r => r.Value.Content?.FirstOrDefault().Value.Schema
            ) ?? []
        )
        {
            if (s.Value != null && !schemas.ContainsKey(s.Key) && !schemas.Values.Any(sc => sc == s.Value))
            {
                schemas.Add(s.Key, s.Value);
            }
            if (s.Value?.Properties != null)
            {
                foreach (var propertySchema in s.Value.Properties)
                {
                    if (
                        !schemas.Values.Any(sc => sc == propertySchema.Value)
                        && !schemas.ContainsKey($"{s.Key}{propertySchema.Key.ToPascalCase()}")
                    )
                    {
                        schemas.Add($"{s.Key}{propertySchema.Key.ToPascalCase()}", propertySchema.Value);
                    }
                }
            }
        }

        foreach (
            var s in model
                .Components?.Responses?.Where(r => r.Value.Content?.Any() ?? false)
                ?.ToDictionary(r => r.Key, r => r.Value.Content?.FirstOrDefault().Value.Schema) ?? []
        )
        {
            if (s.Value != null && !schemas.ContainsKey(s.Key) && !schemas.Values.Any(sc => sc == s.Value))
            {
                schemas.Add(s.Key, s.Value);
            }
        }

        foreach (
            var s in model
                .Paths.SelectMany(p => p.Value.Operations?.Where(o => o.Value.Tags?.Any() ?? false) ?? [])
                .Where(o => o.Value.RequestBody != null)
                .ToDictionary(
                    r => $"{r.Key.Method.ToPascalCase(strictIfUppercase: true)}{model.GetOperationId(r)}Body",
                    r => r.Value.RequestBody?.Content?.FirstOrDefault().Value.Schema
                )
        )
        {
            if (s.Value != null && !schemas.ContainsKey(s.Key) && !schemas.Values.Any(sc => sc == s.Value))
            {
                schemas.Add(s.Key, s.Value);
            }
            if (s.Value != null && s.Value.Items != null && !schemas.Values.Any(sc => sc == s.Value.Items))
            {
                schemas.Add(s.Key + "Items", s.Value.Items);
            }
        }

        foreach (
            var s in model
                .Paths.SelectMany(p => p.Value.Operations?.Where(o => o.Value.Tags?.Any() ?? false) ?? [])
                .Where(o => o.Value.Responses?.Any(r => r.Value.Content?.Any() ?? false) ?? false)
                .ToDictionary(
                    r => $"{r.Key.Method.ToPascalCase(strictIfUppercase: true)}{model.GetOperationId(r)}Response",
                    r => r.Value.Responses?.First().Value.Content?.FirstOrDefault().Value.Schema
                )
        )
        {
            if (s.Value != null && !schemas.ContainsKey(s.Key) && !schemas.Values.Any(sc => sc == s.Value))
            {
                schemas.Add(s.Key, s.Value);
            }
        }

        return schemas
            .Where(s =>
                (
                    s.Value.Type == JsonSchemaType.Object
                    || s.Value.Type is null && s.Value.Properties?.Any() == true
                    || s.Value.Type == JsonSchemaType.String && (s.Value.Enum?.Any() ?? false)
                    || (s.Value.AllOf?.Any() ?? false) && s.Value.AllOf.All(a => a.Type == JsonSchemaType.Object)
                    || (s.Value.AnyOf?.Any() ?? false)
                    || (s.Value.OneOf?.Any() ?? false)
                )
                && s.Value is OpenApiSchema
            )
            .ToDictionary(a => a.Key, a => (OpenApiSchema)a.Value);
    }

    public static string Unplurialize(this string name)
    {
        return name.EndsWith("ies") ? $"{name[..^3]}y" : name.TrimEnd('s');
    }

    private static string GetDomainCore(this IOpenApiSchema schema)
    {
        var length = schema.MaxLength != null ? $"{schema.MaxLength}" : string.Empty;

        if (schema.Format != null)
        {
            return schema.Format + length;
        }
        else if (schema.Type == JsonSchemaType.Array && schema.Items != null)
        {
            return $"{GetDomainCore(schema.Items)}-array";
        }
        else if (schema.Type == JsonSchemaType.Object && schema.AdditionalProperties != null)
        {
            return $"{GetDomainCore(schema.AdditionalProperties)}-map";
        }

        return (schema.Type == JsonSchemaType.Null ? JsonSchemaType.Object : (schema.Type & ~JsonSchemaType.Null))
            + length;
    }

    private static string GetDomainSchema(this OpenApiConfig config, IOpenApiSchema schema)
    {
        var domain = GetDomainCore(schema);
        return TmdGenUtils.GetDomainString(config.Domains, type: domain);
    }
}
