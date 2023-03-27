using System.Collections.Immutable;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using TopModel.Utils;

namespace TopModel.ModelGenerator.OpenApi;

public class OpenApiTmdGenerator : ModelGenerator
{
    private readonly OpenApiConfig _config;
    private readonly ILogger<OpenApiTmdGenerator> _logger;

#nullable disable
    private OpenApiDocument _model;
#nullable enable

    public OpenApiTmdGenerator(ILogger<OpenApiTmdGenerator> logger, OpenApiConfig config)
        : base(logger)
    {
        _config = config;
        _logger = logger;

        if (_config.ModelTags.Count == 0)
        {
            _config.ModelTags.Add("OpenApi");
        }

        if (_config.EndpointTags.Count == 0)
        {
            _config.EndpointTags.Add("OpenApi");
        }
    }

    public override string Name => "OpenApiGen";

    protected override async IAsyncEnumerable<string> GenerateCore()
    {
        var modelReader = new OpenApiStreamReader();
        if (_config.Source.StartsWith("http://") || _config.Source.StartsWith("https://"))
        {
            using var client = new HttpClient();
            var openApi = await client.GetAsync(_config.Source);
            _model = modelReader.Read(await openApi.Content.ReadAsStreamAsync(), out var diagnostic);
        }
        else
        {
            using var stream = File.Open(DirectoryName + "/" + _config.Source, FileMode.Open);
            _model = modelReader.Read(stream, out var diagnostic);
        }

        var modules = _model.Paths
            .SelectMany(p => p.Value.Operations.Where(o => o.Value.Tags.Any()))
            .GroupBy(o => o.Value.Tags.First().Name.ToPascalCase())
            .Where(m => m.Key != "Null" && (_config.Include == null || _config.Include.Contains(m.Key)));

        var referenceMap = modules.ToDictionary(m => m.Key, m => GetModuleReferences(m));

        var modelFileName = $"{Path.Combine(ModelRoot, _config.OutputDirectory, _config.ModelFileName)}.tmd";
        yield return modelFileName;

        using var fw = new FileWriter(modelFileName, _logger, false) { StartCommentToken = "####" };
        fw.WriteLine("---");
        fw.WriteLine($"module: {_config.Module}");
        fw.WriteLine("tags:");
        foreach (var tag in _config.ModelTags)
        {
            fw.WriteLine($"  - {tag}");
        }
        fw.WriteLine();

        var references = new HashSet<string>(referenceMap.SelectMany(r => r.Value).Select(r => r.Id).Distinct());
        foreach (var schema in _model.Components.Schemas.Where(s => s.Value.Type == "object").Where(s => references.Contains(s.Key)).OrderBy(s => s.Key))
        {
            fw.WriteLine("---");
            fw.WriteLine("class:");
            fw.WriteLine($"  name: {schema.Key}");

            if (_config.PreservePropertyCasing)
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
                    WriteProperty(_config, fw, property, _model);
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

        foreach (var schema in _model.Components.Schemas.Where(sh => sh.Value.Properties.Where(p => (p.Value.Enum?.Any() ?? false)).Any()))
        {
            foreach (var property in schema.Value.Properties.Where(p => (p.Value.Enum?.Any() ?? false)))
            {
                fw.WriteLine("---");
                fw.WriteLine("class:");
                fw.WriteLine($"  name: {schema.Key.ToPascalCase()}{property.Key.ToPascalCase()}");

                if (_config.PreservePropertyCasing)
                {
                    fw.WriteLine($"  preservePropertyCasing: true");
                }

                fw.WriteLine($"  comment: enum pour les valeurs de {property.Key.ToPascalCase()}");

                fw.WriteLine();
                fw.WriteLine($"  properties:");

                WriteProperty(_config, fw, property, _model);
                fw.WriteLine();
                fw.WriteLine($"  values:");
                var u = 0;

                foreach (var val in property.Value.Enum.OfType<OpenApiString>())
                {
                    fw.WriteLine($@"    value{u++}: {{ {property.Key}: {val.Value} }}");
                }

                foreach (var val in property.Value.Enum.OfType<OpenApiInteger>())
                {
                    fw.WriteLine($@"    value{u++}: {{ {property.Key}: {val.Value} }}");
                }
            }
        }

        foreach (var module in modules)
        {
            var endpointFileName = $"{Path.Combine(ModelRoot, _config.OutputDirectory, module.Key)}.tmd";
            yield return endpointFileName;

            using var sw = new FileWriter(endpointFileName, _logger, false) { StartCommentToken = "####" };
            sw.WriteLine("---");
            sw.WriteLine($"module: {_config.Module}");
            sw.WriteLine("tags:");
            foreach (var tag in _config.EndpointTags)
            {
                sw.WriteLine($"  - {tag}");
            }
            if (referenceMap[module.Key].Any())
            {
                sw.WriteLine("uses:");
                var use = $"{_config.OutputDirectory.Replace("\\", "/")}/{_config.ModelFileName}".Replace("//", "/");
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

                if (_config.PreservePropertyCasing)
                {
                    sw.WriteLine($"  preservePropertyCasing: true");
                }

                if (operation.Value.Parameters.Any(p => p.In == ParameterLocation.Query || p.In == ParameterLocation.Path) || operation.Value.RequestBody != null)
                {
                    sw.WriteLine("  params:");

                    var bodySchema = GetRequestBodySchema(operation.Value);
                    if (bodySchema != null)
                    {
                        WriteProperty(_config, sw, new("body", bodySchema), _model);
                    }

                    foreach (var param in operation.Value.Parameters.Where(p => p.In == ParameterLocation.Query || p.In == ParameterLocation.Path).OrderBy(p => path.Contains($@"{{{p.Name}}}") ? 0 + p.Name : 1 + p.Name))
                    {
                        sw.WriteLine($"    - name: {param.Name}");
                        sw.WriteLine($"      domain: {GetDomain(_config, param.Name, param.Schema)}");
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

                var responseSchema = GetResponseSchema(operation.Value).Value;
                if (responseSchema != null)
                {
                    sw.WriteLine("  returns:");
                    WriteProperty(_config, sw, new("Result", responseSchema), _model, noList: true);
                }
            }
        }
    }

    private static string FormatDescription(string description)
    {
        if (description == null)
        {
            return string.Empty;
        }

        description = description.Replace(Environment.NewLine, " ");

        return @$"""{description}""";
    }

    private static (string? Kind, string? Name) GetComposition(OpenApiSchema schema, OpenApiDocument model)
    {
        return (
            schema.Items?.Reference != null
                ? "list"
                : schema.Reference != null && model.Components.Schemas.Any(s => s.Value.Type == "object" && s.Value.Reference == schema.Reference)
                ? "object"
                : schema.Type == "object" && schema.AdditionalProperties?.Reference != null
                ? "map"
                : schema.Type == "object" && schema.AdditionalProperties?.Items?.Reference != null
                ? "list-map"
                : null,
            schema?.AdditionalProperties?.Items?.Reference.Id ?? schema?.AdditionalProperties?.Reference?.Id ?? schema?.Items?.Reference?.Id ?? schema?.Reference?.Id);
    }

    private static string GetDomain(OpenApiConfig config, string name, OpenApiSchema schema)
    {
        var resolvedDomain = TmdGenUtils.GetDomainString(config.Domains, name);
        if (resolvedDomain == name)
        {
            return GetDomainSchema(config, schema);
        }
        return resolvedDomain;
    }

    private static string GetDomainCore(OpenApiSchema schema)
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

    private static string GetDomainSchema(OpenApiConfig config, OpenApiSchema schema)
    {
        var domain = GetDomainCore(schema);
        return TmdGenUtils.GetDomainString(config.Domains, domain);
    }

    private static OpenApiSchema? GetRequestBodySchema(OpenApiOperation operation)
    {
        return operation.RequestBody?.Content.First().Value.Schema;
    }

    private static void WriteProperty(OpenApiConfig config, FileWriter sw, KeyValuePair<string, OpenApiSchema> property, OpenApiDocument model, bool noList = false)
    {
        var (kind, name) = GetComposition(property.Value, model);

        if (kind != null && name != null)
        {
            var domainKind = TmdGenUtils.GetDomainString(config.Domains, kind);
            sw.WriteLine($"    {(noList ? string.Empty : "- ")}composition: {name}");
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

    private string GetOperationId(KeyValuePair<OperationType, OpenApiOperation> operation)
    {
        if (operation.Value.OperationId != null)
        {
            return operation.Value.OperationId;
        }

        var path = GetOperationPath(operation.Value).Replace("api/", string.Empty).Trim('/');

        if (!path.Contains('/'))
        {
            return path.ToPascalCase();
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
            if (bodySchema != null)
            {
                var body = GetComposition(bodySchema, _model);
                id += body.Name;

                if (body.Kind != null && body.Kind != "object")
                {
                    id += body.Kind.ToPascalCase();
                }
            }
        }

        return id;
    }

    private string GetOperationPath(OpenApiOperation operation)
    {
        return _model.Paths.Single(p => p.Value.Operations.Any(o => o.Value == operation)).Key[1..];
    }

    private string GetEndpointName(KeyValuePair<OperationType, OpenApiOperation> operation)
    {
        var operationId = GetOperationId(operation);
        var operationsWithId = _model!.Paths.OrderBy(p => p.Key).SelectMany(p => p.Value.Operations.OrderBy(o => o.Key))
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

    private IEnumerable<OpenApiReference> GetModuleReferences(IEnumerable<KeyValuePair<OperationType, OpenApiOperation>> operations)
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

    private KeyValuePair<string, OpenApiSchema> GetResponseSchema(OpenApiOperation operation)
    {
        var response = operation.Responses.FirstOrDefault(r => r.Key == "200").Value;
        if (response != null && response.Content.Any())
        {
            return new(_model.Components.Schemas.FirstOrDefault(s => s.Value == response.Content.First().Value.Schema).Key, response.Content.First().Value.Schema);
        }

        return default;
    }

    private IEnumerable<OpenApiReference> GetSchemaReferences(OpenApiSchema schema, HashSet<OpenApiSchema> visited)
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
}