using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Generator.CSharp;

public class CSharpApiClientGenerator : GeneratorBase
{
    private readonly CSharpConfig _config;
    private readonly IDictionary<string, ModelFile> _files = new Dictionary<string, ModelFile>();
    private readonly ILogger<CSharpApiClientGenerator> _logger;

    public CSharpApiClientGenerator(ILogger<CSharpApiClientGenerator> logger, CSharpConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "CSharpApiClientGen";

    public override IEnumerable<string> GeneratedFiles => _files.Values.Where(f => f.Endpoints.Any()).Select(GetFilePath);

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files)
        {
            _files[file.Name] = file;
            HandleFile(file);
        }
    }

    private string GetFilePath(ModelFile file)
    {
        var className = $"{file.Options?.Endpoints?.FileName ?? file.Name.Split("/").Last()}Client";
        var apiPath = Path.Combine(_config.ApiRootPath.Replace("{app}", file.Endpoints.First().Namespace.App), _config.ApiFilePath.Replace("{module}", file.Module)).Replace("\\", "/");
        return $"{_config.OutputDirectory}/{apiPath}/generated/{className}.cs";
    }

    private void HandleFile(ModelFile file)
    {
        if (!file.Endpoints.Any())
        {
            return;
        }

        var className = $"{file.Options?.Endpoints?.FileName ?? file.Name.Split("/").Last()}Client";
        var apiPath = Path.Combine(_config.ApiRootPath.Replace("{app}", file.Endpoints.First().Namespace.App), _config.ApiFilePath.Replace("{module}", file.Module)).Replace("\\", "/");
        var filePath = $"{_config.OutputDirectory}/{apiPath}/generated/{className}.cs";

        using var fw = new CSharpWriter(filePath, _logger, _config.UseLatestCSharp);

        var hasBody = file.Endpoints.Any(e => e.GetBodyParam() != null);
        var hasReturn = file.Endpoints.Any(e => e.Returns != null);
        var hasJson = hasReturn || hasBody;

        var usings = new List<string>();

        if (!_config.UseLatestCSharp)
        {
            usings.Add("System.Net.Http");
            usings.Add("System.Threading.Tasks");
        }

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

            if (_config.UseLatestCSharp)
            {
                usings.Add("System.Text.Json.Serialization");
            }
        }

        if (file.Endpoints.Any(e => e.GetQueryParams().Any()))
        {
            if (!_config.UseLatestCSharp)
            {
                usings.AddRange(new[] { "System.Collections.Generic", "System.Linq" });
            }

            if (file.Endpoints.Any(e => e.GetQueryParams().Any(qp => _config.GetPropertyTypeName(qp) != "string")))
            {
                usings.Add("System.Globalization");
            }
        }

        foreach (var property in file.Endpoints.SelectMany(e => e.Params.Concat(new[] { e.Returns }).Where(p => p != null)))
        {
            if (property is IFieldProperty fp)
            {
                foreach (var @using in fp.Domain.CSharp!.Usings)
                {
                    usings.Add(@using);
                }
            }

            switch (property)
            {
                case AssociationProperty ap:
                    usings.Add(_config.GetNamespace(ap.Association));
                    break;
                case AliasProperty { Property: AssociationProperty ap2 }:
                    usings.Add(_config.GetNamespace(ap2.Association));
                    break;
                case AliasProperty { PrimaryKey: false, Property: RegularProperty { PrimaryKey: true } rp }:
                    usings.Add(_config.GetNamespace(rp.Class));
                    break;
                case CompositionProperty cp:
                    usings.Add(_config.GetNamespace(cp.Composition));

                    if (cp.DomainKind != null)
                    {
                        usings.AddRange(cp.DomainKind.CSharp!.Usings);
                    }

                    if (!_config.UseLatestCSharp && (cp.Kind == "list" || cp.Kind == "async-list"))
                    {
                        usings.Add("System.Collections.Generic");
                    }

                    break;
            }
        }

        var ns = apiPath.Replace("/", ".");

        fw.WriteUsings(usings.Distinct().Where(u => u != ns).ToArray());
        if (usings.Any())
        {
            fw.WriteLine();
        }

        fw.WriteNamespace(ns);

        fw.WriteSummary(1, $"Client {file.Module}");
        fw.WriteClassDeclaration(className, null);

        fw.WriteLine(2, "private readonly HttpClient _client;");
        if (hasJson)
        {
            if (_config.UseLatestCSharp)
            {
                fw.WriteLine(2, "private readonly JsonSerializerOptions _jsOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };");
            }
            else
            {
                fw.WriteLine(2, "private readonly JsonSerializerOptions _jsOptions = new() { IgnoreNullValues = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };");
            }
        }

        fw.WriteLine();
        fw.WriteSummary(2, "Constructeur");
        fw.WriteParam("client", "HttpClient injecté.");
        fw.WriteLine(2, $"public {className}(HttpClient client)");
        fw.WriteLine(2, "{");
        fw.WriteLine(3, "_client = client;");
        fw.WriteLine(2, "}");

        foreach (var endpoint in file.Endpoints.OrderBy(endpoint => endpoint.Name))
        {
            fw.WriteLine();
            fw.WriteSummary(2, endpoint.Description);

            foreach (var param in endpoint.Params)
            {
                fw.WriteParam(param.GetParamName(), param.Comment);
            }

            fw.WriteReturns(2, endpoint.Returns?.Comment ?? "Task.");

            if (!_config.UseLatestCSharp)
            {
                fw.Write("    ");
            }

            fw.Write("    public async Task");

            var returnType = endpoint.Returns != null ? _config.GetPropertyTypeName(endpoint.Returns) : null;
            if (returnType?.StartsWith("IAsyncEnumerable") ?? false)
            {
                returnType = returnType.Replace("IAsyncEnumerable", "IEnumerable");
            }

            if (returnType != null)
            {
                fw.Write($"<{returnType}>");
            }

            fw.Write($" {endpoint.Name}(");

            foreach (var param in endpoint.Params)
            {
                fw.Write($"{_config.GetPropertyTypeName(param, param.IsRouteParam())} {param.GetParamName()}");

                if (param.IsQueryParam())
                {
                    fw.Write(" = null");
                }

                if (endpoint.Params.Last() != param)
                {
                    fw.Write(", ");
                }
            }

            fw.WriteLine(")");
            fw.WriteLine(2, "{");

            var bodyParam = endpoint.GetBodyParam();

            fw.WriteLine(3, $"await EnsureAuthentication();");

            if (endpoint.GetQueryParams().Any())
            {
                fw.WriteLine(3, "var query = await new FormUrlEncodedContent(new Dictionary<string, string>");
                fw.WriteLine(3, "{");

                foreach (var qp in endpoint.GetQueryParams().Where(qp => !_config.GetPropertyTypeName(qp).Contains("[]")))
                {
                    var toString = _config.GetPropertyTypeName(qp) switch
                    {
                        "string" => string.Empty,
                        _ => $"?.ToString(CultureInfo.InvariantCulture)"
                    };

                    fw.WriteLine(4, $@"[""{qp.GetParamName()}""] = {qp.GetParamName()}{toString},");
                }

                var listQPs = endpoint.GetQueryParams().Where(qp => _config.GetPropertyTypeName(qp).Contains("[]")).ToList();

                if (listQPs.Count == 0)
                {
                    fw.WriteLine(3, "}.Where(kv => kv.Value != null)).ReadAsStringAsync();");
                }
                else
                {
                    if (!_config.UseLatestCSharp)
                    {
                        fw.Write("    ");
                    }

                    fw.Write("        }");
                    foreach (var qp in listQPs)
                    {
                        var toString = _config.GetPropertyTypeName(qp) switch
                        {
                            "string" => string.Empty,
                            "Guid" => ".ToString()",
                            _ => $".ToString(CultureInfo.InvariantCulture)"
                        };

                        var first = listQPs.IndexOf(qp) == 0;
                        fw.WriteLine(first ? 0 : 3, $@"{(first ? string.Empty : " ")}.Concat({qp.Name}?.Select(i => new KeyValuePair<string, string>(""{qp.Name}"", i{toString})) ?? new Dictionary<string, string>())");
                    }

                    fw.WriteLine(3, " .Where(kv => kv.Value != null)).ReadAsStringAsync();");
                }
            }

            fw.WriteLine(3, $"using var res = await _client.SendAsync(new HttpRequestMessage(HttpMethod.{endpoint.Method.ToLower().ToFirstUpper()}, $\"{endpoint.FullRoute}{(endpoint.GetQueryParams().Any() ? "?{query}" : string.Empty)}\"){(bodyParam != null ? $" {{ Content = GetBody({bodyParam.Name}) }}" : string.Empty)}{(returnType != null ? ", HttpCompletionOption.ResponseHeadersRead" : string.Empty)});");
            fw.WriteLine(3, $"await EnsureSuccess(res);");

            if (returnType != null)
            {
                fw.WriteLine(3, $"return await Deserialize<{returnType}>(res);");
            }

            fw.WriteLine(2, "}");
        }

        if (hasReturn)
        {
            fw.WriteLine();
            fw.WriteSummary(2, "Déserialize le contenu d'une réponse HTTP.");
            fw.WriteTypeParam("T", "Type de destination.");
            fw.WriteParam("response", "Réponse HTTP");
            fw.WriteReturns(2, "Contenu.");
            fw.WriteLine(2, "private async Task<T> Deserialize<T>(HttpResponseMessage response)");
            fw.WriteLine(2, "{");
            fw.WriteLine(3, "if (response.StatusCode == HttpStatusCode.NoContent)");
            fw.WriteLine(3, "{");
            fw.WriteLine(4, "return default;");
            fw.WriteLine(3, "}");
            fw.WriteLine();
            fw.WriteLine(3, "using var res = await response.Content.ReadAsStreamAsync();");
            fw.WriteLine(3, "return await JsonSerializer.DeserializeAsync<T>(res, _jsOptions);");
            fw.WriteLine(2, "}");
        }

        fw.WriteLine();
        fw.WriteSummary(2, "Assure que l'authentification est configurée.");
        fw.WriteLine(2, "private partial Task EnsureAuthentication();");

        fw.WriteLine();
        fw.WriteSummary(2, "Gère les erreurs éventuelles retournées par l'API appelée.");
        fw.WriteParam("response", "Réponse HTTP");
        fw.WriteLine(2, "private partial Task EnsureSuccess(HttpResponseMessage response);");

        if (hasBody)
        {
            fw.WriteLine();
            fw.WriteSummary(2, "Récupère le body d'une requête pour l'objet donné.");
            fw.WriteTypeParam("T", "Type source.");
            fw.WriteParam("input", "Entrée");
            fw.WriteReturns(2, "Contenu.");
            fw.WriteLine(2, "private StringContent GetBody<T>(T input)");
            fw.WriteLine(2, "{");
            fw.WriteLine(3, "return new StringContent(JsonSerializer.Serialize(input, _jsOptions), Encoding.UTF8, \"application/json\");");
            fw.WriteLine(2, "}");
        }

        fw.WriteLine(1, "}");
        fw.WriteNamespaceEnd();
    }
}