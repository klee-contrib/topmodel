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

if (config.OpenApi is null)
{
    return;
}

using var loggerFactory = LoggerFactory.Create(l => l.AddSimpleConsole(c => c.SingleLine = true));
var logger = loggerFactory.CreateLogger("open-api");

foreach (var source in config.OpenApi.Sources)
{
    var client = new HttpClient();

    if (!string.IsNullOrEmpty(source.Login))
    {
        if (!secrets.TryGetValue(source.Login, out var secret))
        {
            throw new Exception($"Pas de mot de passe associé au login '{source.Login}'");
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{source.Login}:{secret}")));
    }

    var openApi = await client.GetAsync(source.Url);

    var modelReader = new OpenApiStreamReader();

    var model = modelReader.Read(await openApi.Content.ReadAsStreamAsync(), out var diagnostic);

    Directory.CreateDirectory(config.OutputDirectory);
    using var sw = new FileWriter($"{config.OutputDirectory}/OpenApi.tmd", logger, false) { StartCommentToken = "####" };
    sw.WriteLine("---");
    sw.WriteLine("module: OpenApi");
    sw.WriteLine("tags:");
    sw.WriteLine("  - OpenApi");
    sw.WriteLine();

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

    foreach (var path in model.Paths.OrderBy(p => p.Key))
    {
        foreach (var operation in path.Value.Operations.OrderBy(o => o.Key))
        {
            sw.WriteLine("---");
            sw.WriteLine("endpoint:");
            sw.WriteLine($"  name: {GetEndpointName(operation.Value)}");
            sw.WriteLine($"  method: {operation.Key.ToString().ToUpper()}");
            sw.WriteLine($"  route: {path.Key[1..]}");
            sw.WriteLine($"  description: {FormatDescription(operation.Value.Summary)}");

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
                    sw.WriteLine($"      comment: {FormatDescription(param.Description)}");
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

    foreach (var schema in model.Components.Schemas.OrderBy(s => s.Key))
    {
        sw.WriteLine("---");
        sw.WriteLine("class:");
        sw.WriteLine($"  name: {schema.Key}");
        sw.WriteLine($"  comment: {FormatDescription(schema.Value.Description ?? schema.Key)}");
        sw.WriteLine();
        sw.WriteLine($"  properties:");

        foreach (var property in schema.Value.Properties)
        {
            WriteProperty(sw, property);

            if (schema.Value.Properties.Last().Key != property.Key)
            {
                sw.WriteLine();
            }
        }
    }
}

static string FormatDescription(string description)
{
    description = description.Replace(Environment.NewLine, " ");

    return description.Contains(':')
        ? $"\"{description}\""
        : description;
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