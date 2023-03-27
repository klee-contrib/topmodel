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

    protected override object? GetDomainType(Domain domain)
    {
        return domain.CSharp;
    }

    protected override bool FilterTag(string tag)
    {
        return Config.ResolveVariables(Config.ApiGeneration!, tag) == ApiGeneration.Client;
    }

    protected override string GetFileName(ModelFile file, string tag)
    {
        return Path.Combine(Config.GetApiPath(file, tag), "generated", $"{file.Options.Endpoints.FileName.ToPascalCase()}Client.cs");
    }

    protected override void HandleFile(string filePath, string fileName, string tag, IList<Endpoint> endpoints)
    {
        using var fw = new CSharpWriter(filePath, _logger, Config.UseLatestCSharp);

        var hasBody = endpoints.Any(e => e.GetBodyParam() != null);
        var hasReturn = endpoints.Any(e => e.Returns != null);
        var hasJson = hasReturn || hasBody;

        var usings = new List<string>();

        if (!Config.UseLatestCSharp)
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

            if (Config.UseLatestCSharp)
            {
                usings.Add("System.Text.Json.Serialization");
            }
        }

        if (endpoints.Any(e => e.GetQueryParams().Any()))
        {
            if (!Config.UseLatestCSharp)
            {
                usings.AddRange(new[] { "System.Collections.Generic", "System.Linq" });
            }

            if (endpoints.Any(e => e.GetQueryParams().Any(qp =>
            {
                var typeName = Config.GetPropertyTypeName(qp);
                return !typeName.StartsWith("string") && !typeName.StartsWith("Guid");
            })))
            {
                usings.Add("System.Globalization");
            }
        }

        foreach (var property in endpoints.SelectMany(e => e.Params.Concat(new[] { e.Returns }).Where(p => p != null)))
        {
            if (property is IFieldProperty fp)
            {
                foreach (var @using in fp.Domain.CSharp!.Usings.Select(u => u.ParseTemplate(fp)))
                {
                    usings.Add(@using);
                }

                foreach (var @using in fp.Domain.CSharp!.Annotations
                    .Where(a => (a.Target & Target.Dto) > 0 || (a.Target & Target.Persisted) > 0 && (property.Class?.IsPersistent ?? false))
                    .SelectMany(a => a.Usings)
                    .Select(u => u.ParseTemplate(fp)))
                {
                    usings.Add(@using);
                }
            }

            switch (property)
            {
                case AssociationProperty ap:
                    usings.Add(Config.GetNamespace(ap.Association, tag));
                    break;
                case AliasProperty { Property: AssociationProperty ap2 }:
                    usings.Add(Config.GetNamespace(ap2.Association, tag));
                    break;
                case AliasProperty { PrimaryKey: false, Property: RegularProperty { PrimaryKey: true } rp }:
                    usings.Add(Config.GetNamespace(rp.Class, tag));
                    break;
                case CompositionProperty cp:
                    usings.Add(Config.GetNamespace(cp.Composition, tag));

                    if (cp.DomainKind != null)
                    {
                        usings.AddRange(cp.DomainKind.CSharp!.Usings.Select(u => u.ParseTemplate(cp)));
                        usings.AddRange(cp.DomainKind.CSharp!.Annotations
                        .Where(a => (a.Target & Target.Dto) > 0 || (a.Target & Target.Persisted) > 0 && (property.Class?.IsPersistent ?? false))
                        .SelectMany(a => a.Usings));
                    }

                    if (!Config.UseLatestCSharp && (cp.Kind == "list" || cp.Kind == "async-list"))
                    {
                        usings.Add("System.Collections.Generic");
                    }

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

        fw.WriteSummary(1, $"Client {fileName}");
        fw.WriteClassDeclaration(className, null);

        fw.WriteLine(2, "private readonly HttpClient _client;");
        if (hasJson)
        {
            if (Config.UseLatestCSharp)
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

        foreach (var endpoint in endpoints.OrderBy(endpoint => endpoint.NamePascal))
        {
            fw.WriteLine();
            fw.WriteSummary(2, endpoint.Description);

            foreach (var param in endpoint.Params)
            {
                fw.WriteParam(param.GetParamName(), param.Comment);
            }

            fw.WriteReturns(2, endpoint.Returns?.Comment ?? "Task.");

            if (!Config.UseLatestCSharp)
            {
                fw.Write("    ");
            }

            fw.Write("    public async Task");

            var returnType = endpoint.Returns != null ? Config.GetPropertyTypeName(endpoint.Returns) : null;
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
                fw.Write($"{Config.GetPropertyTypeName(param, param.IsRouteParam() || param.IsQueryParam() && Config.GetDefaultValue(param, Classes) != "null")} {param.GetParamName().Verbatim()}");

                if (param.IsQueryParam())
                {
                    fw.Write($" = {Config.GetDefaultValue(param, Classes)}");
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

                foreach (var qp in endpoint.GetQueryParams().Where(qp => !Config.GetPropertyTypeName(qp).Contains("[]")))
                {
                    var toString = Config.GetPropertyTypeName(qp) switch
                    {
                        "string" => string.Empty,
                        "Guid" or "Guid?" => "?.ToString()",
                        _ => $"?.ToString(CultureInfo.InvariantCulture)"
                    };

                    fw.WriteLine(4, $@"[""{qp.GetParamName()}""] = {qp.GetParamName().Verbatim()}{toString},");
                }

                var listQPs = endpoint.GetQueryParams().Where(qp => Config.GetPropertyTypeName(qp).Contains("[]")).ToList();

                if (listQPs.Count == 0)
                {
                    fw.WriteLine(3, "}.Where(kv => kv.Value != null)).ReadAsStringAsync();");
                }
                else
                {
                    if (!Config.UseLatestCSharp)
                    {
                        fw.Write("    ");
                    }

                    fw.Write("        }");
                    foreach (var qp in listQPs)
                    {
                        var toString = Config.GetPropertyTypeName(qp) switch
                        {
                            "string[]" => string.Empty,
                            "Guid[]" => ".ToString()",
                            _ => $".ToString(CultureInfo.InvariantCulture)"
                        };

                        var first = listQPs.IndexOf(qp) == 0;
                        fw.WriteLine(first ? 0 : 3, $@"{(first ? string.Empty : " ")}.Concat({qp.GetParamName()}?.Select(i => new KeyValuePair<string, string>(""{qp.GetParamName()}"", i{toString})) ?? new Dictionary<string, string>())");
                    }

                    fw.WriteLine(3, " .Where(kv => kv.Value != null)).ReadAsStringAsync();");
                }
            }

            fw.WriteLine(3, $"using var res = await _client.SendAsync(new HttpRequestMessage(HttpMethod.{endpoint.Method.ToPascalCaseStrict()}, $\"{endpoint.FullRoute}{(endpoint.GetQueryParams().Any() ? "?{query}" : string.Empty)}\"){(bodyParam != null ? $" {{ Content = GetBody({bodyParam.NameCamel}) }}" : string.Empty)}{(returnType != null ? ", HttpCompletionOption.ResponseHeadersRead" : string.Empty)});");
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
            fw.WriteTypeParam(2, "T", "Type de destination.");
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
            fw.WriteTypeParam(2, "T", "Type source.");
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