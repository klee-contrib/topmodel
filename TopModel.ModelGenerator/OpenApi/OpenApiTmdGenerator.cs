using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using TopModel.Utils;

namespace TopModel.ModelGenerator.OpenApi;

static class OpenApiTmdGenerator
{
    public async static Task GenerateOpenApi(OpenApiConfig config, ILogger logger, string directoryName, string modelRoot)
    {
        if (config.ModelTags.Count == 0)
        {
            config.ModelTags.Add("OpenApi");
        }

        if (config.EndpointTags.Count == 0)
        {
            config.EndpointTags.Add("OpenApi");
        }

        OpenApiDocument model;
        var modelReader = new OpenApiStreamReader();
        if (config.Source.StartsWith("http://") || config.Source.StartsWith("https://"))
        {
            using var client = new HttpClient();
            var openApi = await client.GetAsync(config.Source);
            model = modelReader.Read(await openApi.Content.ReadAsStreamAsync(), out var diagnostic);
        }
        else
        {
            using var stream = File.Open(directoryName + "/" + config.Source, FileMode.Open);
            model = modelReader.Read(stream, out var diagnostic);
        }

        string GetOperationPath(OpenApiOperation operation)
        {
            return model.Paths.Single(p => p.Value.Operations.Any(o => o.Value == operation)).Key[1..];
        }

        KeyValuePair<string, OpenApiSchema> GetRequestBodySchema(OpenApiOperation operation)
        {
            var bodySchema = operation.RequestBody?.Content.First().Value.Schema;
            if (bodySchema != null)
            {
                return new(model.Components.Schemas.FirstOrDefault(s => s.Value == bodySchema).Key, bodySchema);
            }

            return default;
        }

        KeyValuePair<string, OpenApiSchema> GetResponseSchema(OpenApiOperation operation)
        {
            var response = operation.Responses.FirstOrDefault(r => r.Key == "200").Value;
            if (response != null && response.Content.Any())
            {
                return new(model.Components.Schemas.FirstOrDefault(s => s.Value == response.Content.First().Value.Schema).Key, response.Content.First().Value.Schema);
            }

            return default;
        }

        string GetOperationId(KeyValuePair<OperationType, OpenApiOperation> operation)
        {
            if (operation.Value.OperationId != null)
            {
                return operation.Value.OperationId;
            }

            if (!GetOperationPath(operation.Value).Contains('/'))
            {
                return GetOperationPath(operation.Value);
            }

            var id = operation.Key.ToString().ToPascalCase();

            if (operation.Key == OperationType.Get || operation.Key == OperationType.Head)
            {
                var responseSchema = GetResponseSchema(operation.Value);
                if (responseSchema.Key != null)
                {
                    id += responseSchema.Key;
                }
            }
            else
            {
                var bodySchema = GetRequestBodySchema(operation.Value);
                if (bodySchema.Key != null)
                {
                    id += bodySchema.Key;
                }
            }

            return id;
        }


        string GetEndpointName(KeyValuePair<OperationType, OpenApiOperation> operation)
        {
            var operationId = GetOperationId(operation);
            var operationsWithId = model!.Paths.OrderBy(p => p.Key).SelectMany(p => p.Value.Operations.OrderBy(o => o.Key))
                .Where(o => GetOperationId(o) == operationId)
                .ToList();

            if (operationsWithId.Count == 1)
            {
                return operationId;
            }

            var prefix = operationsWithId.DistinctBy(o => o.Key).Count() > 1
                ? operation.Key.ToString().ToPascalCase()
                : string.Empty;

            var suffix = string.Empty;

            if (prefix == string.Empty)
            {
                suffix += operationsWithId.IndexOf(operation) + 1;
            }
            else
            {
                var operationWithIdAndMethod = operationsWithId.Where(o => o.Key == operation.Key).ToList();
                if (operationWithIdAndMethod.Count > 1)
                {
                    suffix += operationWithIdAndMethod.IndexOf(operation) + 1;
                }
            }

            return $"{prefix}{operationId}{suffix}";
        }

        var modules = model.Paths
            .SelectMany(p => p.Value.Operations.Where(o => o.Value.Tags.Any()))
            .GroupBy(o => o.Value.Tags.First().Name.ToPascalCase())
            .Where(m => m.Key != "Null" && (config.Include == null || config.Include.Contains(m.Key)));

        IEnumerable<OpenApiReference> GetModuleReferences(IEnumerable<KeyValuePair<OperationType, OpenApiOperation>> operations)
        {
            var visited = new HashSet<OpenApiSchema>();

            foreach (var operation in operations)
            {
                if (operation.Value.RequestBody != null)
                {
                    foreach (var reference in GetSchemaReferences(operation.Value.RequestBody.Content.First().Value.Schema, visited))
                    {
                        yield return reference;
                    }
                }

                foreach (var reference in operation.Value.Parameters.SelectMany(p => GetSchemaReferences(p.Schema, visited)))
                {
                    yield return reference;
                }

                var response = operation.Value.Responses.FirstOrDefault(r => r.Key == "200").Value;
                if (response != null && response.Content.Any())
                {
                    foreach (var reference in GetSchemaReferences(response.Content.First().Value.Schema, visited))
                    {
                        yield return reference;
                    }
                }
            }
        }

        IEnumerable<OpenApiReference> GetSchemaReferences(OpenApiSchema schema, HashSet<OpenApiSchema> visited)
        {
            visited.Add(schema);

            if (schema.Reference != null)
            {
                yield return schema.Reference;
            }

            if (schema.Items != null && !visited.Contains(schema.Items))
            {
                foreach (var reference in GetSchemaReferences(schema.Items, visited))
                {
                    yield return reference;
                }
            }

            if (schema.Properties != null)
            {
                foreach (var reference in schema.Properties.Values.Where(p => !visited.Contains(p)).SelectMany(p => GetSchemaReferences(p, visited)))
                {
                    yield return reference;
                }
            }

            if (schema.AdditionalProperties != null && !visited.Contains(schema.AdditionalProperties))
            {
                foreach (var reference in GetSchemaReferences(schema.AdditionalProperties, visited))
                {
                    yield return reference;
                }
            }
        }

        var referenceMap = modules.ToDictionary(m => m.Key, m => GetModuleReferences(m));

        using var fw = new FileWriter($"{Path.Combine(modelRoot, config.OutputDirectory, config.ModelFileName)}.tmd", logger, false) { StartCommentToken = "####" };
        fw.WriteLine("---");
        fw.WriteLine($"module: {config.Module}");
        fw.WriteLine("tags:");
        foreach (var tag in config.ModelTags)
        {
            fw.WriteLine($"  - {tag}");
        }
        fw.WriteLine();

        var references = new HashSet<string>(referenceMap.SelectMany(r => r.Value).Select(r => r.Id).Distinct());
        foreach (var schema in model.Components.Schemas.Where(s => s.Value.Type == "object").Where(s => references.Contains(s.Key)).OrderBy(s => s.Key))
        {
            fw.WriteLine("---");
            fw.WriteLine("class:");
            fw.WriteLine($"  name: {schema.Key}");

            if (config.PreservePropertyCasing)
            {
                fw.WriteLine($"  preservePropertyCasing: true");
            }

            if (schema.Value.Description != null)
            {
                fw.WriteLine($"  comment: {FormatDescription(schema.Value.Description ?? schema.Key)}");
            }
            else
            {
                fw.WriteLine($"  comment: no description provided");
            }

            fw.WriteLine();
            fw.WriteLine($"  properties:");

            foreach (var property in schema.Value.Properties)
            {
                if (!property.Value.Enum.Any())
                {
                    WriteProperty(config, fw, property, model);
                }
                else
                {
                    fw.WriteLine("    - alias:");
                    fw.WriteLine($@"        class: {schema.Key.ToPascalCase()}{property.Key.ToPascalCase()}");
                }

                if (schema.Value.Properties.Last().Key != property.Key)
                {
                    fw.WriteLine();
                }
            }
        }

        foreach (var schema in model.Components.Schemas.Where(sh => sh.Value.Properties.Where(p => (p.Value.Enum?.Any() ?? false)).Any()))
        {
            foreach (var property in schema.Value.Properties.Where(p => (p.Value.Enum?.Any() ?? false)))
            {
                fw.WriteLine("---");
                fw.WriteLine("class:");
                fw.WriteLine($"  name: {schema.Key.ToPascalCase()}{property.Key.ToPascalCase()}");

                if (config.PreservePropertyCasing)
                {
                    fw.WriteLine($"  preservePropertyCasing: true");
                }

                fw.WriteLine($"  comment: enum pour les valeurs de {property.Key.ToPascalCase()}");

                fw.WriteLine();
                fw.WriteLine($"  properties:");

                WriteProperty(config, fw, property, model);
                fw.WriteLine();
                fw.WriteLine($"  values:");
                var u = 0;
                foreach (var val in property.Value.Enum.OfType<OpenApiString>())
                {
                    fw.WriteLine($@"    value{u++}: {{ {property.Key}: {val.Value} }}");
                }
            }
        }

        foreach (var module in modules)
        {
            using var sw = new FileWriter($"{Path.Combine(modelRoot, config.OutputDirectory, module.Key)}.tmd", logger, false) { StartCommentToken = "####" };
            sw.WriteLine("---");
            sw.WriteLine($"module: {config.Module}");
            sw.WriteLine("tags:");
            foreach (var tag in config.EndpointTags)
            {
                sw.WriteLine($"  - {tag}");
            }
            if (referenceMap[module.Key].Any())
            {
                sw.WriteLine("uses:");
                var use = $"{config.OutputDirectory.Replace("\\", "/")}/{config.ModelFileName}".Replace("//", "/");
                if (use.StartsWith("./"))
                {
                    use = use.Replace("./", string.Empty);
                }

                sw.WriteLine($"  - {use}");
            }
            sw.WriteLine();

            foreach (var operation in module.OrderBy(o => GetEndpointName(o)))
            {
                var path = GetOperationPath(operation.Value);

                sw.WriteLine("---");
                sw.WriteLine("endpoint:");
                sw.WriteLine($"  name: {GetEndpointName(operation)}");
                sw.WriteLine($"  method: {operation.Key.ToString().ToUpper()}");
                sw.WriteLine($"  route: {path}");
                if (operation.Value.Summary != null)
                {
                    sw.WriteLine($"  description: {FormatDescription(operation.Value.Summary)}");
                }
                else
                {
                    sw.WriteLine($"  description: no description provided");
                }

                if (config.PreservePropertyCasing)
                {
                    sw.WriteLine($"  preservePropertyCasing: true");
                }

                if (operation.Value.Parameters.Any() || operation.Value.RequestBody != null)
                {
                    sw.WriteLine("  params:");

                    foreach (var param in operation.Value.Parameters.OrderBy(p => path.Contains($@"{{{p.Name}}}") ? 0 + p.Name : 1 + p.Name))
                    {
                        sw.WriteLine($"    - name: {param.Name}");
                        sw.WriteLine($"      domain: {GetDomain(config, param.Name, param.Schema)}");
                        if (param.Description != null)
                        {
                            sw.WriteLine($"      comment: {FormatDescription(param.Description)}");
                        }
                        else
                        {
                            sw.WriteLine($"      comment: no description provided");
                        }
                    }

                    var bodySchema = GetRequestBodySchema(operation.Value).Value;
                    if (bodySchema != null)
                    {
                        WriteProperty(config, sw, new("body", bodySchema), model);
                    }

                }

                var responseSchema = GetResponseSchema(operation.Value).Value;
                if (responseSchema != null)
                {
                    sw.WriteLine("  returns:");
                    WriteProperty(config, sw, new("Result", responseSchema), model, noList: true);
                }
            }
        }
    }

    static string FormatDescription(string description)
    {
        if (description == null)
        {
            return string.Empty;
        }

        description = description.Replace(Environment.NewLine, " ");

        return @$"""{description}""";
    }

    static string GetDomain(OpenApiConfig config, String name, OpenApiSchema schema)
    {
        var resolvedDomain = TmdGenUtils.GetDomainString(config.Domains, name);
        if (resolvedDomain == name)
        {
            return GetDomainSchema(config, schema);
        }
        return resolvedDomain;
    }

    static string GetDomainSchema(OpenApiConfig config, OpenApiSchema schema)
    {
        var domain = GetDomainCore(schema);
        return TmdGenUtils.GetDomainString(config.Domains, domain);
    }

    static string GetDomainCore(OpenApiSchema schema)
    {
        var length = schema.MaxLength != null ? $"{schema.MaxLength}" : string.Empty;

        if (schema.Format != null)
        {
            return schema.Format + length;
        }
        else if (schema.Type == "array")
        {
            return $"{GetDomainCore(schema.Items)}-array";
        }
        else if (schema.Type == "object" && schema.AdditionalProperties != null)
        {
            return $"{GetDomainCore(schema.AdditionalProperties)}-map";
        }

        return schema.Type + length;
    }

    static void WriteProperty(OpenApiConfig config, FileWriter sw, KeyValuePair<string, OpenApiSchema> property, OpenApiDocument model, bool noList = false)
    {
        var kind =
            property.Value.Items?.Reference != null ? "list"
            : property.Value.Reference != null && model.Components.Schemas.Any(s => s.Value.Type == "object" && s.Value.Reference == property.Value.Reference) ? "object"
            : property.Value.Type == "object" && property.Value.AdditionalProperties?.Reference != null ? "map"
            : property.Value.Type == "object" && property.Value.AdditionalProperties?.Items?.Reference != null ? "list-map"
            : null;

        if (kind != null)
        {
            var domainKind = TmdGenUtils.GetDomainString(config.Domains, kind);
            sw.WriteLine($"    {(noList ? string.Empty : "- ")}composition: {property.Value.AdditionalProperties?.Items?.Reference.Id ?? property.Value.AdditionalProperties?.Reference.Id ?? property.Value.Items?.Reference.Id ?? property.Value.Reference!.Id}");
            sw.WriteLine($"    {(noList ? string.Empty : "  ")}name: {property.Key}");
            sw.WriteLine($"    {(noList ? string.Empty : "  ")}kind: {domainKind ?? kind}");
        }
        else
        {
            sw.WriteLine($"    {(noList ? string.Empty : "- ")}name: {property.Key}");
            sw.WriteLine($"    {(noList ? string.Empty : "  ")}domain: {GetDomain(config, property.Key, property.Value)}");
            sw.WriteLine($"    {(noList ? string.Empty : "  ")}required: {(!property.Value.Nullable).ToString().ToLower()}");
        }

        sw.WriteLine($"    {(noList ? string.Empty : "  ")}comment: {FormatDescription(property.Value.Description ?? property.Key)}");
    }
}