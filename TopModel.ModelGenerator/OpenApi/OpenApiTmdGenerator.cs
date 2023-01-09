using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using TopModel.Utils;

namespace TopModel.ModelGenerator.OpenApi;

static class OpenApiTmdGenerator
{
    public async static Task GenerateOpenApi(OpenApiConfig config, ILogger logger, string directoryName, string modelRoot)
    {
        ModelUtils.CombinePath(directoryName, config, c => c.OutputDirectory);
        var i = 0;
        foreach (var source in config.Sources)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"#{i++}");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($" OpenApiGen .......@{source.Key}");

            if (source.Value.ModelTags.Count == 0)
            {
                source.Value.ModelTags.Add("OpenApi");
            }

            if (source.Value.EndpointTags.Count == 0)
            {
                source.Value.EndpointTags.Add("OpenApi");
            }

            OpenApiDocument model;
            var modelReader = new OpenApiStreamReader();
            if (source.Value.Url != null)
            {
                using var client = new HttpClient();
                var openApi = await client.GetAsync(source.Value.Url);
                model = modelReader.Read(await openApi.Content.ReadAsStreamAsync(), out var diagnostic);
            }
            else
            {
                using var stream = File.Open(directoryName + "/" + source.Value.Path!, FileMode.Open);
                model = modelReader.Read(stream, out var diagnostic);
            }


            string GetEndpointName(OpenApiOperation operation)
            {
                var operationsWithId = model!.Paths.OrderBy(p => p.Key).SelectMany(p => p.Value.Operations.OrderBy(o => o.Key))
                    .Where(o => o.Value.OperationId == operation.OperationId)
                    .Select(o => o.Value)
                    .ToList();

                if (operationsWithId.Count == 1)
                {
                    return operation.OperationId;
                }

                return $"{operation.OperationId}{operationsWithId.IndexOf(operation) + 1}";
            }

            var modules = model.Paths
                .SelectMany(p => p.Value.Operations.Where(o => o.Value.Tags.Any()))
                .GroupBy(o => o.Value.Tags.First().Name.ToPascalCase())
                .Where(m => m.Key != "Null" && (source.Value.Include == null || source.Value.Include.Contains(m.Key)));

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

            using var fw = new FileWriter($"{config.OutputDirectory}/{source.Key}/{source.Value.ModelFileName}.tmd", logger, false) { StartCommentToken = "####" };
            fw.WriteLine("---");
            fw.WriteLine($"module: {source.Key.ToPascalCase()}");
            fw.WriteLine("tags:");
            foreach (var tag in source.Value.ModelTags)
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
                    WriteProperty(config, fw, property, model);

                    if (schema.Value.Properties.Last().Key != property.Key)
                    {
                        fw.WriteLine();
                    }
                }
            }

            foreach (var module in modules)
            {
                using var sw = new FileWriter($"{config.OutputDirectory}/{source.Key}/{module.Key}.tmd", logger, false) { StartCommentToken = "####" };
                sw.WriteLine("---");
                sw.WriteLine($"module: {source.Key.ToPascalCase()}");
                sw.WriteLine("tags:");
                foreach (var tag in source.Value.EndpointTags)
                {
                    sw.WriteLine($"  - {tag}");
                }
                if (referenceMap[module.Key].Any())
                {
                    sw.WriteLine("uses:");
                    var use = $"{Path.GetRelativePath(modelRoot, config.OutputDirectory).Replace("\\", "/")}/{source.Key}/{source.Value.ModelFileName}".Replace("//", "/");
                    sw.WriteLine($"  - {use}");
                }
                sw.WriteLine();

                foreach (var operation in module.OrderBy(o => GetEndpointName(o.Value)))
                {
                    var path = model.Paths.Single(p => p.Value.Operations.Any(o => o.Value == operation.Value));

                    sw.WriteLine("---");
                    sw.WriteLine("endpoint:");
                    sw.WriteLine($"  name: {GetEndpointName(operation.Value)}");
                    sw.WriteLine($"  method: {operation.Key.ToString().ToUpper()}");
                    sw.WriteLine($"  route: {path.Key[1..]}");
                    if (operation.Value.Summary != null)
                    {
                        sw.WriteLine($"  description: {FormatDescription(operation.Value.Summary)}");
                    }
                    else
                    {
                        sw.WriteLine($"  description: no description provided");
                    }

                    if (operation.Value.Parameters.Any() || operation.Value.RequestBody != null)
                    {
                        sw.WriteLine("  params:");

                        if (operation.Value.RequestBody != null)
                        {
                            WriteProperty(config, sw, new("Body", operation.Value.RequestBody.Content.First().Value.Schema), model);
                        }

                        foreach (var param in operation.Value.Parameters)
                        {
                            sw.WriteLine($"    - name: {param.Name.ToFirstUpper()}");
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
                    }

                    var response = operation.Value.Responses.FirstOrDefault(r => r.Key == "200").Value;
                    if (response != null && response.Content.Any())
                    {
                        sw.WriteLine("  returns:");
                        WriteProperty(config, sw, new("Result", response.Content.First().Value.Schema), model, noList: true);
                    }
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
        var resolvedDomain = GetDomainString(config, name);
        if (resolvedDomain == name)
        {
            return GetDomainSchema(config, schema);
        }
        return resolvedDomain;
    }

    static string GetDomainSchema(OpenApiConfig config, OpenApiSchema schema)
    {
        var domain = GetDomainCore(schema);
        return GetDomainString(config, domain);
    }

    static string GetDomainString(OpenApiConfig config, string domain)
    {
        return config.Domains.FirstOrDefault(d =>
        {
            if (d.name != null)
            {
                if (d.name.StartsWith('/'))
                {
                    return Regex.IsMatch(domain, d.name[1..^1]);
                }
                else
                {
                    return domain == d.name;
                }

            }
            else if (d.type != null)
            {
                if (d.type.StartsWith('/'))
                {
                    return Regex.IsMatch(domain, d.type[1..^1]);
                }
                else
                {
                    return domain == d.type;
                }
            }

            return false;
        })?.domain ?? domain;
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
            var domainKind = GetDomainString(config, kind);
            sw.WriteLine($"    {(noList ? string.Empty : "- ")}composition: {property.Value.AdditionalProperties?.Items?.Reference.Id ?? property.Value.AdditionalProperties?.Reference.Id ?? property.Value.Items?.Reference.Id ?? property.Value.Reference!.Id}");
            sw.WriteLine($"    {(noList ? string.Empty : "  ")}name: {property.Key.ToFirstUpper()}");
            sw.WriteLine($"    {(noList ? string.Empty : "  ")}kind: {domainKind ?? kind}");
        }
        else
        {
            sw.WriteLine($"    {(noList ? string.Empty : "- ")}name: {property.Key.ToFirstUpper()}");
            sw.WriteLine($"    {(noList ? string.Empty : "  ")}domain: {GetDomain(config, property.Key, property.Value)}");
            sw.WriteLine($"    {(noList ? string.Empty : "  ")}required: {(!property.Value.Nullable).ToString().ToLower()}");
        }

        sw.WriteLine($"    {(noList ? string.Empty : "  ")}comment: {FormatDescription(property.Value.Description ?? property.Key)}");
    }
}