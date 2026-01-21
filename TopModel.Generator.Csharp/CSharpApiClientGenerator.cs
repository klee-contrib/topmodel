using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Csharp;

public class CSharpApiClientGenerator(ILogger<CSharpApiClientGenerator> logger, IFileWriterProvider writerProvider)
    : EndpointsGeneratorBase<CsharpConfig>(logger, writerProvider)
{
    public override string Name => "CSharpApiClientGen";

    protected override bool FilterTag(string tag)
    {
        return Config.ResolveVariables(Config.ApiGeneration!, tag) == ApiGeneration.Client;
    }

    protected override string GetFilePath(ModelFile file, string tag)
    {
        return Path.Combine(
            Config.GetApiPath(file, tag),
            "generated",
            $"{file.Options.Endpoints.FileName.ToPascalCase()}Client.cs"
        );
    }

    protected virtual string GetNamespace(Class classe, string tag)
    {
        return Config.GetNamespace(
            classe,
            classe.Tags.Contains(tag) ? tag : classe.Tags.Intersect(Config.Tags).FirstOrDefault() ?? tag
        );
    }

    protected override void HandleFile(string filePath, string fileName, string tag, IList<Endpoint> endpoints)
    {
        var className = $"{fileName.ToPascalCase()}Client";
        var ns = Config.GetNamespace(endpoints[0], tag);

        HandleFilePartial(
            filePath.Replace($"{Path.DirectorySeparatorChar}generated", string.Empty).Replace(".cs", ".partial.cs"),
            className,
            ns
        );

        using var fw = this.OpenCSharpWriter(filePath);

        var hasBody = endpoints.Any(e => e.GetJsonBodyParam() != null);
        var hasReturn = endpoints.Any(e =>
            e.Returns != null && !new[] { "string", "byte[]" }.Contains(Config.GetType(e.Returns)?.TrimEnd('?'))
        );
        var hasJson = hasReturn || hasBody;

        var hasAsyncEnumerable = endpoints.Any(e =>
            e.Returns != null && (Config.GetType(e.Returns)?.StartsWith("IAsyncEnumerable") ?? false)
        );

        var usings = new List<string>();

        if (endpoints.Any(e => e.Returns != null && !e.Returns.Required))
        {
            usings.Add("System.Net");
        }

        if (hasJson)
        {
            usings.Add("System.Net.Http.Json");
            usings.Add("System.Text.Json");
            usings.Add("System.Text.Json.Serialization");
        }

        if (hasAsyncEnumerable && Config.UseCancellationTokens)
        {
            usings.Add("System.Runtime.CompilerServices");
        }

        if (
            endpoints.Any(e => e.GetQueryParams().Any())
            && endpoints.Any(e =>
                e.GetQueryParams()
                    .Any(qp =>
                    {
                        var typeName = Config.GetType(qp);
                        return typeName.Contains("decimal")
                            || typeName.Contains("double")
                            || typeName.Contains("float");
                    })
            )
        )
        {
            usings.Add("System.Globalization");
        }

        foreach (var property in endpoints.SelectMany(e => e.Properties))
        {
            usings.AddRange(Config.GetDomainImports(property, tag));

            if (property.IsQueryParam())
            {
                usings.AddRange(Config.GetValueImports(property));
            }

            switch (property)
            {
                case { EnumProperty: IProperty ep } when Config.EnumGeneration == EnumGenerationMode.AsEnum:
                    usings.Add(GetNamespace(ep.Class, tag));
                    break;
                case { Composition: Class cpc }:
                    usings.Add(GetNamespace(cpc, tag));
                    break;
            }
        }

        fw.AddUsings(usings.Where(u => u != ns));

        fw.WriteNamespace(ns);

        var primaryConstructor = Config.DotnetVersion >= 8;

        var client = $"{(!primaryConstructor ? "_" : string.Empty)}client";

        while (endpoints.SelectMany(e => e.Params).Any(p => p.GetParamName() == client))
        {
            client = $"_{client}";
        }

        var parameters = $"HttpClient {client}";

        fw.WriteSummary($"Client {fileName}");
        if (primaryConstructor)
        {
            fw.WriteParam(client, "HttpClient injecté.", 0);
        }

        fw.WriteClassDeclaration(
            className,
            inheritedClass: null,
            isRecord: false,
            parameters: primaryConstructor ? parameters : null
        );

        if (!primaryConstructor)
        {
            fw.WriteLine(1, $"private readonly HttpClient {client};");
        }

        if (hasJson)
        {
            fw.WriteLine(
                1,
                "private readonly JsonSerializerOptions _jsOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };"
            );
        }

        if (!primaryConstructor)
        {
            fw.WriteLine();
            fw.WriteSummary(1, "Constructeur");
            fw.WriteParam("client", "HttpClient injecté.");
            fw.WriteLine(1, $"public {className}(HttpClient client)");
            fw.WriteLine(1, "{");
            fw.WriteLine(2, $"{client} = client;");
            fw.WriteLine(1, "}");
        }

        var orderedEndpoints = endpoints.OrderBy(endpoint => endpoint.NamePascal, StringComparer.Ordinal).ToList();

        foreach (var endpoint in orderedEndpoints)
        {
            string GetSafeVariableName(string varName)
            {
                while (endpoint.Params.Any(p => p.GetParamName() == varName))
                {
                    varName = $"_{varName}";
                }

                return varName;
            }

            var ct = GetSafeVariableName("ct");
            var query = GetSafeVariableName("query");
            var res = GetSafeVariableName("res");

            if (orderedEndpoints.IndexOf(endpoint) > 0 || !primaryConstructor || hasJson)
            {
                fw.WriteLine();
            }

            fw.WriteSummary(1, endpoint.Description);

            foreach (var param in endpoint.Params)
            {
                fw.WriteParam(param.GetParamName(), param.Comment);
            }

            if (Config.UseCancellationTokens)
            {
                fw.WriteParam(ct, "CancellationToken.");
            }

            fw.WriteReturns(1, endpoint.Returns?.Comment ?? "Task.");

            fw.Write("    public async ");

            var returnType =
                endpoint.Returns != null
                    ? Config.GetType(endpoint.Returns, nonNullable: endpoint.Returns.Required)
                    : null;

            var isAsyncEnumerable = returnType?.StartsWith("IAsyncEnumerable") ?? false;

            if (isAsyncEnumerable)
            {
                fw.Write(returnType!);
            }
            else
            {
                fw.Write("Task");

                if (returnType != null)
                {
                    fw.Write($"<{returnType}>");
                }
            }

            fw.Write($" {endpoint.NamePascal}(");

            foreach (var param in endpoint.Params)
            {
                fw.Write(
                    $"{Config.GetType(param, nonNullable: param.IsJsonBodyParam() || param.IsRouteParam() || param.IsQueryParam() && Config.GetValue(param) != "null")} {param.GetParamName().Verbatim()}"
                );

                if (param.IsQueryParam())
                {
                    fw.Write($" = {Config.GetValue(param)}");
                }

                if (endpoint.Params[^1] != param || Config.UseCancellationTokens)
                {
                    fw.Write(", ");
                }
            }

            if (Config.UseCancellationTokens)
            {
                fw.Write(
                    $"{(isAsyncEnumerable ? "[EnumeratorCancellation] " : string.Empty)}CancellationToken {ct} = default"
                );
            }

            fw.WriteLine(")");
            fw.WriteLine(1, "{");

            var bodyParam = endpoint.GetJsonBodyParam();

            fw.WriteLine(2, $"await EnsureAuthentication({(Config.UseCancellationTokens ? ct : string.Empty)});");

            if (endpoint.GetQueryParams().Any())
            {
                fw.WriteLine(
                    2,
                    $"var {query} = await new FormUrlEncodedContent(new Dictionary<string, string{(Config.NullableEnable ? "?" : string.Empty)}>"
                );
                fw.WriteLine(2, "{");

                foreach (var qp in endpoint.GetQueryParams().Where(qp => !Config.GetType(qp).Contains("[]")))
                {
                    var type = Config.GetType(qp, nonNullable: Config.GetValue(qp) != "null");
                    var nullable = type.EndsWith('?');
                    var toString = type.TrimEnd("?") switch
                    {
                        "string" => string.Empty,
                        "decimal" or "double" or "float" =>
                            $"{(nullable ? "?" : string.Empty)}.ToString(CultureInfo.InvariantCulture)",
                        "DateTime" or "DateOnly" => $"{(nullable ? "?" : string.Empty)}.ToString(\"o\")",
                        _ => $"{(nullable ? "?" : string.Empty)}.ToString()",
                    };

                    fw.WriteLine(3, $@"[""{qp.GetParamName()}""] = {qp.GetParamName().Verbatim()}{toString},");
                }

                var listQPs = endpoint.GetQueryParams().Where(qp => Config.GetType(qp).Contains("[]")).ToList();

                if (listQPs.Count == 0)
                {
                    fw.WriteLine(
                        2,
                        $"}}.Where(kv => kv.Value != null)).ReadAsStringAsync({(Config.UseCancellationTokens ? ct : string.Empty)});"
                    );
                }
                else
                {
                    fw.Write("        }");
                    foreach (var qp in listQPs)
                    {
                        var toString = Config.GetType(qp) switch
                        {
                            "string[]" => string.Empty,
                            "decimal[]" or "double[]" or "float[]" => $".ToString(CultureInfo.InvariantCulture)",
                            "DateTime[]" or "DateOnly[]" => $".ToString(\"o\")",
                            _ => $".ToString()",
                        };

                        var first = listQPs.IndexOf(qp) == 0;
                        fw.WriteLine(
                            first ? 0 : 3,
                            $@"{(first ? string.Empty : " ")}.Concat({qp.GetParamName()}?.Select(i => new KeyValuePair<string, string{(Config.NullableEnable ? "?" : string.Empty)}>(""{qp.GetParamName()}"", i{toString})) ?? {(primaryConstructor ? "[]" : $"new Dictionary<string, string{(Config.NullableEnable ? "?" : string.Empty)}>()")})"
                        );
                    }

                    fw.WriteLine(
                        2,
                        $" .Where(kv => kv.Value != null)).ReadAsStringAsync({(Config.UseCancellationTokens ? ct : string.Empty)});"
                    );
                }
            }

            fw.WriteLine(
                2,
                $"using var {res} = await {client}.SendAsync(new(HttpMethod.{endpoint.Method.ToPascalCase(strict: true)}, $\"{endpoint.FullRoute}{(endpoint.GetQueryParams().Any() ? $"?{{{query}}}" : string.Empty)}\"){(bodyParam != null ? $" {{ Content = JsonContent.Create({bodyParam.NameCamel}, options: _jsOptions) }}" : string.Empty)}{(returnType != null ? ", HttpCompletionOption.ResponseHeadersRead" : string.Empty)}{(Config.UseCancellationTokens ? ", ct" : string.Empty)});"
            );
            fw.WriteLine(2, $"await EnsureSuccess({res}{(Config.UseCancellationTokens ? $", {ct}" : string.Empty)});");

            if (returnType != null)
            {
                if (!endpoint.Returns!.Required)
                {
                    fw.WriteLine();
                    fw.WriteLine(2, $"if ({res}.StatusCode == HttpStatusCode.NoContent)");
                    fw.WriteLine(2, "{");
                    fw.WriteLine(3, "return null;");
                    fw.WriteLine(2, "}");
                }

                if (returnType.TrimEnd('?') == "string")
                {
                    fw.WriteLine();
                    fw.WriteLine(
                        2,
                        $"return (await {res}.Content.ReadAsStringAsync({(Config.UseCancellationTokens ? ct : string.Empty)})).Trim('\"');"
                    );
                }
                else if (returnType == "byte[]")
                {
                    fw.WriteLine();
                    fw.WriteLine(2, "using var ms = new MemoryStream();");
                    fw.WriteLine(
                        2,
                        $"(await {res}.Content.ReadAsStreamAsync({(Config.UseCancellationTokens ? ct : string.Empty)})).CopyTo(ms);"
                    );
                    fw.WriteLine(2, "return ms.ToArray();");
                }
                else if (isAsyncEnumerable)
                {
                    fw.WriteLine();
                    fw.WriteLine(
                        2,
                        $"await foreach (var {GetSafeVariableName("item")} in res.Content.ReadFromJsonAsAsyncEnumerable<{returnType[17..^1]}>(_jsOptions{(Config.UseCancellationTokens ? $", ct" : string.Empty)}){(Config.UseCancellationTokens ? $".WithCancellation(ct)" : string.Empty)})"
                    );
                    fw.WriteLine(2, "{");
                    fw.WriteLine(
                        3,
                        $"yield return {GetSafeVariableName("item")}{(Config.NullableEnable ? "!" : string.Empty)};"
                    );
                    fw.WriteLine(2, "}");
                }
                else
                {
                    fw.WriteLine();
                    fw.Write(2, $"return ");

                    if (Config.NullableEnable && endpoint.Returns.Required)
                    {
                        fw.Write("(");
                    }

                    fw.Write(
                        $"await {res}.Content.ReadFromJsonAsync<{returnType}>(_jsOptions{(Config.UseCancellationTokens ? $", {ct}" : string.Empty)})"
                    );

                    if (Config.NullableEnable && endpoint.Returns.Required)
                    {
                        fw.Write(")!");
                    }

                    fw.WriteLine(";");
                }
            }

            fw.WriteLine(1, "}");
        }

        fw.WriteLine();
        fw.WriteSummary(1, "Assure que l'authentification est configurée.");

        if (Config.UseCancellationTokens)
        {
            fw.WriteParam("ct", "CancellationToken.");
        }

        fw.WriteLine(
            1,
            $"private partial Task EnsureAuthentication({(Config.UseCancellationTokens ? $"CancellationToken ct = default" : string.Empty)});"
        );

        fw.WriteLine();
        fw.WriteSummary(1, "Gère les erreurs éventuelles retournées par l'API appelée.");
        fw.WriteParam("response", "Réponse HTTP");

        if (Config.UseCancellationTokens)
        {
            fw.WriteParam("ct", "CancellationToken.");
        }

        fw.WriteLine(
            1,
            $"private partial Task EnsureSuccess(HttpResponseMessage response{(Config.UseCancellationTokens ? $", CancellationToken ct = default" : string.Empty)});"
        );

        fw.WriteLine("}");
    }

    protected virtual void HandleFilePartial(string filePath, string className, string ns)
    {
        if (File.Exists(filePath))
        {
            return;
        }

        using var w = this.OpenCSharpWriter(filePath);
        w.EnableHeader = false;

        w.WriteNamespace(ns);

        w.WriteSummary($"Client {className[..^6]}");
        w.WriteClassDeclaration(className, inheritedClass: null, isRecord: false);

        w.WriteLine(
            1,
            $"private partial Task EnsureAuthentication({(Config.UseCancellationTokens ? $"CancellationToken ct" : string.Empty)})"
        );
        w.WriteLine(1, "{");
        w.WriteLine(2, "return Task.CompletedTask;");
        w.WriteLine(1, "}");

        w.WriteLine();
        w.WriteLine(
            1,
            $"private partial Task EnsureSuccess(HttpResponseMessage response{(Config.UseCancellationTokens ? $", CancellationToken ct" : string.Empty)})"
        );
        w.WriteLine(1, "{");
        w.WriteLine(2, "response.EnsureSuccessStatusCode();");
        w.WriteLine(2, "return Task.CompletedTask;");
        w.WriteLine(1, "}");

        w.WriteLine("}");
    }
}
