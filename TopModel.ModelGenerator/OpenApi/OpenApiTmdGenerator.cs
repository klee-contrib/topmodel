using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Reader;
using TopModel.Utils;

namespace TopModel.ModelGenerator.OpenApi;

public class OpenApiTmdGenerator : TmdGenerator
{
    private readonly Dictionary<OpenApiSchema, TmdClass> _classesStore = [];
    private readonly OpenApiConfig _config;
    private readonly ILogger<OpenApiTmdGenerator> _logger;
    private readonly IFileWriterProvider _writerProvider;

#nullable disable
    private OpenApiDocument _model;

#nullable enable

    public OpenApiTmdGenerator(
        ILogger<OpenApiTmdGenerator> logger,
        OpenApiConfig config,
        IFileWriterProvider writerProvider
    )
        : base(logger)
    {
        _config = config;
        _logger = logger;
        _writerProvider = writerProvider;

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

    protected override async IAsyncEnumerable<string> GenerateCore(
        [EnumeratorCancellation] CancellationToken ct = default
    )
    {
        if (_config.Source.StartsWith("http://") || _config.Source.StartsWith("https://"))
        {
            using var client = new HttpClient();
            var openApi = await client.GetAsync(_config.Source, ct);
            var settings = new OpenApiReaderSettings { LeaveStreamOpen = false };
            settings.AddYamlReader();
            _model = (
                await OpenApiDocument.LoadAsync(
                    (MemoryStream)await openApi.Content.ReadAsStreamAsync(ct),
                    "yaml",
                    settings,
                    ct
                )
            ).Document!;
        }
        else
        {
#pragma warning disable S1075
            using var stream = File.Open(DirectoryName + "/" + _config.Source, FileMode.Open);
#pragma warning restore S1075
            string s = string.Empty;
            using (var reader = new StreamReader(stream))
            {
                var fileContent = await reader.ReadToEndAsync(ct);
                s += fileContent;
            }

            var settings = new OpenApiReaderSettings { LeaveStreamOpen = false };
            settings.AddYamlReader();
            _model = OpenApiDocument.Parse(s, "yaml", settings).Document!;
        }

        var modules = _model
            .Paths.SelectMany(p =>
                p.Value.Operations?.Where(o =>
                    o.Value.Tags?.Any(t => t.Name != null || t.Reference.Id != null) ?? false
                )
                ?? []
            )
            .GroupBy(o => (o.Value.Tags?.First().Name ?? o.Value.Tags?.First().Reference.Id ?? "Null").ToPascalCase())
            .Where(m =>
                m.Key != "Null"
                && (_config.Include == null || _config.Include.Select(i => i.ToPascalCase()).Contains(m.Key))
            );

        var modelFileName = $"{Path.Combine(ModelRoot, _config.OutputDirectory, _config.ModelFileName)}.tmd";
        yield return modelFileName;

        var tmdFile = new TmdFile()
        {
            Module = _config.Module,
            Name = _config.ModelFileName,
            Tags = _config.ModelTags.ToList(),
            Path = Path.Combine(_config.OutputDirectory, _config.ModelFileName),
        };

        using var tmdFileWriter = new TmdWriter(
            _writerProvider.OpenFileWriter(modelFileName, _logger),
            tmdFile,
            Path.GetFullPath(ModelRoot)
        );
        var schemaReferences = GetModuleReferences(modules.SelectMany(m => m));
        var schemas = _model.GetSchemas().Where(s => schemaReferences.Contains(s.Value)).ToList();
        foreach (var schema in schemas)
        {
            if (!_classesStore.ContainsKey(schema.Value))
            {
                _classesStore[schema.Value] = new TmdClass();
            }
        }

        foreach (var schema in schemas.OrderBy(r => r.Key))
        {
            if (schema.Value.Type == JsonSchemaType.String && (schema.Value.Enum?.Any() ?? false))
            {
                var classes = tmdFile.Classes.Select(c => c.Name);
                if (classes.Contains($"{_config.ClassPrefix}{schema.Key.ToPascalCase()}"))
                {
                    continue;
                }

                var enumClass = _classesStore[schema.Value];
                enumClass.File = tmdFile;
                enumClass.Name = $"{_config.ClassPrefix}{schema.Key.ToPascalCase()}";
                enumClass.Comment = $"enum pour les valeurs de {schema.Key.ToPascalCase()}";
                enumClass.PreservePropertyCasing = _config.PreservePropertyCasing;

                tmdFile.Classes.Add(enumClass);

                var p = WriteProperty(_config, new("Value", schema.Value), schema: null, tmdFile);
                p.Class = enumClass;
                enumClass.Properties.Add(p);
                AddValues(enumClass, schema.Value);
                enumClass.Unique.Add(["Value"]);
            }
            else
            {
                var classe = _classesStore[schema.Value];
                classe.File = tmdFile;
                classe.PreservePropertyCasing = _config.PreservePropertyCasing;

                var className = schema.Value.Type == JsonSchemaType.Array ? schema.Key.Unplurialize() : schema.Key;
                classe.Name = $"{_config.ClassPrefix}{className.ToPascalCase()}";
                var classes = tmdFile.Classes.Select(c => c.Name);
                if (classes.Contains(classe.Name))
                {
                    continue;
                }

                var parents = schemas.Where(s =>
                    (s.Value.AnyOf ?? []).Contains(schema.Value) || (s.Value.OneOf ?? []).Contains(schema.Value)
                );
                if (parents.Count() == 1)
                {
                    classe.Extends = parents.Single().Key.ToPascalCase();
                }

                tmdFile.Classes.Add(classe);

                if (!string.IsNullOrEmpty(schema.Value.Description?.Trim(' ')))
                {
                    classe.Comment = schema.Value.Description.Format();
                }

                var classeProperties = classe.Properties;
                var properties = schema.Value.GetProperties().ToList();

                foreach (var property in properties)
                {
                    if (!(property.Value.Enum?.Any() ?? false))
                    {
                        var p = WriteProperty(_config, property, schema.Value, tmdFile);
                        p.Class = classe;
                        classeProperties.Add(p);
                    }
                    else
                    {
                        var enumClass = tmdFile.Classes.SingleOrDefault(c =>
                            c.Name == $"{classe.Name}{property.Key.ToPascalCase()}"
                        );
                        if (enumClass == null)
                        {
                            enumClass = new TmdClass()
                            {
                                File = tmdFile,
                                Name = $"{classe.Name}{property.Key.ToPascalCase()}",
                                Comment = $"enum pour les valeurs de {property.Key.ToPascalCase()}",
                                PreservePropertyCasing = _config.PreservePropertyCasing,
                            };

                            tmdFile.Classes.Add(enumClass);
                            var p = WriteProperty(_config, new("Value", property.Value), schema: null, tmdFile);
                            p.Class = enumClass;
                            enumClass.Properties.Add(p);
                            AddValues(enumClass, property.Value);
                            enumClass.Unique.Add(["Value"]);
                        }

                        classeProperties.Add(
                            new TmdAliasProperty()
                            {
                                Alias = enumClass.Properties[0],
                                Name = $"{property.Key}",
                                Comment = @$"{property.Value.Description.Format()}",
                                Class = classe,
                            }
                        );
                    }
                }
            }
        }

        foreach (var cp in tmdFile.Classes.SelectMany(c => c.Properties.OfType<TmdCompositionProperty>()))
        {
            var composition = _classesStore.FirstOrDefault(c => c.Key == cp.CompositionReference);
            cp.Composition = composition.Value;
        }

        foreach (var module in modules)
        {
            var endpointFileName = $"{Path.Combine(ModelRoot, _config.OutputDirectory, module.Key)}.tmd";
            yield return endpointFileName;

            var tmdFileEnpoint = new TmdFile()
            {
                Module = _config.Module,
                Name = module.Key,
                Tags = _config.EndpointTags.ToList(),
                Path = Path.Combine(_config.OutputDirectory, module.Key),
            };

            using var tmdEndpointFileWriter = new TmdWriter(
                _writerProvider.OpenFileWriter(endpointFileName, _logger),
                tmdFileEnpoint,
                ModelRoot
            );

            foreach (var operation in module.OrderBy(o => GetEndpointName(o)))
            {
                var path = _model.GetOperationPath(operation.Value);
                var endPoint = new TmdEndpoint()
                {
                    Name = GetEndpointName(operation),
                    Method = operation.Key.ToString().ToUpper(),
                    Route = path,
                    File = tmdFileEnpoint,
                };
                tmdFileEnpoint.Endpoints.Add(endPoint);
                if (
                    !string.IsNullOrEmpty(operation.Value.Summary) || !string.IsNullOrEmpty(operation.Value.Description)
                )
                {
                    endPoint.Comment = (operation.Value.Summary ?? operation.Value.Description)?.Format(quote: true)!;
                }

                endPoint.PreservePropertyCasing = _config.PreservePropertyCasing;

                if (
                    (
                        operation.Value.Parameters?.Any(p =>
                            p.In == ParameterLocation.Query || p.In == ParameterLocation.Path
                        ) ?? false
                    )
                    || operation.Value.RequestBody != null
                )
                {
                    var bodySchema = operation.Value.GetRequestBodySchema();
                    if (bodySchema != null)
                    {
                        var p = WriteProperty(_config, new("body", bodySchema), bodySchema, tmdFile);
                        if (p is TmdCompositionProperty cp)
                        {
                            cp.Composition = _classesStore.SingleOrDefault(c => c.Key == cp.CompositionReference).Value;
                        }

                        if (p.Comment == TmdProperty.DefaultComment)
                        {
                            var description = operation.Value.RequestBody?.Description.Format();
                            if (!string.IsNullOrEmpty(description))
                            {
                                p.Comment = description;
                            }
                        }

                        endPoint.Params.Add(p);
                    }

                    foreach (
                        var param in (operation.Value.Parameters ?? [])
                            .Where(p => p.In == ParameterLocation.Query || p.In == ParameterLocation.Path)
                            .OrderBy(p => path.Contains($@"{{{p.Name}}}") ? 0 + p.Name : 1 + p.Name)
                    )
                    {
                        TmdProperty property;
                        if ((param.Schema?.Enum ?? []).Any())
                        {
                            var enumClass = tmdFile.Classes.SingleOrDefault(c =>
                                c.Name == $"{endPoint.Name.ToPascalCase()}{param.Name?.ToPascalCase()}"
                            );
                            if (enumClass == null)
                            {
                                enumClass = new TmdClass()
                                {
                                    File = tmdFile,
                                    Name = $"{endPoint.Name.ToPascalCase()}{param.Name?.ToPascalCase()}",
                                    Comment = $"enum pour les valeurs de {param.Name}",
                                    PreservePropertyCasing = _config.PreservePropertyCasing,
                                };

                                tmdFile.Classes.Add(enumClass);
                                var p = WriteProperty(_config, new("Value", param.Schema!), schema: null, tmdFile);
                                p.Class = enumClass;
                                enumClass.Properties.Add(p);
                                AddValues(enumClass, param.Schema!);
                                enumClass.Unique.Add(["Value"]);
                            }

                            property = new TmdAliasProperty()
                            {
                                Alias = enumClass.Properties[0],
                                Name = param.Name,
                                Class = enumClass,
                            };
                        }
                        else
                        {
                            property = new TmdRegularProperty()
                            {
                                Name = param.Name,
                                Domain = _config.GetDomain(param.Name!, param.Schema!),
                            };
                        }

                        endPoint.Params.Add(property);
                        if (!string.IsNullOrEmpty(param.Description?.Trim(' ')))
                        {
                            property.Comment = $@"{param.Description.Format()}";
                        }
                    }
                }

                var responseSchema = _model.GetResponseSchema(operation.Value).Value;
                if (responseSchema != null)
                {
                    var returns = WriteProperty(_config, new("Result", responseSchema), responseSchema, tmdFile);
                    if (returns is TmdCompositionProperty cp)
                    {
                        cp.Composition = _classesStore.SingleOrDefault(c => c.Key == cp.CompositionReference).Value;
                    }

                    var description = operation
                        .Value.Responses?.FirstOrDefault(r => r.Key == "200" || r.Key == "201")
                        .Value.Description?.Format();
                    if (!string.IsNullOrEmpty(description))
                    {
                        returns.Comment = description;
                    }

                    endPoint.Returns = returns;
                }
            }
        }
    }

    private static void AddValues(TmdClass classe, IOpenApiSchema schema)
    {
        foreach (var val in schema.Enum ?? [])
        {
            Dictionary<string, string?> value = new() { { classe.Properties[0].Name, val.AsValue().ToString() } };
            classe.Values.Add(value);
        }
    }

    private static IEnumerable<OpenApiSchema> GetModuleReferences(
        IEnumerable<KeyValuePair<HttpMethod, OpenApiOperation>> operations
    )
    {
        var visited = new HashSet<IOpenApiSchema>();
        foreach (var operation in operations)
        {
            if (operation.Value.RequestBody?.Content != null)
            {
                foreach (
                    var reference in GetSchemaReferences(
                        operation.Value.RequestBody.Content.First().Value.Schema,
                        visited
                    )
                )
                {
                    yield return reference;
                }
            }

            foreach (
                var reference in operation.Value.Parameters?.SelectMany(p => GetSchemaReferences(p.Schema, visited))
                    ?? []
            )
            {
                yield return reference;
            }

            foreach (
                var response in operation
                    .Value.Responses?.Where(r => r.Key == "200" || r.Key == "201")
                    .Select(r => r.Value)
                    ?? []
            )
            {
                if (response != null && (response.Content?.Any() ?? false))
                {
                    foreach (var reference in GetSchemaReferences(response.Content.First().Value.Schema, visited))
                    {
                        yield return reference;
                    }
                }
            }
        }
    }

    private static IEnumerable<OpenApiSchema> GetSchemaReferences(
        IOpenApiSchema? schema,
        HashSet<IOpenApiSchema> visited
    )
    {
        if (schema == null)
        {
            yield break;
        }

        visited.Add(schema);

        if (schema is OpenApiSchemaReference sr)
        {
            foreach (var reference in GetSchemaReferences(sr.Target, visited))
            {
                yield return reference;
            }
        }
        else if (schema is OpenApiSchema sc)
        {
            yield return sc;
        }

        if (schema.Items != null && !visited.Contains(schema.Items))
        {
            foreach (var reference in GetSchemaReferences(schema.Items, visited))
            {
                yield return reference;
            }
        }

        foreach (
            var reference in schema
                .GetProperties()
                .Values.Where(p => !visited.Contains(p))
                .SelectMany(p => GetSchemaReferences(p, visited))
        )
        {
            yield return reference;
        }

        if (schema.AdditionalProperties != null && !visited.Contains(schema.AdditionalProperties))
        {
            foreach (var reference in GetSchemaReferences(schema.AdditionalProperties, visited))
            {
                yield return reference;
            }
        }

        foreach (var oneOff in schema.OneOf ?? [])
        {
            foreach (var reference in GetSchemaReferences(oneOff, visited))
            {
                yield return reference;
            }
        }

        foreach (var anyOff in schema.AnyOf ?? [])
        {
            foreach (var reference in GetSchemaReferences(anyOff, visited))
            {
                yield return reference;
            }
        }
    }

    private string GetEndpointName(KeyValuePair<HttpMethod, OpenApiOperation> operation)
    {
        var operationId = _model.GetOperationId(operation);
        var operationsWithId = _model
            .Paths.OrderBy(p => p.Key)
            .SelectMany(p => (p.Value.Operations ?? []).OrderBy(o => o.Key.Method))
            .Where(o => _model.GetOperationId(o) == operationId)
            .ToList();

        if (operationsWithId.Count == 1)
        {
            return operationId;
        }

        var prefix =
            operationsWithId.DistinctBy(o => o.Key).Count() > 1
                ? operation.Key.Method.ToPascalCase(strictIfUppercase: true)
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

    private TmdProperty WriteProperty(
        OpenApiConfig config,
        KeyValuePair<string, IOpenApiSchema> property,
        IOpenApiSchema? schema,
        TmdFile tmdFile
    )
    {
        var (kind, sc) = OpenApiUtils.GetComposition(property.Value);
        if (
            property.Value.Type == JsonSchemaType.Array
            && (property.Value.Items?.Enum ?? []).Any()
            && property.Value.Items?.Type == JsonSchemaType.String
        )
        {
            var aliasClass = tmdFile.Classes.SingleOrDefault(c =>
                c.Name == $"{_config.ClassPrefix}{property.Key.ToPascalCase()}"
            );
            if (aliasClass == null)
            {
                aliasClass = new TmdClass()
                {
                    File = tmdFile,
                    Name = $"{_config.ClassPrefix}{property.Key.ToPascalCase()}",
                    Comment = $"enum pour les valeurs de {property.Key.ToPascalCase()}",
                    PreservePropertyCasing = _config.PreservePropertyCasing,
                };

                tmdFile.Classes.Add(aliasClass);

                var p = WriteProperty(_config, new("Value", property.Value.Items), schema: null, tmdFile);
                p.Class = aliasClass;
                aliasClass.Properties.Add(p);
                AddValues(aliasClass, property.Value.Items);
            }

            var aliasProperty = new TmdAliasProperty()
            {
                Name = $"{property.Key}",
                Required = schema?.Required?.Contains(property.Key) ?? false,
                Domain = $"{config.GetDomain(property.Key, property.Value)}",
                Alias = aliasClass.Properties[0],
                As = "list",
            };

            if (!string.IsNullOrEmpty(property.Value.Description?.Trim(' ')))
            {
                aliasProperty.Comment = $"{property.Value.Description.Format()}";
            }

            return aliasProperty;
        }

        static OpenApiSchema? GetTarget(OpenApiSchemaReference schema)
        {
            var target = schema.Target;
            while (target is OpenApiSchemaReference oasRef)
            {
                target = oasRef.Target;
            }
            return target as OpenApiSchema;
        }

        if (
            sc != null
            && property.Value.Type != JsonSchemaType.String
            && (
                sc is OpenApiSchema oas && _classesStore.ContainsKey(oas)
                || sc is OpenApiSchemaReference oasRef
                    && GetTarget(oasRef) != null
                    && _classesStore.ContainsKey(GetTarget(oasRef)!)
            )
        )
        {
            var sch = sc is OpenApiSchemaReference scr ? GetTarget(scr) : sc;
            var compositionProperty = new TmdCompositionProperty()
            {
                Name = $"{property.Key}",
                Required = schema?.Required?.Contains(property.Key) ?? false,
            };
            var domainKind = TmdGenUtils.GetDomainString(config.Domains, type: kind);
            if (kind != "object")
            {
                compositionProperty.Domain = $"{domainKind ?? kind}";
            }

            if (!string.IsNullOrEmpty(property.Value.Description?.Trim(' ')))
            {
                compositionProperty.Comment = $"{property.Value.Description.Format()}";
            }

            compositionProperty.CompositionReference = sch;
            return compositionProperty;
        }

        var regularProperty = new TmdRegularProperty()
        {
            Name = $"{property.Key}",
            Required = schema?.Required?.Contains(property.Key) ?? false,
            Domain = $"{config.GetDomain(property.Key, property.Value)}",
        };

        if (!string.IsNullOrEmpty(property.Value.Description?.Trim(' ')))
        {
            regularProperty.Comment = $"{property.Value.Description.Format()}";
        }

        return regularProperty;
    }
}
