using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Csharp;

public class CSharpApiClientGenerator : EndpointsGeneratorBase<CsharpConfig>
{
    private readonly ILogger<CSharpApiClientGenerator> _logger;

    public CSharpApiClientGenerator(ILogger<CSharpApiClientGenerator> logger)
        : base(logger)
    {
        _logger = logger;
    }

    public override string Name => "CSharpApiClientGen";

    protected override bool FilterTag(string tag)
    {
        return Config.ResolveVariables(Config.ApiGeneration!, tag) == ApiGeneration.Client;
    }

    protected override string GetFilePath(ModelFile file, string tag)
    {
        return Path.Combine(Config.GetApiPath(file, tag), "generated", $"{file.Options.Endpoints.FileName.ToPascalCase()}Client.cs");
    }

    protected override void HandleFile(string filePath, string fileName, string tag, IList<Endpoint> endpoints)
    {
        using var fw = new CSharpWriter(filePath, _logger);

        var hasBody = endpoints.Any(e => e.GetBodyParam() != null);
        var hasReturn = endpoints.Any(e => e.Returns != null);
        var hasJson = hasReturn || hasBody;

        var usings = new List<string>();

        if (hasBody)
        {
            usings.Add("System.Text");
        }

        if (hasReturn)
        {
            usings.Add("System.Net");
        }

        if (hasJson)
        {
            usings.Add("System.Text.Json");
            usings.Add("System.Text.Json.Serialization");
        }

        if (endpoints.Any(e => e.GetQueryParams().Any()))
        {
            if (endpoints.Any(e => e.GetQueryParams().Any(qp =>
            {
                var typeName = Config.GetType(qp);
                return !typeName.StartsWith("string") && !typeName.StartsWith("Guid");
            })))
            {
                usings.Add("System.Globalization");
            }
        }

        foreach (var property in endpoints.SelectMany(e => e.Params.Concat(new[] { e.Returns! }).Where(p => p != null)))
        {
            usings.AddRange(Config.GetDomainImports(property, tag));

            if (property is IFieldProperty fp && fp.IsQueryParam())
            {
                usings.AddRange(Config.GetValueImports(fp));
            }

            switch (property)
            {
                case AssociationProperty ap when Config.CanClassUseEnums(ap.Association, Classes):
                    usings.Add(GetNamespace(ap.Association, tag));
                    break;
                case AliasProperty { Property: AssociationProperty ap2 } when Config.CanClassUseEnums(ap2.Association, Classes):
                    usings.Add(GetNamespace(ap2.Association, tag));
                    break;
                case AliasProperty { PrimaryKey: false, Property: RegularProperty { PrimaryKey: true } rp } when Config.CanClassUseEnums(rp.Class, Classes):
                    usings.Add(GetNamespace(rp.Class, tag));
                    break;
                case CompositionProperty cp:
                    usings.Add(GetNamespace(cp.Composition, tag));
                    break;
            }
        }

        var className = $"{fileName.ToPascalCase()}Client";
        var ns = Config.GetNamespace(endpoints.First(), tag);

        fw.WriteUsings(usings.Distinct().Where(u => u != ns).ToArray());
        if (usings.Any())
        {
            fw.WriteLine();
        }

        fw.WriteNamespace(ns);

        fw.WriteSummary($"Client {fileName}");
        fw.WriteClassDeclaration(className, null, false);

        fw.WriteLine(1, "private readonly HttpClient _client;");
        if (hasJson)
        {
            fw.WriteLine(1, "private readonly JsonSerializerOptions _jsOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };");
        }

        fw.WriteLine();
        fw.WriteSummary(1, "Constructeur");
        fw.WriteParam("client", "HttpClient injecté.");
        fw.WriteLine(1, $"public {className}(HttpClient client)");
        fw.WriteLine(1, "{");
        fw.WriteLine(2, "_client = client;");
        fw.WriteLine(1, "}");

        foreach (var endpoint in endpoints.OrderBy(endpoint => endpoint.NamePascal))
        {
            fw.WriteLine();
            fw.WriteSummary(1, endpoint.Description);

            foreach (var param in endpoint.Params)
            {
                fw.WriteParam(param.GetParamName(), param.Comment);
            }

            fw.WriteReturns(1, endpoint.Returns?.Comment ?? "Task.");

            fw.Write("    public async Task");

            var returnType = endpoint.Returns != null ? Config.GetType(endpoint.Returns) : null;
            if (returnType?.StartsWith("IAsyncEnumerable") ?? false)
            {
                returnType = returnType.Replace("IAsyncEnumerable", "IEnumerable");
            }

            if (returnType != null)
            {
                fw.Write($"<{returnType}>");
            }

            fw.Write($" {endpoint.NamePascal}(");

            foreach (var param in endpoint.Params)
            {
                fw.Write($"{Config.GetType(param, nonNullable: param.IsRouteParam() || param.IsQueryParam() && Config.GetValue(param, Classes) != "null")} {param.GetParamName().Verbatim()}");

                if (param.IsQueryParam())
                {
                    fw.Write($" = {Config.GetValue(param, Classes)}");
                }

                if (endpoint.Params.Last() != param)
                {
                    fw.Write(", ");
                }
            }

            fw.WriteLine(")");
            fw.WriteLine(1, "{");

            var bodyParam = endpoint.GetBodyParam();

            fw.WriteLine(2, $"await EnsureAuthentication();");

            if (endpoint.GetQueryParams().Any())
            {
                fw.WriteLine(2, "var query = await new FormUrlEncodedContent(new Dictionary<string, string>");
                fw.WriteLine(2, "{");

                foreach (var qp in endpoint.GetQueryParams().Where(qp => !Config.GetType(qp).Contains("[]")))
                {
                    var toString = Config.GetType(qp) switch
                    {
                        "string" => string.Empty,
                        "Guid" or "Guid?" => "?.ToString()",
                        _ => $"?.ToString(CultureInfo.InvariantCulture)"
                    };

                    fw.WriteLine(3, $@"[""{qp.GetParamName()}""] = {qp.GetParamName().Verbatim()}{toString},");
                }

                var listQPs = endpoint.GetQueryParams().Where(qp => Config.GetType(qp).Contains("[]")).ToList();

                if (listQPs.Count == 0)
                {
                    fw.WriteLine(2, "}.Where(kv => kv.Value != null)).ReadAsStringAsync();");
                }
                else
                {
                    fw.Write("        }");
                    foreach (var qp in listQPs)
                    {
                        var toString = Config.GetType(qp) switch
                        {
                            "string[]" => string.Empty,
                            "Guid[]" => ".ToString()",
                            _ => $".ToString(CultureInfo.InvariantCulture)"
                        };

                        var first = listQPs.IndexOf(qp) == 0;
                        fw.WriteLine(first ? 0 : 3, $@"{(first ? string.Empty : " ")}.Concat({qp.GetParamName()}?.Select(i => new KeyValuePair<string, string>(""{qp.GetParamName()}"", i{toString})) ?? new Dictionary<string, string>())");
                    }

                    fw.WriteLine(2, " .Where(kv => kv.Value != null)).ReadAsStringAsync();");
                }
            }

            fw.WriteLine(2, $"using var res = await _client.SendAsync(new HttpRequestMessage(HttpMethod.{endpoint.Method.ToPascalCase(true)}, $\"{endpoint.FullRoute}{(endpoint.GetQueryParams().Any() ? "?{query}" : string.Empty)}\"){(bodyParam != null ? $" {{ Content = GetBody({bodyParam.NameCamel}) }}" : string.Empty)}{(returnType != null ? ", HttpCompletionOption.ResponseHeadersRead" : string.Empty)});");
            fw.WriteLine(2, $"await EnsureSuccess(res);");

            if (returnType != null)
            {
                fw.WriteLine(2, $"return await Deserialize<{returnType}>(res);");
            }

            fw.WriteLine(1, "}");
        }

        if (hasReturn)
        {
            fw.WriteLine();
            fw.WriteSummary(1, "Déserialize le contenu d'une réponse HTTP.");
            fw.WriteTypeParam(1, "T", "Type de destination.");
            fw.WriteParam("response", "Réponse HTTP");
            fw.WriteReturns(1, "Contenu.");
            fw.WriteLine(1, "private async Task<T> Deserialize<T>(HttpResponseMessage response)");
            fw.WriteLine(1, "{");
            fw.WriteLine(2, "if (response.StatusCode == HttpStatusCode.NoContent)");
            fw.WriteLine(2, "{");
            fw.WriteLine(3, "return default;");
            fw.WriteLine(2, "}");
            fw.WriteLine();
            fw.WriteLine(2, "using var res = await response.Content.ReadAsStreamAsync();");
            fw.WriteLine(2, "return await JsonSerializer.DeserializeAsync<T>(res, _jsOptions);");
            fw.WriteLine(1, "}");
        }

        fw.WriteLine();
        fw.WriteSummary(1, "Assure que l'authentification est configurée.");
        fw.WriteLine(1, "private partial Task EnsureAuthentication();");

        fw.WriteLine();
        fw.WriteSummary(1, "Gère les erreurs éventuelles retournées par l'API appelée.");
        fw.WriteParam("response", "Réponse HTTP");
        fw.WriteLine(1, "private partial Task EnsureSuccess(HttpResponseMessage response);");

        if (hasBody)
        {
            fw.WriteLine();
            fw.WriteSummary(1, "Récupère le body d'une requête pour l'objet donné.");
            fw.WriteTypeParam(1, "T", "Type source.");
            fw.WriteParam("input", "Entrée");
            fw.WriteReturns(1, "Contenu.");
            fw.WriteLine(1, "private StringContent GetBody<T>(T input)");
            fw.WriteLine(1, "{");
            fw.WriteLine(2, "return new StringContent(JsonSerializer.Serialize(input, _jsOptions), Encoding.UTF8, \"application/json\");");
            fw.WriteLine(1, "}");
        }

        fw.WriteLine("}");
    }

    private string GetNamespace(Class classe, string tag)
    {
        return Config.GetNamespace(classe, classe.Tags.Contains(tag) ? tag : classe.Tags.Intersect(Config.Tags).FirstOrDefault() ?? tag);
    }
}