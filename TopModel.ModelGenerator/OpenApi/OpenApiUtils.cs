using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Models.Interfaces;
using Microsoft.OpenApi.Models.References;
using TopModel.Utils;

namespace TopModel.ModelGenerator.OpenApi;

public static class OpenApiUtils
{
    public static string Format(this string? description)
    {
        if (description == null)
        {
            return string.Empty;
        }

        if (description.Contains('"') || description.Contains('\n') || description.Contains(':') || description.Contains('#'))
        {
            var lines = description.ReplaceLineEndings().Split(Environment.NewLine);
            if (string.IsNullOrWhiteSpace(lines.Last()))
            {
                lines = lines.SkipLast(1).ToArray();
            }

            return $"|{Environment.NewLine}{string.Join(Environment.NewLine, lines.Select(line => $"        {line}"))}";
        }
        else
        {
            return description;
        }
    }

    public static (string? Kind, IOpenApiSchema? Schema) GetComposition(this OpenApiDocument model, IOpenApiSchema schema)
    {
        if (schema.AnyOf.Any() || schema.OneOf.Any())
        {
            return ("object", schema);
        }

        return schema?.Items != null
            ? ("list", schema.Items)
            : schema != null
            ? schema.Type == JsonSchemaType.Array
                ? ("list", schema)
                : ("object", schema)
            : schema?.Type == JsonSchemaType.Object && schema.AdditionalProperties != null
            ? ("map", schema.AdditionalProperties)
            : schema?.Type == JsonSchemaType.Object && schema.AdditionalProperties?.Items != null
            ? ("list-map", schema.AdditionalProperties.Items)
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

    public static string GetOperationId(this OpenApiDocument model, KeyValuePair<OperationType, OpenApiOperation> operation)
    {
        if (operation.Value.OperationId != null)
        {
            return operation.Value.OperationId;
        }

        var path = model.GetOperationPath(operation.Value).Replace("api/", string.Empty).Trim('/');

        if (!path.Contains('/'))
        {
            return path.ToPascalCase();
        }

        var id = operation.Key.ToString().ToPascalCase();

        if (operation.Key == OperationType.Get || operation.Key == OperationType.Head)
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
                var (kind, name) = model.GetComposition(bodySchema);
                id += name;

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
        return model.Paths.Single(p => p.Value.Operations.Any(o => o.Value == operation)).Key[1..];
    }

    public static IDictionary<string, IOpenApiSchema> GetProperties(this IOpenApiSchema schema)
    {
        if (schema.Type == JsonSchemaType.Array)
        {
            return schema.Items.GetProperties();
        }

        return schema.Properties
            .Concat(schema.AllOf.Where(a => a.Type == JsonSchemaType.Object).SelectMany(a => a.Properties))
            .ToDictionary(a => a.Key, a => a.Value);
    }

    public static IOpenApiSchema? GetRequestBodySchema(this OpenApiOperation operation)
    {
        return operation.RequestBody?.Content.First().Value.Schema;
    }

    public static KeyValuePair<string, IOpenApiSchema> GetResponseSchema(this OpenApiDocument model, OpenApiOperation operation)
    {
        var response = operation.Responses.FirstOrDefault(r => r.Key == "200" || r.Key == "201").Value;
        if (response != null && response.Content.Any())
        {
            return new(model.Components.Schemas.FirstOrDefault(s => s.Value == response.Content.First().Value.Schema).Key, response.Content.First().Value.Schema);
        }

        return default;
    }

    public static IDictionary<string, IOpenApiSchema> GetSchemas(this OpenApiDocument model)
    {
        var schemas = model?.Components?.Schemas;
        foreach (var s in model.Components.RequestBodies.ToDictionary(r => r.Key, r => r.Value.Content.First().Value.Schema))
        {
            if (!schemas.ContainsKey(s.Key)
                && !schemas.Values.Any(sc => sc == s.Value))
            {
                schemas.Add(s);
            }
        }

        foreach (var s in model.Components.Responses.Where(r => r.Value.Content.Any()).ToDictionary(r => r.Key, r => r.Value.Content.First().Value.Schema))
        {
            if (!schemas.ContainsKey(s.Key)
                && !schemas.Values.Any(sc => sc == s.Value))
            {
                schemas.Add(s);
            }
        }

        foreach (var s in model.Paths
            .SelectMany(p => p.Value.Operations.Where(o => o.Value.Tags.Any()))
            .Where(o => o.Value.RequestBody != null)
            .ToDictionary(r => $"{r.Key}{model.GetOperationId(r)}Body", r => r.Value.RequestBody.Content.First().Value.Schema))
        {
            if (!schemas.ContainsKey(s.Key)
                && !schemas.Values.Any(sc => sc == s.Value))
            {
                schemas.Add(s);
            }
        }

        return schemas.Where(s =>
             s.Value.Type == JsonSchemaType.Object
             || s.Value.Type == JsonSchemaType.String && s.Value.Enum.Any()
             || s.Value.AllOf.Any() && s.Value.AllOf.All(a => a.Type == JsonSchemaType.Object)
             || s.Value.AnyOf.Any()
             || s.Value.OneOf.Any())

         .ToDictionary(a => a.Key, a => a.Value);
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
        else if (schema.Type == JsonSchemaType.Array)
        {
            return $"{GetDomainCore(schema.Items)}-array";
        }
        else if (schema.Type == JsonSchemaType.Object && schema.AdditionalProperties != null)
        {
            return $"{GetDomainCore(schema.AdditionalProperties)}-map";
        }

        return schema.Type + length;
    }

    private static string GetDomainSchema(this OpenApiConfig config, IOpenApiSchema schema)
    {
        var domain = GetDomainCore(schema);
        return TmdGenUtils.GetDomainString(config.Domains, type: domain);
    }
}
