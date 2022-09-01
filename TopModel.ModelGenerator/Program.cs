using System.CommandLine;
using System.CommandLine.Invocation;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using SharpYaml.Serialization;
using TopModel.ModelGenerator;
using TopModel.Utils;

ModelGeneratorConfig config = null!;
var secrets = new Dictionary<string, string>();
string dn = null!;

var command = new RootCommand
{
    new Argument<FileInfo>("configFile", () => new FileInfo("topmodel.yaml"), "Chemin vers le fichier de config."),
    new Argument<FileInfo>("secretsFile", () => new FileInfo("topmodel.secrets"), "Chemin vers le fichier de secrets.")
};

command.Name = "modmodgen";

command.Handler = CommandHandler.Create<FileInfo, FileInfo>((configFile, secretsFile) =>
{
    if (!configFile.Exists)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Le fichier '{configFile.FullName}' est introuvable.");
    }
    else
    {
        config = new Serializer().Deserialize<ModelGeneratorConfig>(configFile.OpenText().ReadToEnd());
        dn = configFile.DirectoryName!;
    }

    if (secretsFile.Exists)
    {
        secrets = File.ReadAllLines(secretsFile.FullName).ToDictionary(l => l.Split(':')[0], l => l.Split(':')[1]);
    }
});

command.Invoke(args);

ModelUtils.CombinePath(dn, config, c => c.OutputDirectory);
ModelUtils.CombinePath(dn, config, c => c.ModelRoot);

if (config.OpenApi is null)
{
    return;
}

using var loggerFactory = LoggerFactory.Create(l => l.AddSimpleConsole(c => c.SingleLine = true));
var logger = loggerFactory.CreateLogger("open-api");

foreach (var source in config.OpenApi.Sources)
{
    var client = new HttpClient();

    if (!string.IsNullOrEmpty(source.Value.Login))
    {
        if (!secrets.TryGetValue(source.Value.Login, out var secret))
        {
            throw new Exception($"Pas de mot de passe associé au login '{source.Value.Login}'");
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{source.Value.Login}:{secret}")));
    }

    OpenApiDocument model;
    var modelReader = new OpenApiStreamReader();
    if (source.Value.Url != null)
    {
        var openApi = await client.GetAsync(source.Value.Url);
        model = modelReader.Read(await openApi.Content.ReadAsStreamAsync(), out var diagnostic);
    }
    else
    {
        model = modelReader.Read(File.Open(dn + "/" + source.Value.Path!, FileMode.Open), out var diagnostic);
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
        .GroupBy(o => o.Value.Tags.First().Name.ToFirstUpper())
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

    using var msw = new FileWriter($"{config.OutputDirectory}/{source.Key}/{source.Value.ModelFileName}.tmd", logger, false) { StartCommentToken = "####" };
    msw.WriteLine("---");
    msw.WriteLine($"module: {source.Key}");
    msw.WriteLine("tags:");
    foreach (var tag in source.Value.Tags)
    {
        msw.WriteLine($"  - {tag}");
    }
    msw.WriteLine();

    var references = new HashSet<string>(referenceMap.SelectMany(r => r.Value).Select(r => r.Id).Distinct());
    foreach (var schema in model.Components.Schemas.Where(s => references.Contains(s.Key)).OrderBy(s => s.Key))
    {
        msw.WriteLine("---");
        msw.WriteLine("class:");
        msw.WriteLine($"  name: {schema.Key}");
        if (schema.Value.Description != null)
        {
            msw.WriteLine($"  comment: {FormatDescription(schema.Value.Description ?? schema.Key)}");
        }
        else
        {
            msw.WriteLine($"  comment: no description provided");
        }

        msw.WriteLine();
        msw.WriteLine($"  properties:");

        foreach (var property in schema.Value.Properties)
        {
            WriteProperty(msw, property);

            if (schema.Value.Properties.Last().Key != property.Key)
            {
                msw.WriteLine();
            }
        }
    }

    foreach (var module in modules)
    {
        using var sw = new FileWriter($"{config.OutputDirectory}/{source.Key}/{module.Key}.tmd", logger, false) { StartCommentToken = "####" };
        sw.WriteLine("---");
        sw.WriteLine($"module: {source.Key}");
        sw.WriteLine("tags:");
        foreach (var tag in source.Value.Tags)
        {
            sw.WriteLine($"  - {tag}");
        }
        if (referenceMap[module.Key].Any())
        {
            sw.WriteLine("uses:");
            sw.WriteLine($"  - {Path.GetRelativePath(config.ModelRoot, config.OutputDirectory).Replace("\\", "/")}/{source.Key}/{source.Value.ModelFileName}");

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
                    WriteProperty(sw, new("Body", operation.Value.RequestBody.Content.First().Value.Schema));
                }

                foreach (var param in operation.Value.Parameters)
                {
                    sw.WriteLine($"    - name: {param.Name.ToFirstUpper()}");
                    sw.WriteLine($"      domain: {GetDomain(param.Schema)}");
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
                WriteProperty(sw, new("Result", response.Content.First().Value.Schema), noList: true);
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

string GetDomain(OpenApiSchema schema)
{
    var domain = GetDomainCore(schema);
    return config.OpenApi.Domains.TryGetValue(domain, out var result) ? result : domain;
}

string GetDomainCore(OpenApiSchema schema)
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

void WriteProperty(FileWriter sw, KeyValuePair<string, OpenApiSchema> property, bool noList = false)
{
    var kind =
        property.Value.Items?.Reference != null ? "list"
        : property.Value.Reference != null ? "object"
        : property.Value.Type == "object" && property.Value.AdditionalProperties?.Reference != null ? "map"
        : property.Value.Type == "object" && property.Value.AdditionalProperties?.Items?.Reference != null ? "list-map"
        : null;

    if (kind != null)
    {
        config.OpenApi.Domains.TryGetValue(kind, out var domainKind);
        sw.WriteLine($"    {(noList ? string.Empty : "- ")}composition: {property.Value.AdditionalProperties?.Items?.Reference.Id ?? property.Value.AdditionalProperties?.Reference.Id ?? property.Value.Items?.Reference.Id ?? property.Value.Reference!.Id}");
        sw.WriteLine($"    {(noList ? string.Empty : "  ")}name: {property.Key.ToFirstUpper()}");
        sw.WriteLine($"    {(noList ? string.Empty : "  ")}kind: {domainKind ?? kind}");
    }
    else
    {
        sw.WriteLine($"    {(noList ? string.Empty : "- ")}name: {property.Key.ToFirstUpper()}");
        sw.WriteLine($"    {(noList ? string.Empty : "  ")}domain: {GetDomain(property.Value)}");
        sw.WriteLine($"    {(noList ? string.Empty : "  ")}required: {(!property.Value.Nullable).ToString().ToLower()}");
    }

    sw.WriteLine($"    {(noList ? string.Empty : "  ")}comment: {FormatDescription(property.Value.Description ?? property.Key)}");
}